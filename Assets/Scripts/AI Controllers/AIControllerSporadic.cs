using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerSporadic : AIController
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
        //If the time is > 1s, go to scan.
        if (Time.time - lastStateChangeTime > 1f)
        {
            ChangeState(AIState.Scan);
            return;
        }

        //Check if the player is nearby
        if (CanHear(target))
        {
            ChangeState(AIState.Flee);
        }
        if (CanSee(target))
        {
            ChangeState(AIState.Flee);
        }
    }

    protected override void DoChaseState()
    {
        // Call the seek action function
        Seek(target);

        //If the AI has been following the tank for 2 seconds.
        if (Time.time - lastStateChangeTime > 2f)
        {
            
            ChangeState(AIState.Attack);
            return;
        }
    }

    protected override void DoScanState()
    {
        // Rotate Clockwise
        pawn.TurnClockwise();

        //If the time is > 2s, go to patrol.
        if (Time.time - lastStateChangeTime > 2f)
        {
            ChangeState(AIState.Patrol);
            return;
        }

        //Check if the player is nearby
        if (CanHear(target))
        {
            ChangeState(AIState.Flee);
        }
        if (CanSee(target))
        {
            ChangeState(AIState.Flee);
        }
    }
    protected override void DoAttackState()
    {
        // Chase
        Seek(target);

        // Shoot on cooldown
        if (Time.time - lastShootTime > fireRate)
        {
            Shoot();
        }

        // If time is greater than 3s, scan.
        if (Time.time - lastStateChangeTime > 3f)
        {
            ChangeState(AIState.Scan);
            return;
        }
    }

    protected override void DoPatrolState()
    {
        // Checks if tank has been in patrol state for longer than 4s.
        if (Time.time - lastStateChangeTime > 4f)
        {
            ChangeState(AIState.Guard);
            return;
        }

        //Can the tank see the player?
        if (CanSee(target))
        {
            ChangeState(AIState.Flee);
            return;
        }
        if (CanHear(target))
        {
            ChangeState(AIState.Flee);
        }

        //Patroling Waypoints.
        Patrol();
    }

    public override void DoFleeState()
    {
        Flee();

        // If time is greater than 3s, run away.
        if (Time.time - lastStateChangeTime > 1f)
        {
            ChangeState(AIState.Chase);
            return;
        }
    }
}
