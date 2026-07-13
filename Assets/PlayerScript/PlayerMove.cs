using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float maxSpeed;

    // public TextManage manage;
 
    private float x;
    private float y;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        
        if (Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0)
        {
            if (speed < maxSpeed)
            {
                speed += Time.deltaTime * 5;
            }
        }
        else if (speed > normalSpeed)
        {
            speed -= Time.deltaTime * 10;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //manage.Action(scanObject);
        }
    }

    void FixedUpdate()
    {
        Vector2 dir = new Vector2(x, y);
        rb.linearVelocity = dir * speed;
    }
}
