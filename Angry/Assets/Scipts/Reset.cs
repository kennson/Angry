using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	public Rigidbody2D projectile;
	public float resetSpeed = 0.025f;

	private float resetSpeedSqr;
	private SpringJoint2D spring;

	void Start() {
		resetSpeedSqr = resetSpeed * resetSpeed;
		spring = projectile.GetComponent<SpringJoint2D> ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.R)) {
			Resets();		
		}

		if (spring == null && projectile.velocity.sqrMagnitude < resetSpeedSqr) {
			Resets();		
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.rigidbody2D == projectile) {
			Resets();		
		}
	}

	void Resets() {
		Application.LoadLevel(Application.loadedLevel);
	}
}
