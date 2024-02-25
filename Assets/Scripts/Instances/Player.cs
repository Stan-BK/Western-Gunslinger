using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player: Instance
{
    public PlayerInputControl InputControl;
    public Gun Gun;
    public UnityAction OnOperationSelected;

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
        InputControl.Enable();
    }

    private void OnDisable()
    {
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
        }        
        
        OnOperationSelected?.Invoke();
    }

    protected override void Shoot()
    {
        Gun.Operator(OperatorOption.SHOOT);
        SwitchStatus(OperatorOption.SHOOT);
        
        if (loadedBullets == ultimateBulletCount)
        {
            loadedBullets = 0;
        }
        else
        {
            loadedBullets -= 1;
        }
        
        if (loadedBullets == 0)
        {
            AvailableOptions[OperatorOption.SHOOT] = false;
        }
    }

    protected override void Defend()
    {
        SwitchStatus(OperatorOption.DEFEND);
    }

    protected override void Load()
    {
        Gun.Operator(OperatorOption.LOAD);
        SwitchStatus(OperatorOption.LOAD);
        loadedBullets += 1;
    }


    #endregion

    #region 状态相关函数

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
    
    public OperatorOption GetCurrentStatus()
    {
        return currentStatus;
    }

    protected override void SwitchStatus(OperatorOption option)
    {
        currentStatus = option;
    }

    #endregion
    
}
