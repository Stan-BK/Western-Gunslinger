using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/UIAnimationEnded")]
public class UIAnimationEndedSO : ScriptableObject
{
    public UnityAction OnUIAnimationEnded;

    public void UIAnimationEnded()
    {
        OnUIAnimationEnded?.Invoke();
    }
}
