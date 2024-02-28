using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/TimeOver")]
public class TimeOverSO : ScriptableObject
{
    public UnityAction OnTimeOver;

    public void TimeOver()
    {
        OnTimeOver?.Invoke();
    }
}
