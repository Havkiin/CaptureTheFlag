using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teams;

public class Flag : MonoBehaviour {

	[SerializeField]
	Team color;

	[SerializeField]
	GameObject holder;

	[SerializeField]
	GameObject victoryText;

	Vector3 startPos;
	Quaternion startRot;
	
	void Start ()
	{
		startPos = transform.position;
		startRot = transform.rotation;
	}

	void Update () {
		
		if (holder)
		{
			transform.position = holder.transform.position + Vector3.up;
			holder.GetComponent<Agent>().hasFlag();

			if (holder.GetComponent<Agent>().isHome())
			{
				victoryText.GetComponent<Text>().text = holder.GetComponent<Agent>().color.ToString() + " Team wins!";
			}

			if (holder.GetComponent<Agent>().isFrozen())
			{
				transform.position = startPos;
				transform.rotation = startRot;
				holder = null;
			}
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.collider.GetComponent<Agent>().color != this.color && !holder)
		{
			holder = col.collider.gameObject;
		}
	}
}
