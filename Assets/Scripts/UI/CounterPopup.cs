using System.Collections.Generic;
using System.Globalization;
using Common;
using DG.Tweening;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class CounterPopup : Popup
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private Vector3 elasticScale = new Vector3(1.5f, 1.5f, 1.0f);
    [SerializeField] private float elasticDuration = 0.5f;
    [SerializeField] private Ease elasticEase = Ease.OutElastic;
    private float _endValue = 0f;

    private float _counter = 0f;
    private bool _counting = false;
    
    protected override void InitPopup()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.SubStartLevel(OnStartLevel);
        eventManager.SubDeath(OnDeath);
        eventManager.SubGoalReached(OnGoalReached);
        // eventManager.SubRealitySwitch(RealitySwitch);
        // counterText.gameObject.SetActive(false);
    }
    
    private void OnStartLevel()
    {
        Sequence sequence = DOTween.Sequence();
        //Bounce the counter elastically
        sequence.Append(counterText.transform.DOScale(elasticScale, elasticDuration).SetEase(elasticEase));
        
        //Return the counter back to its original scale
        sequence.Append(counterText.transform.DOScale(Vector3.one, elasticDuration).SetEase(elasticEase));
        
        sequence.Play();

        _counter = 0f;
        _counting = true;
    }

    private void Update()
    {
        if (_counting)
        {
            _counter += Time.deltaTime;
            counterText.text = _counter.ToString("F2");
        }
    }

    private void OnGoalReached()
    {
        _counting = false;
    }

    private void OnDeath()
    {
        _counting = false;
    }

    private void RealitySwitch(float duration)
    {
        counterText.gameObject.SetActive(true);
        _counter = duration;
        counterText.text = _counter.ToString("F2");
        Sequence sequence = DOTween.Sequence();
        //Bounce the counter elastically
        sequence.Append(counterText.transform.DOScale(elasticScale, elasticDuration).SetEase(elasticEase));
        
        //Return the counter back to its original scale
        sequence.Append(counterText.transform.DOScale(Vector3.one, elasticDuration).SetEase(elasticEase));
        
        sequence.Play();
        
        //Update the numeric value of the text
        DOTween.To(() => _counter, x => _counter = x, 0, duration)
            .OnUpdate(UpdateCounterText)
            .SetEase(Ease.Linear).
            OnComplete(() =>
            {
                counterText.gameObject.SetActive(false);
            });

    }
    
    // Custom callback to update the counterText during the animation
    private void UpdateCounterText()
    {
        counterText.text = _counter.ToString("F2");
    }

    private void OnDestroy()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.UnSubStartLevel(OnStartLevel);
        eventManager.UnSubDeath(OnDeath);
        eventManager.UnSubGoalReached(OnGoalReached);
        // eventManager.UnSubRealitySwitch(RealitySwitch);
    }
}