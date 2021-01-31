using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScore : MonoBehaviour
{
    [SerializeField] Text alive, infected, role;

    private void Update()
    {
        alive.text = $"Players alive: {GameController.instance.AliveCount()}";
        infected.text = $"UFS: {GameController.instance.UFSCount()}";
        if (GameController.players[Client.instance.id])
            role.text = "Your role: " + (GameController.players[Client.instance.id].isUFS ? "Unknown F.S." : "Human");
    }
}
