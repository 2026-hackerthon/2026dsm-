using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Deadable deadable;
    
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration = 5f; // 가속도
    [SerializeField] private float deceleration = 10f; // 감속도   
    
    public Vector3 dirVec;

    private GameObject scanObject;

    // public TextManage manage;
 
    private float x;
    private float y;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        deadable = GetComponent<Deadable>();
        
        speed = normalSpeed;
    }

    void Update()
    {
        if (dirVec != Vector3.zero)
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            //manage.Action(scanObject);
        }

        if (y == 1)
            dirVec = Vector3.up;
        else if (y == -1)
            dirVec = Vector3.down;
        else if (x == -1)
            dirVec = Vector3.left;
        else if (x == 1)
            dirVec = Vector3.right;

        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("this is :" + scanObject.name);
        }
    }

    void FixedUpdate()
    {
        //Vector2 dir = new Vector2(x, y);
        //rb.linearVelocity = dir * speed;
        
        rb.linearVelocity = dirVec * speed;
        
        //Ray
        Debug.DrawRay(rb.position, dirVec, Color.green);
        RaycastHit2D rayHit =  Physics2D.Raycast(rb.position, dirVec, 1, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
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
