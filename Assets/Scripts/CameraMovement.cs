using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;           // target the camera focuses on
    public float lookAngleOffset = 1;   // allows the target to be off-center in the camera's view
    public float rotateSpeed = 5;       // speed of the camera's rotation
    Vector3 offset;                     // distance between the camera and target


    void Start ()
    {
        //set up references
        offset = target.transform.position - transform.position;
    }

    void LateUpdate ()
    {
        // set the camera angle between its current angle and the angle of the target
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime);
        
        // add the mouse x axis to the angle so the player can adjust the rotation if desired
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        Quaternion rotation = Quaternion.Euler(0, angle - horizontal, 0);
        transform.position = target.transform.position - (rotation * offset);
        
        // aim the camera just above the target game object so that the target is not directly centered in the screen
        Vector3 lookTarget = new Vector3(target.transform.position.x, target.transform.position.y + lookAngleOffset, target.transform.position.z);
        transform.LookAt(lookTarget);
    }
}
