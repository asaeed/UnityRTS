using UnityEngine;
using System.Collections;

public class SurvivorRangeScript : MonoBehaviour {

	private SurvivorScript parentScript;
	public GameObject closestZ;

	void Awake() {
		parentScript = transform.parent.GetComponent<SurvivorScript>();
	}

	void OnTriggerEnter(Collider other) {
		
	}

	void OnTriggerStay(Collider other) {
		if (other.tag != "Zombie") return;

		// attack the closest zombie
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

	}
}