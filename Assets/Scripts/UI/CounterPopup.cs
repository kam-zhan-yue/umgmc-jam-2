using System;
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
    [SerializeField] private TMP_Text deathCountText;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_Text fastestRunText;
    [SerializeField] private Vector3 elasticScale = new Vector3(1.5f, 1.5f, 1.0f);
    [SerializeField] private float elasticDuration = 0.5f;
    [SerializeField] private Ease elasticEase = Ease.OutElastic;

    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float stayDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    private bool _counting = false;

    private IGameManager _gameManager;
    
    protected override void InitPopup()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.SubStartLevel(OnStartLevel);
        eventManager.SubDeath(OnDeath);
        eventManager.SubGoalReached(OnGoalReached);
        deathCountText.alpha = 0f;
        _gameManager = ServiceLocator.Instance.Get<IGameManager>();
        _gameManager.SubFastestRun(UpdateFastestRun);
        fastestRunText.gameObject.SetActive(false);
    }

    private void UpdateFastestRun()
    {
        float fastestRun = _gameManager.FastestRunTime();
        if (fastestRun > 0f)
        {
            fastestRunText.gameObject.SetActive(true);
            fastestRunText.SetText(FormatTime(fastestRun));
        }
    }
    
    private void OnStartLevel()
    {
        Sequence sequence = DOTween.Sequence();
        //Bounce the counter elastically
        sequence.Append(counterText.transform.DOScale(elasticScale, elasticDuration).SetEase(elasticEase));
        
        //Return the counter back to its original scale
        sequence.Append(counterText.transform.DOScale(Vector3.one, elasticDuration).SetEase(elasticEase));
        
        sequence.Play();

        _counting = true;
    }

    private void Update()
    {
        if (_counting)
        {
            counterText.text = FormatTime(_gameManager.TotalRunTime());
        }
    }

    private void OnGoalReached()
    {
        _counting = false;
    }

    private void OnDeath(int deaths)
    {
        _counting = false;
        // Set initial alpha to zero
        deathCountText.alpha = 0f;
        deathCountText.SetText($"Deaths: {deaths}");

        // Create a sequence to perform the fade in, stay, and fade out animations
        Sequence sequence = DOTween.Sequence();

        // Fade In
        sequence.Append(deathCountText.DOFade(1.0f, fadeInDuration));

        // Stay
        sequence.AppendInterval(stayDuration);

        // Fade Out
        sequence.Append(deathCountText.DOFade(0.0f, fadeOutDuration));

        sequence.Play();
    }
    
    private string FormatTime(float totalSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
        if (timeSpan.Minutes > 0)
        {
            return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds / 10:D2}.{timeSpan.Milliseconds % 10:D2}";
        }
        else
        {
            return $"{timeSpan.Seconds:D2}.{timeSpan.Milliseconds / 10:D2}";
        }
    }

    private void OnDestroy()
    {
        IEventManager eventManager = ServiceLocator.Instance.Get<IEventManager>();
        eventManager.UnSubStartLevel(OnStartLevel);
        eventManager.UnSubDeath(OnDeath);
        eventManager.UnSubGoalReached(OnGoalReached);
        _gameManager.UnSubFastestRun(UpdateFastestRun);
    }
}
