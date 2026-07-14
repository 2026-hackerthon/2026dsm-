using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backTitleButton;
    [SerializeField] private TextMeshProUGUI deadReasonText;
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

        listenersRegistered = true;
    }

    public void OnDeadUI(string reason)
    {
        Time.timeScale = 0;
        deadReasonText.text = reason;
        gameObject.SetActive(true);
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
