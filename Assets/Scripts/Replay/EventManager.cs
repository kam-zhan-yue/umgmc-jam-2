using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class EventManager : MonoBehaviour, IEventManager
{
    private Action _onDeath;
    private Action _onStartLevel;
    private Action _onGoalReached;
    private Action _onRestartLevel;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IEventManager>(this);
    }
    
    /// <summary>
    /// This is hooked to LevelManager OnSpawnAtCheckPoint
    /// </summary>
    public void OnPlayerSpawned()
    {
        LevelManager.Instance.Players[0].onRespawn.AddListener(OnPlayerRespawn);
    }

    /// <summary>
    /// This is hooked to LevelManager OnPlayerDeath
    /// </summary>
    public void OnPlayerDeath()
    {
        Death();
    }

    public void TutorialOver()
    {
        StartLevel();
    }

    private void OnPlayerRespawn()
    {
        StartLevel();
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

    private void OnDisable()
    {
        LevelManager.Instance.Players[0].onRespawn.RemoveListener(OnPlayerRespawn);
    }


    public void SubGoalReached(Action action) => _onGoalReached += action;
    public void UnSubGoalReached(Action action) => _onGoalReached -= action;
    public void SubRestartLevel(Action action) => _onRestartLevel += action;
    public void UnSubRestartLevel(Action action) => _onRestartLevel -= action;
    public void SubStartLevel(Action action) => _onStartLevel += action;
    public void UnSubStartLevel(Action action) => _onStartLevel -= action;
    public void SubDeath(Action action) => _onDeath += action;
    public void UnSubDeath(Action action) => _onDeath -= action;
    public void Death() => _onDeath?.Invoke();
    public void GoalReached() => _onGoalReached?.Invoke();
    public void RestartLevel() => _onRestartLevel?.Invoke();
    public void StartLevel() => _onStartLevel?.Invoke();
}
