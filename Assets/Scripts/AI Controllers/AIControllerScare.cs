using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerScare : AIController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void DoGuardState()
    {
        //Movement Variables
        pawn.moveSpeed = 8;

        /*if (IsDistanceLessThan(target, chaseDistance))
            {
                ChangeState(AIState.Chase);
            }*/

        //If the time is > 3s, go to patrol.
        if (Time.time - lastStateChangeTime > 2f)
        {
            ChangeState(AIState.Scan);
            return;
        }

        //Check if the player is nearby
        if (CanHear(target))
        {
            ChangeState(AIState.Attack);
        }
        if (CanSee(target))
        {
            ChangeState(AIState.Attack);
        }
    }

    protected override void DoChaseState()
    {
        base.DoChaseState();
    }

    protected override void DoScanState()
    {
        // Rotate Clockwise
        pawn.TurnClockwise();

        // If the AI has been in Scan for 4 seconds.
        if (Time.time - lastStateChangeTime > 4f)
        {
            ChangeState(AIState.Patrol);
            return;
        }

        // If player is within feild of view
        if (CanSee(target))
        {
            Debug.Log("Can See From Scan");
            ChangeState(AIState.Attack);
            return;
        }
        if (CanHear(target))
        {
            ChangeState(AIState.Attack);
        }
    }
    protected override void DoAttackState()
    {
        // Pawn Variables
        pawn.turnSpeed = 600;

        // Chase
        if (target != null)
        {
            Seek(target);
        }

        // Shoot on cooldown
        if (Time.time - lastShootTime > fireRate)
        {
            Shoot();
        }

        // If time is greater than 2s, run away.
        if (Time.time - lastStateChangeTime > 2f)
        {
            ChangeState(AIState.Flee);
            return;
        }
    }

    protected override void DoPatrolState()
    {
        // Checks if tank has been in patrol state for longer than 6s.
        if (Time.time - lastStateChangeTime > 6f)
        {
            ChangeState(AIState.Scan);
            return;
        }

        //Can the tank see the player?
        if (target != null)
        {
            if (CanSee(target))
            {
                Debug.Log("Can See From Patrol");
                ChangeState(AIState.Attack);
                return;
            }
            if (CanHear(target))
            {
                ChangeState(AIState.Attack);
            }
        }

        //Patroling Waypoints.
        Patrol();
    }

    public override void DoFleeState()
    {
        pawn.moveSpeed = 12;
        pawn.turnSpeed = 200;

        Flee();

        // If time is greater than 3s, run away.
        if (Time.time - lastStateChangeTime > 3f)
        {
            ChangeState(AIState.Guard);
            return;
        }
    }
}
