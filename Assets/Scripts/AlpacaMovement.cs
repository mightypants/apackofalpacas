using UnityEngine;
using System.Collections;

public class AlpacaMovement : MonoBehaviour
{
    public float wanderRadiusMultiplier;    // used to set the distance the alpaca will move from its origin while wandering
    public float wanderDelay;               // the length of time between alpaca's random movements
    public float commandSustain;            // the length of time for which the alpaca will obey the player's most recent command

    Transform targetObj;                    // the transform of the target used by the nav mesh agent
    Vector3 targetPos;                      // the current target position; this is an arbitrary position while wandering, tied to a game object when called by the player, for example
    NavMeshAgent nav;                       // the alpaca's nav mesh agent
    Vector3 wanderOrigin;                   // the temporary origin point arount which the alpaca will wander
    float x;
    float z;


    // when wandering, alpaca should stay within a certain radius, but this area should be reset after being moved by player

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        x = this.transform.position.x;
        z = this.transform.position.z;

        wanderOrigin = this.transform.position;
        SetRandomDestination();
    }
    
    void Update()
    {
        Debug.DrawRay(wanderOrigin, Vector3.up, Color.blue);

        x = this.transform.position.x;
        z = this.transform.position.z;
        float targetx = targetPos.x;
        float targetz = targetPos.z;

        // if alpaca is within 1 unit of target, pause movement before setting the next target location
        if (x <= targetx + 1 && x >= targetx -1 && z <= targetz + 1 && z >= targetz -1 )
        {
            StartCoroutine(DelayWander(wanderDelay));
        }
        else if (targetObj != null)
        {
            // update the target position if the target is game object, as it may have moved (usually when the target is the player)
            targetPos = targetObj.position;
            nav.SetDestination(targetPos);
        }
    }
    
    public IEnumerator MoveTowardTarget(GameObject obj)
    {
        // set target to the object that called the method
        targetObj = obj.transform;
        targetPos = targetObj.position;
        nav.SetDestination(targetPos);
        nav.Resume();

        // continue following player for a set amount of time before resuming wandering
        yield return new WaitForSeconds(commandSustain);
        wanderOrigin = this.transform.position;
        StartCoroutine(DelayWander(wanderDelay));
    }

    IEnumerator DelayWander(float delay)
    {
        nav.Stop();
        SetRandomDestination();
        yield return new WaitForSeconds(delay);
        nav.Resume();
    }

    void SetRandomDestination()
    {
        targetObj = null;
        targetPos = Random.insideUnitSphere * wanderRadiusMultiplier + wanderOrigin;
        nav.SetDestination(targetPos);
    }
}
