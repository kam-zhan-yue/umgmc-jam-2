using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class TimeRunnerManager : MonoBehaviour, IGameManager
{
    [SerializeField] private Transform originalCheckpoint;
    [SerializeField] private Vector3 offset;
    
    private float _runTime = 0f;
    private float _counter = 0f;
    private bool _counting = false;
    private float _fastestRunTime = -1f;

    private Action _onSaveFastestRun;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
    }

    private void Start()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.SubStartLevel(OnStartLevel);
        eventManager.SubDeath(OnDeath);
        eventManager.SubGoalReached(OnGoalReached);
        eventManager.SubCheckpoint(OnCheckpoint);
        eventManager.SubRestartLevel(OnRestartLevel);
    }
    
    private void Update()
    {
        if (_counting)
        {
            _counter += Time.deltaTime;
        }
    }

    private void SaveRunTime()
    {
        _runTime += _counter;
        _counter = 0f;
    }

    public float TotalRunTime() => _counter + _runTime;
    public float FastestRunTime() => _fastestRunTime;

    public void SubFastestRun(Action action) => _onSaveFastestRun += action;

    public void UnSubFastestRun(Action action) => _onSaveFastestRun -= action;

    private void OnStartLevel()
    {
        _counting = true;
    }

    private void OnDeath(int deaths)
    {
        _counter = 0f;
        _counting = false;
    }

    private void OnGoalReached()
    {
        _counting = false;
        SaveRunTime();
        SetFastestRunTime();
        _runTime = 0f;
        _counter = 0f;
    }

    private void OnCheckpoint()
    {
        SaveRunTime();
    }

    private void OnRestartLevel()
    {
        LevelManager.Instance.Players[0].transform.position = originalCheckpoint.position + offset;
    }

    private void SetFastestRunTime()
    {
        if (_fastestRunTime < 0f)
        {
            _fastestRunTime = _runTime;
            _onSaveFastestRun?.Invoke();
        }
        else if(_fastestRunTime < _runTime)
        {
            _fastestRunTime = _runTime;
            _onSaveFastestRun?.Invoke();
        }
    }

    private void OnDestroy()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.UnSubStartLevel(OnStartLevel);
        eventManager.UnSubDeath(OnDeath);
        eventManager.UnSubGoalReached(OnGoalReached);
        eventManager.UnSubCheckpoint(OnCheckpoint);
        eventManager.UnSubRestartLevel(OnRestartLevel);
    }
}
