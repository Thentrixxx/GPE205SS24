using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitEditor()
    {
        EditorApplication.isPlaying = false;
    }
        
}
