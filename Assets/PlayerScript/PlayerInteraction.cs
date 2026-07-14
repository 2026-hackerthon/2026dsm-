using UnityEngine;

public sealed class PlayerInteraction : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float interactionRadius = 1f;

    public void TryInteract()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        IInteractable closestTarget = null;
        var closestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            var target = collider.GetComponentInParent<IInteractable>();
            if (target == null)
                continue;

            var distance = ((Vector2)collider.transform.position - (Vector2)transform.position).sqrMagnitude;
            if (distance >= closestDistance)
                continue;

            closestTarget = target;
            closestDistance = distance;
        }

        closestTarget?.Interact(gameObject);
    }
}
