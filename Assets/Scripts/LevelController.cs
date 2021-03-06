﻿using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public GameObject zombiePrefab;
	private Vector3 prefabPosition;

	public float spawnInterval = 10f;
	private float lastSpawnTime = 0;

	void Start () {
		zombiePrefab.CreatePool(60);
		prefabPosition = zombiePrefab.transform.position;

		// create some randomly located lurking zombies, but not close to center
		for (int i = 0; i < 60; i++) {
			// TODO: grab size of the grid dynamically from GridController
			Vector3 randomLocation = new Vector3 (Random.Range (-50f, 50f), prefabPosition.y, Random.Range (-50f, 50f));

			// just don't create them if they happen to exist by the center
			if (Mathf.Abs (randomLocation.x) > 10f || Mathf.Abs (randomLocation.z) > 10f) {
				GameObject z = zombiePrefab.Spawn (randomLocation);
				z.GetComponent<ZombieScript> ().Lurk ();

				//print(ObjectPool.CountSpawned(zombiePrefab) + "of" + ObjectPool.CountPooled(zombiePrefab));
			}
		}
			
		// also a horde wandering in one direction?
	}

	void Update () {
		// at an interval spawn a zombie on a random location on the board
		var curTime = Time.time;
		if (curTime - lastSpawnTime > spawnInterval) {
			lastSpawnTime = curTime;

			// pick a random location but only proceed if it's in the outer area
			Vector3 randomLocation = new Vector3(Random.Range(-50f, 50f), prefabPosition.y, Random.Range(-50f, 50f));
			if (Mathf.Abs(randomLocation.x) < 20f && Mathf.Abs (randomLocation.z) < 20f) return;

			print("spawn");
			GameObject z = zombiePrefab.Spawn(randomLocation);

			// set it on path to center
			var dest = new Vector3(0f, 0f, 0f);
			z.GetComponent<ZombieScript> ().Wander (dest);
		}
	}
}
