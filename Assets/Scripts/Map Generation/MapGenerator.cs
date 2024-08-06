using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    private Room[,] grid;
    public int rows;
    public int cols;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    public GameObject[] gridPrefabs;

    public bool usingCurrentTime;
    public bool isMapOfTheDay;
    public int mapSeed;

    //public GameObject playerControllerPrefab;
    //public GameObject playerPawnPrefab;

    // Start is called before the first frame update
    void Start()
    {
        /*GenerateMap();

        foundPawnSpawnPoints = FindObjectsByType<PawnSpawnPoint>(FindObjectsSortMode.None);

        Transform selectedSpawnPointTransform = foundPawnSpawnPoints[UnityEngine.Random.Range(0, foundPawnSpawnPoints.Length)].transform;

        // Storing playerController as a variable and spawns it at Vector3.zero.
        GameObject playerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Storing the playerPawn as a variable and spawns it at a playerSpawnPoint with its rotation and position.
        GameObject playerPawn = Instantiate(playerPawnPrefab, selectedSpawnPointTransform.position, selectedSpawnPointTransform.rotation) as GameObject;

        playerPawn.AddComponent<NoiseMaker>();

        // Gets the pawn attribute of the playerController object and sets it equal to the Pawn component of the playerPawn object.
        playerController.GetComponent<Controller>().pawn = playerPawn.GetComponent<Pawn>();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject RandomRoomPrefab() 
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public int DateToInt(DateTime dateToUse)
    {
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    public void GenerateMap()
    {
        // Picks a map based on the exact millisecond.
        
        UnityEngine.Random.InitState(mapSeed);
        if (usingCurrentTime)
        {
            UnityEngine.Random.InitState(DateToInt(DateTime.Now));
        }
        // Picks a map based on the date (day based)
        else if (isMapOfTheDay)
        {
            UnityEngine.Random.InitState(DateToInt(DateTime.Now.Date));
        }

        // Clear out the grid - "column" is our X, "row" is our Y.
        grid = new Room[rows, cols];

        //For each grid row
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            
            //For each column in that row
            for (int currentCol = 0; currentCol < cols; currentCol++)
            {
                //Figure out the location.
                float xPosition = roomWidth * currentCol;
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition = new Vector3(xPosition, 0, zPosition);

                //Create a new grid at the appropriate location
                GameObject tempRoomObj = Instantiate (RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Set its parent
                tempRoomObj.transform.parent = this.transform;

                // Give it a meaningful name
                tempRoomObj.name = "Room_" + currentCol + "," + currentRow;

                // Get the room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                //Save it to the grid array
                grid[currentCol, currentRow] = tempRoom;

                //Open vertical facing doors based on row number
                if (currentRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currentRow == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                if (currentCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currentCol == cols - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else 
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }

            }
        }
    }

    public void ToggleMapOfTheDay()
    {
        if (isMapOfTheDay)
        {
            usingCurrentTime = true;
            isMapOfTheDay = false;
        }
        else
        {
            usingCurrentTime = false;
            isMapOfTheDay = true;
        }
    }
}
