using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class torchAudio : MonoBehaviour {

	EventInstance torchEvent;


	// Use this for initialization
	void Start () {
		torchEvent = FMOD_StudioSystem.instance.GetEvent("event:/sfx/environment/torch");

		// set the position of the sound effect to be the player's position
		var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
		torchEvent.set3DAttributes(attributes);
		torchEvent.start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
