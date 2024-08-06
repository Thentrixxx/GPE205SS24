using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Health playerHealth;
    public Image healthBar;

    public void Update()
    {
        float healthRatio = playerHealth.currentHealth / playerHealth.maxHealth;
        healthBar.fillAmount = healthRatio;
    }
}
