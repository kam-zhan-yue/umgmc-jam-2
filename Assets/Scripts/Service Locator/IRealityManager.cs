public interface IRealityManager : IGameService
{
    public void InitReality(Timeline timeline);
    public void RegisterReality(Reality reality);
    public bool TryGetReality(Timeline timeline, out Reality reality);
    public void SetVisible(Timeline timeline);
}