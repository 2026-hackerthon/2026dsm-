using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RescueCountUI : MonoBehaviour
{
    private TextMeshProUGUI rescueCountText;
    private int rescueCount = 0;

    private void Start()
    {
        rescueCountText = GetComponentInChildren<TextMeshProUGUI>();
        rescueCountText.text = rescueCount.ToString();
    }

    public void Init()
    {
        rescueCount = 0;
        rescueCountText.text = rescueCount.ToString();
    }

    private void RescueStudent()
    {
        rescueCount++;
        rescueCountText.text = rescueCount.ToString();
    }
}
