using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject panel;
	private bool panelVisible = false;
	private Vector3 panelPosition;
	private float panelWidth;

	public GameObject selectedUnit;

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

	public void updateUI(GameObject unit) {

		// if prev selection is survior, hide its select ring
		if (selectedUnit != null && selectedUnit.tag == "Survivor")
			selectedUnit.GetComponent<SurvivorScript> ().selectRing.SetActive (false);

		// if survivor, show select ring, populate UI panel and show it
		if (unit.tag == "Survivor") {
			unit.GetComponent<SurvivorScript> ().selectRing.SetActive (true);
			GameObject.Find ("uiImage").GetComponent<Image> ().sprite = unit.GetComponent<SurvivorScript> ().portrait;
			GameObject.Find ("uiName").GetComponent<Text> ().text = unit.GetComponent<SurvivorScript> ().survivorName;
			show ();
		} else {
			hide ();
		}

		// set new selected unit
		selectedUnit = unit;
	}

	public void onAttack() {
		print ("attack button pressed");
	}
		
	public void onMove() {
		print ("move button pressed");
	}

	public void onForage() {
		print ("forage button pressed");

	}

	public void onReturn() {
		print ("return button pressed");
	}

}
