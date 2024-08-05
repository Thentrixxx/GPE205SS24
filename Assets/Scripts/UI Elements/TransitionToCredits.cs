using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToCredits : MonoBehaviour
{
    public void GoToCredits()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateCreditsScreen();
        }
    }
}
