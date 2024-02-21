public class Player: PlayerOption
{
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
        SwitchStatus(OperatorOption.LOAD);
        loadedBullets += 1;
    }

    void SwitchStatus(OperatorOption option)
    {
        currentStatus = option;
    }
}
