using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class DialoguePopup : Popup
{
    [BoxGroup("UI Objects")] public TMP_Text nameText;
    [BoxGroup("UI Objects")] public TMP_Text dialogueText;
    [BoxGroup("Setup Variables")] public Vector3 offset;
    [BoxGroup("Setup Variables")] public float speed = 5f;
    private Tween typeWriterTween;
    private float originalVolume;
    
    public void Show(Vector3 position, string actorName, string dialogue, Action onComplete = null)
    {
        gameObject.SetActive(true);
        position.x += offset.x;
        position.y += offset.y;
        position.z += offset.z;
        transform.position = position;
        nameText.SetText(actorName);
        // dialogueText.SetText(_text);
        string text = string.Empty;
        typeWriterTween = DOTween.To(() => text, _x => text = _x, dialogue, dialogue.Length / speed)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                this.dialogueText.SetText(text);
            })
            .OnComplete(() =>
            {
                this.dialogueText.SetText(dialogue);
                onComplete?.Invoke();
            }).OnKill(() =>
            {
                this.dialogueText.SetText(dialogue);
                onComplete?.Invoke();
            }).SetUpdate(true);
    }

    public void Stop()
    {
        typeWriterTween.Kill();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    protected override void InitPopup()
    {
        
    }
}