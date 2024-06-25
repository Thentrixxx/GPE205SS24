using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerAggressive : AIController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        ChangeState(AIState.Scan);


        if (GameManager.instance != null)
        {
            Debug.Log("GameManager Exists");
            // If it's tracking players
            if (GameManager.instance.players != null)
            {
                Debug.Log("GameManager Found");
                // Register with the player list
                target = GameManager.instance.players[0].pawn.gameObject;
            }
        }
        else
        {
            Debug.Log("Game Manager does not exist");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (pawn != null)
        {
            ProcessInputs();
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
    public override void ChangeState(AIState newState)
    {
        // Save the time when we changed states
        lastStateChangeTime = Time.time;
        // Change the current state
        currentState = newState;
    }

    public override void DoGuardState()
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

    protected override void DoChaseState()
    {
        //Checks for player health
        CheckHealth();

        // Call the seek action function
        Seek(target);

        //Check if the AI can't see the player.
        if (CanSee(target))
        {
            Debug.Log("Can still see the target in Chase");
        }
        else
        {
            if (CanSee(target))
            {
                ChangeState(AIState.Guard);
            }
        }

        //If the AI has been following the tank for 2 seconds.
        if (Time.time - lastStateChangeTime > 2f)
        {
            ChangeState(AIState.Attack);
            return;
        }
    }

    protected override void DoScanState()
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
    }
    protected override void DoAttackState()
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

    protected override void DoPatrolState()
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

        //Checking for health.
        CheckHealth();

        //Patroling Waypoints.
        Patrol();
    }

    public override void DoFleeState()
    {
        Flee();
    }

    // Seeking a GameObject
    public override void Seek(GameObject target)
    {
        Seek(target.transform.position);
    }

    // Seeking a Transform
    public override void Seek(Transform target)
    {
        Seek(target.transform.position);
    }

    //Seeking a targetPosition with a Vector3
    public override void Seek(Vector3 targetPosition)
    {
        // Do seek
        pawn.RotateTowards(targetPosition, false);
        // Move Forward
        pawn.MoveForward();
    }

    protected override bool IsDistanceLessThan(GameObject target, float distance)
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

    public override bool CanHear(GameObject target)
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

    public override bool CanSee(GameObject target)
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

    protected override void Patrol()
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

    protected override void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }

    protected override void Flee()
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

    public override void Shoot()
    {
        lastShootTime = Time.time;
        pawn.Shoot();
    }

    protected override void CheckHealth()
    {
        if (pawn.GetComponent<Health>().currentHealth <= 50)
        {
            ChangeState(AIState.Flee);
        }
    }

    /*public void ChangeMoveSpeed(float moveSpeed)
    {
        pawn.moveSpeed = moveSpeed;
    }

    public void ChangeRotationSpeed(float rotate)
    {
        pawn.moveSpeed = rotate;
    }*/
}
