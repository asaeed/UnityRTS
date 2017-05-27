using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public GameObject sourceUnit;

	private float birthTime;
	private float lifeSpan = 2f;
	private SurvivorScript parentScript;
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
		if (other.tag == "Zombie") {

			// check parent's skill to see if zombie received headshot
			parentScript = sourceUnit.GetComponent<SurvivorScript>();
			zombieScript = other.gameObject.GetComponent<ZombieScript>();

			bool isMiss = false;
			
			// roll the dice
			float r = Random.value;

			// survivor's skill shot determines chance of headshot
			// if miss, there's still chance of body shot
			if (r < parentScript.shootSkill) {

				// headshot
				zombieScript.Die();

			} else if (r - .2f < parentScript.shootSkill) {

				// bodyshot
				zombieScript.Slow(gameObject.GetComponent<Rigidbody>().velocity);

				// if stray bullet hits another zombie, he comes after the survivor who shot
				// TODO: this will work when zombies are programed to wander in the direction they were shot in.
				zombieScript.Wander(parentScript.gameObject.transform.position);

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
