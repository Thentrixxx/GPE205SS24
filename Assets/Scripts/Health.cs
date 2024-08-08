using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (source.controller == null) {
            Debug.Log("Controller is null");
        }
        source.controller.AddToScore(gameObject.GetComponent<Pawn>().rewardPoints);

        controller.RemoveFromLives(1);

        Destroy(gameObject);

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
                        GameManager.instance.SpawnPlayer(playerSpawnPoint, controller, playerOnePawnPrefab);
                        GameManager.instance.setEnemyAITarget();
                    }
                    else
                    {
                        // Checks if there are 2 players dead. If so, end level. If not, continue.
                        GameManager.instance.howManyPlayersDead();
                    }
                }

                // If this is Player Two
                else
                {
                    if (controller.lives > 0)
                    {
                        GameManager.instance.SpawnPlayer(playerTwoSpawnPoint, controller, playerTwoPawnPrefab);
                        GameManager.instance.setEnemyAITarget();
                    }
                    else
                    {
                        // Checks if there are 2 players dead. If so, end level. If not, continue.
                        GameManager.instance.howManyPlayersDead();
                    }
                }
            }

            // If the Game is not Two Player
            else
            {
                if (controller.lives > 0)
                {
                    GameManager.instance.SpawnPlayer(playerSpawnPoint, controller, playerPawnPrefab);
                    GameManager.instance.setEnemyAITarget();
                }
                else
                {
                    Debug.Log("Tank Died");
                    GameManager.instance.onePlayerReset();
                }
            }
        }

    }
}
