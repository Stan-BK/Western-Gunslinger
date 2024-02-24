using UnityEngine.InputSystem;

public class Player: PlayerOption
{
    public PlayerInputControl InputControl;
    public Gun Gun;

    #region 生命周期函数

    protected override void Awake()
    {
        base.Awake();
        
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
    
    public override void Operator(OperatorOption option)
    {
        switch (option)
        {
            case OperatorOption.LOAD : Load(); break;
            case OperatorOption.DEFEND: Defend(); break;
            case OperatorOption.SHOOT: Shoot(); break;
        }        
    }

    protected override void Shoot()
    {
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
        Gun.OnLoadBullet();
        SwitchStatus(OperatorOption.LOAD);
        loadedBullets += 1;
    }

    void SwitchStatus(OperatorOption option)
    {
        currentStatus = option;
    }
}
