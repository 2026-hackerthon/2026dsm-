using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 10f;

    private Vector2 dirVec;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        speed = normalSpeed;
    }

    public void SetMoveInput(Vector2 input)
    {
        dirVec = canMove ? input.normalized : Vector2.zero;
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        canMove = isEnabled;

        if (canMove)
            return;

        dirVec = Vector2.zero;
        speed = normalSpeed;
        rb.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (!canMove)
            return;

        if (dirVec != Vector2.zero)
        {
            if (speed < maxSpeed)
                speed += acceleration * Time.deltaTime;
        }
        else if (speed > normalSpeed)
        {
            speed -= deceleration * Time.deltaTime;
        }

        if (rb.linearVelocity.x > 0)
            sr.flipX = true;
        else if (rb.linearVelocity.x < 0)
            sr.flipX = false;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = canMove ? dirVec * speed : Vector2.zero;
    }
}
