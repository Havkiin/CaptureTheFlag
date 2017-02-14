using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teams;

public class Flag : MonoBehaviour {

	[SerializeField]
	Team color;

	[SerializeField]
	GameObject holder;

	Vector3 startPos;
	
	void Start ()
	{
		startPos = transform.position;
	}

	void Update () {
		
		if (holder)
		{
			transform.parent = holder.transform;
			transform.parent.gameObject.GetComponent<Agent>().hasFlag();

			if (holder.GetComponent<Agent>().isFrozen())
			{
				transform.position = startPos;
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
