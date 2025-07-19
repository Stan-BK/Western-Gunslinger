using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class GuidanceManager : Singleton<GuidanceManager>
{
    public GuidancesSO GuidanceSO;
    public GameObject Shadow;
    public GameObject GuidanceObject;
    public WaitGuidance WaitForGuidance = new WaitGuidance();

    public static int GuidancesLen
    {
        get;
        private set;
    }

    private Guidance[] Guidances;
    private GameObject currentGuidanceGO;
    private IEnumerator<Guidance> _enumerator;
    private bool isGuideEnded;
    
    protected override void Awake()
    {
        base.Awake();
        Guidances = GuidanceSO.Guidances.ToArray();
        GuidancesLen = Guidances.Length;
    }

    public void PlayerGuidance(Guidance guidance)
    {
        Time.timeScale = 0;
        currentGuidanceGO = Instantiate(GuidanceObject, GameObject.Find(guidance.Name).transform);
        currentGuidanceGO.GetComponent<RectTransform>().anchoredPosition = guidance.Pos;
        currentGuidanceGO.GetComponent<TMP_Text>().text = guidance.Text;
        Shadow.SetActive(true);

        ClickHandler.SetInteractiveOnly(GameObject.Find(guidance.InteractiveOptionType.ToString()).GetComponent<Button>());
    }

    public void ExitGuidance()
    {
        if (_enumerator is null) return;
        Destroy(currentGuidanceGO);
        if (_enumerator.Current.AutoPlay && !isGuideEnded)
        {
            WaitForGuidance.PlayerNextGuidance();
            ClickHandler.InteractiveAll();
            return;
        }
        Time.timeScale = 1;
        Shadow.SetActive(false);
        ClickHandler.InteractiveAll();
    }

    public IEnumerator StartGuidance()
    {
        _enumerator = GuidanceSO.Guidances.GetEnumerator();
        while (_enumerator.MoveNext())
        {
            yield return WaitForGuidance;
            PlayerGuidance(_enumerator.Current);
            WaitForGuidance = new WaitGuidance();
        }
        isGuideEnded = true;
#if UNITY_EDITOR
        UnityEngine.PlayerPrefs.SetInt("isNewPlayer", 1);
#else
        WX.StorageSetIntSync("isNewPlayer", 1);
#endif
    }

    public class WaitGuidance : IEnumerator
    {
        bool _canPlayNext = false;
        public bool MoveNext()
        {
            return !_canPlayNext;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public object Current { get; }

        public void PlayerNextGuidance()
        {
            _canPlayNext = true;
        } 
    }
}

[Serializable]
public struct Guidance
{
    public string Name;
    public Vector2 Pos;
    public string Text;
    public OperatorOption InteractiveOptionType;
    public bool AutoPlay;
}
