using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording
{
    public ReplayObject replayObject { get; private set; }
    private Queue<ReplayData> _originalQueue;
    private Queue<ReplayData> _replayQueue;

    public Recording(Queue<ReplayData> queue)
    {
        _originalQueue = new Queue<ReplayData>(queue);
        _replayQueue = new Queue<ReplayData>(queue);
    }

    public void Restart()
    {
        _replayQueue = new Queue<ReplayData>(_originalQueue);
    }

    public bool PlayNextFrame()
    {
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

    public void InstantiateReplayObject(GameObject prefab)
    {
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
        if(replayObject)
            Object.Destroy(replayObject);
    }
}
