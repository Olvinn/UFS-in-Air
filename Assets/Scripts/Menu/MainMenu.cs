using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [System.Obsolete]
    public void JoinGame()
    {
        Application.LoadLevel(3);
    }

    [System.Obsolete]
    public void VolumeSettings()
    {
        Application.LoadLevel(1);
    }

    [System.Obsolete]
    public void Exit()
    {
        Application.LoadLevel(2);
    }
}
