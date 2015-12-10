using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class AudioManager : MonoBehaviour 
{
    private EventInstance ambience;
    private ParameterInstance windSpeedParam;
    private ParameterInstance windIntensityParam;

	void Start() 
    {
        ambience = FMOD_StudioSystem.instance.GetEvent("event:/sfx/environment/ambience");
        ambience.getParameter("windSpeed", out windSpeedParam);
        ambience.getParameter("windIntensity", out windIntensityParam);
        windSpeedParam.setValue(0);
        windIntensityParam.setValue(0);
        ambience.start();
        StartCoroutine(VaryWindParam(windSpeedParam));
        StartCoroutine(VaryWindParam(windIntensityParam));
	}

    IEnumerator VaryWindParam(ParameterInstance param)
    {
        float currValue;
        param.getValue(out currValue);

        // random range is based on the current settings in FMOD
        float newValue = Random.Range(0, 10);
        float amountOfChange = Mathf.Abs(currValue - newValue);

        // a larger change in speed should be applied over a longer period of time
        float changeTime = amountOfChange * 3;
        float elapsedTime = 0;

        while (elapsedTime < changeTime)
        {
            currValue = Mathf.Lerp(currValue, newValue, (elapsedTime / changeTime));
            param.setValue(currValue);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        StartCoroutine(VaryWindParam(param));
    }
}
