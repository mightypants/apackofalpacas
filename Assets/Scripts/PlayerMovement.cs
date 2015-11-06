using UnityEngine;
using System.Collections.Generic;
using FMOD.Studio;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10;
    public float turnSpeed = 60;
    public float jumpSpeed = 5;
    public float fluteReach = 100;
    public Transform cameraTransform;
    public float gravity = 9.8f;
    
    private CharacterController characterController;
    private FMOD.Studio.EventInstance fluteCall1; 
    private float vertSpeed;
    
    void Start()
    {
        //set up references
        characterController = GetComponent<CharacterController>();
        fluteCall1 = FMOD_StudioSystem.instance.GetEvent("event:/sfx/player/flute1"); 
    }
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Flute1"))
        {
            // start the flute sound effect if key just pressed
            PlayFlute(true);
        }
        else if (Input.GetKey(KeyCode.Q) || Input.GetButton("Flute1"))
        {
            // update the position of the sound effect if key is held
            PlayFlute(false);
        }
        else if (Input.GetKeyUp(KeyCode.Q) || Input.GetButtonUp("Flute1"))
        {
            // stop sound effect if released
            fluteCall1.stop(STOP_MODE.ALLOWFADEOUT);
        }
        
        // get input and call the Move and Turn methods
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        
        if (h != 0 || v != 0)
        {
        	Turn(h, v);
        }
    }
    
    void Move(float h, float v)
    {
        // player movement is relative to the camera
        Vector3 forward = cameraTransform.forward.normalized;
        Vector3 movement = (h * cameraTransform.right +  v * forward).normalized;
        
        //vertSpeed = 0;
        
        if (characterController.isGrounded)
        {
            // jump
            if (Input.GetAxis("Jump") > 0)
            {
            	vertSpeed = jumpSpeed;
            }
        }
        
        vertSpeed -= gravity * Time.deltaTime;
        movement.y = vertSpeed;
        characterController.Move(movement * movementSpeed * Time.deltaTime);
    
    }
    
    void Turn(float h, float v)
    {
    
    	Vector3 relativePos = cameraTransform.TransformDirection(new Vector3(h, 0f, v));
    	relativePos.y = 0.0f;
    	Quaternion rotation = Quaternion.LookRotation(relativePos);
    	transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
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
                AlpacaMovement alpaca = c.gameObject.GetComponent<AlpacaMovement>();
                alpaca.MoveTowardTarget(gameObject);
            }
        }
    }
}

