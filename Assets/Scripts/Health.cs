using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount, Pawn source)
    {
        currentHealth -= amount;
        Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);
        currentHealth = Mathf.Clamp (currentHealth, 0, maxHealth);
        GameManager.instance.hitSound.Play();

        if (currentHealth <= 0)
        {
            GameManager.instance.deathSound.Play();
            Die(source);
        }
    }

    public void Heal(float amount, Pawn source)
    {
        currentHealth += amount;

        Debug.Log(source.name + " did " + amount + " healing to " + gameObject.name);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Die(Pawn source)
    {
        PlayerController controller = gameObject.GetComponent<Pawn>().controller as PlayerController;
        int scoreToAdd = gameObject.GetComponent<Pawn>().rewardPoints;

        /*source.controller.AddToScore(scoreToAdd);*/

        source.controller.AddToScore(gameObject.GetComponent<TankPawn>().rewardPoints);

        source.controller.RemoveFromLives(1);

        Destroy(source);

        //TODO
        if (controller != null)
        {
            // Spawnpoint Transforms from the Game Manager
            Transform playerSpawnPoint = GameManager.instance.playerSpawnPoint;
            Transform playerTwoSpawnPoint = GameManager.instance.playerTwoSpawnPoint;

            //Controller Prefabs from the Game Manager
            GameObject playerControllerPrefab = GameManager.instance.playerControllerPrefab;
            GameObject playerOneControllerPrefab = GameManager.instance.playerOneControllerPrefab;
            GameObject playerTwoControllerPrefab = GameManager.instance.playerTwoControllerPrefab;

            // Pawn Prefabs from the Game Manager
            GameObject playerPawnPrefab = GameManager.instance.playerPawnPrefab;
            GameObject playerOnePawnPrefab = GameManager.instance.playerOnePawnPrefab;
            GameObject playerTwoPawnPrefab = GameManager.instance.playerTwoPawnPrefab;



            //If the Game is Two Player
            if (GameManager.instance.isTwoPlayer)
            {
                // If this is Player One
                if (controller.isPlayerOne)
                {
                    if (controller.lives > 0)
                    {
                        GameManager.instance.SpawnPlayer(playerSpawnPoint, playerOneControllerPrefab, playerOnePawnPrefab);
                        controller.lives -= 1;
                    }
                    else
                    {
                        
                    }
                }

                // If this is Player Two
                else
                {
                    if (controller.lives > 0)
                    {
                        GameManager.instance.SpawnPlayer(playerTwoSpawnPoint, playerTwoControllerPrefab, playerTwoPawnPrefab);
                        controller.lives -= 1;
                    }
                    else
                    {
                        
                    }
                }
            }

            // If the Game is not Two Player
            else
            {
                if (controller.lives > 0)
                {
                    GameManager.instance.SpawnPlayer(playerSpawnPoint, playerControllerPrefab, playerPawnPrefab);
                    controller.lives -= 1;
                }
                else
                {
                    GameManager.instance.ActivateGameOverScreen();
                }
            }
        }

    }
}
