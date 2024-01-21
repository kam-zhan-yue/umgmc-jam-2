using System;

public interface IEventManager : IGameService
{
    public void SubGoalReached(Action action);
    public void UnSubGoalReached(Action action);
    public void SubRestartLevel(Action action);
    public void UnSubRestartLevel(Action action);
    public void SubStartLevel(Action action);
    public void UnSubStartLevel(Action action);
    public void SubDeath(Action<int> action);
    public void UnSubDeath(Action<int> action);
    public void SubRealitySwitch(Action<float> action);
    public void UnSubRealitySwitch(Action<float> action);
    public void RealitySwitch(float duration);
}