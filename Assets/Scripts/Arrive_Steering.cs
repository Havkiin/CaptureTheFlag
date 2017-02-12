using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive_Steering : MonoBehaviour {

	[SerializeField]
	GameObject target;

	[SerializeField]
	float acceleration;

	[SerializeField]
	float velocityMax;

	[SerializeField]
	float nearVelocityMax;

	float nearRadius;
	float arrivalRadius;
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
	bool aligned;

	void Start () {

		nearRadius = 5.0f;
		arrivalRadius = 0.2f;

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
			direction = (target.transform.position - transform.position).normalized;
			aligned = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) < 1.0f;

			if (GetComponent<Rigidbody>().velocity.magnitude < 1.0f)
			{
				if (distanceFromTarget > nearRadius && !aligned)
				{
					Align();
				}
				else
				{
					Arrive();
				}
			}
			else
			{
				if (Mathf.Abs(Vector3.Angle(transform.forward, direction)) <= 30.0f && distanceFromTarget < 10.0f)
				{
					Align();
					Arrive();
				}
				else
				{
					if (!aligned)
					{
						Stop();
						Align();
					}
					else
					{
						Arrive();
					}
				}
			}
		}
	}

	void Arrive ()
	{
		if (distanceFromTarget > nearRadius)
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
		else if (distanceFromTarget > arrivalRadius)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude < nearVelocityMax)
			{
				GetComponent<Rigidbody>().velocity += direction * acceleration * Time.fixedDeltaTime;
			}
			else
			{
				GetComponent<Rigidbody>().velocity = direction.normalized * nearVelocityMax;
			}
		}
	}

	void Align ()
	{
		goalFacing = (target.transform.position - transform.position).normalized;
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

	void Stop ()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	public void setTarget(GameObject t)
	{
		target = t;
	}
}
