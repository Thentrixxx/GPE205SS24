using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupSpeed : Powerup
{
    public float speedToAdd;

    public override void Apply(PowerupManager target)
    {
        TankPawn tankPawn = target.GetComponent<TankPawn>();
        float targetMoveSpeed = tankPawn.moveSpeed;

        if (tankPawn != null)
        {
            tankPawn.moveSpeed = targetMoveSpeed + speedToAdd;
        }
    }

    public override void Remove(PowerupManager target)
    {
        TankPawn tankPawn = target.GetComponent<TankPawn>();
        float targetMoveSpeed = tankPawn.moveSpeed;

        if (tankPawn != null)
        {
            tankPawn.moveSpeed = targetMoveSpeed - speedToAdd;
        }
    }

}
