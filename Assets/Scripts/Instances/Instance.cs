using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instance: MonoBehaviour
{
    protected int loadedBullets;
    protected int ultimateBulletCount = 6;
    protected bool isDead;
    protected OperatorOption currentStatus;

    public abstract void Operator(OperatorOption option);
    protected abstract void Shoot();
    protected abstract void Defend();
    protected abstract void Load();

    protected abstract void SwitchStatus(OperatorOption option);
}