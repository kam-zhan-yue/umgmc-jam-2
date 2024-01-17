using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField] private GameObject replayObjectPrefab;
    private Recording _recording;
    public Queue<ReplayData> recordingQueue { get; private set; } = new();

    private RecordState _recordState;

    private enum RecordState
    {
        Idle = 0, 
        Recording = 1,
        Replaying = 2,
    }
    
    private void Start()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.SubStartLevel(OnStartLevel);
        eventManager.SubGoalReached(OnGoalReached);
        eventManager.SubRestartLevel(OnRestartLevel);
    }

    private void OnStartLevel()
    {
        StartRecording();
    }

    private void OnGoalReached()
    {
        StartReplay();
    }

    private void OnRestartLevel()
    {
        RestartReplay();
    }

    private void StartRecording()
    {
        _recordState = RecordState.Recording;
    }

    private void Update()
    {
        switch (_recordState)
        {
            case RecordState.Idle:
                break;
            case RecordState.Recording:
                break;
            case RecordState.Replaying:

                bool hasNextFrame = _recording.PlayNextFrame();
        
                //Check if we are finished, so that we can restart
                if (!hasNextFrame)
                {
                    RestartReplay();
                }
                break;
        }
    }

    public void RecordFrame(ReplayData data)
    {
        if (_recordState == RecordState.Recording)
        {
            recordingQueue.Enqueue(data);
            Debug.Log($"Data: {data}");
        }
    }

    private void StartReplay()
    {
        //Don't start the replay unless there is data.
        if (recordingQueue.Count == 0)
        {
            return;
        }
        _recordState = RecordState.Replaying;
        //Initialise recording
        _recording = new Recording(recordingQueue);
        //Reset the current recording queue for next record
        recordingQueue.Clear();
        //Instantiate recording object
        _recording.InstantiateReplayObject(replayObjectPrefab);
    }

    private void RestartReplay()
    {
        _recordState = RecordState.Replaying;
        _recording.Restart();
    }

    private void ResetReplay()
    {
        _recordState = RecordState.Idle;
        _recording.DestroyReplayObject();
        _recording = null;
    }
    
    private void OnDestroy()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.UnSubGoalReached(OnGoalReached);
        eventManager.UnSubRestartLevel(OnRestartLevel);
        eventManager.UnSubStartLevel(OnStartLevel);
    }
}
