using UnityEngine;
using System.Collections;

public class AlpacaMovement : MonoBehaviour
{
    Transform target;
    NavMeshAgent nav;
    bool isMoving;
    
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        isMoving = false;
    }
    
    void Update()
    {
        if (isMoving)
        {
            float x = this.transform.position.x;
            float z = this.transform.position.z;
            float targetx = target.position.x;
            float targetz = target.position.z;
            
            if (x == targetx && z == targetz)
            {
                nav.SetDestination(this.transform.position);
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
    }
}
