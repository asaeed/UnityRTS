
/*
Zombie Behavior

Zombies walk in the direction of loud sounds and attack anyone who comes into range
their modes include:
- move (or wander - ie walking in a direction as opposed to a location)
- attack
- wait


states:

lurk
- slow speed
- random movement in a contained area
- transition to wander when hunger level increases
- transition to attack when survivor in range

* a horde able to trap a survivor who can be rescued and join the party

wander
- medium speed
- moving towards one destination with some random shifts
- transition to attack when survivor in range
- transition to lurk when arriving at destination
- change directon when blocked by obstacle

attack
- fast speed
- follows a survivor who is within range
- transitions to wander after survivor leaves range
- transitions to lurk after successful kill

attributes:

health
hunger

* zombies are "off the grid",
  they don't have to stick to the middle of a grid cell as survivors do, 
  but they do have personal space bubbles configured in the nav mesh settings

*/

using UnityEngine;
using System.Collections;

public class ZombieScript : UnitScript {
	
	public GameObject target;
	private float lastAttackTime = 0f;
	private float coolDownTime = 4f;

	private float lurkInterval;
	private float lastLurkTime = 0f;
	private float lurkDistance = 1f;

	public Vector3 wanderTo;
	private float wanderInterval = 1f;
	private float lastWanderTime = 0f;
	private float wanderDistance = 1f;

	public enum ZombieState {
		Lurk,
		Wander,
		Attack
	}
	public ZombieState state;
	
	void OnEnable() {
		InputController.OnGameObjectClicked += SelectUnit;
	}
	
	void OnDisable() {
		InputController.OnGameObjectClicked -= SelectUnit;
	}

	void Awake() {
		Lurk();
	}
	
	void Start() {
		
	}
	
	void Update() {
		var currentTime = Time.fixedTime;

		if (state == ZombieState.Lurk) {
			if (currentTime - lastLurkTime > lurkInterval) {
				var r = Random.Range (0f, 1f);
				var xCoord = Mathf.Sin (r * (2 * Mathf.PI)) * 1.5f;
				var zCoord = Mathf.Cos (r * (2 * Mathf.PI)) * 1.5f;
				//print (xCoord + ", " + zCoord);

				var moveToPosition = new Vector3 (transform.position.x + xCoord, transform.position.y, transform.position.z + zCoord);
				MoveUnit (moveToPosition);

				// set zombie up to move in 4-8 seconds
				lurkInterval = Random.Range (4f, 8f);
				lastLurkTime = currentTime;
			}
		} else if (state == ZombieState.Wander) {
			if (currentTime - lastWanderTime > wanderInterval) {

				// wander is a slow stagger to a destination
				var positionDiff = transform.position - wanderTo;

				// if we are there, transition to lurk
				if (positionDiff.magnitude < 1f) {
					Lurk ();
				} else {
					var moveToPosition = transform.position - positionDiff.normalized * 1.5f;
					MoveUnit (moveToPosition);
				}

				lastWanderTime = currentTime;
			}
		}
	}
	
	public override void SelectUnit(GameObject g, Vector3 hit) {
		if (g != gameObject) return;

		// base class Unit's method SelectUnit called (blinks object)
		base.SelectUnit(g, hit);

		// update ui
		GameObject.Find("UI").GetComponent<UIController>().updateUI(gameObject);
	}
	
	public void Lurk() {
		//print("zombie starts lurking");
		target = null;

		// set zombie up to move in 4-8 seconds
		lurkInterval = Random.Range(4f, 8f);

		state = ZombieState.Lurk;
	}
		
	public void Wander(Vector3 dest) {
		//print("zombie wandering");

		// set wander destination
		wanderTo = dest;

		state = ZombieState.Wander;
	}

	public void Attack(GameObject tar) {
		//print("zombie attacking");
		target = tar;
		state = ZombieState.Attack;
	}

	public void Die() {
		//print("zombie dies!");
		AbstractGoTween tween = Go.to(gameObject.transform, .8f, new GoTweenConfig()
			.scale(0f)
			.onComplete(onDieComplete));
	}

	public void onDieComplete(AbstractGoTween tween) {
		//print("in onDieComplete");
		Destroy(gameObject);
	}

	public void Slow(Vector3 bulletVelocity) {
		//print("zombie slowed!");
		this.gameObject.GetComponent<Rigidbody>().velocity = bulletVelocity/4;
		StartCoroutine(EndSlow());
	}

	private IEnumerator EndSlow() {
		yield return new WaitForSeconds(1);
		this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}
}
