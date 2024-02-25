using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/AnimationEnded")]
public class AnimationEndedSO : ScriptableObject
{
    public UnityAction OnAnimationEnded;

    public void AnimationEnded()
    {
        OnAnimationEnded?.Invoke();
    }
}
