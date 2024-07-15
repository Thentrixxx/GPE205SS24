using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject pickupPrefab;
    public float spawnDelay;
    private float nextSpawnTime;
    private Transform tf;
    private GameObject spawnedPickup;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;

        tf = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedPickup == null)
        {
            // if it's time to spawn a pickup
            if (Time.time > nextSpawnTime)
            {
                // Spawn it and set the next time
                spawnedPickup = Instantiate(pickupPrefab, tf.position, Quaternion.identity) as GameObject;

                nextSpawnTime = Time.time + spawnDelay;
            }
        }

        else
        {
            //Otherwise, the object still exists, so postpone the spawn.
            nextSpawnTime = Time.time + spawnDelay;
        }
        
    }
}
