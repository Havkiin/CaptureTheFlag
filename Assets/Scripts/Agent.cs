using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	[SerializeField]
	GameObject terrain;

	[SerializeField]
	GameObject pointer;

	float boundZplus;
	float boundZminus;
	float boundXplus;
	float boundXminus;

	void Start () {

		Vector3 terrainBounds = terrain.GetComponent<Renderer>().bounds.size;
		Vector3 terrainPos = terrain.transform.position;

		boundXplus = terrainPos.x + (terrainBounds.x / 2);
		boundXminus = terrainPos.x - (terrainBounds.x / 2);
		boundZplus = terrainPos.z + (terrainBounds.z / 2);
		boundZminus = terrainPos.z - (terrainBounds.z / 2);

		pointer = Instantiate(pointer);
	}
	
	
	void FixedUpdate () {

		pointer.transform.position = new Vector3(transform.position.x + transform.forward.normalized.x, transform.position.y, transform.position.z + transform.forward.normalized.z);
		pointer.transform.rotation = Quaternion.LookRotation(transform.forward, transform.right);

		if (transform.position.x < boundXminus)
		{
			transform.position = new Vector3(boundXplus, transform.position.y, transform.position.z);
		}
		else if (transform.position.x > boundXplus)
		{
			transform.position = new Vector3(boundXminus, transform.position.y, transform.position.z);
		}

		if (transform.position.z < boundZminus)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, boundZplus);
		}
		else if (transform.position.z > boundZplus)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, boundZminus);
		}
	}
}
