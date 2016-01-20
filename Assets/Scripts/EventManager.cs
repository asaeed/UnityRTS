using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public delegate void GameObjectEventHandler(GameObject e, Vector3 hitPoint);
	public delegate void GlobalEventHandler();
	public delegate void GlobalEventHandlerVec3(Vector3 v);
	
	public static event GameObjectEventHandler OnGameObjectClicked;
	public static event GameObjectEventHandler OnGameObjectHeld;
	public static event GlobalEventHandler OnClicked;
	public static event GlobalEventHandlerVec3 OnDrag;

	public float dragSpeed = .05f;
	public float dragThresh = 8f;

	private Vector2 dragStartPos = new Vector2();
	private bool isDragging = false;

	
	void Start () {
	
	}
	
	void Update() {
	
		// mouse event
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, 100)) {
				if (Input.GetMouseButtonDown(0)) {
					print("mousebutton DOWN");
					//dragStartPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					dragStartPos.x = Input.mousePosition.x;
					dragStartPos.y = Input.mousePosition.y;

				}

				if (Input.GetMouseButton(0)) {
					// fire held event for whatever object you're over
					if (OnGameObjectHeld != null)
						OnGameObjectHeld(hit.transform.gameObject, hit.point);

					// detect drag
					Vector2 dragCurPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					Vector2 dragDiff = dragCurPos - dragStartPos;
					if (dragDiff.magnitude > dragThresh) {
						isDragging = true;
                        // for perspective camera use:
						transform.Translate(-dragDiff.x * dragSpeed - dragDiff.y * dragSpeed, 0, -dragDiff.y * dragSpeed + dragDiff.x * dragSpeed);
                        // for orthographic camera use:
                        // transform.Translate(-dragDiff.x * dragSpeed, -dragDiff.y * dragSpeed, 0);
                        dragStartPos.x = Input.mousePosition.x;
						dragStartPos.y = Input.mousePosition.y;
					}
				}

				if (Input.GetMouseButtonUp(0)) {
					print("mousebutton UP");

					// ignore if dragging
					if (!isDragging) {
						if (OnGameObjectClicked != null)
							OnGameObjectClicked(hit.transform.gameObject, hit.point);
					}

					// on any mouseup, drag ends
					isDragging = false;
				}
			}	
			// also fire global click
			if (OnClicked != null)
				OnClicked();
		}
		
		// TODO: is it better performance-wise to rewrite all above events for touch?
		// drag event
		// if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
		// 	Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition * Time.deltaTime;
		// 	transform.Translate(-touchDeltaPosition.x * dragSpeed, -touchDeltaPosition.y * dragSpeed, 0);
		// }
		
	}
}
