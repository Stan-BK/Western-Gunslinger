using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    [Header("控制UI")]
    public GameObject GamingUI;
    public GameObject MenuUI;
    public GameObject RoundUI;
    [Header("事件对象")]
    public RoundStartStopSO RoundStartStopSO;
    public GameStartStopSO GameStartStopSO;
    public UIAnimationEndedSO UIAnimationEndedSO;

    public bool isUIAnimationEnded = false;
    [SerializeField]private List<GameObject> GamingOptionUI;
    private Dictionary<OperatorOption, GameObject> GamingOptionUIDic = new Dictionary<OperatorOption, GameObject>();
    private Dictionary<OperatorOption, bool> AvailableOptions = new Dictionary<OperatorOption, bool>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var ui in GamingOptionUI)
        {
            OperatorOption optionType = ui.GetComponent<OperationUI>().GetOperationType();
            GamingOptionUIDic.Add(optionType, ui);
        }
    }

    private void OnEnable()
    {
        RoundStartStopSO.OnRoundStartStop += OnRoundStartStop;
        GameStartStopSO.OnGameStartStop += OnGameStartStop;
    }
    private void OnDisable()
    {
        RoundStartStopSO.OnRoundStartStop -= OnRoundStartStop;
        GameStartStopSO.OnGameStartStop -= OnGameStartStop;
    }

    private void OnGameStartStop(bool isStart)
    {
        if (isStart)
            ShowGamingUI();
        else
            ShowMenuUI();
    }

    private void OnRoundStartStop(bool isStart, [CanBeNull] Dictionary<OperatorOption, bool> dictionary)
    {
        if (isStart)
        {
            isUIAnimationEnded = false;
            AvailableOptions = dictionary;
            RoundUI.SetActive(true);
            GamingOptionFilter();
        }
    }

    public void ShowGamingUI()
    {
        MenuUI.SetActive(false);
        GamingUI.SetActive(true);
        RoundUI.SetActive(true); // 防止首次进入游戏时，回合选项UI被UIAnimationEnded事件关闭
    }

    public void ShowMenuUI()
    {
        MenuUI.SetActive(true);
        GamingUI.SetActive(false);
    }

    public void GamingOptionFilter()
    {
        foreach (var Operator in Enum.GetNames(typeof(OperatorOption)))
        {
            var key = Enum.Parse<OperatorOption>(Operator);
            GameObject ui;
            GamingOptionUIDic.TryGetValue(key, out ui);
            ui?.SetActive(AvailableOptions[key]);
        }
    }

    public void UIAnimationEnded()
    {
        // 防止多次通知
        if (isUIAnimationEnded) return;
        isUIAnimationEnded = true;
        RoundUI.SetActive(false);
        UIAnimationEndedSO.UIAnimationEnded();
    }
}
