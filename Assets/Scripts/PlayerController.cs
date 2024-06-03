using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    //Keycodes allow the designer to set what keys do what in the engine.
    public KeyCode MoveForward;
    public KeyCode MoveBackward;
    public KeyCode TurnClockwise;
    public KeyCode TurnCounterClockwise;

    public KeyCode TestPress;

    // Calls ProcessInputs every game tick.
    void Update()
    {
        ProcessInputs();
    }

    //Checks for player inputs every game tick.
    public override void ProcessInputs()
    {
        //Setting functions that happen when the player presses the keys aligned with the KeyCodes.
        if(Input.GetKey(MoveForward))
        {
            Debug.Log("Moving Forwards");
            pawn.MoveForward();
        }

        if (Input.GetKey(MoveBackward))
        {
            Debug.Log("Moving Backwards");
            pawn.MoveBackward();
        }

        if (Input.GetKey(TurnClockwise))
        {
            Debug.Log("Turning Right");
            pawn.TurnClockwise();
        }

        if (Input.GetKey(TurnCounterClockwise))
        {
            Debug.Log("Turning Left");
            pawn.TurnCounterClockwise();
        }

        if (Input.GetKey(TestPress))
        {
            pawn.TestPress();
        }
    }
}
