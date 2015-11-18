using UnityEngine;
using System.Collections;

public class ScaleMovement : MonoBehaviour {

    public float speed = 3;
    public float balancedHeight;
    
    private int alpacasPresent;
    private Vector3 balancedPosition;
    private Vector3 targetPosition;
    private ArrayList alpacas;

    void Start() {
        balancedPosition = new Vector3(this.transform.position.x, balancedHeight, this.transform.position.z);
        alpacas = new ArrayList();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Alpaca") 
        {  
            alpacasPresent++;
            alpacas.Add(c.gameObject);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Alpaca") 
        {  
            alpacasPresent--;
            alpacas.Remove(c.gameObject);
        }
    }

    public int GetWeight()
    {
        return alpacasPresent;
    }

    public IEnumerator AdjustPosition(int movement)
    {
        if (alpacas.Count > 0)
        {
            foreach (GameObject a in alpacas)
            {
                AlpacaMovement alpacaMovement = a.GetComponent<AlpacaMovement>();
                alpacaMovement.ToggleNavAgent();
                alpacaMovement.DisableSummon();
            }
        }

        targetPosition = new Vector3(balancedPosition.x, balancedPosition.y + movement, balancedPosition.z);
        bool movingUp = targetPosition.y - this.transform.position.y > 0 ? true : false;

        // without this offset, the scale never gets to the target position exactly so the alpaca(s) never gets it's agent reenabled
        float targetOffset;
        targetOffset = 0.1f;

        if (movingUp)
        {
            while (this.transform.position.y < targetPosition.y - targetOffset)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * speed);
                yield return null;
            }
        }
        else
        {
            while (this.transform.position.y > targetPosition.y + targetOffset)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * speed);
                yield return null;
            }
        }

        foreach (GameObject a in alpacas)
        {
            AlpacaMovement alpacaMovement = a.GetComponent<AlpacaMovement>();
            alpacaMovement.ToggleNavAgent();
            alpacaMovement.EnableSummon();
        }
    }
}
