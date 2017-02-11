using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Teams
{
	public enum Team { Blue, Red };

	public class Agent : MonoBehaviour
	{

		[SerializeField]
		GameObject terrainRight;

		[SerializeField]
		GameObject terrainLeft;

		[SerializeField]
		GameObject pointer;

		[SerializeField]
		public Team color;

		float boundZplus;
		float boundZminus;
		float boundXplus;
		float boundXminus;

		void Start()
		{

			Vector3 terrainBoundsRight = terrainRight.GetComponent<Renderer>().bounds.size;
			Vector3 terrainBoundsLeft = terrainLeft.GetComponent<Renderer>().bounds.size;
			Vector3 terrainRightPos = terrainRight.transform.position;
			Vector3 terrainLeftPos = terrainLeft.transform.position;

			boundXplus = terrainRightPos.x + (terrainBoundsRight.x / 2);
			boundXminus = terrainLeftPos.x - (terrainBoundsLeft.x / 2);
			boundZplus = terrainLeftPos.z + (terrainBoundsLeft.z / 2);
			boundZminus = terrainLeftPos.z - (terrainBoundsLeft.z / 2);

			pointer = Instantiate(pointer);
		}


		void FixedUpdate()
		{

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

			if (Input.GetKeyDown(KeyCode.K))
			{
				gameObject.GetComponent<Flee_Steering>().enabled = false;
				gameObject.GetComponent<Arrive_Steering>().enabled = false;

				gameObject.GetComponent<Flee_Kinematic>().enabled = true;
				gameObject.GetComponent<Arrive_Kinematic>().enabled = true;
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				gameObject.GetComponent<Flee_Steering>().enabled = true;
				gameObject.GetComponent<Arrive_Steering>().enabled = true;

				gameObject.GetComponent<Flee_Kinematic>().enabled = false;
				gameObject.GetComponent<Arrive_Kinematic>().enabled = false;
			}
		}
	}
}
