using UnityEngine;
using System.Collections;

public class ZombieRangeScript : MonoBehaviour {
	
	private ZombieScript parentScript;
	public GameObject closestUnit;

	void Awake() {
		parentScript = transform.parent.GetComponent<ZombieScript>();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Unit") {
			print("player in range!");
			//parentScript.Attack(other.gameObject);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag != "Unit") return;

		var unit = other.gameObject;
		if (closestUnit == null) {
			closestUnit = unit;
			parentScript.MoveZombie(unit.transform.position);
		} else {
			var unitDist = (unit.transform.position - transform.position).sqrMagnitude;
			var closestUnitDist = (closestUnit.transform.position - transform.position).sqrMagnitude;
			if (unitDist <= closestUnitDist) {
				closestUnit = unit;
				parentScript.MoveZombie(unit.transform.position);
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Unit") {
			//parentScript.Guard();
		}
	}
}