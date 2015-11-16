using UnityEngine;
using System.Collections;

public class ScaleControl : MonoBehaviour
{

	public GameObject scaleEnd1;
    public GameObject scaleEnd2;
    public int moveDistance = 3;

    private ScaleMovement scaleEnd1Mov;
    private ScaleMovement scaleEnd2Mov;
    private int lastWeightDiff;

	void Start()
    {
        scaleEnd1Mov = scaleEnd1.GetComponent<ScaleMovement>();
        scaleEnd2Mov = scaleEnd2.GetComponent<ScaleMovement>();
        lastWeightDiff = 0;
	}
	
	// Update is called once per frame
	void Update()
    {
        int end1Weight = scaleEnd1Mov.GetWeight();
        int end2Weight = scaleEnd2Mov.GetWeight();



        int newWeightDiff = end1Weight - end2Weight;

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

        lastWeightDiff = newWeightDiff;
	}

    void AdjustScale()
    {

    }
}
