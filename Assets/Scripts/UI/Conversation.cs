using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Conversation : MonoBehaviour
{
    [SerializeField] private PopupSettings settings;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] private UnityEvent onConversationStart; 
    [SerializeField] private List<string> dialogue = new();
    [SerializeField] private GameObject conversationUI;
    [SerializeField] private UnityEvent onConversationEnd; 
    
    private Queue<string> _dialogue = new();


    private bool conversationStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        PackDialogue();
        conversationUI.SetActive(false);
    }

    private void PackDialogue()
    {
        foreach (var o in dialogue) _dialogue.Enqueue(o);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>()) // Assuming your character has the tag "Player"
        {
            if(!conversationStarted)
            {
                onConversationStart?.Invoke();
                conversationStarted = true;
                LoadNextConversation();
            }
            conversationUI.SetActive(true); //show ui
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>()) // Assuming your character has the tag "Player"
        {
            conversationUI.SetActive(false); //show ui
        }
    }

    private void LoadNextConversation()
    {
        if (textBox == null) return;
        if(_dialogue.Count>0)
        {
            settings.PlayButton();
            textBox.SetText(_dialogue.Dequeue());//set text
        }
        else
        {
            Debug.Log("End Dialogue");
            conversationUI.SetActive(false);
            onConversationEnd?.Invoke();
            GameManager.Instance.Reset();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(conversationStarted)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LoadNextConversation();
            }
        }
    }
}
