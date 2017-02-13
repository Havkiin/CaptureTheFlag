using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teams;

public class Wander_Steering : MonoBehaviour {

	[SerializeField]
	float speed;

	float orientation;
	float orientation2;
	float multiplicand;
	float rotationSpeed;
	Vector3 goal;
	Vector3 direction;

	void Start () {

		goal = transform.forward * 2.0f;
		rotationSpeed = Mathf.PI;
	}
	
	void FixedUpdate () {

		if (GetComponent<Agent>().color.ToString() == "Red")
		{
			if (transform.position.x <= GetComponent<Agent>().getXMiddle() + 1.5f && Mathf.Abs(Vector3.Angle(goal, -Vector3.left)) > 60.0f)
			{
				multiplicand = 5.0f;
			}
			else if (transform.position.x >= GetComponent<Agent>().getXPlus() - 1.5f && Mathf.Abs(Vector3.Angle(goal, Vector3.left)) > 60.0f)
			{
				multiplicand = -5.0f;
			}
			else
			{
				multiplicand = Random.Range(-1.0f, 1.0f);
			}
		}
		else if (GetComponent<Agent>().color.ToString() == "Blue")
		{
			if (transform.position.x >= GetComponent<Agent>().getXMiddle() - 1.5f && Mathf.Abs(Vector3.Angle(goal, Vector3.left)) > 60.0f)
			{
				multiplicand = -5.0f;
			}
			else if (transform.position.x <= GetComponent<Agent>().getXMinus() + 1.5f && Mathf.Abs(Vector3.Angle(goal, -Vector3.left)) > 60.0f)
			{
				multiplicand = 5.0f;
			}
			else
			{
				multiplicand = Random.Range(-1.0f, 1.0f);
			}
		}

		orientation = multiplicand * rotationSpeed;

		goal = Quaternion.Euler(0.0f, orientation, 0.0f) * goal;
		transform.forward = Vector3.Slerp(transform.forward, goal, Time.fixedDeltaTime);

		GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.fixedDeltaTime;
	}
}
