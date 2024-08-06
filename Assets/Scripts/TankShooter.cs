using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TankShooter : Shooter
{
    //Audio Files
    public AudioSource shootSound;

    public Transform firepointTransform;
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifespan)
    {
        // Play the sound
        shootSound.Play();

        // Instantiate bullet
        GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject;

        // Get the DamageOnHit Component
        DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

        // If it exists
        if (doh != null)
        {
            // Apply doh values
            doh.damageDone = damageDone;
            doh.owner = GetComponent<Pawn>();
        }

        // Get the rigidbody component
        Rigidbody rb = newShell.GetComponent<Rigidbody>();

        // if rigidbody exists
        if (rb != null)
        {
            // Add force to the spawned rigidbody
            rb.AddForce(firepointTransform.forward * fireForce);
        }

        // Destroy after a set time
        Destroy(newShell, lifespan);
    }
}
