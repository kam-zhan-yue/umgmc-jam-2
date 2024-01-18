using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class RealityManager : MonoBehaviour, IRealityManager
{
    [NonSerialized, ShowInInspector, ReadOnly]
    private Reality[] _realities = Array.Empty<Reality>();

    private void Awake()
    {
        ServiceLocator.Instance.Register<IRealityManager>(this);
    }

    public void InitReality(Timeline timeline)
    {
        SetVisible(timeline);
        for (int i = 0; i < _realities.Length; ++i)
        {
            if (_realities[i].timeline != timeline)
            {
                _realities[i].Hide();
            }
        }
    }

    public bool TryGetReality(Timeline timeline, out Reality reality)
    {
        for (int i = 0; i < _realities.Length; ++i)
        {
            if (_realities[i].timeline == timeline)
            {
                reality = _realities[i];
                return true;
            }
        }

        reality = null;
        return false;
    }

    public Reality GetReality(Timeline timeline)
    {
        for (int i = 0; i < _realities.Length; ++i)
        {
            if (_realities[i].timeline == timeline)
            {
                return _realities[i];
            }
        }

        return null;
    }

    public void SetVisible(Timeline timeline)
    {
        Reality future = GetReality(Timeline.Future);
        Reality present = GetReality(Timeline.Present);
        Reality past = GetReality(Timeline.Past);
        switch (timeline)
        {
            case Timeline.Present:
                future.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
                present.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
                past.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
                break;
            case Timeline.Future:
                future.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
                present.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
                past.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
                break;
            case Timeline.Past:
                future.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
                present.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
                past.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnValidate()
    {
        UpdateReality();
    }

    [Button]
    private void UpdateReality()
    {
        _realities = FindObjectsOfType<Reality>();
        for (int i = 0; i < _realities.Length; ++i)
        {
            _realities[i].UpdateReality();
        }
    }
}
