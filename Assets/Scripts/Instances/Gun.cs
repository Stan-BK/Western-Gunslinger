using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

// 控制枪支动画行为
public class Gun : Instance
{
    public bool isShoot;
    public ParticleSystem particleSystem;
    public Animator GunAnimation;
    public AnimationEndedSO AnimationEndedSO;
    public UIAnimationEndedSO UIAnimationEndedSO; // 枪支动画开始于UI动画结束之后
    
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
        UIAnimationEndedSO.OnUIAnimationEnded += OnUIAnimationEnded;
    }

    private void OnDisable()
    {
        UIAnimationEndedSO.OnUIAnimationEnded -= OnUIAnimationEnded;
    }

    #endregion

    #region 事件函数
    private void OnUIAnimationEnded()
    {
        if (GameManager.Instance.isPlaying)
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
    
    protected override void UltimateShoot()
    {
        GunAnimation.SetTrigger("Ultimate_Shoot");
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
            case OperatorOption.ULTIMATE_SHOOT: UltimateShoot(); break;
        }
    }

    #endregion
    
    
}