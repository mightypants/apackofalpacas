    using UnityEngine;
using System.Collections;
using SlideTriggerVisual;

public class Switch : MonoBehaviour 
{
    public GameObject switchTarget;                 // what the switch object is connected to.
    public string switchTargetAudio;                // the name (including path) of the FMOD sound effect the target will play 
    //public Texture activeTexture;                    // texture that is applied when switch activates game object
    //public Texture defaultTexture;                // texture applied to game object normally (used to return to normal state). 
    public int requiredAlpacas = 1;                 // the number of alpacas needed to activate the switch
    
    //private Animation switchTargetAnimation;        // animation for target of switch (door animation, cage animation, etc).   
    //private Renderer[] switchTargetActiveColor;     // attaches to the color of the object, changing it's active color.
    private bool isActivated;
    private string alpacaObjectTag = "Alpaca";      // tag switch searches for
    private int alpacasPresent;                     // the number of alpacas currently on the switch
    private string playerObjectTag = "Player";

    private ParticleSystem[] switchParticles;       //array of child particle system objects
    private ParticleSystem switchToDoorEffect;      //particle system that controls animation from switch to door.
    private ParticleSystem playerOnSwitchEffect;    //particle system that starts when player stands on door.

    
    void Start() 
    {
        //switchTargetActiveColor = switchTarget.GetComponentsInChildren<Renderer>();
        //switchTargetAnimation = switchTarget.GetComponent<Animation>();
        alpacasPresent = 0;
        switchParticles = GetComponentsInChildren<ParticleSystem> ();

        foreach(ParticleSystem particleEffect in switchParticles)
        {
            if(particleEffect.name == "SwitchParticles")
            {
                playerOnSwitchEffect = particleEffect;
            }
            else if(particleEffect.name == "SwitchTrail")
            {
                switchToDoorEffect = particleEffect;
            }
        }

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == alpacaObjectTag) 
        {  
            // as soon as the alpaca hits the trigger, it should lock on to the switch and stay put
            AlpacaMovement alpaca = c.gameObject.GetComponent<AlpacaMovement>();
            StartCoroutine(alpaca.MoveTowardTarget(gameObject));
            alpacasPresent++;
            Debug.Log("An alpaca is present!! Number: " + alpacasPresent);
        
            if (alpacasPresent >= requiredAlpacas)
            {
                FMOD_StudioSystem.instance.PlayOneShot(switchTargetAudio, switchTarget.transform.position);
                isActivated = true;
                switchToDoorEffect.Play();
                //switchTargetAnimation.Play();
                
//                foreach(Renderer r in switchTargetActiveColor)
//                {
//                    r.material.mainTexture = activeTexture;
//                }
            }
        } 
        else if(c.tag == playerObjectTag)
        {
            playerOnSwitchEffect.Play();
        }
    }
    
    void OnTriggerExit(Collider c)
    {
        if (c.tag == alpacaObjectTag)
        {
            alpacasPresent--;
            Debug.Log(alpacasPresent);
        } else if(c.tag == playerObjectTag)
        {
            playerOnSwitchEffect.Stop();
        }
        
        if (alpacasPresent <= requiredAlpacas)
        {
            //FMOD_StudioSystem.instance.PlayOneShot(switchTargetAudio, switchTarget.transform.position);
            isActivated = false;
            switchToDoorEffect.Stop();
//            foreach (Renderer r in switchTargetActiveColor) 
//            {
//                r.material.mainTexture = defaultTexture;
//            }
        }
    }

    public bool IsActivated()
    {
        return this.isActivated;
    }
}
