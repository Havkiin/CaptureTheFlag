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

	float turnRadius = 5.0f;
	float nearRadius = 3.0f;
	float arrivalRadius = 1.0f;
	float rotationSpeedRads = 50.0f;
	bool aligned;

	float distanceFromTarget;
	Vector3 direction;
	
	void FixedUpdate () {

		distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
		direction = (target.transform.position - transform.position);
		aligned = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) < 1.0f;

		if (GetComponent<Rigidbody>().velocity.magnitude < 2.0f)
		{
			if (distanceFromTarget > turnRadius && !aligned)
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
			if (Vector3.Angle(transform.forward, direction) <= 30.0f && distanceFromTarget < 10.0f)
			{
				Rotate();
				Arrive();
			}
			else
			{
				if (!aligned)
				{
					Rotate();
				}
				else
				{
					Arrive();
				}
			}
		}
	}

	void Arrive()
	{
		if (distanceFromTarget > nearRadius)
		{
			GetComponent<Rigidbody>().velocity = direction.normalized * speed * Time.fixedDeltaTime;
		}
		else if (distanceFromTarget > arrivalRadius)
		{
			GetComponent<Rigidbody>().velocity = direction.normalized * nearSpeed * Time.fixedDeltaTime;
		}
	}

	void Rotate ()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeedRads * Time.fixedDeltaTime);
	}
}
