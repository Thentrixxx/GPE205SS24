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

        if (tankPawn.controller.score > ((GameManager.instance.mapGenerator.cols * GameManager.instance.mapGenerator.rows) / 2) * 40)
        {
            GameManager.instance.DeactivateAllStates();
            GameManager.instance.EnableMainMenuCamera();
            GameManager.instance.ActivateVictoryScreen();
        }
    }

    public override void Remove(PowerupManager target)
    {
        Pawn tankPawn = target.GetComponent<Pawn>();

        tankPawn.controller.RemoveFromScore(scoreToAdd);
    }
}
