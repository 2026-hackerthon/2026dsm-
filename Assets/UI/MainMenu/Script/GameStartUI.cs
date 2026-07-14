using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(GameStart);
        if (quitButton != null)
            quitButton.onClick.AddListener(GameQuit);
    }

    private void GameStart()
    {
        SceneManager.LoadScene(1);
    }
    
    private void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
}
