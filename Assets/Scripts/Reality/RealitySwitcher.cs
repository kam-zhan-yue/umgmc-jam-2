using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

public class RealitySwitcher : MonoBehaviour
{
    //Inspector Exposed Variables
    [SerializeField] private SpriteMask presentMask;
    [SerializeField] private SpriteMask futureMask;
    [SerializeField] private SpriteMask pastMask;

    [SerializeField] private Vector3 targetScale = new Vector3(20f, 20f, 20f);
    [SerializeField] private float scaleDuration = 0.5f;

    //Private Variables
    private Timeline previousTimeline = Timeline.Present;
    private Timeline timeline = Timeline.Present;
    private bool _transitioning = false;


    [SerializeField] private float realitySwitchCooldown = 3f;
    [SerializeField] private float realityInstabilityCooldown = 3f;
    private StopWatch realitySwitcherTimer;
    private StopWatch switchBackTimer;
    private void Start()
    {
        ServiceLocator.Instance.Get<IRealityManager>().InitReality(Timeline.Present);
        realitySwitcherTimer = new(realitySwitchCooldown);
        switchBackTimer = new(realityInstabilityCooldown);
    }

    private void Update()
    {
        if(realitySwitcherTimer.UpdateTimer() && !realitySwitcherTimer.isChecked) // can swtich time
        {
            bool alteredTime = false;
            if (Input.GetKeyDown(KeyCode.Mouse0) && !_transitioning)
            {
                Transition(Timeline.Past);
                alteredTime = true;
                //Debug.Log("Timeline to the past");
            }
            //if (Input.GetKeyDown(KeyCode.Mouse2) && !_transitioning)
            //{
            //    //The reality will shift back automatically for the instability of time and space distortion 
            //    //Transition(Timeline.Present);
            //}
            if (Input.GetKeyDown(KeyCode.Mouse1) && !_transitioning)
            {
                Transition(Timeline.Future);
                alteredTime = true;
                //Debug.Log("Timeline to the future");
            }
            if (alteredTime)
            {
                switchBackTimer = new(realityInstabilityCooldown);
                realitySwitcherTimer.isChecked = true;
            }
        }
        else //you cant swtich time, when the switch back timer reaches 0, switch back to present, and you can use the switcher again
        {
            if(switchBackTimer.UpdateTimer())
            {
                if (timeline != Timeline.Present)
                {
                    Transition(Timeline.Present);
                    //Debug.Log("Timeline disto back to present");
                    realitySwitcherTimer.ForceReset();
                }
            }

        }
    }

    private void Transition(Timeline newTimeline)
    {
        if (timeline == newTimeline)
        {
            return;
        }

        previousTimeline = timeline;
        timeline = newTimeline;

        IRealityManager realityManager = ServiceLocator.Instance.Get<IRealityManager>();
        if (realityManager.TryGetReality(previousTimeline, out Reality previousReality))
        {
            previousReality.Show();
            previousReality.DeactivateColliders();
        }

        if (realityManager.TryGetReality(timeline, out Reality reality))
        {
            reality.Show();
            reality.ActivateColliders();
        }
        
        Debug.Log($"Transitioning to: {timeline}");
        _transitioning = true;
        switch (timeline)
        {
            case Timeline.Present:
                futureMask.gameObject.SetActive(false);
                presentMask.gameObject.SetActive(true);
                pastMask.gameObject.SetActive(false);
                realityManager.SetVisible(Timeline.Present);
                TransitionTween(presentMask.transform);
                break;
            case Timeline.Future:
                futureMask.gameObject.SetActive(true);
                presentMask.gameObject.SetActive(false);
                pastMask.gameObject.SetActive(false);
                realityManager.SetVisible(Timeline.Future);
                TransitionTween(futureMask.transform);
                break;
            case Timeline.Past:
                futureMask.gameObject.SetActive(false);
                presentMask.gameObject.SetActive(false);
                pastMask.gameObject.SetActive(true);
                realityManager.SetVisible(Timeline.Past);
                TransitionTween(pastMask.transform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void TransitionTween(Transform maskTransform)
    {
        maskTransform.localScale = Vector3.zero;
        maskTransform.DOScale(targetScale, scaleDuration).OnComplete(EndTransition);
    }

    private void EndTransition()
    {
        IRealityManager realityManager = ServiceLocator.Instance.Get<IRealityManager>();

        if (realityManager.TryGetReality(previousTimeline, out Reality toHide))
        {
            toHide.Hide();
        }
        _transitioning = false;
    }
}
