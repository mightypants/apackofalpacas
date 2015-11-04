using UnityEngine;
using System.Collections.Generic;
using FMOD.Studio;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10;
    public float turningSpeed = 60;
    public float jumpForce = 5;
    public float fluteReach = 100;
    public Transform cameraTransform;
    public float groundCheckDistance;
    
    private Rigidbody playerRigidBody;
    private FMOD.Studio.EventInstance fluteCall1; 
    private bool isGrounded;
    
    void Start()
    {
        //set up references
        playerRigidBody = GetComponent<Rigidbody>();
        fluteCall1 = FMOD_StudioSystem.instance.GetEvent("event:/sfx/player/flute1"); 
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
        float h = Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime;
        float v = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;
        CheckGroundStatus();
        Move(h, v);
    }
    
    
    void Move(float h, float v)
    {

        Vector3 input = new Vector3(h, 0.0f, v).normalized;
        Debug.DrawRay(transform.position, input * 3, Color.blue);


        //transform.rotation = Quaternion.LookRotation(input);
        Vector3 forward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = h * cameraTransform.right +  v * forward;
   
        Debug.Log(movement);

    
        // jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRigidBody.AddForce(Vector3.up * jumpForce);
        }
    
        transform.Translate(movement);

            
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
    
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
    
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * .3f));
    
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}

