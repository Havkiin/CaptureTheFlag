using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teams;

public class Wander_Kinematic : MonoBehaviour {

	[SerializeField]
	float speed;

	float orientation;
	float orientation2;
	float multiplicand;
	float rotationSpeed;

	void Start () {

		rotationSpeed = Mathf.PI;
	}
	
	void FixedUpdate () {

		if (GetComponent<Agent>().color.ToString() == "Red")
		{
			if (transform.position.x <= GetComponent<Agent>().getXMiddle() + 1.0f)
			{
				multiplicand = 3.0f;
			}
			else if (transform.position.x >= GetComponent<Agent>().getXPlus() - 1.0f)
			{
				multiplicand = -3.0f;
			}
			else
			{
				multiplicand = Random.Range(-1.0f, 1.0f);
			}
		}
		else if (GetComponent<Agent>().color.ToString() == "Blue")
		{
			if (transform.position.x >= GetComponent<Agent>().getXMiddle() - 1.0f)
			{
				multiplicand = -3.0f;
			}
			else if (transform.position.x <= GetComponent<Agent>().getXMinus() + 1.0f)
			{
				multiplicand = 3.0f;
			}
			else
			{
				multiplicand = Random.Range(-1.0f, 1.0f);
			}
		}

		orientation = multiplicand * rotationSpeed * Time.fixedDeltaTime;
		orientation2 = Random.Range(-1.0f, 1.0f) * rotationSpeed * Time.fixedDeltaTime;
		transform.forward += new Vector3(orientation, 0.0f, orientation2);

		GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.fixedDeltaTime;
	}
}
