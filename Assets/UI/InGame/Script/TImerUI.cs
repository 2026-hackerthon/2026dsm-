using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TImerUI : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    [SerializeField] private float Maxtime;
    private float timer = 0;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        timer = Maxtime;
        TimerApply();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        TimerApply();
    }
    
    private void TimerApply()
    {
        int min = (int)(timer / 60);
        float sec = timer % 60;
        textMeshPro.text = $"{min:00}:{sec:00.00}";
    }
}
