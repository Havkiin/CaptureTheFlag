using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour {

	[SerializeField]
	float speed;

	float orientation;
	float rotationSpeed;

	void Start () {

		rotationSpeed = Mathf.PI;
	}
	
	void FixedUpdate () {

		orientation = Random.Range(-1.0f, 1.0f) * rotationSpeed * Time.fixedDeltaTime;

		transform.forward += new Vector3(orientation, 0.0f, orientation);

		GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.fixedDeltaTime;
	}
}
