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

    [SerializeField]private List<GameObject> GamingOptionUI;
    private Dictionary<OperatorOption, GameObject> GamingOptionUIDic = new Dictionary<OperatorOption, GameObject>();

    private void Awake()
    {
        foreach (var ui in GamingOptionUI)
        {
            OperatorOption optionType = ui.GetComponent<OperationUI>().GetOperationType();
            GamingOptionUIDic.Add(optionType, ui);
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
            GameObject ui;
            GamingOptionUIDic.TryGetValue(key, out ui);
            ui?.SetActive(Player.AvailableOptions[key]);
        }
    }
}
