using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void SelectUnit(GameObject g, Vector3 hit) {
		BlinkUnit (g);
	}

	public void BlinkUnit(GameObject g) {
		GridController.selectedUnit = g;

		// animate to show selection
		Color curColor = GetComponent<Renderer>().material.color;
		curColor.a = .5f;
		AbstractGoTween tweenSelect = Go.to(g.transform, .1f, new GoTweenConfig()
			.materialColor(curColor)
			.setIterations(4, GoLoopType.PingPong));
	}

	public void ToggleUI(bool show) {
		var ui = GameObject.Find("UI").gameObject;
		var uiScript = ui.GetComponent<UIController>();
		if (show)
			uiScript.show ();
		else
			uiScript.hide ();
	}

	public void MoveUnit(Vector3 dest) {
		UnityEngine.AI.NavMeshAgent nma = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
		nma.SetDestination(dest);
	}
}
