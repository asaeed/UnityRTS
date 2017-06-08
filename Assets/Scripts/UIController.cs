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

		LeanTween.moveLocalX (panel, panelPosition.x - panelWidth, .2f);
		LeanTween.alphaCanvas(panel.GetComponent<CanvasGroup>(), 1f, .2f);
	}

	public void hide() {
		if (!panelVisible) return;

		panelVisible = false;

		LeanTween.moveLocalX (panel, panelPosition.x, .2f);
		LeanTween.alphaCanvas(panel.GetComponent<CanvasGroup>(), 0f, .2f);
	}

	public void onAttack() {

	}
		
	public void onMove() {

	}

	public void onForage() {

	}

	public void onReturn() {

	}

}
