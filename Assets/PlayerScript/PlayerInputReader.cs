using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        if (!TryGetComponent(out playerMove))
        {
            Debug.LogError($"{nameof(PlayerInputReader)} requires a {nameof(PlayerMove)} component.", this);
            enabled = false;
            return;
        }

        if (!TryGetComponent(out playerInteraction))
        {
            Debug.LogError($"{nameof(PlayerInputReader)} requires a {nameof(PlayerInteraction)} component.", this);
            enabled = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerMove == null)
            return;

        playerMove.SetMoveInput(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && playerInteraction != null)
            playerInteraction.TryInteract();
    }
}
