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
        // random range is based on the current settings in FMOD
        float newWindSpeed = Random.Range(0, 10);
        float speedChange = Mathf.Abs(currWindSpeed - newWindSpeed);

        // a larger change in speed should be applied over a longer period of time
        float changeTime = speedChange * 3;
        float delay = Random.Range(1, 2);
        float elapsedTime = 0;

        while (elapsedTime < changeTime)
        {
            currWindSpeed = Mathf.Lerp(currWindSpeed, newWindSpeed, (elapsedTime / changeTime));
            windSpeedParam.setValue(currWindSpeed);
            elapsedTime += Time.deltaTime;

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
