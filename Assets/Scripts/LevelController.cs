using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public GameObject zombiePrefab;
	public float spawnInterval = 8f;

	private float lastSpawnTime = 0;

	// Use this for initialization
	void Start () {
		zombiePrefab.CreatePool(4);
	}
	
	// Update is called once per frame
	void Update () {
		// every second spawn a zombie on a random location on the board
		var curTime = Time.time;
		if (curTime - lastSpawnTime > spawnInterval) {
			lastSpawnTime = curTime;

			// spawn in a random location
			Vector3 prefabPosition = zombiePrefab.transform.position;

			// pick a random location but only proceed if it's in the outer area
			Vector3 randomLocation = new Vector3(Random.Range(-50f, 50f), prefabPosition.y, Random.Range(-50f, 50f));
			print(randomLocation);
			if (Mathf.Abs(randomLocation.x) < 20f || Mathf.Abs (randomLocation.z) < 20f) return;

			print("spawn");
			GameObject z = zombiePrefab.Spawn(randomLocation);

			// set it on path to center
			var dest = new Vector3(0f, 0f, 0f);
			var nma = z.GetComponent<NavMeshAgent>();
			nma.SetDestination(dest);
			

		}
	}
}
