public interface IDialogueManager : IGameService
{
    public void StartDialogue(DialogueEvent dialogueEvent);
    public void EndDialogue(DialogueEvent dialogueEvent);
}