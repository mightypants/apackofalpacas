using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;           // target the camera focuses on
    public float lookAngleOffset = 1;   // allows the target to be off-center in the camera's view
    public float rotateSpeed = 5;       // speed of the camera's rotation
	public float zoomSpeed = 5;

    private Vector3 offset;             // distance between the camera and target
	//private Vector3 lastDist;

    void Start()
    {
        //set up references
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
		float h = Input.GetAxis("Horizontal2");
		float v = Input.GetAxis("Vertical2");
		float angle = transform.eulerAngles.y;
        
		if (h >= 0.5f || h <= -0.5f )
		{
			angle += h * rotateSpeed;
		}

		//lastDist += v * Vector3.forward * zoomSpeed;

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * offset);
        
        // aim the camera just above the target game object so that the target is not directly centered in the screen
        Vector3 lookTarget = new Vector3(target.transform.position.x, target.transform.position.y + lookAngleOffset, target.transform.position.z);
        transform.LookAt(lookTarget);
        
        // pin the camera to the wall if it bumps into one, rather than having it go through
        Ray wallLookRay = new Ray(lookTarget,this.transform.position - lookTarget);
        RaycastHit wallHit;
        if (Physics.Raycast(wallLookRay, out wallHit, Vector3.Distance(this.transform.position, lookTarget)))
        {
        	transform.position = wallHit.point;
        }
    }
}
