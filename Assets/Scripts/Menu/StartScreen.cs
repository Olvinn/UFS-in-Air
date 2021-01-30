using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] Text playersCount;
    [SerializeField] Button play;

    private void Update()
    {
        playersCount.text = $"{Client.instance.roomCount}/6";

        if (Client.instance.roomCount >= 3)
        {
            play.interactable = true;
            playersCount.color = Color.green;
        }
        else
        {
            play.interactable = false;
            playersCount.color = Color.red;
        }
    }
}
