using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField] private string deathReason = "Fire hazard.";

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveAndEnabled)
            return;

        Deadable deadable = other.GetComponentInParent<Deadable>();
        if (deadable != null)
            deadable.Dead(deathReason);
    }
}
