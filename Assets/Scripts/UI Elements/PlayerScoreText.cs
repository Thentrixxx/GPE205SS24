using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreText : MonoBehaviour
{
    public TMP_Text scoreText;
    public TankPawn tankPawn;

    public void Update()
    {
        float score = tankPawn.controller.score;

        scoreText.text = score.ToString();
    }
}
