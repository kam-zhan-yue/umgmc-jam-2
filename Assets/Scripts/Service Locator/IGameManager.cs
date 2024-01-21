using System;

public interface IGameManager : IGameService
{
    public float TotalRunTime();
    public float FastestRunTime();
    public void SubFastestRun(Action action);
    public void UnSubFastestRun(Action action);

}