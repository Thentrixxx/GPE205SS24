using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : Controller
{
    //Keycodes allow the designer to set what keys do what in the engine.
    public KeyCode MoveForward;
    public KeyCode MoveBackward;
    public KeyCode TurnClockwise;
    public KeyCode TurnCounterClockwise;
    public KeyCode ShootKey;

    public KeyCode TestPress;

    public float volumeDistance;

    private NoiseMaker noiseMaker;

    // Calls ProcessInputs every game tick.

    private void Start()
    {
        // If the GameManager exists
        if (GameManager.instance != null)
        {
            // If it's tracking players
            if (GameManager.instance.players != null)
            {
                // Register with the player list
                GameManager.instance.players.Add(this);
            }
        }

        noiseMaker = pawn.gameObject.GetComponent<NoiseMaker>();
    }
    void Update()
    {
        if (pawn != null)
        {
            ProcessInputs();
        }
    }

    //Checks for player inputs every game tick.
    public override void ProcessInputs()
    {
        //Setting functions that happen when the player presses the keys aligned with the KeyCodes.
        if (Input.GetKey(MoveForward))
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

        if (Input.GetKeyDown(ShootKey))
        {
            pawn.Shoot();
        }

        if (Input.GetKey(TestPress))
        {
            pawn.TestPress();
        }

        if (Input.GetKey(MoveForward) || Input.GetKey(MoveBackward) || Input.GetKey(TurnClockwise) || Input.GetKey(TurnCounterClockwise))
        {
            if (noiseMaker != null)
            {
                noiseMaker.volumeDistance = volumeDistance;
            }
        }
        else
        {
            if (noiseMaker != null)
            {
                noiseMaker.volumeDistance = 0;
            }
        }
    }

    public override void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public override void RemoveFromScore(int scoreToAdd)
    {
        score -= scoreToAdd;
    }

    public override void AddToLives(int livesToAdd)
    {
        lives += livesToAdd;
    }

    public override void RemoveFromLives(int livesToRemove)
    {
        lives -= livesToRemove;

    }

    public void OnDestroy()
    {
        {
            // If the GameManager exists
            if (GameManager.instance != null)
            {
                // If it's tracking the players
                if (GameManager.instance.players != null)
                {
                    // Deregister with the GameManager
                    GameManager.instance.players.Remove(this);
                }
            }
        }
    }
}
