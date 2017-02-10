using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive_Kinematic : MonoBehaviour {

	[SerializeField]
	GameObject target;

	[SerializeField]
	float speed;

	[SerializeField]
	float nearSpeed;

	Vector3 direction;
	float nearRadius;
	float arrivalRadius;
	float rotationSpeedRads;
	float distanceFromTarget;
	bool aligned;

	void Start () {

		nearRadius = 3.0f;
		arrivalRadius = 1.0f;
		rotationSpeedRads = 300.0f;
	}
	
	void FixedUpdate () {

		distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
		direction = (target.transform.position - transform.position).normalized;
		aligned = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) < 1.0f;

		if (GetComponent<Rigidbody>().velocity.magnitude < speed * 0.1f)
		{
			if (distanceFromTarget > nearRadius && !aligned)
			{
				Rotate();
			}
			else
			{
				Arrive();
			}
		}
		else
		{
			if (Mathf.Abs(Vector3.Angle(transform.forward, direction)) <= 30.0f && distanceFromTarget < nearRadius)
			{
				Rotate();
				Arrive();
			}
			else
			{
				if (!aligned)
				{
					Stop();
					Rotate();
				}
				else
				{
					Arrive();
				}
			}
		}
	}

	void Arrive ()
	{
		if (distanceFromTarget > nearRadius)
		{
			GetComponent<Rigidbody>().velocity = direction * speed * Time.fixedDeltaTime;
		}
		else if (distanceFromTarget > arrivalRadius)
		{
			GetComponent<Rigidbody>().velocity = direction * nearSpeed * Time.fixedDeltaTime;
		}
	}

	void Rotate ()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeedRads * Time.fixedDeltaTime);
	}

	void Stop ()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}
}
