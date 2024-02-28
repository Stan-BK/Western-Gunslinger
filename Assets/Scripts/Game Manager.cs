using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    public GameObject Timeline;
    private TimelinePreferences timeline;
    public Player Player;
    public RoundStartStopSO RoundStartStopSo;
    public GameStartStopSO GameStartStopSO;
    public AnimationEndedSO AnimationEndedSO;
    public Countdown Countdown;
    
    public float RoundCountTime;
    private float roundTimer = 0;
    private bool _isPlaying = false;
    private bool isFirstPlay = true;
    private bool isTimeOver = false;
    private IInformation PlayerInformation;
    
    public bool isPlaying
    {
        get => _isPlaying;
    }

    #region 生命周期函数

    protected override void Awake()
    {
        base.Awake();
        Countdown.RoundCountTime = RoundCountTime;
        
        PlayerInformation = Player.GetPlayerInfo();
    }

    private void OnEnable()
    {
        AnimationEndedSO.OnAnimationEnded += OnAnimationEnded;
        Player.OnOperationSelected += OnPlayerOperationSelected;
    }

    private void OnDisable()
    {
        AnimationEndedSO.OnAnimationEnded -= OnAnimationEnded;
        Player.OnOperationSelected -= OnPlayerOperationSelected;
    }
    #endregion

    #region 事件函数

    // 结算动画结束
    private void OnAnimationEnded()
    {
        NewRound(); 
    }

    private void OnPlayerOperationSelected()
    {
        OnRoundEnded();
    }

    #endregion

    public void StartGame()
    {
        if (isFirstPlay)
        {
            isFirstPlay = false;
            // 首次进入游戏有镜头过渡动画，依靠时间线完成后激活标识来开启游戏
            Timeline.SetActive(true);
        }
        else
        {
            GameLoaded();
        }
    }

    public void GameLoaded()
    {
        GameStartStopSO.GameStartStop(true);
        _isPlaying = true;
        NewRound();
    }

    public void NewRound()
    {
        isTimeOver = false;
        RoundStartStopSo.RoundStartStop(true, Player.AvailableOptions);
    }

    private void OnRoundEnded()
    {
        isTimeOver = true;
        RoundSettle();
    }
    
    public void RoundSettle()
    {
        RoundStartStopSo.RoundStartStop(false, PlayerInformation: Player.GetPlayerInfo());

        // NewRound();
    }
}
