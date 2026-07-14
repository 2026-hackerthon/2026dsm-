using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float maxSpeed;

    private Vector3 dirVec;

    private GameObject scanObject;

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
        Vector2 dir = new Vector2(x, y);
        rb.linearVelocity = dir * speed;
        
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
}
