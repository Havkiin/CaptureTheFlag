using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teams;

public class Flag : MonoBehaviour {

	[SerializeField]
	Team color;

	[SerializeField]
	GameObject holder;
	
	void Update () {
		
		if (holder)
		{
			transform.parent = holder.transform;
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
