using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    public Mover mover;

    // Setting a variable named "mover" equal to the Mover component of a pawn.
    public virtual void Start()
    {
        mover = GetComponent<Mover>();
    }

    private void Update()
    {
        
    }

    //Declares functions for children pawns to use.
    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void TurnClockwise();
    public abstract void TurnCounterClockwise();
    public abstract void TestPress();
}
