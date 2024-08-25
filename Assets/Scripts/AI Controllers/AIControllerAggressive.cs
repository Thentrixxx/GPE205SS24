using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerAggressive : AIController
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
        // Safety, in case something goes to this state.
        if (Time.time - lastStateChangeTime > 1f)
        {
            ChangeState(AIState.Scan);
        }

        // Check for tank health to be below 25
        CheckHealth();
    }

    protected override void DoChaseState()
    {
        // Safety, in case something goes to this state.
        if (Time.time - lastStateChangeTime > 1f)
        {
            ChangeState(AIState.Scan);
        }

        CheckHealth();
    }

    protected override void DoScanState()
    {
        //Movement Variables
        pawn.turnSpeed = 500;

        // Rotate Clockwise
        pawn.TurnClockwise();

        // Check for tank health to be below 25
        CheckHealth();

        // If the AI has been in Scan for 3 seconds.
        if (Time.time - lastStateChangeTime > 3f)
        {
            ChangeState(AIState.Patrol);
            return;
        }

        // If player is within feild of view
        if (target != null)
        {
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
    }
    protected override void DoAttackState()
    {
        // Pawn Variables
        pawn.turnSpeed = 300;
        fireRate = 0.8f;

        // Chase
        Seek(target);

        // Check for tank health to be below 25
        CheckHealth();

        // Shoot on cooldown
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
                ChangeState(AIState.Scan);
            }
        }
    }

    protected override void DoPatrolState()
    {
        // Tank Movement Variables
        pawn.turnSpeed = 300;

        // Checks if tank has been in patrol state for longer than 6s.
        if (Time.time - lastStateChangeTime > 3f)
        {
            ChangeState(AIState.Scan);
            return;
        }

        // If player is within feild of view
        if (target != null)
        {
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

        //Patroling Waypoints.
        Patrol();

        // Check for tank health to be below 25
        CheckHealth();
    }

    public override void DoFleeState()
    {
        // Tank Variables
        pawn.moveSpeed = 7;
        pawn.turnSpeed = 200;

        // Run away!!!
        Flee();

    }

    protected override void CheckHealth()
    {
        if (pawn.GetComponent<Health>().currentHealth <= 25)
        {
            ChangeState(AIState.Flee);
        }
    }
}
