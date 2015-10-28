using UnityEngine;
using System.Collections;

public class FollowMovement : MonoBehaviour
{
	Transform player;
	NavMeshAgent nav;
	
	
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		nav = GetComponent <NavMeshAgent> ();
	}
	
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.E))
		{
		nav.SetDestination (player.position);
		}
		else
		{
		  nav.enabled = false;
		}
	}
}
