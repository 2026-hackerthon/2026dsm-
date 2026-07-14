using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TImerUI : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    [SerializeField] private float Maxtime;
    private float timer = 0;
    [SerializeField] private GameOverUI gameOverUI;
    private bool isGameOver = false;

    private void Awake()
    {
        isGameOver = false;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        timer = Maxtime;
        TimerApply();
    }

    public void Init()
    {
        isGameOver = false;
        timer = Maxtime;
        TimerApply();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        TimerApply();
        if (timer < 0 && !isGameOver)
        {
            isGameOver = true;
            gameOverUI.OnDeadUI("시간안에 탈출하지 못했습니다...");
        }
    }
    
    private void TimerApply()
    {
        int min = (int)(timer / 60);
        float sec = timer % 60;
        if (min == 0 && sec <= 10)
            textMeshPro.color = new Color(0.8f, 0, 0);
        textMeshPro.text = $"{min:00}:{sec:00.00}";
    }
}
