using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander_Steering : MonoBehaviour {

	[SerializeField]
	float speed;

	Vector3 direction;

	void Start () {

		direction = Vector3.forward;
	}
	
	void FixedUpdate () {

		GetComponent<Rigidbody>().velocity = direction * speed * Time.fixedDeltaTime;
	}
}
