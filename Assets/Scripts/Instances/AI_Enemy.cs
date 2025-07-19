using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI_Enemy : Player
{
    public Animator EnemyAnimator;
    public UIAnimationEndedSO UIAnimationEndedSO; // 枪支动画开始于UI动画结束之后
    
    protected override void OnEnable()
    {
        base.OnEnable();
        UIAnimationEndedSO.OnUIAnimationEnded += OnUIAnimationEnded;
    }

    protected virtual void OnDisable()
    {
        base.OnDisable();
        UIAnimationEndedSO.OnUIAnimationEnded -= OnUIAnimationEnded;
    }

    protected override void OnTimeOver()
    {
        //
    }

    public new void Operator(OperatorOption? option)
    {
        if (option is null)
        {
            var types = Enum.GetValues(typeof(OperatorOption));
            int val = Random.Range(1, types.Length);
            option = (OperatorOption)val;
        }

        base.Operator((OperatorOption)option);
    }

    protected override void OnGameStartStop(bool isStart)
    {
        if (isStart)
        {
            Init();
            EnemyAnimator.SetBool("isDead", isDead);
            Gun.Recover();
        }
        else
        {
            if (GameManager.Instance.isPlayerWin)
            {
                isDead = true;
                EnemyAnimator.SetBool("isDead", isDead);
                Gun.Dead();
            }
            else
            {
                TriggerStatusCallBack();
            }
        }
    }
    
    protected void OnUIAnimationEnded()
    {
        if (GameManager.Instance.isPlaying)
            TriggerStatusCallBack();
    }
    
    void TriggerStatusCallBack()
    {
        switch (currentStatus)
        {
            case OperatorOption.LOAD : EnemyAnimator.SetTrigger("Load"); break;
            case OperatorOption.SHOOT: EnemyAnimator.SetTrigger("Shoot"); break;
            case OperatorOption.DEFEND: EnemyAnimator.SetTrigger("Defend"); break;
            case OperatorOption.ULTIMATE_SHOOT: EnemyAnimator.SetTrigger("Shoot"); break;
        }
    }
}
