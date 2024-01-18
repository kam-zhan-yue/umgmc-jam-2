using System;
using UnityEngine;

public class StopWatch
{
    private float timeInterval = 1f; public float Cooldown { get { return timeInterval; } }
    private float internalTimer = 0f;
    public bool isChecked = false;
    public StopWatch()
    {

    }

    public StopWatch(float timeInterval)
    {
        this.timeInterval = timeInterval;
    }

    /// <summary>
    /// Return true if time has reached
    /// </summary>
    /// <returns></returns>
    public bool UpdateTimer()
    {
        internalTimer += Time.deltaTime;
        if (internalTimer >= timeInterval)
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

