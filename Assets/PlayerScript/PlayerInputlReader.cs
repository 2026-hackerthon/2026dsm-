using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerMove playerMove;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove.dirVec = context.ReadValue<Vector2>().normalized;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }
}
