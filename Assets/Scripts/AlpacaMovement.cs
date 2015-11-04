using UnityEngine;
using System.Collections;

public class AlpacaMovement : MonoBehaviour
{
    Transform target;
    NavMeshAgent nav;
    bool isMoving;
    float x;
    float z;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        isMoving = false;
        x = this.transform.position.x;
        z = this.transform.position.z;
    }
    
    void Update()
    {
        if (isMoving)
        {
             x = this.transform.position.x;
             z = this.transform.position.z;
            float targetx = target.position.x;
            float targetz = target.position.z;
            
            if (x <= targetx + 1 && x >= targetx -1 && z <= targetz + 1 && z >= targetz -1 )
            {
                nav.Stop();
                isMoving = false;
            }    
            else
            {
                nav.SetDestination(target.position);
            }
        }
    }
    
    public void MoveTowardTarget(GameObject obj)
    {
        target = obj.transform;
        isMoving = true;
        nav.Resume();
    }
}
