using UnityEngine;
using FMOD.Studio;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed = 10;
	public float turningSpeed = 60;
	public float jumpForce = 5;

	private Rigidbody playerRigidBody;
	private bool isGrounded;
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
		// play flute sound effect if key just pressed, update position if held, stop if released
		if (Input.GetKeyDown(KeyCode.Q))
		{
			var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
			fluteCall1.set3DAttributes(attributes);
			fluteCall1.start();
		}
		else if (Input.GetKey(KeyCode.Q))
		{
			var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
			fluteCall1.set3DAttributes(attributes);;
		}
		else if (Input.GetKeyUp(KeyCode.Q))
		{
			fluteCall1.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	void FixedUpdate()
	{
		// get input and call the Move method
		float h = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
		float v = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
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

}
