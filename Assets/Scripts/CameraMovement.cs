using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;                   // target the camera focuses on
    public float targetHeight = 1.5f;           // the height, relative to the player, where the camera will point
    public float baseOrbitSpeed = 2f;           // starting speed of the camera's orbit around the player
    public float maxOrbitSpeed = 5;             // top speed of the orbit around the player, after accelerating from baseOrbitSpeed
    public float baseRotateSpeed = .1f;         // starting speed of the camera's rotation around the x axis
    public float maxRotateSpeed = .5f;          // top speed rotation around the x axis, after accelerating from baseRotateSpeed
    public float maxXAngle = 20f;
    public float minXAngle = -15f;
    public float maxDistance = 15f;
    public float minDistance = 2f;
    public float smooth = 6f;


    private float orbitSpeed;                   // current speed of the camera's orbit around the player
    private float rotateSpeed;                  // current speed of the rotation around the x axis
    private float yAngle;                       // current angle on the y axis
    private float defaultXAngle;                // default angle on the x axis, can snap back to this angle using Reset()
    private float xAngle;                       // current angle on the x axis
    private Vector3 defaultOffset;              // starting position of the camera relative to its target
    private Vector3 offset;                     // current position of the camera relative to its target
    
    void Start()
    {
        //set up references
        defaultOffset = target.transform.position - transform.position;
        offset = defaultOffset;
        yAngle = target.transform.eulerAngles.y;
        orbitSpeed = baseOrbitSpeed;
        rotateSpeed = baseRotateSpeed;
        defaultXAngle = target.transform.eulerAngles.x;
        xAngle = defaultXAngle;
    }

    void LateUpdate()
    {
        float h = Input.GetAxis("Horizontal2");
        float v = Input.GetAxis("Vertical2");
        Vector3 lookTarget = new Vector3(target.transform.position.x, target.transform.position.y + targetHeight, target.transform.position.z);

        if (h >= 0.5f || h <= -0.5f )
        {
            yAngle += h * orbitSpeed;
            // accelerate up to top orbit speed
            orbitSpeed = Mathf.Clamp(orbitSpeed * 1.075f, baseOrbitSpeed, maxOrbitSpeed);
        }
        else 
        {
            // reset orbitSpeed for acceleration on next movement
            orbitSpeed = baseOrbitSpeed;
        }

        if (v >= 0.8f || v <= -0.8f )
        {
            xAngle = Mathf.Clamp(xAngle - v * rotateSpeed, minXAngle, maxXAngle);
            // accelerate up to max rotation speed
            rotateSpeed = Mathf.Clamp(rotateSpeed * 1.05f, baseRotateSpeed, maxRotateSpeed);

            // once min or max X angle is hit, continued same-direction v input will zoom in/out
            if (xAngle >= maxXAngle || xAngle <= minXAngle)
            {
                offset = offset - Vector3.forward * v / 20 ;   
            }
        }
        else
        {
            // reset rotateSpeed for acceleration on next movement
            rotateSpeed = baseRotateSpeed;
        }

        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = Vector3.Lerp(transform.position, target.transform.position - (rotation * offset), Time.deltaTime * smooth);

        // reset to the default position relative to the target
        if (Input.GetButtonDown("CamReset"))
        {
            yAngle = target.transform.eulerAngles.y;
            xAngle = defaultXAngle;
            offset = defaultOffset;
        }
        
        // aim the camera just above the target game object so that the target is not directly centered in the screen

        transform.LookAt(lookTarget);
        
        // pin the camera to the wall if it bumps into one, rather than having it go through
        Ray backRay = new Ray(lookTarget,this.transform.position - lookTarget);
        RaycastHit wallHit;
        if (Physics.Raycast(backRay, out wallHit, Vector3.Distance(this.transform.position, lookTarget)))
        {
            transform.position = wallHit.point;
        }
    }
}
