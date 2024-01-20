using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class EventManager : MonoBehaviour, IEventManager
{
    private Action _onStartLevel;
    private Action _onGoalReached;
    private Action _onRestartLevel;

    private Recorder recorder;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IEventManager>(this);
    }

    private void Start()
    {
        //LevelManager.Instance.Players[0].SubscribeToCharacterSpawn(OnPlayerSpawned);
        //LevelManager.Instance.Players[0].onPlayerDeath.AddListener(OnPlayerDeath);
    }


    private bool subbed = false;
    private void Update()
    {
        //if(!subbed)
        //{
        //    LevelManager.Instance.onSpawnAtCheckPoint.AddListener(OnRespawn);
        //    LevelManager.Instance.Players[0].onPlayerDeath.AddListener(OnPlayerDeath);
        //    subbed = true;
        //}

        if(recorder == null)
        {
            recorder = LevelManager.Instance.Players[0].GetComponentInChildren<Recorder>();
            LevelManager.Instance.onPlayerDeath.AddListener(OnPlayerDeath);
            LevelManager.Instance.Players[0].onRespawn.AddListener(OnRespawn);
            Debug.Log("Start recording");
            recorder.StartRecording();
        }


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

    private void OnRespawn()
    {
        Debug.Log("OnRespawn");
        recorder.StartReplay();
    }

    private void OnPlayerDeath()
    {
        //RestartLevel();
        //recorder.ResetReplay();
    }

    private void OnDestroy()
    {
        //LevelManager.Instance.Players[0].onPlayerDeath.RemoveListener(OnPlayerDeath);
        //LevelManager.Instance.Players[0].UnsubscribeFromCharacterSpawn(OnPlayerSpawned);
        LevelManager.Instance.onPlayerDeath.RemoveListener(OnPlayerDeath);
        LevelManager.Instance.Players[0].onRespawn.RemoveListener(OnRespawn);

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
