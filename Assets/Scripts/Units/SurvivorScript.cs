
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

Alt:
Attack - chances of hit
Range - (melee or short/long range firearm)
Speed - chance of missing zombie attack, speed of attack and movement
Gather - amount of resources found

Some are well suited to attack or defend.  
Others are better at gathering resources.

Unit can be assigned one of 5 states:
- move 
  - associated with destination
  - transition to wait, after destination reached
  
- attack 
  - associated with a zombie or stationary target
  - transition to wait after target defeated 
  
- wait 
  - stands ground and attacks any enemy passers by
  
- gather 
  - go off to nearest building to forage, TBD: when zombies are attacking, fight or flight?
  
- return 
  - return until within range of base
  - 

ATTACK
When attacking, unit fires at zombies in range starting with closest.
If zombie is in contact distance, player can push once with 4 second cooldown.
Units roll the dice against DODGE skill to see if zombie's attack is a hit, a single hit will turn the unit.

GATHER
Resources can be used to make defenses (traps) and weapons.
Weapons have different advantages, like reload speed, range, accuracy, spread
Traps can be set up and then zombies can be lured into them

*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SurvivorScript : UnitScript {

	public string survivorName;
	public float health = 20;

	public enum SurvivorState {
		Wait,
		Move,
		Attack,
		Gather,
		Return
	}
	public SurvivorState state = SurvivorState.Wait;

	public GameObject bulletPrefab;

	[Range(0f,1f)]
	public float shootSkill = .30f;

	public GameObject target;
	private float lastShotTime = 0f;
	private float coolDownTime = .5f;
	private float chanceOfPause = .3f; 

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
	}

	void Update() {
		float curTime = Time.time;
		if (this.state == SurvivorState.Attack && target != null) {
			if (curTime - lastShotTime > coolDownTime) {
				 
				// this creates a little randomness in shots fired
				if (Random.value > chanceOfPause)
					Shoot(this.target);
				lastShotTime = curTime;
			}
		}
	}

	public override void SelectUnit(GameObject g, Vector3 hit) {
		if (g != gameObject) return;

		// base class Unit's method SelectUnit called
		base.SelectUnit(g, hit);

		// populate ui
		GameObject.Find("UI_Name").GetComponent<Text>().text = survivorName;

		// show ui
		ToggleUI(true);
	}

	public void Attack(GameObject target) {
		this.target = target;
		this.state = SurvivorState.Attack;
	}

	public void Wait() {
		this.target = null;
		this.state = SurvivorState.Wait;
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
