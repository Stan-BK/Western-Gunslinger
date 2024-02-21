using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/RoundStartStop")]
public class RoundStartStopSO : ScriptableObject
{
    public UnityAction<bool> OnRoundStartStop;
    
    public void RoundStartStop(bool isStart)
    {
        OnRoundStartStop?.Invoke(isStart);
    }
}
