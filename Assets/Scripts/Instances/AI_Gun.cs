using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Gun : Gun
{
    protected override void OnEnable()
    {
        UIAnimationEndedSO.OnUIAnimationEnded += OnUIAnimationEnded;
    }

    protected virtual void OnDisable()
    {
        UIAnimationEndedSO.OnUIAnimationEnded -= OnUIAnimationEnded;
    }
    public override void AnimationEnded()
    {
        //
    }
}
