using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField] private GameObject replayObjectPrefab;
    private List<Recording> _recordings;
    public Queue<ReplayData> recordingQueue { get; private set; } = new();
    private float frames = 0;

    public List<Tuple<float,Queue<ReplayData>>> AllRecordings { get; private set; } = new();

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


    
    public void StopRecording()
    {
        _recordState = RecordState.Idle;
        Debug.Log("Data Added");
        AllRecordings.Add(new Tuple<float, Queue<ReplayData>>(frames, recordingQueue));
        frames = 0;
    }



    public void StartRecording()
    {
        _recordState = RecordState.Recording;
    }

    

    public void RecordFrame(ReplayData data)
    {
        if (_recordState == RecordState.Recording)
        {
            recordingQueue.Enqueue(data);
            frames++;
            //Debug.Log($"Data: {data}");
        }
    }

    public int topN = 3;

    public void StartReplay()
    {
        //Don't start the replay unless there is data.
        if (AllRecordings.Count <= 0)
        {
            Debug.Log("nothign happened");
            return;
        }
        _recordState = RecordState.Replaying;
        //Initialise recording
        _recordings = new();
        AllRecordings.Sort((tuple1, tuple2) => tuple1.Item1.CompareTo(tuple2.Item1));

        Debug.Log("StartReplay");
        Debug.Log(AllRecordings.Count);

        for (int i = 0; i < topN && i < AllRecordings.Count; i++)
        {
            var o = AllRecordings[i];
            _recordings.Add(new Recording(o.Item2));
            _recordings[i].InstantiateReplayObject(replayObjectPrefab);
        }

        recordingQueue.Clear();
    }

    public void RestartReplay()
    {
        _recordState = RecordState.Replaying;
        foreach (var o in _recordings)
        {
            o.Restart();
        }
    }

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
                break;
            case RecordState.Replaying:

                foreach (var o in _recordings)
                {
                    bool hasNextFrame = o.PlayNextFrame();
                    //Check if we are finished, so that we can restart
                    if (!hasNextFrame)
                    {
                        o.DestroyReplayObject();
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
    }
}
