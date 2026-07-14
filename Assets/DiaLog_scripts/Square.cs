using UnityEngine;

public class Square : MonoBehaviour
{
    public TextManage manage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMouseEnter()
    {
        manage.Action(gameObject);
    }
}
