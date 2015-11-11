using UnityEngine;
using System.Collections;

public class DoorLift : MonoBehaviour {

	public float raiseHeight;
	private Vector3 targetPos;
	
	void Start () {
		targetPos = new Vector3(transform.position.x, transform.position.y + raiseHeight, transform.position.z);
	}

	public IEnumerator RaiseDoor()
	{
		while (transform.position.y < targetPos.y)
		{
			transform.Translate(0, .1f, 0);
			yield return null;
		}
	}
}
