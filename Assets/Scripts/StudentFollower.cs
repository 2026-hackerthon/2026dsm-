using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class StudentFollower : MonoBehaviour
{
    private static StudentFollower lastFollower;

    [SerializeField, Min(0.1f)] private float followDistance = 1.2f;
    [SerializeField, Min(0.1f)] private float moveSpeed = 4f;
    [SerializeField, Min(1f)] private float teleportDistance = 8f;

    private Rigidbody2D body;
    private Transform followTarget;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    public void BeginFollowing(Transform player)
    {
        followTarget = lastFollower != null && lastFollower != this
            ? lastFollower.transform
            : player;
        lastFollower = this;
        enabled = true;
    }

    public void StopFollowing()
    {
        followTarget = null;
        body.linearVelocity = Vector2.zero;
        enabled = false;

        if (lastFollower == this)
            lastFollower = null;
    }

    private void FixedUpdate()
    {
        if (followTarget == null)
            return;

        Vector2 targetPosition = followTarget.position;
        Vector2 difference = targetPosition - body.position;
        float distance = difference.magnitude;

        if (distance > teleportDistance)
        {
            Vector2 behind = distance > 0f ? -difference / distance : Vector2.left;
            body.position = targetPosition + behind * followDistance;
            return;
        }

        if (distance <= followDistance)
            return;

        float moveDistance = Mathf.Min(moveSpeed * Time.fixedDeltaTime, distance - followDistance);
        body.MovePosition(body.position + difference.normalized * moveDistance);
    }

    private void OnDestroy()
    {
        if (lastFollower == this)
            lastFollower = null;
    }
}
