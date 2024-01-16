public interface IRealityManager : IGameService
{
    public void InitReality(Timeline timeline);
    public bool TryGetReality(Timeline timeline, out Reality reality);
    public void SetVisible(Timeline timeline);
}