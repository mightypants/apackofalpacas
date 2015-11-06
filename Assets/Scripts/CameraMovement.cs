using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;           // target the camera focuses on
    public float lookAngleOffset = 1;   // allows the target to be off-center in the camera's view
    public float rotateSpeed = 5;       // speed of the camera's rotation

    private Vector3 offset;             // distance between the camera and target

    void Start ()
    {
        //set up references
        offset = target.transform.position - transform.position;
    }

    void LateUpdate ()
    {
        float currentAngle = transform.eulerAngles.y;
        float horizontal = Input.GetAxis("Horizontal2") * rotateSpeed;
        Quaternion rotation = Quaternion.Euler(0, currentAngle + horizontal, 0);
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
