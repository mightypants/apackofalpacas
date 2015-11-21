using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;                   // target the camera focuses on
    public float targetHeight = 1.5f;           // the height, relative to the player, where the camera will point

    public float baseOrbitSpeed = 2f;                // speed of the camera's orbit around the player
    public float maxOrbitSpeed = 5;
    public float baseRotateSpeed = .1f;             // the rate of the camera's rotation around the x axis
    public float maxRotateSpeed = .5f;
    public float maxXAngle = 15f;
    public float minXAngle = -15f;

    private float orbitSpeed;                // speed of the camera's orbit around the player
    private float rotateSpeed;
    private float yAngle;
    private float defaultXAngle;
    private float xAngle;
    private Vector3 offset;                     // vector describing the space between the camera and target


    void Start()
    {
        //set up references
        offset = target.transform.position - transform.position;
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

        if (h >= 0.5f || h <= -0.5f )
        {
            yAngle += h * orbitSpeed;
            orbitSpeed = Mathf.Clamp(orbitSpeed * 1.075f, baseOrbitSpeed, maxOrbitSpeed);
        }
        else 
        {
            orbitSpeed = baseOrbitSpeed;
        }

        if (v >= 0.8f || v <= -0.8f )
        {
            xAngle = Mathf.Clamp(xAngle + v * rotateSpeed, minXAngle, maxXAngle);
            rotateSpeed = Mathf.Clamp(rotateSpeed * 1.05f, baseRotateSpeed, maxRotateSpeed);
        }
        else
        {
            rotateSpeed = baseRotateSpeed;
        }

        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = target.transform.position - (rotation * offset);

        if (Input.GetButtonDown("CamReset"))
        {
            yAngle = target.transform.eulerAngles.y;
            xAngle = defaultXAngle;
            //StartCoroutine(Reset());;
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

    IEnumerator Reset()
    {
        float targetYAngle = target.transform.eulerAngles.y;
        //float targetXAngle = defaultXAngle;

        Debug.Log("t: " + targetYAngle);
        Debug.Log("c: " + yAngle);

        while (yAngle > targetYAngle)
        {
            //yAngle = Mathf.Lerp(yAngle, targetYAngle, Time.deltaTime * 3);
            yAngle -= 2f;
            //xAngle = Mathf.Lerp(yAngle, targetXAngle, Time.deltaTime * 1f);
            yield return new WaitForEndOfFrame();
        }


    }
}
