using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using TMPro;
using UnityEngine;

public class Conversation : MonoBehaviour
{

    [SerializeField] private List<string> dialogue = new();
    [SerializeField] private GameObject conversationUI;


    private TextMeshProUGUI textBox;
    private Queue<string> _dialogue = new();


    private bool conversationStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        PackDialogue();
        textBox = conversationUI.GetComponentInChildren<TextMeshProUGUI>();
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
            textBox.SetText(_dialogue.Dequeue());//set text
        }
        else
        {
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
