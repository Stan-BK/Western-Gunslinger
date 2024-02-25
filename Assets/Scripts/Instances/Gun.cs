using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : Instance
{
    public bool isShoot;
    public ParticleSystem particleSystem;
    public Animator GunAnimation;
    public RoundStartStopSO RoundStartStopSO;
    public AnimationEndedSO AnimationEndedSO;

    #region 生命周期函数
    
    void Update()
    {
        if (isShoot)
        {
            isShoot = false;
            FireParticle();
        }
    }

    private void OnEnable()
    {
        RoundStartStopSO.OnRoundStartStop += OnRoundStartStop;
    }

    private void OnDisable()
    {
        RoundStartStopSO.OnRoundStartStop -= OnRoundStartStop;
    }

    #endregion

    #region 事件函数
    private void OnRoundStartStop(bool isStart)
    {
        if (!isStart)
            TriggerStatusCallBack();
    }

    public void AnimationEnded()
    {
        AnimationEndedSO.AnimationEnded();
    }

    #endregion


    #region 行为函数
    public override void Operator(OperatorOption option)
    {
        SwitchStatus(option);     
    }
    
    protected override void Shoot()
    {
        GunAnimation.SetTrigger("Shoot");
    }

    protected override void Defend()
    {
        //
    }

    protected override void Load()
    {
        GunAnimation.SetTrigger("Load");
    }
    
    void FireParticle()
    {
        particleSystem.Play();   
    }

    #endregion

    #region 

    protected override void SwitchStatus(OperatorOption option)
    {
        currentStatus = option;
    }

    void TriggerStatusCallBack()
    {
        switch (currentStatus)
        {
            case OperatorOption.LOAD : Load(); break;
            case OperatorOption.SHOOT: Shoot(); break;
        }
    }

    #endregion
    
    
}