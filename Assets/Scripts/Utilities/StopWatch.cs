using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class StopWatch
{
    private float timeInterval = 1f; public float Cooldown { get { return timeInterval; } }
    private FloatReference _internalTimer;
    public bool isChecked = false;
    public StopWatch()
    {

    }

    public StopWatch(FloatReference floatReference, float timeInterval)
    {
        _internalTimer = floatReference;
        _internalTimer.Value = 0f;
        this.timeInterval = timeInterval;
    }

    /// <summary>
    /// Return true if time has reached
    /// </summary>
    /// <returns></returns>
    public bool UpdateTimer()
    {
        _internalTimer.Value += Time.deltaTime;
        if (_internalTimer >= timeInterval)
        {
            timeInterval = 0;
            return true;
        }
        return false;
    }

    public void ForceReset()
    {
        timeInterval = 0f;
        isChecked = false;
    }
}

