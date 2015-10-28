using UnityEngine;
using System.Collections.Generic;
using FMOD.Studio;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10;
    public float turningSpeed = 60;
    public float jumpForce = 5;
    public float fluteReach = 100;

    private Rigidbody playerRigidBody;
    private FMOD.Studio.EventInstance fluteCall1; 
    private FMOD.Studio.ParameterInstance sustainingFlute;

    void Start()
    {
        //set up references
        playerRigidBody = GetComponent<Rigidbody>();
        fluteCall1 = FMOD_StudioSystem.instance.GetEvent("event:/sfx/player/flute1"); 
        fluteCall1.getParameter("sustaining", out sustainingFlute);
        sustainingFlute.setValue(0);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // start the flute sound effect if key just pressed
            PlayFlute(true);

        }
        else if (Input.GetKey(KeyCode.Q))
        {
            // update the position of the sound effect if key is held
            PlayFlute(false);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            // stop sound effect if released
            fluteCall1.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    void FixedUpdate()
    {
        // get input and call the Move method
        float h = Input.GetAxisRaw("Horizontal") * turningSpeed * Time.deltaTime;
        float v = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;
        Move(h, v);
    }


    void Move(float h, float v)
    {
        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidBody.AddForce(Vector3.up * jumpForce);
        }
        
        //move
        transform.Rotate(0, h, 0);
        transform.Translate(0, 0, v);
    }

    void PlayFlute(bool keyDown)
    {
        // set the position of the sound effect to be the player's position
        var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
        fluteCall1.set3DAttributes(attributes);

        if (keyDown)
        {
            fluteCall1.start();
        }

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, fluteReach);
        
        foreach(Collider c in hitColliders){
            
            if (c.tag == "Alpaca")
            {
                // call alpaca's method
            }
        }
    }
}

