using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField] private string deathReason = "위험 요소에 닿았습니다.";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveAndEnabled)
            return;

        Deadable deadable = other.GetComponentInParent<Deadable>();
        if (deadable != null)
            deadable.Dead(deathReason);
    }
}
