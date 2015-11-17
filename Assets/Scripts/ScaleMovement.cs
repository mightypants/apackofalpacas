using UnityEngine;
using System.Collections;

public class ScaleMovement : MonoBehaviour {

    public float speed = 3;
    
    private int alpacasPresent;
    private Vector3 balancedPosition;
    private Vector3 targetPosition;
    private ArrayList alpacas;

    void Start() {
        balancedPosition = this.transform.position;
        alpacas = new ArrayList();
    }
    
    void Update() {

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
        foreach (GameObject a in alpacas)
        {
            AlpacaMovement alpacaMovement = a.GetComponent<AlpacaMovement>();
            alpacaMovement.ToggleNavAgent();
            Debug.Log(a.name);
        }

        targetPosition = new Vector3(balancedPosition.x, balancedPosition.y + movement, balancedPosition.z);

        while (this.transform.position.y > targetPosition.y + .1f)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }

        foreach (GameObject a in alpacas)
        {
            AlpacaMovement alpacaMovement = a.GetComponent<AlpacaMovement>();
            alpacaMovement.ToggleNavAgent();
        }
    }


}
