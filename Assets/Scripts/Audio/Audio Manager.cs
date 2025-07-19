using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource PlayerAudioSource;
    public AudioSource EnemyAudioSource;
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

    private void OnRoundStartStop(bool isStart, Dictionary<OperatorOption, bool> arg1, IInformation PlayerInfo, IInformation EnemyInfo)
    {
        if (!isStart)
        {
            SwitchAudioClip(PlayerAudioSource, PlayerInfo.GetCurrentStatus);
            SwitchAudioClip(EnemyAudioSource, EnemyInfo.GetCurrentStatus);
        }
    }

    private void SwitchAudioClip(AudioSource audioSource, OperatorOption option)
    {
        switch (option)
        {
            case OperatorOption.LOAD: audioSource.clip = LoadClip; break;
            case OperatorOption.SHOOT: audioSource.clip = ShootClip; break;
            case OperatorOption.ULTIMATE_SHOOT: audioSource.clip = UltimateShootClip; break;
            case OperatorOption.DEFEND: audioSource.clip = null; break;
        }
    }

    void PlayAudioClip()
    {
        PlayerAudioSource.Play();
        EnemyAudioSource.Play();
    }
}
