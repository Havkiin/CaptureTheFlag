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

		int captain1 = Random.Range(0, 2);
		int captain2 = Random.Range(0, 2);

		team1[captain1].GetComponent<Agent>().setCaptain();
		team2[captain2].GetComponent<Agent>().setCaptain();
	}
	
	void Update () {
		
	}
}
