using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instance: MonoBehaviour
{
    protected int loadedBullets = 0;
    [SerializeField] protected int ultimateBulletCount = 6;
    protected bool isDead;
    protected OperatorOption currentStatus;

    public abstract void Operator(OperatorOption option);
    protected abstract void Shoot();
    protected abstract void Defend();
    protected abstract void Load();

    protected abstract void UltimateShoot();

    protected void SwitchStatus(OperatorOption option)
    {
        currentStatus = option;
    }
}