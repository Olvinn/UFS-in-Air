using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public void YesButton()
    {
        Application.Quit();
    }

    public void NoButton()
    {
        Application.LoadLevel(0);
    }
}
