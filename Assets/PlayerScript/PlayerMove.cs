using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Deadable deadable;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    
    [SerializeField] private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration = 5f; // 가속도
    [SerializeField] private float deceleration = 10f; // 감속도   
    
    private Vector2 dirVec;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        deadable = GetComponent<Deadable>();
        
        speed = normalSpeed;
    }

    public void SetMoveInput(Vector2 input)
    {
        dirVec = input.normalized;
    }

    void Update()
    {
        if (dirVec != Vector2.zero)
        {
            if (speed < maxSpeed)
            {
                speed += acceleration * Time.deltaTime;
            }
        }
        else
        {
            if (speed > normalSpeed)
            {
                speed -= deceleration * Time.deltaTime;
            }
        }

        if (rb.linearVelocity.x > 0)
            sr.flipX = true;
        else if (rb.linearVelocity.x < 0)
            sr.flipX = false;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = dirVec * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Dead");
            deadable.Dead();
        }
    }
}
