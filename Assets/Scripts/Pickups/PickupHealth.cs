using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    public PowerupHealth powerup;
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
        // variable to store other object's Powerup Manager
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();

        // If the other object actually has a Powerup Manager
        if (powerupManager != null)
        {
            // Add the powerup
            powerupManager.Add(powerup);

            // Destroy this pickup
            Destroy(gameObject);
        }
    }
}
