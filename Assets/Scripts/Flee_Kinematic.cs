using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee_Kinematic : MonoBehaviour {

	[SerializeField]
	GameObject target;

	[SerializeField]
	float speed;

	float turnRadius;
	float rotationSpeedRads;
	float distanceFromTarget;
	Vector3 direction;

	void Start () {

		turnRadius = 5.0f;
		rotationSpeedRads = 300.0f;
	}

	void FixedUpdate()
	{
		if (target)
		{
			distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
			direction = (transform.position - target.transform.position).normalized;

			if (distanceFromTarget > turnRadius && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) > 1.0f)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeedRads * Time.fixedDeltaTime);
			}
			else
			{
				GetComponent<Rigidbody>().velocity = direction * speed * Time.fixedDeltaTime;
			}
		}
	}
}
