using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MainTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float timeRemaining = 3;
    private bool timerstarted = false;


    private void Start()
    {
        timerText.text = "4";
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = string.Format("{0:0}", timeRemaining);

        }
        else
        {
            timerText.text = "";
            if (timerstarted == false) {
                Timer.StartTimer();
                CarController.gamestarted();
                CPUCarController.started();
                timerstarted = true;
            }
            
            
        }
    }

}
