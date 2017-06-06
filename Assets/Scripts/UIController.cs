using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	public GameObject panel;
	private bool panelVisible = false;
	private Vector3 panelPosition;
	private float panelWidth;

	void Start () {
		panelPosition = panel.transform.localPosition;
		panelWidth = panel.transform.GetComponent<RectTransform> ().rect.width;
	}

	public void show() {
		if (panelVisible) return;

		panelVisible = true;
		var tweenMoveUI = Go.to (panel.transform, .2f, new GoTweenConfig ()
			.localPosition(new Vector3 (panelPosition.x - panelWidth, panelPosition.y, panelPosition.z)));
		LeanTween.alphaCanvas(panel.GetComponent<CanvasGroup>(), 1f, .2f);
	}

	public void hide() {
		if (!panelVisible) return;

		panelVisible = false;
		AbstractGoTween tweenUI = Go.to (panel.transform, .2f, new GoTweenConfig ()
			.localPosition(new Vector3 (panelPosition.x, panelPosition.y, panelPosition.z)));
		LeanTween.alphaCanvas(panel.GetComponent<CanvasGroup>(), 0f, .2f);
	}
}
