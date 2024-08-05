using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupLives : Powerup
{
    public int livesToAdd = 1;
    public int livesToRemove = 1;
    public override void Apply(PowerupManager target)
    {
        Pawn tankPawn = target.GetComponent<Pawn>();

        tankPawn.controller.AddToLives(livesToAdd);
    }

    public override void Remove(PowerupManager target)
    {
        Pawn tankPawn = target.GetComponent<Pawn>();

        tankPawn.controller.RemoveFromLives(livesToRemove);
    }
}
