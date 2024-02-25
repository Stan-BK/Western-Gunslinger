using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Countdown : MonoBehaviour
{
    public bool isTimeOver = false;
    [HideInInspector]public float RoundCountTime;
    public RoundStartStopSO RoundStartStopSO;
    public UnityAction TimeOver;
    public TMP_Text TextMeshPro;
    private float timer = 0;
    private bool isPlaying = false;
    private Color textColor;
    
    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            TextMeshPro.text = ((int)(RoundCountTime - timer)).ToString() + "S";
            float offset = 1 - (timer / RoundCountTime);
            TextMeshPro.color = new Color(Color.red.r * (1 - offset) + textColor.r * offset, Color.red.g * (1 - offset) + textColor.g * offset, Color.red.b * (1 - offset) + textColor.b * offset, textColor.a) ;
            if (timer >= RoundCountTime)
            {
                timer = 0;
                isTimeOver = true;
                isPlaying = false;
            }
        }
    }

    private void Awake()
    {
        TextMeshPro.text = ((int)(RoundCountTime - timer)).ToString() + "S";
        textColor = TextMeshPro.color;
    }

    private void OnEnable()
    {
        RoundStartStopSO.OnRoundStartStop += OnRoundStartStop;
    }

    private void OnDisable()
    {
        RoundStartStopSO.OnRoundStartStop -= OnRoundStartStop;
    }

    private void OnRoundStartStop(bool isStart)
    {
        if (isStart)
        {
            isPlaying = true;
            isTimeOver = false;
            TextInit();
        }
        else
        {
            isPlaying = false;
        }
    }

    void TextInit()
    {
        TextMeshPro.text = (int)RoundCountTime + "S";
        TextMeshPro.color = textColor;
    }
}
