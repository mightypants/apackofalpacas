    using UnityEngine;
using System.Collections;

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
    private DoorLift switchTargetDoorLift;             // attaches to the color of the object, changing it's active color.
    private string characterObjectTag = "Alpaca";   // tag switch searches for
    private int alpacasPresent;                     // the number of alpacas currently on the switch
    
    
    void Start() 
    {
        //switchTargetActiveColor = switchTarget.GetComponentsInChildren<Renderer>();
        //switchTargetAnimation = switchTarget.GetComponent<Animation>();
        switchTargetDoorLift = switchTarget.GetComponent<DoorLift>();
        alpacasPresent = 0;
    }
    
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == characterObjectTag) 
        {  
            // as soon as the alpaca hits the trigger, it should lock on to the switch and stay put
            AlpacaMovement alpaca = c.gameObject.GetComponent<AlpacaMovement>();
            StartCoroutine(alpaca.MoveTowardTarget(gameObject));
            alpacasPresent++;
            Debug.Log("An alpaca is present!! Number: " + alpacasPresent);
        
            if (alpacasPresent >= requiredAlpacas)
            {
                FMOD_StudioSystem.instance.PlayOneShot(switchTargetAudio, switchTarget.transform.position);
                //StartCoroutine(switchTargetDoorLift.RaiseDoor());
                isActivated = true;

                //switchTargetAnimation.Play();
                
//                foreach(Renderer r in switchTargetActiveColor)
//                {
//                    r.material.mainTexture = activeTexture;
//                }
            }
        } 
        else 
        {
            Debug.Log ("You aren't an Alpaca!!");//remove when final
        }
    }
    
    void OnTriggerExit(Collider c)
    {
        if (c.tag == characterObjectTag)
        {
            alpacasPresent--;
            Debug.Log(alpacasPresent);
        }
        
        if (alpacasPresent <= requiredAlpacas)
        {
            isActivated = false;
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
