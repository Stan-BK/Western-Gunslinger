#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/RoundStartStop")]
public class RoundStartStopSO : ScriptableObject
{
    public UnityAction<bool, Dictionary<OperatorOption, bool>?> OnRoundStartStop;
    
    public void RoundStartStop(bool isStart, Dictionary<OperatorOption, bool> AvailableOptions = null)
    {
        OnRoundStartStop?.Invoke(isStart, AvailableOptions);
    }
}
