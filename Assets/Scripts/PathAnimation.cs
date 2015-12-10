using UnityEngine;
using System.Collections;

public class PathAnimation : MonoBehaviour {

    Transform[] pathSegments;
    ParticleSystem[] pathEffects;

	void Start() 
    {
        pathEffects = gameObject.GetComponentsInChildren<ParticleSystem>();
	}
	
    public void Animate()
    {
        foreach (ParticleSystem p in pathEffects)
        {
            p.Play(true);
        }
    }
}
