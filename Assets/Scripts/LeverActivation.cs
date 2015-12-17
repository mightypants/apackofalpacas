using UnityEngine;
using System.Collections;

public class LeverActivation : MonoBehaviour 
{
    public GameObject target;                 // what the switch object is connected to.
    public float rotationOffset = 45;
    public float positionOffset = .35f;

    private DoorLift targetMover;
    private Vector3 activePosition;
    private Quaternion activeRotation;
    private bool isActivated;
    
    void Start() 
    {
        targetMover = target.GetComponent<DoorLift>();
        isActivated = false;
        activePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + positionOffset);
        activeRotation = Quaternion.Euler(transform.rotation.x + rotationOffset, transform.rotation.y, transform.rotation.z);
    }
    
    void OnTriggerStay(Collider c)
    {
        if (c.tag == "Player") 
        {  
            if (Input.GetButton("Activate") && !isActivated)
            {
                isActivated = true;
                targetMover.NotifyActiveStatus(true);
				Debug.Log("lever active");
                transform.position = activePosition;
                transform.rotation = activeRotation;
                FMOD_StudioSystem.instance.PlayOneShot("event:/sfx/environment/puzzlePiece/pressureSwitch", transform.position);
            }
        } 
    }
    
    public bool IsActivated()
    {
        return this.isActivated;
    }


}
