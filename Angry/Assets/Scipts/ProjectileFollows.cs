﻿using UnityEngine;
using System.Collections;

public class ProjectileFollows : MonoBehaviour {

	public Transform projectile;
	public Transform farLeft;
	public Transform farRight;

	void Update () {
		Vector3 newPosition = transform.position;
		newPosition.x = projectile.position.x;
		newPosition.x = Mathf.Clamp (newPosition.x, farLeft.position.x, farRight.position.x);
		transform.position = newPosition;
	}
}
