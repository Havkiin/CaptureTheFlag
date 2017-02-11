using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee_Steering : MonoBehaviour {

	[SerializeField]
	GameObject target;

	[SerializeField]
	float acceleration;

	[SerializeField]
	float velocityMax;

	float turnRadius;
	float distanceFromTarget;
	Vector3 direction;

	Quaternion goalOrientation;
	Vector3 goalFacing;
	float slowDownThreshold;
	float maxRotationSpeedRads;
	float maxRotationAccelerationRads;
	float goalRotationSpeedRads;
	float rotationSpeedRads;
	float accelerationRads;
	float timeToTarget;

	void Start () {

		turnRadius = 5.0f;

		slowDownThreshold = 5.0f;
		maxRotationSpeedRads = 2.0f;
		maxRotationAccelerationRads = 1.0f;
		goalRotationSpeedRads = 0.0f;
		rotationSpeedRads = 0.0f;
		accelerationRads = 0.1f;
		timeToTarget = 0.0f;
	}
	
	void FixedUpdate () {

		if (target)
		{
			distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
			direction = (transform.position - target.transform.position).normalized;

			if (distanceFromTarget > turnRadius && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) > 1.0f)
			{
				FaceAway();
			}
			else
			{
				if (GetComponent<Rigidbody>().velocity.magnitude < velocityMax)
				{
					GetComponent<Rigidbody>().velocity += direction * acceleration * Time.fixedDeltaTime;
				}
				else
				{
					GetComponent<Rigidbody>().velocity = direction.normalized * velocityMax;
				}
			}
		}
	}

	void FaceAway()
	{
		goalFacing = (transform.position - target.transform.position).normalized;
		rotationSpeedRads = maxRotationSpeedRads * (Vector3.Angle(goalFacing, this.transform.forward) / slowDownThreshold);

		if (rotationSpeedRads != 0.0f)
		{
			timeToTarget = Vector3.Angle(goalFacing, this.transform.forward) / rotationSpeedRads;
		}
		else
		{
			timeToTarget = 0.1f;
		}

		accelerationRads = (goalRotationSpeedRads - rotationSpeedRads) / timeToTarget;

		if (accelerationRads >= maxRotationAccelerationRads)
		{
			accelerationRads = maxRotationAccelerationRads;
		}

		rotationSpeedRads = rotationSpeedRads + (accelerationRads * Time.fixedDeltaTime);

		goalOrientation = Quaternion.LookRotation(goalFacing, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, goalOrientation, rotationSpeedRads);
	}
}
