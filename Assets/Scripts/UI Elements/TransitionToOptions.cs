using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToOptions : MonoBehaviour
{
    public void GoToOptions()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ActivateOptionsScreen();
        }
    }
}
