using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerupList;
    private List<Powerup> removedPowerupQueue;

    // Start is called before the first frame update
    void Start()
    {
        powerupList = new List<Powerup>();
        removedPowerupQueue = new List<Powerup>();
    }

    // Update is called once per frame
    void Update()
    {
        DecrementPowerupTimers();
    }

    private void LateUpdate()
    {
        ApplyRemovePowerupsQueue();
    }

    // The Add function will eventually add a powerup
    public void Add (Powerup powerupToAdd)
    {
        powerupToAdd.Apply(this);

        powerupList.Add(powerupToAdd);
    }

    // The Remove function will eventualy have a powerup.
    public void Remove(Powerup powerupToRemove)
    {
        // Reverse the application of the powerup
        powerupToRemove.Remove(this);

        //Remove the powerup from the "to be removed" queue
        removedPowerupQueue.Add(powerupToRemove);
    }

    public void DecrementPowerupTimers()
    {
        foreach (Powerup powerup in powerupList)
        {
            // Subtract the time it took to draw the frame from the duration
            powerup.duration -= Time.deltaTime;

            if (powerup.duration <= 0 && !powerup.isPermanent)
            {
                Remove(powerup);
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        // For each powerup in RemovedPowerupQueue, remove the corresponding powerup in the powerupsList
        foreach (Powerup powerup in removedPowerupQueue)
        {
            powerupList.Remove(powerup);
        }

        // Remove the contents of our temporary powerup list
        removedPowerupQueue.Clear();
    }
}