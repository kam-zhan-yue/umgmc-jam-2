using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField] private GameObject replayObjectPrefab;
    private Recording _recording;
    public Queue<ReplayData> recordingQueue { get; private set; } = new();
    private bool isDoingReplay = false;

    private void Start()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.SubGoalReached(OnGoalReached);
        eventManager.SubRestartLevel(OnRestartLevel);
    }

    private void OnGoalReached()
    {
        StartReplay();
    }

    private void OnRestartLevel()
    {
        RestartReplay();
    }

    private void Update()
    {
        if (!isDoingReplay)
        {
            return;
        }

        bool hasNextFrame = _recording.PlayNextFrame();
        
        //Check if we are finished, so that we can restart
        if (!hasNextFrame)
        {
            RestartReplay();
        }
    }

    public void RecordFrame(ReplayData data)
    {
        recordingQueue.Enqueue(data);
        Debug.Log($"Data: {data}");
    }

    private void StartReplay()
    {
        isDoingReplay = true;
        //Initialise recording
        _recording = new Recording(recordingQueue);
        //Reset the current recording queue for next record
        recordingQueue.Clear();
        //Instantiate recording object
        _recording.InstantiateReplayObject(replayObjectPrefab);
    }

    private void RestartReplay()
    {
        isDoingReplay = true;
        _recording.Restart();
    }

    private void ResetReplay()
    {
        isDoingReplay = false;
        _recording.DestroyReplayObject();
        _recording = null;
    }
    
    private void OnDestroy()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.UnSubGoalReached(OnGoalReached);
        eventManager.UnSubRestartLevel(OnRestartLevel);
    }
}
