using UnityEngine;
using System.Collections;

public class ScaleMovement : MonoBehaviour {

    public float speed = 3;

	private int alpacasPresent;
    private Vector3 balancedPosition;
    private Vector3 targetPosition;

	void Start() {
        balancedPosition = this.transform.position;

	}
	
	void Update() {
        //Debug.Log(gameObject.name + "alpacasPresent: " + alpacasPresent);
	}

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("enter");
        if (c.tag == "Alpaca") 
        {  
            // as soon as the alpaca hits the trigger, it should lock on to the switch and stay put
//            AlpacaMovement alpaca = c.gameObject.GetComponent<AlpacaMovement>();
//            StartCoroutine(alpaca.MoveTowardTarget(gameObject));
            alpacasPresent++;
        }
    }

	void OnTriggerExit(Collider c)
	{
        Debug.Log("exit");
        if (c.tag == "Alpaca") 
        {  
            alpacasPresent--;
        }
	}

	public int GetWeight()
	{
		return alpacasPresent;
	}

    public IEnumerator AdjustPosition(int movement)
    {

        targetPosition = new Vector3(balancedPosition.x, balancedPosition.y + movement, balancedPosition.z);

        while (targetPosition != this.transform.position)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }      
    }


}
