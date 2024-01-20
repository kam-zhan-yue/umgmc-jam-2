using System.Collections;
using System.Collections.Generic;
using Cainos.LucidEditor;
using Common;
using MoreMountains.CorgiEngine;
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
        dialogueEventStarted.Raise();
        LevelManager.Instance.FreezeCharacters();
    }

    public void EndDialogue(DialogueEvent dialogueEvent)
    {
        dialogueEventEnded.Raise();
        LevelManager.Instance.UnFreezeCharacters();
    }
}
