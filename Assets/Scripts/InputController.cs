using UnityEngine;
using System.Collections;
using Lean.Touch;
using System.Collections.Generic;

public class InputController : MonoBehaviour {

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
	private bool isFingerDown = false;

	
	void Start () {
	
	}

	// TODO: replace what's in Update() with LeanTouch events tied here in OnEnable()

	protected virtual void OnEnable()
	{
		// Hook into the events we need
		LeanTouch.OnFingerDown  += OnFingerDown;
		LeanTouch.OnFingerSet   += OnFingerSet;
		LeanTouch.OnFingerUp    += OnFingerUp;
		LeanTouch.OnGesture     += OnGesture;
		LeanTouch.OnFingerSwipe += OnFingerSwipe;
	}

	protected virtual void OnDisable()
	{
		// Unhook the events
		LeanTouch.OnFingerDown  -= OnFingerDown;
		LeanTouch.OnFingerSet   -= OnFingerSet;
		LeanTouch.OnFingerUp    -= OnFingerUp;
		LeanTouch.OnGesture     -= OnGesture;
		LeanTouch.OnFingerSwipe -= OnFingerSwipe;
	}

	public void OnFingerDown(LeanFinger finger)
	{
		if (finger.StartedOverGui == true)
		{
			return;
		}

		// set variable to hold finger down state
		isFingerDown = true;
	}

	public void OnFingerSet(LeanFinger finger)
	{
		if (finger.ScreenDelta.magnitude > 1f)
			Debug.Log("Finger " + finger.Index + " is held on the screen");
	}

	public void OnFingerUp(LeanFinger finger)
	{
		// Debug.Log("Finger " + finger.Index + " finished touching the screen");
		isFingerDown = false;
	}

	public void OnFingerTap(LeanFinger finger)
	{
		Debug.Log("Finger " + finger.Index + " tapped the screen");
	}

	public void OnFingerSwipe(LeanFinger finger)
	{
		var swipe = finger.SwipeScreenDelta;
		if (swipe.x < -Mathf.Abs (swipe.y)) {
			Debug.Log ("You swiped left!");
		}
		if (swipe.x > Mathf.Abs (swipe.y)) {
			Debug.Log ("You swiped right!");
		}
	}

	public void OnGesture(List<LeanFinger> fingers)
	{
		/*
			Debug.Log ("Gesture with " + fingers.Count + " finger(s)");
			Debug.Log ("    pinch scale: " + LeanGesture.GetPinchScale (fingers));
			Debug.Log ("    twist degrees: " + LeanGesture.GetTwistDegrees (fingers));
			Debug.Log ("    twist radians: " + LeanGesture.GetTwistRadians (fingers));
			Debug.Log ("    screen delta: " + LeanGesture.GetScreenDelta (fingers));
		*/

//		if (currentState == StateType.Move) {
//			chairSelector.rotateObject (LeanGesture.GetScreenDelta (fingers).x * -.25f); 
//		} else if (currentState == StateType.Swap) {
//			chairSelector.slideChairs (LeanGesture.GetScreenDelta (fingers).x);
//		}
	}

	
	void Update() {
	
		// mouse event
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, 100)) {
				if (Input.GetMouseButtonDown(0)) {
					//print("mousebutton DOWN");
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

                        // TODO: don't drag out of bounds of grid
                        //print(transform.position);
                        float xDrag = -dragDiff.x * dragSpeed - dragDiff.y * dragSpeed;
                        float zDrag = -dragDiff.y * dragSpeed + dragDiff.x * dragSpeed;

                        // dragged more than threshold so move the transform (camera in this case)
                        isDragging = true;
						transform.Translate(xDrag, 0, zDrag);  // for perspective camera
                        // transform.Translate(-dragDiff.x * dragSpeed, -dragDiff.y * dragSpeed, 0);  // for orthographic camera
                        dragStartPos.x = Input.mousePosition.x;
						dragStartPos.y = Input.mousePosition.y;
					}
				}

				if (Input.GetMouseButtonUp(0)) {
					//print("mousebutton UP");

					// this prevents a tap on the gui from triggering the event in the world
					var isOverGui = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject (-1);
					//var isOverGui = false;

					// ignore if dragging
					if (!isDragging && !isOverGui) {
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
