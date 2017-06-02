using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	private bool visible = false;
	private Vector3 initialPosition;

	void Start () {
		initialPosition = this.transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void show() {
		if (visible) return;

		var uiWidth = this.transform.GetComponent<RectTransform> ().rect.width;

		visible = true;
		AbstractGoTween tweenUI = Go.to (transform, .1f, new GoTweenConfig ()
			.localPosition(new Vector3 (initialPosition.x - uiWidth, initialPosition.y, initialPosition.z)));
	}

	public void hide() {
		if (!visible) return;

		visible = false;
		AbstractGoTween tweenUI = Go.to (transform, .1f, new GoTweenConfig ()
			.localPosition(new Vector3 (initialPosition.x, initialPosition.y, initialPosition.z)));
	}
}
