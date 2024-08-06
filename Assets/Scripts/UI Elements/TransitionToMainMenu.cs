using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToMainMenu : MonoBehaviour
{
    

    public void GoToMainMenu()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateMainMenuScreen();
        }
    }

}
