using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Unity.Collections;
using UnityAtoms.BaseAtoms;
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
    [SerializeField] private MMFeedbacks switchFeedback;

    [SerializeField] private Vector3 targetScale = new Vector3(20f, 20f, 20f);
    [SerializeField] private float scaleDuration = 0.5f;

    //Private Variables
    private Timeline previousTimeline = Timeline.Present;
    private Timeline timeline = Timeline.Present;
    private bool _transitioning = false;


    [SerializeField] private float realitySwitchCooldown = 1f;
    [SerializeField] private float realityInstabilityCooldown = 1f;

    private bool _changing = false;
    
    private void Start()
    {
        ServiceLocator.Instance.Get<IRealityManager>().InitReality(timeline);
        ActivateMasks(timeline);
        // _realitySwitcherTimer = new(realitySwitchCooldown);
        // _switchBackTimer = new(realityInstabilityCooldown);
    }
    
    private void Update()
    {
        if (!_changing)
        {
            ChangeReality();
        }
    }

    private void ChangeReality()
    {
        bool alteredTime = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(()=>
            {
                Transition(Timeline.Past, true);
            });
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            {
                Transition(Timeline.Present, false);
            });
            sequence.Play();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(()=>
            {
                Transition(Timeline.Future, true);
            });
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            {
                Transition(Timeline.Present, false);
            });
            sequence.Play();
        }
    }

    private void Transition(Timeline newTimeline, bool playAnimation)
    {
        if (timeline == newTimeline)
        {
            return;
        }

        Debug.Log("DEBUG | Transition, Play Feedback");
        switchFeedback.PlayFeedbacks();

        if (playAnimation)
        {
            ServiceLocator.Instance.Get<IEventManager>().RealitySwitch(realityInstabilityCooldown);
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
            Debug.Log("DEBUG | Transition, Show Timeline");
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
