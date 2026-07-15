using UnityEngine;

public class PlayerMouseFollow : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public float stopDistance = 0.1f; // 커서와 이 거리 이하면 멈춤

    [Header("참조")]
    public Camera mainCamera;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector2 mouseWorldPos = GetMouseWorldPosition();
        Vector2 currentPos = rb.position;

        Vector2 direction = mouseWorldPos - currentPos;
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            Vector2 moveDir = direction.normalized;
            Vector2 newPos = currentPos + moveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -mainCamera.transform.position.z; // 오소그래픽 기준 카메라와의 거리
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        return new Vector2(worldPos.x, worldPos.y);
    }
}