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
		float boundXRightPlus;
		float boundXRightMinus;
		float boundXLeftMinus;

		bool captain;
		bool flag;
		bool home;
		bool kinematic;
		bool steering;

		void Start()
		{

			Vector3 terrainBoundsRight = terrainRight.GetComponent<Renderer>().bounds.size;
			Vector3 terrainBoundsLeft = terrainLeft.GetComponent<Renderer>().bounds.size;
			Vector3 terrainRightPos = terrainRight.transform.position;
			Vector3 terrainLeftPos = terrainLeft.transform.position;

			boundXRightPlus = terrainRightPos.x + (terrainBoundsRight.x / 2);
			boundXRightMinus = terrainRightPos.x - (terrainBoundsRight.x / 2);
			boundXLeftMinus = terrainLeftPos.x - (terrainBoundsLeft.x / 2);
			boundZplus = terrainLeftPos.z + (terrainBoundsLeft.z / 2);
			boundZminus = terrainLeftPos.z - (terrainBoundsLeft.z / 2);

			home = true;
			flag = false;
			kinematic = true;
			steering = false;

			pointer = Instantiate(pointer);
		}


		void FixedUpdate()
		{

			pointer.transform.position = new Vector3(transform.position.x + transform.forward.normalized.x, transform.position.y, transform.position.z + transform.forward.normalized.z);
			pointer.transform.rotation = Quaternion.LookRotation(transform.forward, transform.right);

			if (transform.position.x < boundXLeftMinus)
			{
				transform.position = new Vector3(boundXRightPlus, transform.position.y, transform.position.z);
			}
			else if (transform.position.x > boundXRightPlus)
			{
				transform.position = new Vector3(boundXLeftMinus, transform.position.y, transform.position.z);
			}

			if (transform.position.z < boundZminus)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, boundZplus);
			}
			else if (transform.position.z > boundZplus)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, boundZminus);
			}

			if (color.ToString() == "Red")
			{
				if (transform.position.x < boundXRightMinus)
				{
					home = false;
				}
				else
				{
					home = true;
				}
			}
			else if (GetComponent<Agent>().color.ToString() == "Blue")
			{
				if (transform.position.x > boundXRightMinus)
				{
					home = false;
				}
				else
				{
					home = true;
				}
			}

			if (captain)
			{
				if (!flag)
				{
					if (kinematic)
					{
						GetComponent<Wander_Kinematic>().enabled = false;
						GetComponent<Arrive_Kinematic>().enabled = true;
					}
					else
					{
						GetComponent<Wander_Steering>().enabled = false;
						GetComponent<Arrive_Steering>().enabled = true;
					}

					if (color.ToString() == "Red")
					{

						GetComponent<Arrive_Kinematic>().setTarget(GameObject.Find("FlagBlue"));
						GetComponent<Arrive_Steering>().setTarget(GameObject.Find("FlagBlue"));
					}
					else if (color.ToString() == "Blue")
					{
						GetComponent<Arrive_Kinematic>().setTarget(GameObject.Find("FlagRed"));
						GetComponent<Arrive_Steering>().setTarget(GameObject.Find("FlagRed"));
					}
				}
				else
				{
					GetComponent<Arrive_Kinematic>().setTarget(transform.parent.gameObject);
					GetComponent<Arrive_Steering>().setTarget(transform.parent.gameObject);
				}
			}
			else
			{
				if (kinematic)
				{
					GetComponent<Wander_Kinematic>().enabled = true;
					GetComponent<Wander_Steering>().enabled = false;
				}
				else
				{
					GetComponent<Wander_Steering>().enabled = true;
					GetComponent<Wander_Kinematic>().enabled = false;
				}
			}

			if (Input.GetKeyDown(KeyCode.K))
			{
				kinematic = true;
				steering = false;
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				steering = true;
				kinematic = false;
			}
		}

		public void setCaptain ()
		{
			captain = true;
		}

		public bool isCaptain ()
		{
			return captain;
		}

		public void hasFlag ()
		{
			flag = true;
		}

		public float getXPlus ()
		{
			return boundXRightPlus;
		}

		public float getXMinus ()
		{
			return boundXLeftMinus;
		}

		public float getXMiddle ()
		{
			return boundXRightMinus;
		}

		void OnCollisionEnter (Collision col)
		{
			if (col.collider.gameObject.GetComponent<Agent>() && col.collider.gameObject.GetComponent<Agent>().color != color && home)
			{
				//Freeze the other agent
			}
		}
	}
}
