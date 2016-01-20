using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour {

	public static GridController instance;
	public List<List<GridCell>> grid;

	public static GameObject selectedUnit;

	void OnAwake() {
		instance = this;
	}

	void OnEnable() {
		EventManager.OnGameObjectClicked += SelectGridCell;
	}

	void OnDisable() {
		EventManager.OnGameObjectClicked -= SelectGridCell;
	}

	void Start() {

	}

	void SelectGridCell(GameObject g, Vector3 hit) {
		if (g != gameObject) return;
		if (selectedUnit == null) return;
		if (selectedUnit.transform.tag != "Unit") return;

		Vector3 dest = new Vector3(Mathf.Round(hit.x), 0f, Mathf.Round(hit.z));
		//print(dest);

		var unitScript = selectedUnit.GetComponent<UnitScript>();
		unitScript.MoveUnit(dest);
	}

}
