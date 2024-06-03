using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    //Sets a pawn variable to what the Pawn is.
    public Pawn pawn;

    //Creates a ProcessInputs function to be overriden later. This will allow the system to process what the controller is pressing.
    public abstract void ProcessInputs();
}
