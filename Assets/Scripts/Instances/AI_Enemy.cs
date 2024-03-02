using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI_Enemy : Player
{

    public override void Operator(OperatorOption option = OperatorOption.LOAD)
    {
        var types = Enum.GetValues(typeof(OperatorOption));
        int val = Random.Range(0, types.Length);

        base.Operator((OperatorOption)val);
    }

    protected override void OnGameStartStop(bool isStart)
    {
        if (isStart)
        {
            Init();
            Gun.Recover();
        }
        else
        {
            if (GameManager.Instance.isPlayerWin)
            {
                isDead = true;
                Gun.Dead();
            }
        }
    }
}
