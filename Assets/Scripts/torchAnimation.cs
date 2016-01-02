using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class torchAnimation : MonoBehaviour {

	EventInstance torchEvent;
	ParticleSystem flameParticles;


	// Use this for initialization
	void Start () {
		torchEvent = FMOD_StudioSystem.instance.GetEvent("event:/sfx/environment/torch");

		// set the position of the sound effect to be the player's position
		var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
		torchEvent.set3DAttributes(attributes);

		flameParticles = GetComponentInChildren<ParticleSystem>();

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			torchEvent.start();
			flameParticles.Play();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			torchEvent.stop(STOP_MODE.IMMEDIATE);
			flameParticles.Stop();
		}
	}
}
