using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract void Start();

    //Setting a function that will use a Vector3 for direction, and a float for the speed of the tank.
    public abstract void Move(Vector3 direction, float speed);

    //Setting a function that will use a float for how fast the tank will rotate.
    public abstract void Rotate(float rotationSpeed);
}
