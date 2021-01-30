using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsMenu : MonoBehaviour
{
    public static void OnMusicValueChanged()
    {
        
    }
    public static void OnAmbienceValueChanged()
    {
        
    }
    public static void OnStepsValueChanged()
    {
        
    }
    public static void OnScreamsValueChanged()
    {

    }

    [System.Obsolete]
    public void ButtonBack()
    {
        Application.LoadLevel(0);
    }
}
