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
			print("player in range!");
			targetCounter++;
			//parentScript.Attack(other.gameObject);
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