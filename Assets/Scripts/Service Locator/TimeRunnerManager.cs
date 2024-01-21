using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class TimeRunnerManager : MonoBehaviour, IGameManager
{
    private float _runTime = 0f;
    private float _counter = 0f;
    private bool _counting = false;
    
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
    }

    private void OnCheckpoint()
    {
        Debug.Log("TimeRunnerManager OnCheckpoint");
        SaveRunTime();
    }

    private void OnDestroy()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.UnSubCheckpoint(OnCheckpoint);
    }
}
