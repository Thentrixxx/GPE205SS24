using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PowerupScore : Powerup
{
    public int scoreToAdd = 10;

    public override void Apply(PowerupManager target)
    {
        Pawn tankPawn = target.GetComponent<Pawn>();

        tankPawn.controller.AddToScore(scoreToAdd);
    }

    public override void Remove(PowerupManager target)
    {
        Pawn tankPawn = target.GetComponent<Pawn>();

        tankPawn.controller.RemoveFromScore(scoreToAdd);
    }
}
