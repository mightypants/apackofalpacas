using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class DoorLift : MonoBehaviour {

    public float raiseHeight;
    public int requiredSwitches;
    public float speed;

    private int currentActiveSwitches;
    private int prevActiveSwitches;
    private Vector3 openPosition;
    private Vector3 closedPosition;
    private EventInstance doorSlideAudio;
    
    void Start () {
        currentActiveSwitches = 0;
        prevActiveSwitches = 0;
        openPosition = new Vector3(transform.position.x, transform.position.y + raiseHeight, transform.position.z);
        closedPosition = transform.position;
        doorSlideAudio = FMOD_StudioSystem.instance.GetEvent("event:/sfx/environment/puzzlePiece/slidingStoneDoor");
        var attributes = FMOD.Studio.UnityUtil.to3DAttributes(this.transform.position);
        doorSlideAudio.set3DAttributes(attributes);
    }

    void Update()
    {
        if (currentActiveSwitches != prevActiveSwitches && currentActiveSwitches >= requiredSwitches)
        {
            StartCoroutine(RaiseDoor());
            prevActiveSwitches = currentActiveSwitches;
        }
//        else 
//        {
//            StartCoroutine(LowerDoor());
//        }
    }

    public IEnumerator RaiseDoor()
    {
        yield return new WaitForSeconds(0.5f);

        doorSlideAudio.start();

        while (transform.position.y < openPosition.y - .2f)
        {
            transform.position = Vector3.Lerp(this.transform.position, openPosition, Time.deltaTime * speed);
            yield return null;
        }

        doorSlideAudio.release();
        doorSlideAudio.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public IEnumerator LowerDoor()
    {
        while (transform.position.y > closedPosition.y)
        {
            transform.position = Vector3.Lerp(this.transform.position, closedPosition, Time.deltaTime * speed);
            yield return null;
        }
    }

    public void NotifyActiveStatus(bool active)
    {
        if (active)
        {
            prevActiveSwitches = currentActiveSwitches;
            currentActiveSwitches++;
        }
        else if (!active)
        {
            prevActiveSwitches = currentActiveSwitches;
            currentActiveSwitches--;
        }
    }
}
