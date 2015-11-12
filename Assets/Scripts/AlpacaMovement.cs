using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class AlpacaMovement : MonoBehaviour
{
    public float wanderRadiusMultiplier;            // used to set the distance the alpaca will move from its origin while wandering
    public float wanderDelay;                       // the length of time between alpaca's random movements
    public float commandSustain;                    // the length of time for which the alpaca will obey the player's most recent command
    public bool isSummoned;                         // whether the alpaca has been summoned to a particular target (vs. wandering)
                                                    
    Transform targetObj;                            // the transform of the target used by the nav mesh agent
    Vector3 targetPos;                              // the current target position; this is an arbitrary position while wandering, tied to a game object when called by the player, for example
    NavMeshAgent nav;                               // the alpaca's nav mesh agent
    Vector3 wanderOrigin;                           // the temporary origin point arount which the alpaca will wander
    private EventInstance alpacaHum;


    // TODO: wander origin should be further from switch after use

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        wanderOrigin = this.transform.position;
        SetRandomDestination();

		isSummoned = false;

        alpacaHum = FMOD_StudioSystem.instance.GetEvent("event:/sfx/alpaca/hum");
    }
    
    void Update()
    {
        Debug.DrawRay(wanderOrigin, Vector3.up, Color.blue);

        float x = this.transform.position.x;
        float z = this.transform.position.z;
        float targetx = targetPos.x;
        float targetz = targetPos.z;

        if (isSummoned)
        {
            // update the target position if the target is a game object, as it may have moved (usually when the target is the player)
            targetPos = targetObj.position;
            nav.SetDestination(targetPos);
        }
        else 
        {
            // if alpaca has arrived at its wander target, pause movement before setting the next target location
            if (x == targetx && z == targetz )
            {
                SetRandomDestination();
            }
        }
    }
    
    public IEnumerator MoveTowardTarget(GameObject obj)
    {


		// set target to the object that called the method
        targetObj = obj.transform;
        targetPos = targetObj.position;
        nav.SetDestination(targetPos);
        isSummoned = true;

        // we don't want the alpacas walking directly into the player, but other targets--switches, etc.--should allow this
        if (targetObj.tag == "Player")
        {
            nav.stoppingDistance = 2.5f;
        }
		else 
		{
			nav.stoppingDistance = 0f;
		}

        // continue following player for a set amount of time before resuming wandering
        yield return new WaitForSeconds(commandSustain + Random.value * 3);

        // reset wander origin so the alpaca doesn't return to a previous origin, which may be very far away
        wanderOrigin = this.transform.position;
        isSummoned = false;
        SetRandomDestination();

		var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
		alpacaHum.set3DAttributes(attributes);
		alpacaHum.start();
        alpacaHum.release();
    }

      // eventually would be good to have some pause between movements, but right now is too buggy
//    IEnumerator DelayWander(float delay)
//    {
//        // stop all movement and set a new target
//        nav.Stop();
//        SetRandomDestination();
//
//        // resume wander after a pause 
//        yield return new WaitForSeconds(delay + Random.value * 3);
//        nav.Resume();
//    }

    void SetRandomDestination()
    {
        targetObj = null;
        targetPos = Random.insideUnitSphere * wanderRadiusMultiplier + wanderOrigin;
        nav.SetDestination(targetPos);
        nav.stoppingDistance = 0f;
    }
}
