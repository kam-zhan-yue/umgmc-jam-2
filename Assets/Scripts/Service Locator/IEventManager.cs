using System;

public interface IEventManager : IGameService
{
    public void SubGoalReached(Action action);
    public void UnSubGoalReached(Action action);
    public void SubRestartLevel(Action action);
    public void UnSubRestartLevel(Action action);
}