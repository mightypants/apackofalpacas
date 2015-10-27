using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
	public GameObject player;          // Currently player-- will be remapped to alpaca.
	public GameObject switchTarget;    // what the switch object is connected to.
	public GameObject targetsTarget;  //where the object switch effects moves (empty gameobject in hierarchy labeled {var}Target.
	public float speed = 2f;   //*BROKEN* speed at which game object moves to end position.
	public Texture activeTexture;  //texture that is applied when switch activates game object
	public Texture defaultTexture;  //texture applied to game object normally (used to return to normal state).

	private Vector3 startPos;  //beginning transform position of target.
	private Vector3 endPos;    // location of "Target's target" Game Object.
	private Renderer[] switchTargetActive;  //attaches to the color of the object, changing it's active color.
	private string playerObjectTag = "Player";  //rename to Alpaca tag when alpaca AI is done.


	void Start() 
	{
		startPos = switchTarget.transform.position;
		endPos = targetsTarget.transform.position;
		switchTargetActive = switchTarget.GetComponentsInChildren<Renderer>();
	}



	void OnTriggerEnter(Collider c)
	{
		//switch to Alpaca when asset is created
		if (c.tag == playerObjectTag) {  
			switchTarget.transform.position = Vector3.Lerp (startPos, endPos, speed);
			foreach(Renderer r in switchTargetActive)
			{
			r.material.mainTexture = activeTexture;
			}
		} else 
		{
			Debug.Log ("You aren't the Player!");//remove when final
		}
	}


	void OnTriggerExit(Collider c)
	{
		switchTarget.transform.position = startPos;
		foreach (Renderer r in switchTargetActive) {
			r.material.mainTexture = defaultTexture;
		}
	}

	
}
