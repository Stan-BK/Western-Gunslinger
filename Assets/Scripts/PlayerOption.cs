using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerOption: MonoBehaviour
{
    protected int loadedBullets;
    protected int ultimateBulletCount = 6;
    protected bool isDead;
    protected OperatorOption currentStatus;

    public Dictionary<OperatorOption, bool> AvailableOptions = new Dictionary<OperatorOption, bool>();

    public abstract void Operator(OperatorOption option);
    protected abstract void Shoot();
    protected abstract void Defend();
    protected abstract void Load();

    protected virtual void Awake()
    {
        Init();
    }

    public void Init()
    {
        loadedBullets = 0;
        isDead = false;
        foreach (var Operator in Enum.GetNames(typeof(OperatorOption)))
        {
            AvailableOptions.Add(Enum.Parse<OperatorOption>(Operator), true);
        }

        AvailableOptions[OperatorOption.SHOOT] = false;
    }
    
    public Dictionary<OperatorOption, bool> GetCurrentStatus()
    {
        return AvailableOptions;
    }
}