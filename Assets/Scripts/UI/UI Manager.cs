using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public GameObject Gaming;
    public GameObject Menu;
    public Player Player;
    public RoundStartStopSO RoundStartStopSo;
    public GameStartStopSO GameStartStopSO;

    private List<GameObject> GamingOptionUI;
    public Dictionary<OperatorOption, GameObject> GamingOptionUIDic;

    private void Awake()
    {
        foreach (var ui in GamingOptionUI)
        {
            OperatorOption optionType = ui.GetComponent<OperationUI>().GetOperationType();
            GamingOptionUIDic[optionType] = ui;
        }
    }

    private void OnEnable()
    {
        RoundStartStopSo.OnRoundStartStop += OnRoundStartStop;
        GameStartStopSO.OnGameStartStop += OnGameStartStop;
    }
    private void OnDisable()
    {
        RoundStartStopSo.OnRoundStartStop -= OnRoundStartStop;
        GameStartStopSO.OnGameStartStop -= OnGameStartStop;
    }

    private void OnGameStartStop(bool isStart)
    {
        if (isStart)
            ShowGamingUI();
        else
            ShowMenuUI();
    }

    private void OnRoundStartStop(bool isStart)
    {
        GamingOptionFilter();
    }

    public void ShowGamingUI()
    {
        Menu.SetActive(false);
        Gaming.SetActive(true);
    }

    public void ShowMenuUI()
    {
        Menu.SetActive(true);
        Gaming.SetActive(false);
    }

    public void GamingOptionFilter()
    {
        foreach (var Operator in Enum.GetNames(typeof(OperatorOption)))
        {
            var key = Enum.Parse<OperatorOption>(Operator);
            GamingOptionUIDic[key].SetActive(Player.AvailableOptions[key]);
        }
    }
}
