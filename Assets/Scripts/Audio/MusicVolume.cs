using UnityEngine;
using System.Collections;
using MoreMountains.CorgiEngine;

public class MusicVolume : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>()) // Assuming your character has the tag "Player"
        {
            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Character>())
        {
            audioSource.Stop();
        }
    }
}

