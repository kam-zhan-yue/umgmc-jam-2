using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScript", menuName = "ScriptableObjects/DialogueScript", order = 100)]
public class DialogueScript : ScriptableObject
{
    public bool useCamera = true;
    public bool playOnce = true;
    public List<string> actors = new();
    public List<DialogueGroup> groups = new();
    
    [HorizontalGroup()]
    [Button((ButtonSizes.Large)), GUIColor(0.2f, 1f, 0)]
    public void AddGroup()
    {
        groups.Add(new DialogueGroup());
    }

    public List<DialogueAction> GetActions(int _groupIndex)
    {
        if (_groupIndex > 0 && _groupIndex < groups.Count)
            return groups[_groupIndex].actions;
        return new List<DialogueAction>();
    }
}
