using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource clickSource;
    public void playClick()
    {
        clickSource = GameManager.instance.clickSound;
        clickSource.Play();
    }
}
