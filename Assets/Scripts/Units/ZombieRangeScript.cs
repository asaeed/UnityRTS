using UnityEngine;
using System.Collections;

public class ZombieRangeScript : MonoBehaviour {
	
	private ZombieScript parentScript;
	public GameObject closestUnit;
	private int targetCounter = 0;

	void Awake() {
		parentScript = transform.parent.GetComponent<ZombieScript>();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Survivor") {
			
			targetCounter++;

		} else if (other.tag == "Zombie") {
			var otherZombieScript = other.GetComponent<ZombieScript> ();

			// if this zombie is lurking and other is wandering, this decides to go to the same destination as other one
			// TODO: the other zombie should be closer, or else horde builds too fast, need another shorter range sphere?
			if (parentScript.state == ZombieScript.ZombieState.Lurk) {
				if (otherZombieScript.state == ZombieScript.ZombieState.Wander) {
					parentScript.Wander (otherZombieScript.wanderTo);
				}
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag != "Survivor") return;
		var survivor = other.gameObject;

		if (closestUnit == null) {
			closestUnit = survivor;
			parentScript.MoveUnit(survivor.transform.position);
		} else {
			var unitDist = (survivor.transform.position - transform.position).sqrMagnitude;
			var closestUnitDist = (closestUnit.transform.position - transform.position).sqrMagnitude;
			if (unitDist <= closestUnitDist) {
				closestUnit = survivor;
				parentScript.MoveUnit(survivor.transform.position);
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Survivor") {
			targetCounter--;

			if (targetCounter == 0)
				parentScript.Wander(other.gameObject.transform.position);
		}
	}
}