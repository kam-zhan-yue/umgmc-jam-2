using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    // [SerializeField] private int topN = -1;
    [SerializeField] private GameObject replayObjectPrefab;
    private List<Recording> _recordings = new();
    private Queue<ReplayData> RecordingQueue { get; set; } = new();

    private RecordState _recordState;
    
    private enum RecordState
    {
        Idle = 0, 
        Recording = 1,
        //This is not really used as the replays are going to occur while recording, too
        // Replaying = 2,
    }
    
    private void Start()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.SubStartLevel(OnStartLevel);
        eventManager.SubGoalReached(OnGoalReached);
        eventManager.SubRestartLevel(OnRestartLevel);
        eventManager.SubDeath(OnDeath);
    }

    private void OnStartLevel()
    {
        //Start Recording the current player run
        
        //Also restart all existing recordings
        for (int i = 0; i < _recordings.Count; ++i)
        {
            _recordings[i].Play();
            _recordings[i].TryInstantiateReplayObject(replayObjectPrefab);
        }
        StartRecording();
    }

    private void OnGoalReached()
    {
        StopRecording();
    }

    private void OnRestartLevel()
    {
    }

    private void OnDeath(int deaths)
    {
        StopRecording();
    }

    // Method to add a new Recording instance to the list
    private void AddRecording(Queue<ReplayData> replayData)
    {
        if (replayData.Count > 0)
        {
            Recording recording = new Recording(replayData);
            _recordings.Add(recording);
        }
    }

    private void StopRecording()
    {
        //Try to add the recording in all recordings after stopping recording
        _recordState = RecordState.Idle;
        AddRecording(RecordingQueue);
        RecordingQueue.Clear();
        for (int i = 0; i < _recordings.Count; ++i)
        {
            _recordings[i].Stop();
            _recordings[i].DestroyReplayObject();
        }
    }

    private void StartRecording()
    {
        _recordState = RecordState.Recording;
    }
    
    public void RecordFrame(ReplayData data)
    {
        if (_recordState == RecordState.Recording)
        {
            RecordingQueue.Enqueue(data);
        }
    }

    // private void StartReplay()
    // {
    //     //Don't start the replay unless there is data.
    //     if (AllRecordings.Count <= 0)
    //     {
    //         Debug.Log("nothign happened");
    //         return;
    //     }
    //     _recordState = RecordState.Replaying;
    //     //Initialise recording
    //     _recordings = new();
    //     AllRecordings.Sort((tuple1, tuple2) => tuple1.Item1.CompareTo(tuple2.Item1));
    //
    //     Debug.Log("StartReplay");
    //     Debug.Log(AllRecordings.Count);
    //
    //     for (int i = 0; i < topN && i < AllRecordings.Count; i++)
    //     {
    //         var o = AllRecordings[i];
    //         _recordings.Add(new Recording(o.Item2));
    //         _recordings[i].TryInstantiateReplayObject(replayObjectPrefab);
    //     }
    //
    //     RecordingQueue.Clear();
    // }

    //this is not in use
    public void ResetReplay()
    {
        Debug.Log("Who called me");
        _recordState = RecordState.Idle;
        foreach (var o in _recordings)
        {
            o.DestroyReplayObject();
        }
        _recordings = null;
    }
    

    private void Update()
    {
        switch (_recordState)
        {
            case RecordState.Idle:
                break;
            case RecordState.Recording:
                //Play all all existing, if any
                for (int i = 0; i < _recordings.Count; ++i)
                {
                    Recording recording = _recordings[i];
                    if (!recording.IsFinished)
                    {
                        // Debug.Log($"Playing Recording: {i}");
                        bool hasNextFrame = recording.PlayNextFrame();
                        if (!hasNextFrame)
                        {
                            recording.Stop();
                            recording.DestroyReplayObject();
                            // Debug.Log($"Stopping: {i}");
                        }
                    }
                }
                break;
        }
    }



    private void OnDestroy()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.UnSubGoalReached(OnGoalReached);
        eventManager.UnSubRestartLevel(OnRestartLevel);
        eventManager.UnSubStartLevel(OnStartLevel);
        eventManager.UnSubDeath(OnDeath);
    }
}
