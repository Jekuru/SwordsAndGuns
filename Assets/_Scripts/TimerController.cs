using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{

    public TMP_Text timer;
    public float roundTime;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        timer.text = TimeSpan.FromSeconds(roundTime).ToString(@"m\:ss");
        currentTime = roundTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < 1f)
        {
            timer.text = "0:00";
            timer.color = Color.red;
        }
        else
        {
            currentTime -= 1 * Time.deltaTime;
            timer.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
        }
    }
}
