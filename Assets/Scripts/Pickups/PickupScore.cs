using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScore : MonoBehaviour
{
    public PowerupScore scorePowerup;
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

        GameManager.instance.powerupSound.Play();

        // If the other object has a powerup manager
        if (powerupManager != null)
        {
            // Add the powerup
            powerupManager.Add(scorePowerup);

            //Destroy this pickup
            Destroy(gameObject);
        }
    }
}
