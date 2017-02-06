using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee_Kinematic : MonoBehaviour {

	[SerializeField]
	GameObject target;

	[SerializeField]
	float speed;

	float turnRadius = 5.0f;
	float rotationSpeedRads = 50.0f;

	float distanceFromTarget;
	Vector3 direction;
	
	void FixedUpdate () {

		distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
		direction = transform.position - target.transform.position;
		direction.Normalize();

		if (distanceFromTarget > turnRadius && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) > 1.0f)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeedRads * Time.fixedDeltaTime);
		}
		else
		{
			GetComponent<Rigidbody>().AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Impulse);
		}
	}
}
