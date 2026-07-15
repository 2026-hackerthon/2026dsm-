using UnityEngine;

public enum RescueStudentState
{
    Waiting,
    Talking,
    Following,
    Rescued
}

[RequireComponent(typeof(StudentFollower))]
public sealed class RescuableStudent : MonoBehaviour, IInteractable
{
    [SerializeField] private string studentName;
    [SerializeField, TextArea] private string[] dialogueLines;
    [SerializeField] private Collider2D interactionCollider;

    private StudentFollower follower;
    private Rigidbody2D body;
    private Transform player;
    private PlayerMove playerMove;

    public RescueStudentState State { get; private set; }

    private void Awake()
    {
        follower = GetComponent<StudentFollower>();
        body = GetComponent<Rigidbody2D>();

        if (interactionCollider == null)
            interactionCollider = GetComponent<Collider2D>();

        State = RescueStudentState.Waiting;
    }

    public void Interact(GameObject interactor)
    {
        if (State == RescueStudentState.Waiting)
            BeginDialogue(interactor);
        else if (State == RescueStudentState.Talking)
            AdvanceDialogue();
    }

    public void CompleteRescue(Vector2 safePosition)
    {
        if (State != RescueStudentState.Following)
            return;

        State = RescueStudentState.Rescued;
        follower.StopFollowing();
        body.position = safePosition;
        interactionCollider.enabled = false;
    }

    private void BeginDialogue(GameObject interactor)
    {
        PlayerMove move = interactor.GetComponent<PlayerMove>();
        if (move == null || DialoguePanel.Instance == null)
        {
            Debug.LogError("Student interaction requires PlayerMove and DialoguePanel.", this);
            return;
        }

        if (!DialoguePanel.Instance.TryOpen(studentName, dialogueLines))
            return;

        player = interactor.transform;
        playerMove = move;
        playerMove.SetMovementEnabled(false);
        State = RescueStudentState.Talking;
    }

    private void AdvanceDialogue()
    {
        if (!DialoguePanel.Instance.Advance())
            return;

        playerMove.SetMovementEnabled(true);

        if (GameSession.Instance == null || !GameSession.Instance.TryRescueStudent())
        {
            Debug.LogError("Student rescue could not update GameSession.", this);
            State = RescueStudentState.Waiting;
            return;
        }

        State = RescueStudentState.Following;
        transform.SetParent(null, true);
        follower.BeginFollowing(player);
    }
}
