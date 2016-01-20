using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour {
	
	public GameObject target;
	public string state;
	
	void OnEnable() {
		EventManager.OnGameObjectClicked += SelectZombie;
	}
	
	void OnDisable() {
		EventManager.OnGameObjectClicked -= SelectZombie;
	}
	
	void Start() {
		this.state = "guard";
	}
	
	void Update() {
		if (this.state == "attack") {
			//MoveZombie(target.transform.position);
		}
	}
	
	void SelectZombie(GameObject g, Vector3 hit) {
		if (g != gameObject) return;
		GridController.selectedUnit = g;

		// animate to show selection
		Color curColor = GetComponent<Renderer>().material.color;
		curColor.a = .5f;
		AbstractGoTween tween = Go.to(g.transform, .1f, new GoTweenConfig()
			//.scale(.8f)
			.materialColor(curColor)
			.setIterations(4, GoLoopType.PingPong ));
	}

	public void MoveZombie(Vector3 dest) {
		NavMeshAgent nma = transform.GetComponent<NavMeshAgent>();
		nma.SetDestination(dest);
	}

	public void Attack(GameObject target) {
		print("zombie attacking");
		print(target);
		this.target = target;
		this.state = "attack";
	}
	
	public void Guard() {
		this.target = null;
		this.state = "guard";
	}

	public void Die() {
		print("zombie dies!");
		AbstractGoTween tween = Go.to(gameObject.transform, .8f, new GoTweenConfig()
			.scale(0f)
			.onComplete(onDieComplete));
	}

	public void onDieComplete(AbstractGoTween tween) {
		print("in onDieComplete");
		Destroy(gameObject);
	}

	public void Slow() {
		print("zombie slowed!");
	}
}
