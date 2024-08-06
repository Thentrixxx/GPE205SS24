using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Health enemyHealth;
    public Image healthBar;

    public void Update()
    {
        float healthRatio = enemyHealth.currentHealth / enemyHealth.maxHealth;
        healthBar.fillAmount = healthRatio;
    }

}
