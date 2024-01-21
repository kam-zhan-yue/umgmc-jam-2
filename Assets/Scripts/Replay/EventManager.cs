using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using MoreMountains.CorgiEngine;
using Sirenix.OdinInspector;
using UnityEngine;

public class EventManager : MonoBehaviour, IEventManager
{
    private Action<int> _onDeath;
    private Action _onStartLevel;
    private Action _onGoalReached;
    private Action _onRestartLevel;
    private Action<float> _onRealitySwitch;
    private Action _onCheckpoint;
    private int _deathCount = 0;

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
        LevelManager.Instance.onCheckpointSaved += CheckpointSaved;
    }

    private void CheckpointSaved() => _onCheckpoint?.Invoke();

    /// <summary>
    /// This is hooked to LevelManager OnPlayerDeath
    /// </summary>
    public void OnPlayerDeath()
    {
        Death();
    }

    [Button]
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
        if (LevelManager.Instance && LevelManager.Instance.Players.Count > 0)
        {
            LevelManager.Instance.Players[0].onRespawn.RemoveListener(OnPlayerRespawn);
        }
        LevelManager.Instance.onCheckpointSaved -= CheckpointSaved;
    }


    public void SubGoalReached(Action action) => _onGoalReached += action;
    public void UnSubGoalReached(Action action) => _onGoalReached -= action;
    public void SubRestartLevel(Action action) => _onRestartLevel += action;
    public void UnSubRestartLevel(Action action) => _onRestartLevel -= action;
    public void SubStartLevel(Action action) => _onStartLevel += action;
    public void UnSubStartLevel(Action action) => _onStartLevel -= action;
    public void SubDeath(Action<int> action) => _onDeath += action;
    public void UnSubDeath(Action<int> action) => _onDeath -= action;
    public void SubRealitySwitch(Action<float> action) => _onRealitySwitch += action;
    public void UnSubRealitySwitch(Action<float> action) => _onRealitySwitch -= action;

    public void Death() => _onDeath?.Invoke(++_deathCount);
    public void GoalReached() => _onGoalReached?.Invoke();
    public void RestartLevel() => _onRestartLevel?.Invoke();
    public void StartLevel() => _onStartLevel?.Invoke();
    public void RealitySwitch(float duration) => _onRealitySwitch?.Invoke(duration);
    public void SubCheckpoint(Action action) => _onCheckpoint += action;
    public void UnSubCheckpoint(Action action) => _onCheckpoint -= action;
}
