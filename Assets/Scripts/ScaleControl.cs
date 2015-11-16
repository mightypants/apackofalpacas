using UnityEngine;
using System.Collections;

public class ScaleControl : MonoBehaviour
{

    public GameObject scaleEnd1;			// one of the ends of the scale
    public GameObject scaleEnd2;			// the other one
    public int moveDistance = 3;			// how far each scale end moves from balanced to its extremes

    private ScaleMovement scaleEnd1Mov;		// the scale end's movement/weight monitoring script
    private ScaleMovement scaleEnd2Mov;		// the other one
    private int lastWeightDiff;				// the difference in weight for the previous frame

    void Start()
    {
        scaleEnd1Mov = scaleEnd1.GetComponent<ScaleMovement>();
        scaleEnd2Mov = scaleEnd2.GetComponent<ScaleMovement>();
        lastWeightDiff = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        // find the difference in weight applied to each of the scale ends
        int end1Weight = scaleEnd1Mov.GetWeight();
        int end2Weight = scaleEnd2Mov.GetWeight();
        int newWeightDiff = end1Weight - end2Weight;

        // depending whether the weight difference has changed since the previous frame, instruct the scale ends to adjust their positions accordingly
        if (newWeightDiff != lastWeightDiff)
        {
            switch (newWeightDiff)
            {
            case -1:
                StartCoroutine(scaleEnd1Mov.AdjustPosition(moveDistance));
                StartCoroutine(scaleEnd2Mov.AdjustPosition(-moveDistance));
                break;
            case 0:
                StartCoroutine(scaleEnd1Mov.AdjustPosition(0));
                StartCoroutine(scaleEnd2Mov.AdjustPosition(0));
                break;
            case 1:
                StartCoroutine(scaleEnd1Mov.AdjustPosition(-moveDistance));
                StartCoroutine(scaleEnd2Mov.AdjustPosition(moveDistance));
                break;
            }
        }

        // update the value of lastWeightDiff for the next frame
        lastWeightDiff = newWeightDiff;
    }
}
