using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class ClearZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameSession.Instance?.CompleteGame();
    }
}
