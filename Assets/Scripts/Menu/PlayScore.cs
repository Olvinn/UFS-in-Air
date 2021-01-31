using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScore : MonoBehaviour
{
    [SerializeField] Text alive, infected, role;
    [SerializeField] GameObject UFSwin, Humwin;

    private void Start()
    {
        GameController.instance.OnHumansWin.AddListener(() => Humwin.SetActive(true));
        GameController.instance.OnUFSWin.AddListener(() => UFSwin.SetActive(true));
    }

    private void Update()
    {
        alive.text = $"Players alive: {GameController.instance.AliveCount()}";
        infected.text = $"UFS: {GameController.instance.UFSCount()}";
        if (GameController.players[Client.instance.id])
            role.text = "Your role: " + (GameController.players[Client.instance.id].isUFS ? "Unknown F.S." : "Human");
    }
}
