﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimerScript : MonoBehaviour {

    public TextMeshProUGUI gameTimerText;
    float gameTimer = 0f;
    bool isNotPaused = false;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {

        gameTimer += Time.deltaTime;

        int sec = (int)(gameTimer % 60);
        int min = (int)(gameTimer / 60) % 60;
        int hrs = (int)(gameTimer / 3600) % 24;

        string timerString = string.Format("Elapsed time: {0:0}:{1:00}:{2:00}", hrs, min, sec);

        gameTimerText.SetText(timerString);
        
    }
}
