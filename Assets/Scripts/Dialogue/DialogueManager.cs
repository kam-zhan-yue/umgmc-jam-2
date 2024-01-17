using System.Collections;
using System.Collections.Generic;
using Cainos.LucidEditor;
using Common;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialogueManager
{
    [FoldoutGroup("Game Events")] public GameEvent dialogueEventStarted;
    [FoldoutGroup("Game Events")] public GameEvent dialogueEventEnded;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IDialogueManager>(this);
    }

    public void StartDialogue(DialogueEvent dialogueEvent)
    {
        Debug.Log($"Start Dialogue: {dialogueEvent}");
        dialogueEventStarted.Raise();
    }

    public void EndDialogue(DialogueEvent dialogueEvent)
    {
        Debug.Log($"End Dialogue: {dialogueEvent}");
        dialogueEventEnded.Raise();
    }
}
