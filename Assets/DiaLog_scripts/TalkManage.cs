using System.Collections.Generic;
using UnityEngine;

public class TalkManage : MonoBehaviour
{
    private Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    private void GenerateData()
    {
        talkData.Add(1, new[] { "불이 났어. 지금 나가야 해!" });
        talkData.Add(2, new[] { "조금만 더 자면 안 돼?", "정말 위험하다고? 알겠어, 같이 가자." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.TryGetValue(id, out string[] talks) || talkIndex >= talks.Length)
            return null;

        return talks[talkIndex];
    }
}
