using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    public void MainMenu()
    {
        GameController.instance.DisconnectFromRoom();
        GameController.instance.LoadMainMenu();
    }
}
