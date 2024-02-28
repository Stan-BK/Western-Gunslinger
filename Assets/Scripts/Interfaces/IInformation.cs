public interface IInformation
{
    bool isActiveSelect { get; }
    OperatorOption GetCurrentStatus();
    int GetLoadedBullets();
}