using UnityEngine;

public class Npc : MonoBehaviour
{
    private GameObject a;
    [SerializeField] private GameObject interaction_key;
    private Transform interaction_pos;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        interaction_pos = GetComponent<Transform>();
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            a = Instantiate(interaction_key, interaction_pos.position + offset, transform.rotation);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(a);
        }
    }
}
    