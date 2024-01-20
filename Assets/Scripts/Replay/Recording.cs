using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording
{
    public ReplayObject replayObject { get; private set; }
    private Queue<ReplayData> _originalQueue;
    private Queue<ReplayData> _replayQueue;
    public int Frames => _originalQueue.Count;

    private RecordingState _recordingState = RecordingState.Idle;
    public bool IsFinished => _recordingState == RecordingState.Finished;

    private enum RecordingState
    {
        Idle = 0,
        Playing = 1,
        Finished = 2,
    }

    public Recording(Queue<ReplayData> queue)
    {
        _originalQueue = new Queue<ReplayData>(queue);
        _replayQueue = new Queue<ReplayData>(queue);
    }

    public void Play()
    {
        _replayQueue = new Queue<ReplayData>(_originalQueue);
        _recordingState = RecordingState.Playing;
        Debug.Log("Play Recording");
    }

    public bool PlayNextFrame()
    {
        //If not playing, then don't play.
        if (_recordingState != RecordingState.Playing)
        {
            Debug.Log("Stop due to record state");
            return false;
        }
        
        if (replayObject == null)
        {
            Debug.LogError("Tried to play next frame, but replay object is null");
            return false;
        }

        if (_replayQueue.Count > 0)
        {
            ReplayData data = _replayQueue.Dequeue();
            replayObject.SetDataForFrame(data);
            return true;
        }
        return false;
    }

    public void TryInstantiateReplayObject(GameObject prefab)
    {
        //Only instantiate if there is no replay object and there is data to replay
        if (_replayQueue.Count > 0)
        {
            ReplayData startingData = _replayQueue.Peek();
            GameObject replayGameObject = Object.Instantiate(prefab, startingData.position, Quaternion.identity);
            if (replayGameObject.TryGetComponent(out ReplayObject replay))
            {
                replayObject = replay;
            }
        }
    }

    public void DestroyReplayObject()
    {
        if (replayObject)
        {
            Object.Destroy(replayObject.gameObject);
        }
    }

    public void Stop()
    {
        Debug.Log("Stop Recording");
        _recordingState = RecordingState.Finished;
    }
}
