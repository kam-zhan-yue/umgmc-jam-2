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
    [SerializeField] private SpriteMask presentBackgroundMask;
    [SerializeField] private SpriteMask futureBackgroundMask;
    [SerializeField] private SpriteMask pastBackgroundMask;

    [SerializeField] private Vector3 targetScale = new Vector3(20f, 20f, 20f);
    //[SerializeField] private float scaleDuration = 0.5f;

    //Private Variables
    private Timeline previousTimeline = Timeline.Present;
    private Timeline timeline = Timeline.Present;
    private bool _transitioning = false;


    [SerializeField] private float realitySwitchCooldown = 1f;
    [SerializeField] private float realityInstabilityCooldown = 1f;
    private StopWatch realitySwitcherTimer;
    private StopWatch switchBackTimer;
    
    private void Start()
    {
        ServiceLocator.Instance.Get<IRealityManager>().InitReality(timeline);
        ActivateMasks(timeline);
        realitySwitcherTimer = new(realitySwitchCooldown);
        switchBackTimer = new(realityInstabilityCooldown);
    }

    private void Update()
    {
        if(realitySwitcherTimer.UpdateTimer() && !realitySwitcherTimer.isChecked) // can swtich time
        {

            if (ChangeReality())
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

    private bool ChangeReality()
    {
        bool alteredTime = false;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_transitioning)
        {
            Transition(Timeline.Past);
            alteredTime = true;
            //Debug.Log("Timeline to the past");
        }
        #region - not in use -
        //if (Input.GetKeyDown(KeyCode.Mouse2) && !_transitioning)
        //{
        //    //The reality will shift back automatically for the instability of time and space distortion 
        //    //Transition(Timeline.Present);
        //}
        #endregion
        if (Input.GetKeyDown(KeyCode.Mouse1) && !_transitioning)
        {
            Transition(Timeline.Future);
            alteredTime = true;
            //Debug.Log("Timeline to the future");
        }

        return alteredTime;
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
        
        ActivateMasks(timeline);
        switch (timeline)
        {
            case Timeline.Present:
                realityManager.SetVisible(Timeline.Present);
                TransitionTween(presentMask.transform);
                TransitionTween(presentBackgroundMask.transform);
                break;
            case Timeline.Future:
                realityManager.SetVisible(Timeline.Future);
                TransitionTween(futureMask.transform);
                TransitionTween(futureBackgroundMask.transform);
                break;
            case Timeline.Past:
                realityManager.SetVisible(Timeline.Past);
                TransitionTween(pastMask.transform);
                TransitionTween(pastBackgroundMask.transform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ActivateMasks(Timeline target)
    {
        futureMask.gameObject.SetActive(target == Timeline.Future);
        futureBackgroundMask.gameObject.SetActive(target == Timeline.Future);
        presentMask.gameObject.SetActive(target == Timeline.Present);
        presentBackgroundMask.gameObject.SetActive(target == Timeline.Present);
        pastMask.gameObject.SetActive(target == Timeline.Past);
        pastBackgroundMask.gameObject.SetActive(target == Timeline.Past);
    }

    private void TransitionTween(Transform maskTransform)
    {
        maskTransform.localScale = Vector3.zero;
        maskTransform.DOScale(targetScale, realitySwitchCooldown).OnComplete(EndTransition);
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
