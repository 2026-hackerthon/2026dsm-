using UnityEngine;
using UnityEngine.EventSystems;

public class GameTitleUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;

    [SerializeField] private float normalSpeed = 60f;
    [SerializeField] private float fastSpeed = 360f;

    private float currentSpeed;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        rectTransform.Rotate(0, 0, currentSpeed * Time.deltaTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentSpeed = fastSpeed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currentSpeed = normalSpeed;
    }
}