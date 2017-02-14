using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teams;

public class GameManager : MonoBehaviour {

	[SerializeField]
	GameObject[] team1;

	[SerializeField]
	GameObject[] team2;

	void Start () {

		int captain1 = Random.Range(0, 3);
		int captain2 = Random.Range(0, 3);

		team1[captain1].GetComponent<Agent>().setCaptain();
		team2[captain2].GetComponent<Agent>().setCaptain();

		int helper1;
		int helper2;

		do
		{
			helper1 = Random.Range(0, 3);
		}
		while (helper1 == captain1);

		do
		{
			helper2 = Random.Range(0,3);
		}
		while (helper2 == captain2);

		team1[helper1].GetComponent<Agent>().setHelper();
		team2[helper2].GetComponent<Agent>().setHelper();

		foreach (GameObject agent in team1)
		{
			if (!agent.GetComponent<Agent>().isCaptain() && !agent.GetComponent<Agent>().isHelper())
			{
				agent.GetComponent<Agent>().setDefender();
			}
		}

		foreach (GameObject agent in team2)
		{
			if (!agent.GetComponent<Agent>().isCaptain() && !agent.GetComponent<Agent>().isHelper())
			{
				agent.GetComponent<Agent>().setDefender();
			}
		}
	}
	
	void Update () {
		
	}
}
