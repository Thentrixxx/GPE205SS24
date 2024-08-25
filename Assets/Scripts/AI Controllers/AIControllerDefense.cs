using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerDefense : AIController
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
        // Checks if health is below 25
        CheckHealth();

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

        //Checks for health <= 25
        CheckHealth();

        // If the AI has been in Scan for 4 seconds.
        if (Time.time - lastStateChangeTime > 5f)
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
        if (CanHear(target))
        {
            ChangeState(AIState.Chase);
        }
    }
    protected override void DoAttackState()
    {
        // Chase
        if (target != null)
        {
            AntiSeek(target);
        }

        // Shoot on cooldown
        if (Time.time - lastShootTime > fireRate)
        {
            Shoot();
        }

        // If time is greater than 2s, run away.
        if (Time.time - lastStateChangeTime > 2f)
        {
            ChangeState(AIState.Chase);
            return;
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
        // Checks if tank has been in patrol state for longer than 6s.
        if (Time.time - lastStateChangeTime > 7f)
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
        if (CanHear(target))
        {
            ChangeState(AIState.Chase);
        }

        //Patroling Waypoints.
        Patrol();
    }

    public override void DoFleeState()
    {
        Flee();
    }

    public void AntiSeek(GameObject target)
    {
        AntiSeek(target.transform.position);
    }

    public void AntiSeek(Vector3 targetPosition)
    {
        // Do seek
        pawn.RotateTowards(targetPosition, false);
        // Move Forward
        pawn.MoveBackward();
    }

    protected override void CheckHealth()
    {
        if (pawn.GetComponent<Health>().currentHealth <= 25)
        {
            ChangeState(AIState.Flee);
        }
    }
}
