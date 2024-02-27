using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("控制UI")]
    public GameObject GamingUI;
    public GameObject MenuUI;
    public GameObject RoundUI;
    public TMP_Text BulletUI;
    [Header("事件对象")]
    public RoundStartStopSO RoundStartStopSO;
    public GameStartStopSO GameStartStopSO;
    public UIAnimationEndedSO UIAnimationEndedSO;

    public bool isUIAnimationEnded = false;
    [SerializeField]private List<GameObject> GamingOptionUI;
    private Dictionary<OperatorOption, bool> AvailableOptions = new Dictionary<OperatorOption, bool>();
    private Dictionary<OperatorOption, Button> GamingButtons = new Dictionary<OperatorOption, Button>();
    private List<Button> MenuButtons = new List<Button>();

    #region 生命周期函数
    
    protected override void Awake()
    {
        base.Awake();
        foreach (var ui in GamingOptionUI)
        {
            OperatorOption optionType = ui.GetComponent<OperationUI>().GetOperationType();
            var button = ui.GetComponent<Button>();
            GamingButtons.Add(optionType, button);
        }

        foreach (var button in MenuUI.GetComponentsInChildren<Button>())
        {
            MenuButtons.Add(button);
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

    #endregion

    #region 事件函数

    private void OnGameStartStop(bool isStart)
    {
        if (isStart)
        {
            ShowGamingUI();
            EnableMenuButtonInteractive();
        }
        else
            ShowMenuUI();
    }

    private void OnRoundStartStop(bool isStart, [CanBeNull] Dictionary<OperatorOption, bool> dictionary, IInformation playerInformation)
    {
        if (isStart)
        {
            isUIAnimationEnded = false;
            AvailableOptions = dictionary;
            RoundUI.SetActive(true);
            
            GamingOptionFilter();
        }
        else
        {
            var currentBullet = playerInformation.GetLoadedBullets();
            BulletUI.text = currentBullet.ToString();
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

    #endregion
    
    

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

    public void EnableMenuButtonInteractive()
    {
        foreach (var button in MenuButtons)
        {
            button.interactable = true;
        }
    }

    public void GamingOptionFilter()
    {
        foreach (var Operator in AvailableOptions)
        {
            GamingButtons[Operator.Key].interactable = Operator.Value;
        }
    }

}
