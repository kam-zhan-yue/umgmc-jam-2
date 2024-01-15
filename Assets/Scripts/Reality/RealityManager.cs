using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class RealityManager : MonoBehaviour, IRealityManager
{
    private readonly Dictionary<Timeline, Reality> _realities = new Dictionary<Timeline, Reality>();

    private void Awake()
    {
        ServiceLocator.Instance.Register<IRealityManager>(this);
    }

    public void InitReality(Timeline timeline)
    {
        SetVisible(timeline);
        foreach (KeyValuePair<Timeline, Reality> reality in _realities)
        {
            if (reality.Key != timeline)
                reality.Value.gameObject.SetActive(false);
        }
    }

    public void RegisterReality(Reality reality)
    {
        _realities.Add(reality.timeline, reality);
    }

    public bool TryGetReality(Timeline timeline, out Reality reality)
    {
        return _realities.TryGetValue(timeline, out reality);
    }

    public void SetVisible(Timeline timeline)
    {
        Reality future = _realities[Timeline.Future];
        Reality present = _realities[Timeline.Present];
        Reality past = _realities[Timeline.Past];
        switch (timeline)
        {
            case Timeline.Present:
                future.SetVisibleOutsideMask();
                present.SetVisibleInsideMask();
                past.SetVisibleInsideMask();
                break;
            case Timeline.Future:
                future.SetVisibleInsideMask();
                present.SetVisibleOutsideMask();
                past.SetVisibleOutsideMask();
                break;
            case Timeline.Past:
                future.SetVisibleOutsideMask();
                present.SetVisibleOutsideMask();
                past.SetVisibleInsideMask();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
