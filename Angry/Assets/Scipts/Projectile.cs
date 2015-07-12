using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float maxStretch = 3.0f;
	public LineRenderer catapultFront;
	public LineRenderer catapultBack;

	private SpringJoint2D spring;
	private Transform catapult;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private float circleRadius;
	private bool clickedOn;
	private Vector2 prevVelocity;

	void Awake() {
		spring = GetComponent<SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
	}
	
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray (catapultFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = collider2D as CircleCollider2D;
		circleRadius = circle.radius;
	}

	void Update () {
		if (clickedOn) 
			Dragging ();

		if (spring != null) {
			if(!rigidbody2D.isKinematic && prevVelocity.sqrMagnitude > rigidbody2D.velocity.sqrMagnitude){
				Destroy(spring);
				rigidbody2D.velocity = prevVelocity;
			}	

			if(!clickedOn)
				prevVelocity = rigidbody2D.velocity;

			LineRendererUpdate();
		} else {
			catapultFront.enabled = false;
			catapultBack.enabled = false;
		}
	}

	void LineRendererSetup() {
		catapultFront.SetPosition (0, catapultFront.transform.position);
		catapultBack.SetPosition (0, catapultBack.transform.position);

		catapultFront.sortingLayerName = "Foreground";
		catapultBack.sortingLayerName = "Foreground";

		catapultFront.sortingOrder = 3;
		catapultBack.sortingOrder = 1;
	}

	void OnMouseDown() {
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp() {
		spring.enabled = true;
		rigidbody2D.isKinematic = false;
		clickedOn = false;
	}

	void Dragging() {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererUpdate() {
		Vector2 catapultToProjectile = transform.position - catapultFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint (catapultToProjectile.magnitude + circleRadius);
		catapultFront.SetPosition (1, holdPoint);
		catapultBack.SetPosition (1, holdPoint);
	}
}
