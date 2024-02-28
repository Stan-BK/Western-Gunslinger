using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player: Instance, IInformation
{
    public PlayerInputControl InputControl;
    public Gun Gun;
    public UnityAction OnOperationSelected;
    public TimeOverSO TimeOverSO;
    public bool isActive;
    
    public Dictionary<OperatorOption, bool> AvailableOptions = new Dictionary<OperatorOption, bool>();

    #region 生命周期函数

    void Awake()
    {
        Init();
        
        InputControl = new PlayerInputControl();

        InputControl.Player.Load.started += OnLoadBullet;
        InputControl.Player.Defend.started += OnDefend;
        InputControl.Player.Fire.started += OnShoot;
    }

    private void OnEnable()
    {
        TimeOverSO.OnTimeOver += OnTimeOver;
        InputControl.Enable();
    }

    private void OnDisable()
    {
        TimeOverSO.OnTimeOver -= OnTimeOver;
        InputControl.Disable();
    }

    #endregion

    #region 键盘响应函数
    
    private void OnLoadBullet(InputAction.CallbackContext obj)
    {
        Operator(OperatorOption.LOAD);
    } 

    private void OnDefend(InputAction.CallbackContext obj)
    {
        Operator(OperatorOption.LOAD);
    } 
    
    private void OnShoot(InputAction.CallbackContext obj)
    {
        Operator(OperatorOption.SHOOT);
    }
    
    #endregion

    #region 行为函数

    public override void Operator(OperatorOption option)
    {
        switch (option)
        {
            case OperatorOption.LOAD : Load(); break;
            case OperatorOption.DEFEND: Defend(); break;
            case OperatorOption.SHOOT: Shoot(); break;
            case OperatorOption.ULTIMATE_SHOOT: UltimateShoot(); break;
        }        
        
        OnOperationSelected?.Invoke();
        isActive = true;
    }

    protected override void UltimateShoot()
    {
        if (loadedBullets != ultimateBulletCount)
        {
            Shoot();
            return;
        }
        Gun.Operator(OperatorOption.ULTIMATE_SHOOT);
        SwitchStatus(OperatorOption.ULTIMATE_SHOOT);
        
        loadedBullets = 0;
        
        AvailableOptions[OperatorOption.ULTIMATE_SHOOT] = false;
        AvailableOptions[OperatorOption.SHOOT] = false;
    }

    protected override void Shoot()
    {
        if (loadedBullets == 0)
        {
            Load();
            return;
        }
        Gun.Operator(OperatorOption.SHOOT);
        SwitchStatus(OperatorOption.SHOOT);
        
        loadedBullets -= 1;
        
        AvailableOptions[OperatorOption.ULTIMATE_SHOOT] = false;
        if (loadedBullets == 0)
        {
            AvailableOptions[OperatorOption.SHOOT] = false;
        }
    }

    protected override void Defend()
    {
        Gun.Operator(OperatorOption.DEFEND);
        SwitchStatus(OperatorOption.DEFEND);
    }

    protected override void Load()
    {
        Gun.Operator(OperatorOption.LOAD);
        SwitchStatus(OperatorOption.LOAD);
        AvailableOptions[OperatorOption.SHOOT] = true;
        loadedBullets += 1;
        if (loadedBullets == ultimateBulletCount)
            AvailableOptions[OperatorOption.ULTIMATE_SHOOT] = true;
    }

    #endregion

    #region 状态相关函数

    public void Init()
    {
        isActive = true;
        isDead = false;
        foreach (var Operator in Enum.GetNames(typeof(OperatorOption)))
        {
            AvailableOptions.Add(Enum.Parse<OperatorOption>(Operator), true);
        }

        AvailableOptions[OperatorOption.ULTIMATE_SHOOT] = false;
        AvailableOptions[OperatorOption.SHOOT] = false;
    }
    
    public OperatorOption GetCurrentStatus()
    {
        return currentStatus;
    }

    public int GetLoadedBullets()
    {
        return loadedBullets;
    }

    public bool isActiveSelect => isActive;

    public IInformation GetPlayerInfo()
    {
        return this;
    }

    protected override void SwitchStatus(OperatorOption option)
    {
        currentStatus = option;
    }

    private void OnTimeOver()
    {
        isActive = false;
        Operator(OperatorOption.ULTIMATE_SHOOT);
    }
    #endregion
    
}
