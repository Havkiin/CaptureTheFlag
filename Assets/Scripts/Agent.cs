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

		[SerializeField]
		GameObject otherTeam;

		float boundZplus;
		float boundZminus;
		float boundXRightPlus;
		float boundXRightMinus;
		float boundXLeftMinus;
		
		bool captain;
		bool helper;
		bool defender;
		bool flag;
		bool home;
		bool frozen;
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

			//Toroidal arena
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

			//Sets home according to team
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

			/***** DECISION MAKING *****/

			//FROZEN (TOUCHED BY AN ENEMY WITHIN ITS HOME)
			if (frozen)
			{
				GetComponent<Arrive_Kinematic>().enabled = false;
				GetComponent<Flee_Kinematic>().enabled = false;
				GetComponent<Wander_Kinematic>().enabled = false;
				GetComponent<Arrive_Steering>().enabled = false;
				GetComponent<Flee_Steering>().enabled = false;
				GetComponent<Wander_Steering>().enabled = false;
			}
			else
			{
				//TEAM CAPTAIN (GETS THE FLAG AND RETURNS IT)
				if (captain)
				{
					//Get the flag
					if (!flag)
					{
						if (kinematic)
						{
							GetComponent<Wander_Kinematic>().enabled = false;
							GetComponent<Arrive_Kinematic>().enabled = true;
						}
						else if (steering)
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
					//Return it home
					else
					{
						GetComponent<Arrive_Kinematic>().setTarget(transform.parent.gameObject);
						GetComponent<Arrive_Steering>().setTarget(transform.parent.gameObject);
					}
				}
				//HELPER (UNFREEZES CAPTAIN)
				else if (helper)
				{
					foreach (Transform mate in transform.parent)
					{
						if (mate.gameObject.GetComponent<Agent>() && mate.gameObject.GetComponent<Agent>().frozen)
						{
							GetComponent<Arrive_Kinematic>().setTarget(mate.gameObject);
							GetComponent<Arrive_Steering>().setTarget(mate.gameObject);

							if (kinematic)
							{
								GetComponent<Wander_Kinematic>().enabled = false;
								GetComponent<Arrive_Kinematic>().enabled = true;
							}
							else if (steering)
							{
								GetComponent<Wander_Steering>().enabled = false;
								GetComponent<Arrive_Steering>().enabled = true;
							}
						}
						else
						{
							if (kinematic)
							{
								GetComponent<Arrive_Kinematic>().enabled = false;
								GetComponent<Wander_Kinematic>().enabled = true;
								GetComponent<Wander_Steering>().enabled = false;
							}
							else if (steering)
							{
								GetComponent<Arrive_Steering>().enabled = false;
								GetComponent<Wander_Steering>().enabled = true;
								GetComponent<Wander_Kinematic>().enabled = false;
							}
						}
					}
				}
				//DEFENDER (FREEZES OPPONENTS WITHIN ITS BASE)
				else if (defender)
				{
					foreach (Transform opponent in otherTeam.transform)
					{
						if (opponent.gameObject.GetComponent<Agent>() && !opponent.gameObject.GetComponent<Agent>().home && !opponent.gameObject.GetComponent<Agent>().frozen)
						{
							GetComponent<Arrive_Kinematic>().setTarget(opponent.gameObject);
							GetComponent<Arrive_Steering>().setTarget(opponent.gameObject);

							if (kinematic)
							{
								GetComponent<Wander_Kinematic>().enabled = false;
								GetComponent<Arrive_Kinematic>().enabled = true;
							}
							else if (steering)
							{
								GetComponent<Wander_Steering>().enabled = false;
								GetComponent<Arrive_Steering>().enabled = true;
							}
						}
						else
						{
							if (kinematic)
							{
								GetComponent<Wander_Kinematic>().enabled = true;
								GetComponent<Wander_Steering>().enabled = false;
							}
							else if (steering)
							{
								GetComponent<Wander_Steering>().enabled = true;
								GetComponent<Wander_Kinematic>().enabled = false;
							}
						}
					}
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

		public void setHelper ()
		{
			helper = true;
		}

		public bool isHelper()
		{
			return helper;
		}

		public void setDefender ()
		{
			defender = true;
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

		public bool isFrozen()
		{
			return frozen;
		}

		public bool isHome ()
		{
			return home;
		}

		void OnCollisionEnter (Collision col)
		{
			if (col.collider.gameObject.GetComponent<Agent>() && !home)
			{
				if (col.collider.gameObject.GetComponent<Agent>().color != color)
				{
					//Get frozen by opponent
					frozen = true;
					//gameObject.GetComponent<Rigidbody>().isKinematic = true;
				}
				else if (col.collider.gameObject.GetComponent<Agent>().color == color)
				{
					//Get unfrozen by teammate
					frozen = false;
					gameObject.GetComponent<Rigidbody>().isKinematic = false;
				}
			}
		}
	}
}
