using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public GameObject sourceUnit;

	private float birthTime;
	private float lifeSpan = 2f;
	private UnitScript parentScript;
	private ZombieScript zombieScript;

	void OnEnable() {
		// destroy self after few seconds
		birthTime = Time.time;
	}

	void Update() {
		if (Time.time - birthTime > lifeSpan){
			this.Recycle();
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.transform.tag == "Zombie") {

			// check parent's skill to see if zombie received headshot
			parentScript = sourceUnit.GetComponent<UnitScript>();
			zombieScript = other.gameObject.GetComponent<ZombieScript>();

			bool isMiss = false;
			
			// roll the dice
			float r = Random.Range(0f, 10f);
			if (r < parentScript.shootSkill) {
				// headshot
				zombieScript.Die();
			} else if (r - 1f < parentScript.shootSkill) {
				// bodyshot
				zombieScript.Slow();
			} else {
				// miss
				isMiss = true;
			}

			// destroy bullet last, parent becomes object pool
			if (!isMiss)
				this.Recycle();
		}
	}
}
