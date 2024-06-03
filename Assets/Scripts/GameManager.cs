using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;

    public Transform playerSpawnPoint;
    
    public static GameManager instance;

    // Awake is called before the start of the game.
    public void Awake()
    {
        
    }

    //Start is called once at the start of the game.
    public void Start()
    {
        // Checks if there's only one instance of GameManager, otherwise destroys the previous version.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // Storing playerController as a variable and spawns it at Vector3.zero.
        GameObject playerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Storing the playerPawn as a variable and spawns it at a playerSpawnPoint with its rotation and position.
        GameObject playerPawn = Instantiate(playerPawnPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation) as GameObject;

        // Gets the pawn attribute of the playerController object and sets it equal to the Pawn component of the playerPawn object.
        playerController.GetComponent<Controller>().pawn = playerPawn.GetComponent<Pawn>();
    }
}
