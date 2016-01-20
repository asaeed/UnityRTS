
/*
Unit Behavior

Player will receive X number of units at start of round.  
If a unit is bit (single successful attack by zombie) it turns.
That dead unit is replaced by a live one somewhere on the map.  
it needs to be rescued by the player to gain a new unit.

Units have the following skills:
Shoot - shooting accuracy
Dodge - chance of missing zombie attack
Speed - speed of shooting and movement
Gather - amount of resources found

Some are well suited to attack or defend.  
Others are better at gathering resources.

Unit can be assigned one of 5 modes:
- move - associated with destination, after reached transition to wait
- attack - associated with a target, after target defeated transition to wait
- wait - stands ground and attacks any enemy passers by
- gather - go off to nearest building to forage, TBD: when zombies are attacking, fight or flight?
- return - return to base

ATTACK
When attacking, unit fires at zombies in range starting with closest.
If zombie is in contact distance, player can push once with 4 second cooldown.
Units roll the dice against DODGE skill to see if zombie's attack is a hit, a single hit will turn the unit.

GATHER
Resources can be used to make defenses (traps) and weapons.
Weapons have different advantages, like reload speed, range, accuracy, spread
Traps can be set up and then zombies can be lured into them

ZOMBIES
Zombies walk in the direction of loud sounds and attack anyone who comes into range
their modes include:
- move (or wander - ie walking in a direction as opposed to a location)
- attack
- wait



*/


using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour {

	public GameObject bulletPrefab;
	public float shootSkill = 3f;

	public GameObject target;
	public string state;
	private float lastShotTime = 0f;
	private float coolDownTime = 2f;

	void OnEnable() {
		EventManager.OnGameObjectClicked += SelectUnit;
	}
	
	void OnDisable() {
		EventManager.OnGameObjectClicked -= SelectUnit;
	}

	void Start() {
		// TODO: pool should only be created once, move to a singleton
		// or single instance script
		bulletPrefab.CreatePool(5);
		this.state = "guard";
	}

	void Update() {
		float curTime = Time.time;
		if (this.state == "attack" && target != null) {
			if (curTime - lastShotTime > coolDownTime) {
				Shoot(this.target);
				lastShotTime = curTime;
			}
		}
	}

	void SelectUnit(GameObject g, Vector3 hit) {
		if (g != gameObject) return;
		GridController.selectedUnit = g;

		// animate to show selection
		Color curColor = GetComponent<Renderer>().material.color;
		curColor.a = .5f;
		AbstractGoTween tween = Go.to(g.transform, .1f, new GoTweenConfig()
			//.scale(.8f)
			.materialColor(curColor)
			.setIterations(4, GoLoopType.PingPong ));
	}

	public void MoveUnit(Vector3 dest) {
		NavMeshAgent nma = transform.GetComponent<NavMeshAgent>();
		nma.SetDestination(dest);
	}

	public void Attack(GameObject target) {
		this.target = target;
		this.state = "attack";
	}

	public void Guard() {
		this.target = null;
		this.state = "guard";
	}

	public void Shoot(GameObject target) {
		//transform.LookAt(target.transform.position);

		GameObject bullet = bulletPrefab.Spawn(transform.position, transform.rotation);
		//bullet.transform.parent = transform;
		bullet.GetComponent<BulletScript>().sourceUnit = gameObject;
		bullet.transform.LookAt(target.transform.position);

		Vector3 direction = (target.transform.position - transform.position).normalized;
		direction.y = 0;
		bullet.GetComponent<Rigidbody>().velocity = direction * 10;
	}
}
