using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject panel;
	private bool panelVisible = false;
	private Vector3 panelPosition;
	private float panelWidth;

	public Button attackButton;
	public Button moveButton;
	//public GameObject gatherButton;
	//public GameObject returnButton;

	public GameObject selectedUnit;

	public enum UIState {
		None,
		MovePressed,
		AttackPressed
	}
	public UIState state = UIState.None;

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
		if (state == UIState.AttackPressed) {
			state = UIState.None;
			attackButton.image.CrossFadeColor(attackButton.colors.normalColor, .1f, true, true);
		} else {
			state = UIState.AttackPressed;
			moveButton.image.CrossFadeColor(moveButton.colors.normalColor, .1f, true, true);
			attackButton.image.CrossFadeColor(attackButton.colors.pressedColor, .1f, true, true);
		}
	}
		
	public void onMove() {
		if (state == UIState.MovePressed) {
			state = UIState.None;
			moveButton.image.CrossFadeColor(moveButton.colors.normalColor, .1f, true, true);
		} else {
			state = UIState.MovePressed;
			attackButton.image.CrossFadeColor(attackButton.colors.normalColor, .1f, true, true);
			moveButton.image.CrossFadeColor(moveButton.colors.pressedColor, .1f, true, true);
		}
	}

	public void onForage() {
		state = UIState.None;
		moveButton.image.CrossFadeColor(moveButton.colors.normalColor, .1f, true, true);
		attackButton.image.CrossFadeColor(attackButton.colors.normalColor, .1f, true, true);

	}

	public void onReturn() {
		state = UIState.None;
		moveButton.image.CrossFadeColor(moveButton.colors.normalColor, .1f, true, true);
		attackButton.image.CrossFadeColor(attackButton.colors.normalColor, .1f, true, true);
	}

}
