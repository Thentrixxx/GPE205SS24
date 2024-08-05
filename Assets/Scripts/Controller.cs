using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    //Sets a pawn variable to what the Pawn is.
    public Pawn pawn;

    public int score = 0;

    public int lives = 3;

    //Creates a ProcessInputs function to be overriden later. This will allow the system to process what the controller is pressing.
    public abstract void ProcessInputs();

    public abstract void AddToScore(int scoreToAdd);
    public abstract void RemoveFromScore(int scoreToRemove);

    public abstract void AddToLives(int livesToAdd);
    public abstract void RemoveFromLives(int livesToRemove);
}
