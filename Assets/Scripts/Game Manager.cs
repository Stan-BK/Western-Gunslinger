using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public GameObject Timeline;
    private TimelinePreferences timeline;
    public Player Player;
    public RoundStartStopSO RoundStartStopSo;
    public GameStartStopSO GameStartStopSO;
    public float RoundCountTime;
    private float roundTimer = 0;
    private bool isPlaying = false;
    private bool isFirstPlay = true;
    
    private void Update()
    {
        roundTimer += Time.deltaTime;
        if (roundTimer >= RoundCountTime)
        {
            roundTimer = 0;
            RoundSettle();
        }
    }

    public void StartGame()
    {
        if (isFirstPlay)
            // 首次进入游戏有镜头过渡动画，依靠时间线完成后激活标识来开启游戏
            Timeline.SetActive(true);
        else
        {
            GameLoaded();
        }
    }

    public void GameLoaded()
    {
        GameStartStopSO.GameStartStop(true);
        NewRound();
    }

    public void NewRound()
    {
        RoundStartStopSo.RoundStartStop(true);
    }
    
    public void RoundSettle()
    {
        Player.GetCurrentStatus();
        RoundStartStopSo.RoundStartStop(false);
        
        
    }
}
