public interface IInformation
{
    bool isActiveSelect { get; }
    OperatorOption GetCurrentStatus { get; }
    int GetLoadedBullets();
}