using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip LoadClip;
    public AudioClip ShootClip;
    public AudioClip UltimateShootClip;
    public RoundStartStopSO RoundStartStopSO;
    public UIAnimationEndedSO UIAnimationEndedSO;
    
    private void OnEnable()
    {
        RoundStartStopSO.OnRoundStartStop += OnRoundStartStop;
        UIAnimationEndedSO.OnUIAnimationEnded += OnUIAnimationEnded;
    }

    private void OnDisable()
    {
        RoundStartStopSO.OnRoundStartStop -= OnRoundStartStop;
        UIAnimationEndedSO.OnUIAnimationEnded -= OnUIAnimationEnded;
    }

    private void OnUIAnimationEnded()
    {
        if (GameManager.Instance.isPlaying)
            PlayAudioClip();
    }

    private void OnRoundStartStop(bool isStart, Dictionary<OperatorOption, bool> arg1, IInformation PlayerInfo)
    {
        if (!isStart)
        {
            SwitchAudioClip(PlayerInfo.GetCurrentStatus());
        }
    }

    private void SwitchAudioClip(OperatorOption option)
    {
        switch (option)
        {
            case OperatorOption.LOAD: AudioSource.clip = LoadClip; break;
            case OperatorOption.SHOOT: AudioSource.clip = ShootClip; break;
            case OperatorOption.ULTIMATE_SHOOT: AudioSource.clip = UltimateShootClip; break;
            case OperatorOption.DEFEND: AudioSource.clip = null; break;
        }
    }

    void PlayAudioClip()
    {
        AudioSource.Play();
    }
}
