using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public AudioSource menuMusicSource;

    public AudioSource deathSound;
    public AudioSource hitSound;
    public AudioSource powerupSound;

    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;

    public GameObject playerOneControllerPrefab;
    public GameObject playerOnePawnPrefab;

    public GameObject playerTwoControllerPrefab;
    public GameObject playerTwoPawnPrefab;

    //AI Prefabs
    public GameObject AIControllerPrefab;
    public GameObject AIControllerScaredPrefab;
    public GameObject AIControllerAggressivePrefab;
    public GameObject AIControllerSporadicPrefab;
    public GameObject AIControllerDefensePrefab;

    public GameObject enemyPawnPrefab;

    // Player and AI Tank Spawnpoints
    public Transform playerSpawnPoint;
    public Transform playerTwoSpawnPoint;
    public Transform enemySpawnPoint;
    public Transform enemyScaredSpawnPoint;
    public Transform enemyAggressiveSpawnPoint;
    public Transform enemySporadicSpawnPoint;
    public Transform enemyDefenseSpawnPoint;

    public static GameManager instance;

    public List<PlayerController> players;

    // AI Tank Waypoints
    public Transform[] waypoints;
    public Transform[] waypointsScare;
    public Transform[] waypointsAggressive;
    public Transform[] waypointsSporadic;
    public Transform[] waypointsDefense;

    // All of the "found" spawn points in the level
    private PawnSpawnPoint[] foundPawnSpawnPoints;
    private PawnTwoSpawnPoint[] foundPawnTwoSpawnPoints;
    private AINormalSpawnPoint[] foundAINormalSpawnPoints;
    private AIScaredSpawnPoint[] foundAIScaredSpawnPoints;
    private AIAggressiveSpawnPoint[] foundAIAggressiveSpawnPoints;
    private AIDefensiveSpawnPoint[] foundAIDefensiveSpawnPoints;
    private AISporadicSpawnPoint[] foundAISporadicSpawnPoints;

    // Map Generator
    public MapGenerator mapGenerator;

    //Camera
    public GameObject mainMenuCamera;

    // Game States
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsStateObject;
    public GameObject CreditsStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;
    public GameObject VictoryScreenStateObject;

    //enums
    public enum ControllerType { Normal, Scared, Aggressive, Sporadic, Defense }
    public ControllerType controllerType;

    public bool isRandomGeneration;
    public bool isTwoPlayer;

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
        if (isRandomGeneration)
        {
            DeactivateAllStates();
            ActivateTitleScreen();

            menuMusicSource.Play();
        }
        else
        {
            DoGameplay();
        }
    }

    // Deactivates all Game States.
    private void DeactivateAllStates()
    {
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsStateObject.SetActive(false);
        CreditsStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
    }

    public void ActivateTitleScreen()
    {
        // Deactivate all states.
        DeactivateAllStates();

        //Activate the title screen
        TitleScreenStateObject.SetActive(true);

        // Do whatever needs to be done during the title screen.
    }

    public void ActivateMainMenuScreen()
    {
        // Deactivate all states.
        DeactivateAllStates();

        //Activate the title screen
        MainMenuStateObject.SetActive(true);

        // Do whatever needs to be done during the main menu.
    }

    public void ActivateOptionsScreen()
    {
        // Deactivate all states.
        DeactivateAllStates();

        //Activate the title screen
        OptionsStateObject.SetActive(true);

        // Do whatever needs to be done during the options.
    }

    public void ActivateCreditsScreen()
    {
        // Deactivate all states.
        DeactivateAllStates();

        //Activate the title screen
        CreditsStateObject.SetActive(true);

        // Do whatever needs to be done during the credits.
    }

    // Everything that was in Awake is now here for ease of use.
    public void ActivateGameplayState()
    {
        // Deactivate all states.
        DeactivateAllStates();

        //Activate the title screen
        GameplayStateObject.SetActive(true);

        //Disables the main menu camera so that the gameplay ones appear
        DisableMainMenuCamera();

        // Do whatever needs to be done during the gameplay.
        DoGameplay();
    }


    public void DoGameplay()
    {
        menuMusicSource.Pause();
        // Do whatever needs to be done during the gameplay.
        players = new List<PlayerController>();

        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap();

        foundPawnSpawnPoints = FindObjectsByType<PawnSpawnPoint>(FindObjectsSortMode.None);
        foundPawnTwoSpawnPoints = FindObjectsByType<PawnTwoSpawnPoint>(FindObjectsSortMode.None);

        foundAINormalSpawnPoints = FindObjectsByType<AINormalSpawnPoint>(FindObjectsSortMode.None);
        foundAIScaredSpawnPoints = FindObjectsByType<AIScaredSpawnPoint>(FindObjectsSortMode.None);
        foundAIAggressiveSpawnPoints = FindObjectsByType<AIAggressiveSpawnPoint>(FindObjectsSortMode.None);
        foundAIDefensiveSpawnPoints = FindObjectsByType<AIDefensiveSpawnPoint>(FindObjectsSortMode.None);
        foundAISporadicSpawnPoints = FindObjectsByType<AISporadicSpawnPoint>(FindObjectsSortMode.None);

        Transform selectedSpawnPointTransform = foundPawnSpawnPoints[UnityEngine.Random.Range(0, foundPawnSpawnPoints.Length)].transform;
        Transform selectedSpawnPointTwoTransform = foundPawnTwoSpawnPoints[UnityEngine.Random.Range(0, foundPawnTwoSpawnPoints.Length)].transform;

        playerSpawnPoint = selectedSpawnPointTransform;
        playerTwoSpawnPoint = selectedSpawnPointTwoTransform;

        Transform selectedAINormalSpawnPointTransform = null;
        Transform selectedAIScaredSpawnPointTransform = null;
        Transform selectedAIAggressiveSpawnPointTransform = null;
        Transform selectedAIDefensiveSpawnPointTransform = null;
        Transform selectedAISporadicSpawnPointTransform = null;

        if (foundAINormalSpawnPoints.Length != 0)
        {
            selectedAINormalSpawnPointTransform = foundAINormalSpawnPoints[UnityEngine.Random.Range(0, foundAINormalSpawnPoints.Length)].transform;
            waypoints[0] = selectedAINormalSpawnPointTransform.GetComponent<Waypoint>().waypoint.transform;
            waypoints[1] = selectedAINormalSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.transform;
            waypoints[2] = selectedAINormalSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.transform;
            waypoints[3] = selectedAINormalSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.waypoint.transform;
        }

        if (foundAIScaredSpawnPoints.Length != 0)
        {
            selectedAIScaredSpawnPointTransform = foundAIScaredSpawnPoints[UnityEngine.Random.Range(0, foundAIScaredSpawnPoints.Length)].transform;
            waypointsScare[0] = selectedAIScaredSpawnPointTransform.GetComponent<Waypoint>().waypoint.transform;
            waypointsScare[1] = selectedAIScaredSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.transform;
            waypointsScare[2] = selectedAIScaredSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.transform;
            waypointsScare[3] = selectedAIScaredSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.waypoint.transform;
        }

        if (foundAIAggressiveSpawnPoints.Length != 0)
        {
            selectedAIAggressiveSpawnPointTransform = foundAIAggressiveSpawnPoints[UnityEngine.Random.Range(0, foundAIAggressiveSpawnPoints.Length)].transform;
            waypointsAggressive[0] = selectedAIAggressiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.transform;
            waypointsAggressive[1] = selectedAIAggressiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.transform;
            waypointsAggressive[2] = selectedAIAggressiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.transform;
            waypointsAggressive[3] = selectedAIAggressiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.waypoint.transform;
        }

        if (foundAIDefensiveSpawnPoints.Length != 0)
        {
            selectedAIDefensiveSpawnPointTransform = foundAIDefensiveSpawnPoints[UnityEngine.Random.Range(0, foundAIDefensiveSpawnPoints.Length)].transform;
            waypointsDefense[0] = selectedAIDefensiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.transform;
            waypointsDefense[1] = selectedAIDefensiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.transform;
            waypointsDefense[2] = selectedAIDefensiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.transform;
            waypointsDefense[3] = selectedAIDefensiveSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.waypoint.transform;
        }

        if (foundAISporadicSpawnPoints.Length != 0)
        {
            selectedAISporadicSpawnPointTransform = foundAISporadicSpawnPoints[UnityEngine.Random.Range(0, foundAISporadicSpawnPoints.Length)].transform;
            waypointsSporadic[0] = selectedAISporadicSpawnPointTransform.GetComponent<Waypoint>().waypoint.transform;
            waypointsSporadic[1] = selectedAISporadicSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.transform;
            waypointsSporadic[2] = selectedAISporadicSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.transform;
            waypointsSporadic[3] = selectedAISporadicSpawnPointTransform.GetComponent<Waypoint>().waypoint.waypoint.waypoint.waypoint.transform;
        }

        SpawnPlayer();

        if (isRandomGeneration)
        {
            SpawnEnemy(ControllerType.Normal, selectedAINormalSpawnPointTransform);
            SpawnEnemy(ControllerType.Scared, selectedAIScaredSpawnPointTransform);
            SpawnEnemy(ControllerType.Aggressive, selectedAIAggressiveSpawnPointTransform);
            SpawnEnemy(ControllerType.Sporadic, selectedAISporadicSpawnPointTransform);
            SpawnEnemy(ControllerType.Defense, selectedAIDefensiveSpawnPointTransform);
        }
        else
        {
            SpawnEnemy(ControllerType.Normal, enemySpawnPoint);
            SpawnEnemy(ControllerType.Scared, enemyScaredSpawnPoint);
            SpawnEnemy(ControllerType.Aggressive, enemyAggressiveSpawnPoint);
            SpawnEnemy(ControllerType.Sporadic, enemySporadicSpawnPoint);
            SpawnEnemy(ControllerType.Defense, enemyDefenseSpawnPoint);
        }
    }
    public void ActivateGameOverScreen()
    {
        // Deactivate all states.
        DeactivateAllStates();

        //Activate the title screen
        GameOverScreenStateObject.SetActive(true);

        // Enables the Main Menu camera
        EnableMainMenuCamera();

        // Do whatever needs to be done during the game over.
    }

    public void ActivateVictoryScreen()
    {
        // Deactivate all states.
        DeactivateAllStates();

        // Activate the title screen
        VictoryScreenStateObject.SetActive(true);

        // Enables the main menu camera
        EnableMainMenuCamera();

        // Do whatever needs to be done during the game over.
    }

    public void DisableMainMenuCamera()
    {
        mainMenuCamera.SetActive(false);
    }

    public void EnableMainMenuCamera()
    {
        mainMenuCamera.SetActive(true);
    }

    public void SpawnPlayer()
    {
        /*
        // Storing playerController as a variable and spawns it at Vector3.zero.
        GameObject playerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Storing the playerPawn as a variable and spawns it at a playerSpawnPoint with its rotation and position.
        GameObject playerPawn = Instantiate(playerPawnPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation) as GameObject;

        playerPawn.AddComponent<NoiseMaker>();

        // Gets the pawn attribute of the playerController object and sets it equal to the Pawn component of the playerPawn object.
        playerController.GetComponent<Controller>().pawn = playerPawn.GetComponent<Pawn>();
        */

        //Spawns player two only if the game is two player.
        if (isTwoPlayer)
        {
            SpawnPlayer(playerSpawnPoint, playerOneControllerPrefab, playerOnePawnPrefab);
            SpawnPlayer(playerTwoSpawnPoint, playerTwoControllerPrefab, playerTwoPawnPrefab);
        }
        else
        {
            SpawnPlayer(playerSpawnPoint, playerControllerPrefab, playerPawnPrefab);
        }
    }

    public void SpawnPlayer(Transform spawnPosition, GameObject controllerPrefab, GameObject pawnPrefab)
    {
        // Storing playerController as a variable and spawns it at Vector3.zero.
        GameObject playerController = Instantiate(controllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Storing the playerPawn as a variable and spawns it at a playerSpawnPoint with its rotation and position.
        GameObject playerPawn = Instantiate(pawnPrefab, spawnPosition.position, spawnPosition.rotation) as GameObject;

        playerPawn.AddComponent<NoiseMaker>();

        // Gets the pawn attribute of the playerController object and sets it equal to the Pawn component of the playerPawn object.
        playerController.GetComponent<Controller>().pawn = playerPawn.GetComponent<Pawn>();

        //Assign the pawn's controller variable to the controller of the component.
        playerPawn.GetComponent<Pawn>().controller = playerController.GetComponent<Controller>();
    }

    

    public void SpawnEnemy(ControllerType controllerType, Transform spawnTransform)
    {
        GameObject AIController = null;

        // For random generation with a lot of spawns.
        if (isRandomGeneration)
        {
            switch (controllerType)
            {
                case ControllerType.Normal:
                    if (spawnTransform != null)
                    {
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
                    }
                    else
                    {
                        Debug.Log("No Normal Spawnpoint Found");
                    }
                    break;

                case ControllerType.Scared:
                    if (spawnTransform != null)
                    {
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
                    }
                    else
                    {
                        Debug.Log("No Scared Spawnpoint Found");
                    }
                    break;

                case ControllerType.Aggressive:
                    if (spawnTransform != null)
                    {
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
                    }
                    else
                    {
                        Debug.Log("No Aggressive Spawnpoint Found");
                    }
                    break;

                case ControllerType.Sporadic:
                    if (spawnTransform != null)
                    {
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
                    }
                    else
                    {
                        Debug.Log("No Sporadic Spawnpoint Found");
                    }
                    break;

                case ControllerType.Defense:
                    if (spawnTransform != null)
                    {
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
                    }
                    else
                    {
                        Debug.Log("No Defensive Spawnpoint Found");
                    }
                    break;
            }
        }

        // For normal generation with preset spawns.
        else {
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
        }

        if (spawnTransform != null)
        {
            // Storing the playerPawn as a variable and spawns it at a playerSpawnPoint with its rotation and position.
            GameObject enemyPawn = Instantiate(enemyPawnPrefab, spawnTransform.position, Quaternion.identity) as GameObject;

            // Gets the pawn attribute of the playerController object and sets it equal to the Pawn component of the playerPawn object.
            AIController.GetComponent<Controller>().pawn = enemyPawn.GetComponent<Pawn>();
        }
    }

    public void ToggleTwoPlayer()
    {
        if (isTwoPlayer)
        {
            isTwoPlayer = false;
        }
        else
        {
            isTwoPlayer = true;
        }
    }

}
