using UnityEngine;
using System.Collections;
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
    private ParticleSystem characterParticles;
    private FMOD.Studio.EventInstance fluteCall1;
    private float fluteCall1Length = 3.0f;
    private float vertSpeed;

    
    void Start()
    {
        //set up references
        characterController = GetComponent<CharacterController>();
        characterParticles = GameObject.Find("Player/Flute Radius").GetComponent<ParticleSystem>();
        fluteCall1 = FMOD_StudioSystem.instance.GetEvent("event:/sfx/player/flute1"); 
    }
    
    
    void Update()
    {
        if (Input.GetButtonDown("Flute1"))
        {
            // start the flute sound effect if key just pressed
            StartCoroutine(PlayFlute());
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
    
    IEnumerator PlayFlute()
    {
        // set the position of the sound effect to be the player's position
        var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
        fluteCall1.set3DAttributes(attributes);
        fluteCall1.start();

        characterParticles.Play(true);

        float start = Time.time;
        float time = start;

        while (time <= start + fluteCall1Length)
        {
            // update the position of the sound as the player moves
            attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
            fluteCall1.set3DAttributes(attributes);

            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, fluteReach);
            
            foreach(Collider c in hitColliders)
                {
                
                if (c.tag == "Alpaca")
                {
                    AlpacaMovement alpaca = c.gameObject.GetComponent<AlpacaMovement>();
                    StartCoroutine(alpaca.MoveTowardTarget(gameObject));
                }
            }

            time += Time.deltaTime;
            yield return null;
        }
    }
}

