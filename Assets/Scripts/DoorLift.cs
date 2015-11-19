using UnityEngine;
using System.Collections;

public class DoorLift : MonoBehaviour {

    public GameObject[] switches;
    public float raiseHeight;
    public int requiredSwitches;

    private Vector3 openPosition;
    private Vector3 closedPosition;
    //public string doorAudio;                // the name (including path) of the FMOD sound effect the target will play 
    
    void Start () {
        openPosition = new Vector3(transform.position.x, transform.position.y + raiseHeight, transform.position.z);
        closedPosition = transform.position;
    }

    void Update()
    {
        int currentActiveSwitches = 0;

        foreach (GameObject s in switches)
        {
            // need to accommodate different types of switches; this solution is really ugly but will work until I have time to fix it
            if (s.GetComponent<Switch>() != null)
            {
                Switch switchAction = s.GetComponent<Switch>();

                if (switchAction.IsActivated())
                {
                    currentActiveSwitches++;
                }
            }
            else if (s.GetComponent<LeverActivation>() != null)
            {
                LeverActivation leverAction = s.GetComponent<LeverActivation>();
                
                if (leverAction.IsActivated())
                {
                    currentActiveSwitches++;
                }
            }
        }

        if (currentActiveSwitches >= requiredSwitches)
        {
            StartCoroutine(RaiseDoor());
        }
//        else 
//        {
//            StartCoroutine(LowerDoor());
//        }
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
        while (transform.position.y > closedPosition.y)
        {
            transform.Translate(0, -0.1f, 0);
            yield return null;
        }
    }


}
