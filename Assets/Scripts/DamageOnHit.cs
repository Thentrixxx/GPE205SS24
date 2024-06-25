using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damageDone;
    public Pawn owner;

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
        // Get the Health Component of the other game object
        Health otherHealth = other.gameObject.GetComponent<Health>();

        // Only do damage if the health component exists
        if (otherHealth != null )
        {
            // Do damage
            otherHealth.TakeDamage(damageDone, owner);
        }

        // Destroy damaging object
        Destroy(gameObject);
    }
}
