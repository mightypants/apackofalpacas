using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class AudioManager : MonoBehaviour 
{
    private EventInstance ambience;
    private ParameterInstance windSpeedParam;
    private float currWindSpeed;


	void Start() 
    {
        ambience = FMOD_StudioSystem.instance.GetEvent("event:/sfx/environment/ambience");
        ambience.getParameter("windSpeed", out windSpeedParam);
        windSpeedParam.setValue(0);
        ambience.start();
        Invoke("DelaySpeedVariation", 3);
	}
	
	void Update()
    {
	    
	}

    IEnumerator VaryWindSpeed()
    {
        float newWindSpeed = Random.Range(0, 10);
        float speedChange = Mathf.Abs(currWindSpeed - newWindSpeed);
        float changeTime = speedChange * 3;
        float delay = Random.Range(1, 2);
        float elapsedTime = 0;

        Debug.Log("new speed: " + newWindSpeed);
        Debug.Log("speed change: " + speedChange);
        Debug.Log("changeTime: " + changeTime);
        Debug.Log("delay: " + delay);

        while (elapsedTime < changeTime)
        {
            currWindSpeed = Mathf.Lerp(currWindSpeed, newWindSpeed, (elapsedTime / changeTime));
            windSpeedParam.setValue(currWindSpeed);
            elapsedTime += Time.deltaTime;

            Debug.Log("current " + currWindSpeed);
            yield return null;
        }

        Invoke("DelaySpeedVariation", delay);
    }

    void DelaySpeedVariation()
    {
        StartCoroutine(VaryWindSpeed());
        Debug.Log("vary called");
    }
}
