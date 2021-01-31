﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScore : MonoBehaviour
{
    [SerializeField] Text alive, infected;

    private void Update()
    {
        alive.text = $"Players alive: {GameController.instance.AliveCount()}";
        infected.text = $"UFS: {GameController.instance.UFSCount()}";
    }
}