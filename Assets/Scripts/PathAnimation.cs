using UnityEngine;
using System.Collections;

public class PathAnimation : MonoBehaviour {

    ParticleSystem[] pathEffects;

	void Start() 
    {
        pathEffects = gameObject.GetComponentsInChildren<ParticleSystem>();
	}
	
    public void Animate()
    {
        if (pathEffects.Length > 0)
        {
            foreach (ParticleSystem p in pathEffects)
            {
                p.Play(true);
            }
        }
    }
}
