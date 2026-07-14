using System.Collections.Generic;
using UnityEngine;

public class TalkManage : MonoBehaviour
{
    Dictionary<int, string[]> TalkData;

    void Awake()
    {
        TalkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        TalkData.Add(1, new string[] { "아아 마이크 테스트" });
        TalkData.Add(2, new string[] { "언나더 원데이 투 마 라잎", "이거 진짜에요?" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!TalkData.ContainsKey(id))
            return null;

        string[] talks = TalkData[id];
        if (talkIndex >= talks.Length)
            return null;

        return talks[talkIndex];
    }
}