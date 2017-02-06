using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive_Steering : MonoBehaviour {

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
	float velocityMax = 4.0f;

	float distanceFromTarget;
	Vector3 direction;

	void FixedUpdate()
	{
		distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
		direction = target.transform.position - transform.position;
		direction.Normalize();

		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeedRads * Time.fixedDeltaTime);

		if (GetComponent<Rigidbody>().velocity.magnitude < velocityMax)
		{
			if (distanceFromTarget > nearRadius)
			{
				GetComponent<Rigidbody>().AddForce(transform.forward.normalized * speed * Time.fixedDeltaTime, ForceMode.Impulse);
				Debug.Log(transform.forward.normalized);
			}
			else if (distanceFromTarget > arrivalRadius)
			{
				GetComponent<Rigidbody>().AddForce(transform.forward.normalized * nearSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
			}
		}
	}
}
