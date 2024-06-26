using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;

    public GameObject AIControllerPrefab;
    public GameObject AIControllerScaredPrefab;
    public GameObject AIControllerAggressivePrefab;
    public GameObject AIControllerSporadicPrefab;
    public GameObject AIControllerDefensePrefab;

    public GameObject enemyPawnPrefab;

    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;
    public Transform enemyScaredSpawnPoint;
    public Transform enemyAggressiveSpawnPoint;
    public Transform enemySporadicSpawnPoint;
    public Transform enemyDefenseSpawnPoint;

    public static GameManager instance;

    public List<PlayerController> players;

    public Transform[] waypoints;
    public Transform[] waypointsScare;
    public Transform[] waypointsAggressive;
    public Transform[] waypointsSporadic;
    public Transform[] waypointsDefense;

    public enum ControllerType { Normal, Scared, Aggressive, Sporadic, Defense }
    public ControllerType controllerType;

    // Awake is called before the start of the game.
    public void Awake()
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

        players = new List<PlayerController>();

        SpawnPlayer();
        SpawnEnemy(ControllerType.Normal, enemySpawnPoint);
        SpawnEnemy(ControllerType.Scared, enemyScaredSpawnPoint);
        SpawnEnemy(ControllerType.Aggressive, enemyAggressiveSpawnPoint);
        SpawnEnemy(ControllerType.Sporadic, enemySporadicSpawnPoint);
        SpawnEnemy(ControllerType.Defense, enemyDefenseSpawnPoint);
    }

    //Start is called once at the start of the game.
    public void Start()
    {
        
    }

    public void SpawnPlayer()
    {
        // Storing playerController as a variable and spawns it at Vector3.zero.
        GameObject playerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Storing the playerPawn as a variable and spawns it at a playerSpawnPoint with its rotation and position.
        GameObject playerPawn = Instantiate(playerPawnPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation) as GameObject;

        playerPawn.AddComponent<NoiseMaker>();

        // Gets the pawn attribute of the playerController object and sets it equal to the Pawn component of the playerPawn object.
        playerController.GetComponent<Controller>().pawn = playerPawn.GetComponent<Pawn>();
    }

    public void SpawnEnemy(ControllerType controllerType, Transform spawnTransform)
    {
        GameObject AIController = null;

        switch (controllerType)
        {
            case ControllerType.Normal:
                AIController = Instantiate(AIControllerPrefab, spawnTransform.position, Quaternion.identity) as GameObject;

                //Assigns the waypoints to the Normal Tank.
                if (AIController.GetComponent<AIController>() != null)
                {
                    AIController normalTank = AIController.GetComponent<AIController>();

                    for (int i = 0; i < waypoints.Length; i++)
                    {
                        normalTank.waypoints[i] = waypoints[i];
                    }
                }
                break;

            case ControllerType.Scared:
                AIController = Instantiate(AIControllerScaredPrefab, spawnTransform.position, Quaternion.identity) as GameObject;

                //Assigns the waypoints to the Scared Tank.
                if (AIController.GetComponent<AIControllerScare>() != null)
                {
                    AIController scaredTank = AIController.GetComponent<AIControllerScare>();

                    for (int i = 0; i < waypointsScare.Length; i++)
                    {
                        scaredTank.waypoints[i] = waypointsScare[i];
                    }
                }
                break;

            case ControllerType.Aggressive:
                Debug.Log("Setting Aggressive");
                AIController = Instantiate(AIControllerAggressivePrefab, spawnTransform.position, Quaternion.identity) as GameObject;

                //Assigns the waypoints to the Scared Tank.
                if (AIController.GetComponent<AIControllerAggressive>() != null)
                {
                    AIController aggressiveTank = AIController.GetComponent<AIControllerAggressive>();

                    for (int i = 0; i < waypointsAggressive.Length; i++)
                    {
                        aggressiveTank.waypoints[i] = waypointsAggressive[i];
                    }
                }
                break;

            case ControllerType.Sporadic:
                Debug.Log("Setting Sporadic");
                AIController = Instantiate(AIControllerSporadicPrefab, spawnTransform.position, Quaternion.identity) as GameObject;

                //Assigns the waypoints to the Scared Tank.
                if (AIController.GetComponent<AIControllerSporadic>() != null)
                {
                    AIController sporadicTank = AIController.GetComponent<AIControllerSporadic>();

                    for (int i = 0; i < waypointsSporadic.Length; i++)
                    {
                        sporadicTank.waypoints[i] = waypointsSporadic[i];
                    }
                }
                break;

            case ControllerType.Defense:
                Debug.Log("Setting Defense");
                AIController = Instantiate(AIControllerDefensePrefab, spawnTransform.position, Quaternion.identity) as GameObject;

                //Assigns the waypoints to the Scared Tank.
                if (AIController.GetComponent<AIControllerDefense>() != null)
                {
                    AIController defenseTank = AIController.GetComponent<AIControllerDefense>();

                    for (int i = 0; i < waypointsDefense.Length; i++)
                    {
                        defenseTank.waypoints[i] = waypointsDefense[i];
                    }
                }
                break;
        }

        // Storing the playerPawn as a variable and spawns it at a playerSpawnPoint with its rotation and position.
        GameObject enemyPawn = Instantiate(enemyPawnPrefab, spawnTransform.position, Quaternion.identity) as GameObject;

        // Gets the pawn attribute of the playerController object and sets it equal to the Pawn component of the playerPawn object.
        AIController.GetComponent<Controller>().pawn = enemyPawn.GetComponent<Pawn>();
    }
}
