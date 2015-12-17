using UnityEngine;
using System.Collections;
using FMOD.Studio;

public class AlpacaMovement : MonoBehaviour
{
    public GameObject targetObj;                    // defaults to the player, but can change to a player-defined destination

    private Vector3 targetPos;                      // the current target position; either the player's position or a player-defined target
    private NavMeshAgent nav;                       // the alpaca's nav mesh agent
    private ParticleSystem alpacaParticles;         // the alpaca's particle effect
    private EventInstance alpacaHum;                // audio event for the alpaca hum
    private bool isSummonable;                      // used to prevent the alpaca from being summoned when unable to move (such as when on a moving scale piece)
    private bool isLockedToTarget;                  // locked to a player-defined target

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        alpacaParticles = GetComponentInChildren<ParticleSystem>();
        isSummonable = true;
        isLockedToTarget = false;

        if (targetObj != null)
        {
            targetPos = targetObj.transform.position;
            nav.SetDestination(targetPos);
        }
        else 
        {
            targetPos = transform.position;
        }

        alpacaHum = FMOD_StudioSystem.instance.GetEvent("event:/sfx/alpaca/hum");
    }
    
    void Update()
    {
        // update the target position to follow the player's movement
        if (targetObj != null)
        {
            if (targetObj.tag == "Player")
            {
                targetPos = targetObj.transform.position;
                nav.SetDestination(targetPos);
            }
        }
    }
    
    public void MoveTowardTarget(GameObject obj)
    {
        // destroy previous destination target (i.e. non-player) if it exists
        if (targetObj != null)
        {
            if (isLockedToTarget)
            {
                Destroy(targetObj, 1f);
                targetObj = null;
            }
        }

        if (obj.tag == "Player")
        {
            nav.stoppingDistance = 2.5f;
            isLockedToTarget = false;
        }
        else 
        {
            nav.stoppingDistance = 0f;
            isLockedToTarget = true;
            alpacaParticles.Play(true);
        }

        // set target to the destination object passed by the player
        nav.enabled = true;
        targetObj = obj;
        targetPos = targetObj.transform.position;
        nav.SetDestination(targetPos);

        // set up and play hum sound
        var attributes = FMOD.Studio.UnityUtil.to3DAttributes(transform.position);
        alpacaHum.set3DAttributes(attributes);
        alpacaHum.start();
    }

    public void ToggleNavAgent()
    {
        nav.enabled = !nav.enabled;
    }

    public void DisableSummon()
    {
        isSummonable = false;
    }

    public void EnableSummon()
    {
        isSummonable = true;
    }

    public bool GetSummonStatus()
    {
        return isSummonable;
    }
}
