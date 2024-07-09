using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Network;
using UnityEngine;
using UnityEngine.Serialization;
using WeChatWASM;
using Debug = UnityEngine.Debug;

public class GameManager : Singleton<GameManager>
{
    public GameObject Timeline;
    public Player Player;
    public AI_Enemy Enemy;
    public RoundStartStopSO RoundStartStopSo;
    public GameStartStopSO GameStartStopSO;
    public AnimationEndedSO AnimationEndedSO;
    public Countdown Countdown;
    
    public float RoundCountTime;
    private float roundTimer = 0;
    private bool _isPlaying = false;
    private bool isFirstPlay = true;
    private bool _isPlayerWin = false;
    private bool isNeedGuidance = false;
    private IInformation PlayerInformation;
    
    public bool isPlaying
    {
        get => _isPlaying;
    }

    public bool isPlayerWin
    {
        get => _isPlayerWin;
    }

    #region 生命周期函数

    protected override void Awake()
    {
        WX.SetPreferredFramesPerSecond(24);
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
        SettleDown();
        
        if (isPlaying)
            NewRound();
    }

    private void OnPlayerOperationSelected()
    {
        OnRoundEnded();
    }

    #endregion

    public void MultiPlay()
    {
        NetClient.Instance.Init("127.0.0.1", 8888);
        NetClient.Instance.Connect();
    }

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
        isNeedGuidance = PlayerPrefs.GetInt("isNewPlayer") == 0;
        if (isNeedGuidance)
        {
            StartCoroutine(GuidanceManager.Instance.StartGuidance());
            PlayGuidance();
        }
    }

    public void NewRound()
    {
        if (isNeedGuidance)
            PlayGuidance();
        RoundStartStopSo.RoundStartStop(true, Player.AvailableOptions);
    }

    private void OnRoundEnded()
    {
        RoundSettle();
    }
    
    public void RoundSettle()
    {
        // 触发敌人结算
        Enemy.Operator();
        RoundStartStopSo.RoundStartStop(false, PlayerInformation: Player.GetPlayerInfo(), EnemyInformation: Enemy.GetPlayerInfo());
    }

    void SettleDown()
    {
        OperatorOption PlayerStatus = Player.GetCurrentStatus();
        OperatorOption EnemyStatus = Enemy.GetCurrentStatus();

        if (PlayerStatus == OperatorOption.LOAD)
        {
            if (EnemyStatus == OperatorOption.ULTIMATE_SHOOT || EnemyStatus == OperatorOption.SHOOT) GameOver(false);
        } else if (PlayerStatus == OperatorOption.SHOOT)
        {
            if (EnemyStatus == OperatorOption.ULTIMATE_SHOOT) GameOver(false);
            else if (EnemyStatus == OperatorOption.LOAD) GameOver(true);
        } else if (PlayerStatus == OperatorOption.ULTIMATE_SHOOT)
        {
            if (EnemyStatus != OperatorOption.ULTIMATE_SHOOT) GameOver(true);
        }
        else
        {
            if (EnemyStatus == OperatorOption.ULTIMATE_SHOOT) GameOver(false);
        }
    }
    
    void GameOver(bool isPlayerWin)
    {
        _isPlayerWin = isPlayerWin;
        _isPlaying = false;
        GameStartStopSO.GameStartStop(false);
    }

    public void PlayGuidance()
    {
        GuidanceManager.Instance.WaitForGuidance.PlayerNextGuidance();

    }
}
