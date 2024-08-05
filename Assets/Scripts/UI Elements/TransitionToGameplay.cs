using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToGameplay : MonoBehaviour
{
    public void GoToGameplay()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateGameplayState();
        }
    }
}
