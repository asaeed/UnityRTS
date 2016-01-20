using UnityEngine;
using System.Collections;

public class UnitRangeScript : MonoBehaviour {

	private UnitScript parentScript;
	public GameObject closestZ;

	void Awake() {
		parentScript = transform.parent.GetComponent<UnitScript>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Zombie") {
			print("enemy in range!");
			parentScript.Attack(other.gameObject);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag != "Zombie") return;

		var z = other.gameObject;
		if (closestZ == null) {
			closestZ = z;
			parentScript.Attack(z);
		} else {
			var zDist = (z.transform.position - transform.position).sqrMagnitude;
			var closestZDist = (closestZ.transform.position - transform.position).sqrMagnitude;
			if (zDist <= closestZDist) {
				closestZ = z;
				parentScript.Attack(z);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Zombie") {
			parentScript.Guard();
		}
	}
}