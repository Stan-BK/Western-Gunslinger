 using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/GameStartStop")]
public class GameStartStopSO : ScriptableObject
{
    public UnityAction<bool> OnGameStartStop;
    
    public void GameStartStop(bool isStart)
    {
        OnGameStartStop?.Invoke(isStart);
    }
}
