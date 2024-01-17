using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public DialoguePopup dialoguePopupPrefab;

    private DialoguePopup dialoguePopup;

    [BoxGroup("Setup Variables")] public Vector3 offset;
    private Collider2D sizeCollider;

    private void Awake()
    {
        sizeCollider = GetComponent<Collider2D>();
    }
    
    private void Start()
    {
        dialoguePopup = Instantiate(dialoguePopupPrefab);
        dialoguePopup.gameObject.SetActive(false);
    }

    [Button]
    public void ShowMessage(string actorName, string dialogue, Action onComplete = null)
    {
#if UNITY_EDITOR
        if(dialoguePopup == null)
            dialoguePopup = Instantiate(dialoguePopupPrefab);
        if(sizeCollider == null)
            sizeCollider = GetComponent<Collider2D>();
#endif
        
        Bounds bounds = sizeCollider.bounds;
        Vector3 dialoguePosition = bounds.center;
        dialoguePosition.y = bounds.center.y + (bounds.extents.y);
        dialoguePopup.Show(dialoguePosition + offset, actorName, dialogue, onComplete);
    }

    public void ForceEndMessage()
    {
        dialoguePopup.Stop();
    }

    public void HideMessage()
    {
        dialoguePopup.Hide();
    }

    public void PlayAnimation(string animationName, Action onComplete = null)
    {
        
    }

    public void ForceEndAnimation()
    {
        
    }

    public void StopAnimation(string animationName)
    {
        
    }
}
