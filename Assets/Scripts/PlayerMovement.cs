using UnityEngine;
using System.Collections;
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
    public GameObject summonTarget;
    
    private CharacterController characterController;
    private ParticleSystem characterParticles;
    private EventInstance fluteCall1;
    private float vertSpeed;

    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterParticles = GameObject.Find("Player/Flute Radius").GetComponent<ParticleSystem>();
        fluteCall1 = FMOD_StudioSystem.instance.GetEvent("event:/sfx/player/flute1");
    }
    
    
    void Update()
    {
        if (Input.GetButtonDown("Flute2"))
        {
            // start the flute sound effect if key just pressed
            StartCoroutine(PlayFlute(fluteCall1));
            SummonAlpaca(true);
        }

        if (Input.GetButtonDown("Flute1"))
        {
            // start the flute sound effect if key just pressed
            StartCoroutine(PlayFlute(fluteCall1));
            SummonAlpaca(false);
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
        
        if (characterController.isGrounded)
        {
            // jump
            if (Input.GetButtonDown("Jump"))
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
    
    IEnumerator PlayFlute(EventInstance fluteAudio)
    {
        // set the position of the sound effect to be the player's position
        var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
        fluteAudio.set3DAttributes(attributes);
        fluteAudio.start();

        characterParticles.Play(true);

        float start = Time.time;
        float time = start;

        while (time <= start + 5f)
        {
            // update the position of the sound as the player moves
            attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
            fluteAudio.set3DAttributes(attributes);

            time += Time.deltaTime;
            yield return null;
        }

        fluteAudio.release();
    }

    void SummonAlpaca(bool locationSpecified)
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, fluteReach);

        foreach (Collider c in hitColliders) {

            AlpacaMovement alpacaMovement = c.gameObject.GetComponent<AlpacaMovement>();

            if (c.tag == "Alpaca") {
                // instantiate a marker for the new alpaca destination
                Vector3 destinationPosition = transform.position;
                GameObject destinationObj = locationSpecified ? (GameObject) Instantiate(summonTarget, destinationPosition, Quaternion.identity) : gameObject;

                // call the alpaca to the destination
                alpacaMovement.MoveTowardTarget(destinationObj);
            }
        }
    }
}

