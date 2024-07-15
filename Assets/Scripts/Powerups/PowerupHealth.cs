using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]

public class PowerupHealth : Powerup
{
    public float healthToAdd;

    public override void Apply (PowerupManager target)
    {
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.Heal(healthToAdd, target.GetComponent<Pawn>());
        }
    }

    public override void Remove (PowerupManager target)
    {
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(healthToAdd, target.GetComponent<Pawn>());
        }
    }

}
