using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    private Rigidbody rb;

    // Start is called before the first frame update.
    // Setting a variable "rb" equal to the Rigidbody of the Tank.
    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 direction, float speed)
    {
        //Setting a moveVector variable equal to the direction * speed * deltaTime.
        //This gets the direction and the speed the tank will move forward.
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;

        //Sets the Rigidbody of the tank equal to the position currently + the moveVector every frame.
        rb.MovePosition(rb.position + moveVector);
    }

    public override void Rotate(float rotationSpeed)
    {
        //Setting a rotationSpeed variable equal to the rotation speed * deltaTime.
        //This allows the tank to rotate at a speed of rotationSpeed * Time.deltaTime.
        gameObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
