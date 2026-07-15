using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrapTrigger : MonoBehaviour
{
    [Header("낙하 오브젝트 설정")]
    [SerializeField] private string fallObjectName = "FallObject"; // 찾을 자식 오브젝트 이름
    [SerializeField] private float fallSpeed = 15f;                 // 낙하 속도
    [SerializeField] private float fallDelayBetweenObjects = 0.3f; // 오브젝트별 낙하 시작 딜레이

    private bool hasTriggered = false; // 중복 발동 방지 (필요 없으면 제거해도 됨)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        hasTriggered = true;

        StartCoroutine(TriggerFallObjects());
    }

    private IEnumerator TriggerFallObjects()
    {
        // 이 트리거 오브젝트 하위의 모든 자식 중 이름이 fallObjectName인 것들 수집
        List<Transform> fallObjects = new List<Transform>();
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child != transform && child.name == fallObjectName)
                fallObjects.Add(child);
        }

        float targetY = transform.position.y; // FallTrigger(이 오브젝트)의 y위치

        foreach (Transform obj in fallObjects)
        {
            StartCoroutine(FallToY(obj, targetY));
            yield return new WaitForSeconds(fallDelayBetweenObjects); // 다음 오브젝트 낙하까지 딜레이
        }
    }

    private IEnumerator FallToY(Transform obj, float targetY)
    {
        while (obj.position.y > targetY)
        {
            Vector3 pos = obj.position;
            pos.y -= fallSpeed * Time.deltaTime;
            if (pos.y < targetY) pos.y = targetY;
            obj.position = pos;
            yield return null;
        }
    }
}