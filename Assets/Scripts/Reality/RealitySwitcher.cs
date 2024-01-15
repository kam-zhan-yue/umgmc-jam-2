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

    private void Start()
    {
        ServiceLocator.Instance.Get<IRealityManager>().InitReality(Timeline.Present);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !_transitioning)
        {
            Transition(Timeline.Present);
        }
        if (Input.GetKeyDown(KeyCode.W) && !_transitioning)
        {
            Transition(Timeline.Past);
        }
        if (Input.GetKeyDown(KeyCode.E) && !_transitioning)
        {
            Transition(Timeline.Future);
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
        }

        if (realityManager.TryGetReality(timeline, out Reality reality))
        {
            reality.Show();
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
