using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour 
{
    public GameObject target;                 // what the switch object is connected to.
    public int requiredAlpacas = 1;                 // the number of alpacas needed to activate the switch

    private DoorLift targetMover;
    private bool isActivated;
    private string characterObjectTag = "Alpaca";   // tag switch searches for
    private int alpacasPresent;                     // the number of alpacas currently on the switch
    
    
    void Start() 
    {
        alpacasPresent = 0;
        targetMover = target.GetComponent<DoorLift>();
    }
    
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == characterObjectTag) 
        {  
            // as soon as the alpaca hits the trigger, it should lock on to the switch and stay put
            AlpacaMovement alpaca = c.gameObject.GetComponent<AlpacaMovement>();
            //StartCoroutine(alpaca.MoveTowardTarget(gameObject));
            alpacasPresent++;
        
            if (alpacasPresent >= requiredAlpacas)
            {
                isActivated = true;
                targetMover.NotifyActiveStatus(true);
                Debug.Log("tried to call");
            }
        } 
    }
    
    void OnTriggerExit(Collider c)
    {
        if (c.tag == characterObjectTag)
        {
            alpacasPresent--;

            if (alpacasPresent < requiredAlpacas)
            {
                isActivated = false;
                targetMover.NotifyActiveStatus(false);
            }
        }
        

    }

    public bool IsActivated()
    {
        return this.isActivated;
    }
}
