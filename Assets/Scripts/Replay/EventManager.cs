using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class EventManager : MonoBehaviour, IEventManager
{
    private Action _onStartLevel;
    private Action _onGoalReached;
    private Action _onRestartLevel;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IEventManager>(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartLevel();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
                GoalReached();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            RestartLevel();
        }
    }
    
    public void SubGoalReached(Action action) => _onGoalReached += action;
    public void UnSubGoalReached(Action action) => _onGoalReached -= action;
    public void SubRestartLevel(Action action) => _onRestartLevel += action;
    public void UnSubRestartLevel(Action action) => _onRestartLevel -= action;
    public void SubStartLevel(Action action) => _onStartLevel += action;
    public void UnSubStartLevel(Action action) => _onStartLevel -= action;
    public void GoalReached() => _onGoalReached?.Invoke();
    public void RestartLevel() => _onRestartLevel?.Invoke();
    public void StartLevel() => _onStartLevel?.Invoke();
}
