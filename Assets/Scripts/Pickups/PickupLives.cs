using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLives : MonoBehaviour
{
    public PowerupLives livesPowerup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        // Variable to store other object's Powerup Manager
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();

        // If the other object has a powerup manager
        if (powerupManager != null)
        {
            // Add the powerup
            powerupManager.Add(livesPowerup);

            //Destroy this pickup
            Destroy(gameObject);
        }
    }
}
