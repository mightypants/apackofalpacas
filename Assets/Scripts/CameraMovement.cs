using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;                   // target the camera focuses on
    public float targetHeight = 1.5f;           // the height, relative to the player, where the camera will point
    public float defaultTargetHeight = 1.5f;    // allows the player to snap back to the original height after moving around
    public float orbitSpeed = 5;                // speed of the camera's orbit around the player
    public float rotateSpeed = .5f;               // the rate of the camera's rotation around the x axis

    private float angle;
    private float defaultAngle;
    private Vector3 offset;                     // vector describing the space between the camera and target


    void Start()
    {
        //set up references
        offset = target.transform.position - transform.position;
        angle = target.transform.eulerAngles.y;
        defaultAngle = target.transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (Input.GetButtonDown("CamReset"))
        {
            targetHeight = defaultTargetHeight;
            angle = defaultAngle;
        }

        float h = Input.GetAxis("Horizontal2");
        float v = Input.GetAxis("Vertical2");
        
        if (h >= 0.5f || h <= -0.5f )
        {
            angle += h * orbitSpeed;
        }

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * offset);


        if (v >= 0.5f || v <= -0.5f )
        {
            targetHeight += v * rotateSpeed;
        }
        
        // aim the camera just above the target game object so that the target is not directly centered in the screen
        Vector3 lookTarget = new Vector3(target.transform.position.x, target.transform.position.y + targetHeight, target.transform.position.z);
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
