using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class RescueZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        RescuableStudent student = other.GetComponentInParent<RescuableStudent>();
        if (student != null)
            student.CompleteRescue(transform.position);
    }
}
