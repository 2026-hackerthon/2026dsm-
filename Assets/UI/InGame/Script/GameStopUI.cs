using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class GameStopUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backTitleButton;
    [SerializeField] private Button continueButton;
    
    private bool listenersRegistered = false;

    private void Awake()
    {
        RegisterListeners();
    }

    public void Init()
    {
        Time.timeScale = 1;
        RegisterListeners();
        gameObject.SetActive(false);
    }

    private void RegisterListeners()
    {
        if (listenersRegistered) return;

        restartButton.onClick.AddListener(Restart);
        backTitleButton.onClick.AddListener(BackTitle);
        continueButton.onClick.AddListener(Continue);

        listenersRegistered = true;
    }

    public void stop()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }
    
    private void Continue()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    
    private void Restart()
    {
        
    }
    
    private void BackTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }
}