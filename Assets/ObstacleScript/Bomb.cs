using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject explosion;   // 폭발 이펙트

    private Color originColor;
    private bool isTriggered = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originColor = sr.color;

        if (explosion != null)
            explosion.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(BombRoutine());
        }
    }

    IEnumerator BombRoutine()
    {
        // 2번 깜빡임
        for (int i = 0; i < 2; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);

            sr.color = originColor;
            yield return new WaitForSeconds(0.2f);
        }

        // 폭발 이펙트 실행
        if (explosion != null)
        {
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
        }

        // 폭탄 비활성화
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        isTriggered = false;
        sr.color = originColor;
    }
}