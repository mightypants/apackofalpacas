using UnityEngine;
using System.Collections;

public class DoorLift : MonoBehaviour {

    public GameObject[] switches;
    public float raiseHeight;
    public int requiredSwitches;

    private Vector3 openPosition;
    private Vector3 closedPosition;

    
    void Start () {
        openPosition = new Vector3(transform.position.x, transform.position.y + raiseHeight, transform.position.z);
        closedPosition = transform.position;
    }

    void Update()
    {
        int currentActiveSwitches = 0;

        foreach (GameObject s in switches)
        {
            Switch switchAction = s.GetComponent<Switch>();

            if (switchAction.IsActivated())
            {
                currentActiveSwitches++;
            }
        }

        if (currentActiveSwitches >= requiredSwitches)
        {
            StartCoroutine(RaiseDoor());
        }
        else 
        {
            StartCoroutine(LowerDoor());
        }
    }

    public IEnumerator RaiseDoor()
    {
        while (transform.position.y < openPosition.y)
        {
            transform.Translate(0, .1f, 0);
            yield return null;
        }
    }

    public IEnumerator LowerDoor()
    {
        while (transform.position.y > openPosition.y)
        {
            transform.Translate(0, -0.1f, 0);
            yield return null;
        }
    }


}
