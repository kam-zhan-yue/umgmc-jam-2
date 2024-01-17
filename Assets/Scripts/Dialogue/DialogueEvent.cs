using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueEvent : MonoBehaviour
{
    [Serializable]
    public class DialogueUnityEvent
    {
        public string id;
        public UnityEvent unityEvent;
    }

    [FoldoutGroup("UI Objects")] public PopupSettings settings;
    [FoldoutGroup("Scene Variables")] public List<DialogueActor> actors = new();

    [FoldoutGroup("Setup Variables")] [InlineEditor()]
    public DialogueScript script;

    //Private Variables
    private readonly Dictionary<string, DialogueActor> _actorDictionary = new();
    private readonly Queue<DialogueGroup> _dialogueQueue = new();
    private DialogueGroup _currentGroup;
    private Tween _typeWriterTween;
    private int _finishedEvents = 0;
    private int _totalEvents = 0;
    private bool _played = false;

    private void Awake()
    {
        for (int i = 0; i < actors.Count; ++i)
        {
            _actorDictionary.Add(actors[i].key, actors[i]);
        }

    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        //If already played, then don't bother
        if (script.playOnce && _played)
            return;
        if (collider2D.gameObject.TryGetComponent(out DialogueTrigger _))
        {
            if (collider2D.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
            }
            StartEvent();
        }
    }

    [Button]
    public void StartEvent()
    {
        //If already played, then don't bother
        if (script.playOnce && _played)
            return;
        _dialogueQueue.Clear();
        for (int i = 0; i < script.groups.Count; ++i)
            _dialogueQueue.Enqueue(script.groups[i]);
        DequeueDialogue();
        ServiceLocator.Instance.Get<IDialogueManager>().StartDialogue(this);
    }

    private void DequeueDialogue()
    {
        if (_dialogueQueue.Count > 0)
        {
            _finishedEvents = 0;
            _totalEvents = 0;
            _currentGroup = _dialogueQueue.Dequeue();
            List<DialogueAction> actions = _currentGroup.actions;
            for (int i = 0; i < actions.Count; ++i)
                PlayAction(actions[i]);
        }
        else
        {
            EndEvent();
        }
    }
    
    private void NextStarted(InputAction.CallbackContext callbackContext)
    {
        MoveDialogue();
    }
    
    [Button]
    private void MoveDialogue()
    {
        settings.PlayButton();
        //If not finished, then force finish
        if (!CurrentGroupFinished())
        {
            _finishedEvents = _totalEvents;
            List<DialogueAction> actions = _currentGroup.actions;
            for (int i = 0; i < actions.Count; ++i)
                ForceEndAction(actions[i]);
        }
        //Else, start a new group
        else
        {
            List<DialogueAction> actions = _currentGroup.actions;
            for (int i = 0; i < actions.Count; ++i)
                EndAction(actions[i]);
            DequeueDialogue();
        }
    }

    private void PlayAction(DialogueAction action)
    {
        switch (action.type)
        {
            case DialogueType.Message:
                if (_actorDictionary.TryGetValue(action.actor, out DialogueActor messageActor))
                {
                    _totalEvents++;
                    messageActor.dialogueObject.ShowMessage(action.GetName(), action.message, OnEventComplete);
                }
                break;
            case DialogueType.Animation:
                if (_actorDictionary.TryGetValue(action.actor, out DialogueActor animationActor))
                {
                    _totalEvents++;
                    animationActor.dialogueObject.PlayAnimation(action.animationName, OnEventComplete);
                }
                break;
            case DialogueType.Pause:
            default:
                break;
        }
    }
    
    private void OnEventComplete()
    {
        _finishedEvents++;
    }

    private bool CurrentGroupFinished()
    {
        return _finishedEvents >= _totalEvents;
    }

    private void EndAction(DialogueAction action)
    {
        switch (action.type)
        {
            case DialogueType.Message:
                if (_actorDictionary.TryGetValue(action.actor, out DialogueActor messageActor))
                    messageActor.dialogueObject.HideMessage();
                break;
            case DialogueType.Animation:
                if (_actorDictionary.TryGetValue(action.actor, out DialogueActor animationActor)) 
                    animationActor.dialogueObject.StopAnimation(action.animationName);
                break;
            case DialogueType.Pause:
            default:
                break;
        }
    }

    private void ForceEndAction(DialogueAction action)
    {
        switch (action.type)
        {
            case DialogueType.Message:
                if (_actorDictionary.TryGetValue(action.actor, out DialogueActor messageActor))
                    messageActor.dialogueObject.ForceEndMessage();
                break;
            case DialogueType.Animation:
                if (_actorDictionary.TryGetValue(action.actor, out DialogueActor animationActor)) 
                    animationActor.dialogueObject.ForceEndAnimation();
                break;
            case DialogueType.Pause:
            default:
                break;
        }
    }
        
    private void EndEvent()
    {
        _played = true;
        ServiceLocator.Instance.Get<IDialogueManager>().EndDialogue(this);
    }
}
