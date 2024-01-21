using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "TimeRunner";  // Fixed typo here

    public void StartGameButton()
    {
        SceneManager.LoadScene(newGameLevel);
    }
}
