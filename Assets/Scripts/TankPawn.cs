using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;
[RequireComponent(typeof(TankMover))]

public class TankPawn : Pawn
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void MoveForward()
    {
        // Checks if the mover exists in the tank pawn.
        // If it does, move the tank forward. If not, send a warning.
        // The Move function is handled in the "TankMover" script.
        if (mover != null)
        {
            mover.Move(transform.forward, moveSpeed);
        }

        else
        {
            Debug.LogWarning("Warning: No Mover component is attached to the Pawn.");
        }
    }
    public override void MoveBackward()
    {
        // Checks if the mover exists in the tank pawn.
        // If it does, move the tank backwards. If not, send a warning.
        if (mover != null)
        {
            mover.Move(transform.forward, -moveSpeed);
        }

        else
        {
            Debug.LogWarning("Warning: No Mover component is attached to the Pawn.");
        }
    }
    public override void TurnClockwise()
    {
        // Checks if the mover exists in the tank pawn.
        // If it does, rotate the tank right. If not, send a warning.
        // The Rotate function is handled in the "TankMover" script.
        if (mover != null)
        {
            mover.Rotate(turnSpeed);
        }

        else
        {
            Debug.LogWarning("Warning: No Mover component is attached to the Pawn.");
        }
    }
    public override void TurnCounterClockwise()
    {
        // Checks if the mover exists in the tank pawn.
        // If it does, rotate the tank left. If not, send a warning.
        if (mover != null)
        {
            mover.Rotate(-turnSpeed);
        }
        
        else
        {
            Debug.LogWarning("Warning: No Mover component is attached to the Pawn.");
        }
    }

    // Function to reload the scene. Testing if the GameManager is a singleton.
    public override void TestPress()
    {
        SceneManager.LoadScene("Main");
    }

    public override void RotateTowards(Vector3 targetPosition, bool isScared)
    {
        Vector3 vectorToTarget;

        if (isScared)
        {
            vectorToTarget = transform.position - targetPosition;
        }
        else
        {
            vectorToTarget = targetPosition - transform.position;
        }

        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public override void Shoot()
    {
        shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifespan);
    }
}
