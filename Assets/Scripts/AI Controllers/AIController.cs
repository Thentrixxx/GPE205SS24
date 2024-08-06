using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AIController : Controller
{
    // Enum for all AIState States.
    public enum AIState { Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost}

    // Variable for the current state
    public AIState currentState;

    // The tank it will chase after
    public GameObject target;
    public GameObject target1;
    public GameObject target2;

    // Calculating distance between the player tank and the AI
    public float distanceX;
    public float distanceY;
    public float distanceZ;
    public float distanceTotalTank1;
    public float distanceTotalTank2;

    // Self Distance Variables
    public float chaseDistance;
    public float hearingDistance;
    public float fieldOfView;
    public float seeingDistance;
    public float fleeDistance;
    public float fireRate;

    // Waypoints
    public Transform[] waypoints;
    public float waypointStopDistance;
    protected int currentWaypoint = 0;

    //Timer Variables
    protected float lastStateChangeTime;
    protected float lastShootTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ChangeState(AIState.Scan);
        

        if (GameManager.instance != null)
        {
            Debug.Log("GameManager Exists");
            // If it's tracking players
            if (GameManager.instance.players != null)
            {
                target = GameManager.instance.players[0].pawn.gameObject;
            }
            // Checking if player 2 exists
            if (GameManager.instance.isTwoPlayer)
            {
                target1 = GameManager.instance.players[0].pawn.gameObject;
                target2 = GameManager.instance.players[1].pawn.gameObject;
            }
            else
            {
                target = GameManager.instance.players[0].pawn.gameObject;
            }
        }
        else
        {
            Debug.Log("Game Manager does not exist");
        }
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (pawn != null)
        {
            ProcessInputs();

            if (GameManager.instance.isTwoPlayer)
            {
                if (IsDistanceLessThan(target2, 15))
                {
                    if (IsDistanceLessThan(target1, 15))
                    {
                        target = target1;
                    }
                    else
                    {
                        target = target2;
                    }
                }
            }
        }
        else
        {

        }
    }

    public override void ProcessInputs()
    {
        switch (currentState)
        {
            case AIState.Guard:
                // Do Guard State
                DoGuardState();
                break;

            case AIState.Chase:
                // Do Chase State
                DoChaseState();
                break;

            case AIState.Flee:
                // Do Flee State
                DoFleeState();
                break;

            case AIState.Patrol:
                // Do Patrol State
                DoPatrolState();
                break;

            case AIState.Attack:
                DoAttackState();
                break;

            case AIState.Scan:
                // Do that states behavior
                DoScanState();
                break;

            case AIState.BackToPost:
                // Do BackToPost State
                break;

        }
    }

    //Helper function for Changing States
    public virtual void ChangeState(AIState newState)
    {
        // Save the time when we changed states
        lastStateChangeTime = Time.time;
        // Change the current state
        currentState = newState;
    }

    public virtual void DoGuardState()
    {
        //Movement variables.
        pawn.moveSpeed = 8;
        pawn.turnSpeed = 200;

        /*if (IsDistanceLessThan(target, chaseDistance))
            {
                ChangeState(AIState.Chase);
            }*/

        //If the time is > 3s, go to patrol.
        if (Time.time - lastStateChangeTime > 3f)
        {
            ChangeState(AIState.Patrol);
            return;
        }

        // Do guard state
        CheckHealth();
        if (CanHear(target))
        {
            ChangeState(AIState.Chase);
        }
        if (CanSee(target))
        {
            ChangeState(AIState.Chase);
        }
    }

    protected virtual void DoChaseState()
    {
        //Checks for player health
        CheckHealth();

        // Call the seek action function
        Seek(target);

        //Check if the AI can't see the player.
        /*if (CanSee(target))
        {
            Debug.Log("Can still see the target in Chase");
        }
        else
        {
            if (!CanSee(target))
            {
                ChangeState(AIState.Guard);
            }
        }*/

        //If the AI has been following the tank for 2 seconds.
        if (Time.time - lastStateChangeTime > 2f)
        {
            if (CanSee(target))
            {
                ChangeState(AIState.Attack);
                return;
            }
            else
            {
                ChangeState(AIState.Scan);
            }
        }
    }

    protected virtual void DoScanState()
    {
        //Checking for the player health.
        CheckHealth();

        // Rotate Clockwise
        pawn.TurnClockwise();

        // If the AI has been in Scan for 3 seconds.
        if (Time.time - lastStateChangeTime > 3f)
        {
            ChangeState(AIState.Patrol);
            return;
        }

        // If player is within feild of view
        if (CanSee(target))
        {
            Debug.Log("Can See From Scan");
            ChangeState(AIState.Chase);
            return;
        }

        //If AI can hear the player
        if (CanHear(target))
        {
            ChangeState(AIState.Chase);
        }
    }
    protected virtual void DoAttackState()
    {
        //Tank Movement Variables
        pawn.moveSpeed = 4;
        pawn.turnSpeed = 100;
        CheckHealth();
        // Chase
        Seek(target);
        // Shoot
        if (Time.time - lastShootTime > fireRate)
        {
            Shoot();
        }

        //Check if the AI can't see the player.
        if (CanSee(target))
        {
            Debug.Log("Can still see the target in Attack");
        }
        else
        {
            if (!CanSee(target))
            {
                ChangeState(AIState.Guard);
            }
        }
    }

    protected virtual void DoPatrolState()
    {
        // Checks if tank has been in scan state for longer than 5s.
        if (Time.time - lastStateChangeTime > 5f)
        {
            ChangeState(AIState.Scan);
            return;
        }

        //Can the tank see the player?
        if (CanSee(target))
        {
            Debug.Log("Can See From Patrol");
            ChangeState(AIState.Chase);
            return;
        }

        //If can hear the player.
        if (CanHear(target))
        {
            ChangeState(AIState.Chase);
        }

        //Checking for health.
        CheckHealth();

        //Patroling Waypoints.
        Patrol();
    }

    public virtual void DoFleeState()
    {
        Flee();
    }

    // Seeking a GameObject
    public virtual void Seek(GameObject target)
    {
        Seek(target.transform.position);
    }

    // Seeking a Transform
    public virtual void Seek(Transform target)
    {
        Seek(target.transform.position);
    }

    //Seeking a targetPosition with a Vector3
    public virtual void Seek(Vector3 targetPosition)
    {
        // Do seek
        pawn.RotateTowards(targetPosition, false);
        // Move Forward
        pawn.MoveForward();
    }

    protected virtual bool IsDistanceLessThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public virtual bool CanHear(GameObject target)
    {
        //Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        if (noiseMaker == null)
        {
            return false;
        }

        if (noiseMaker.volumeDistance <= 0)
        {
            return false;
        }

        float totalDistance = noiseMaker.volumeDistance + hearingDistance;

        //If the distance between pawn and target is closer than the total distance.
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual bool CanSee(GameObject target)
    {
        Vector3 agentToTargetVector = target.transform.position - pawn.transform.position;

        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

        if (angleToTarget < fieldOfView)
        {
            RaycastHit hit;

            if (Physics.Raycast(pawn.transform.position, agentToTargetVector, out hit, seeingDistance))
            {
                GameObject hitGameObject = hit.transform.gameObject;

                if (hitGameObject == target)
                {
                    return true;
                }
            }
                return false;
        }
        return false;
    }

    protected virtual void Patrol()
    {
        // If we have a enough waypoints in our list to move to a current waypoint
        if (waypoints.Length > currentWaypoint)
        {
            // Then seek that waypoint
            Seek(waypoints[currentWaypoint]);
            // If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            RestartPatrol();
        }
    }

    protected virtual void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }

    protected virtual void Flee()
    {
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);
        float percentOfFleeDistance = targetDistance / fleeDistance;
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;

        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;

        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance * percentOfFleeDistance;

        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);
    }

    public virtual void Shoot()
    {
        lastShootTime = Time.time;
        pawn.Shoot();
    }

    protected virtual void CheckHealth()
    {
        if (pawn.GetComponent<Health>().currentHealth <= 50)
        {
            ChangeState(AIState.Flee);
        }
    }

    public override void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public override void RemoveFromScore(int scoreToAdd)
    {
        score -= scoreToAdd;
    }

    public override void AddToLives(int livesToAdd)
    {
        lives += livesToAdd;
    }

    public override void RemoveFromLives(int livesToRemove)
    {
        lives -= livesToRemove;
    }
}
