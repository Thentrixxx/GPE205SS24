using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupLandMine : Powerup
{
    public float healthToRemove;

    public override void Apply(PowerupManager target)
    {
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(healthToRemove, target.GetComponent<Pawn>());
        }
    }

    public override void Remove(PowerupManager target)
    {
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.Heal(healthToRemove, target.GetComponent<Pawn>());
        }
    }
}
