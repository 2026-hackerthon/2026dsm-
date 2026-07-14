using System;
using UnityEngine;

public enum GameSessionState
{
    Playing,
    GameOver,
    Cleared
}

public sealed class GameSession : MonoBehaviour
{
    public const int MaxRescuedStudents = 4;

    public static GameSession Instance { get; private set; }

    [SerializeField, Min(1f)] private float sessionDuration = 480f;

    public float RemainingTime { get; private set; }
    public int RescuedStudentCount { get; private set; }
    public bool HasCardKey { get; private set; }
    public GameSessionState State { get; private set; }

    public event Action<float> RemainingTimeChanged;
    public event Action<int> RescueCountChanged;
    public event Action<bool> CardKeyChanged;
    public event Action<GameSessionState> StateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate GameSession instance detected.", this);
            Destroy(this);
            return;
        }

        Instance = this;
        RemainingTime = sessionDuration;
        RescuedStudentCount = 0;
        HasCardKey = false;
        State = GameSessionState.Playing;
    }

    private void Start()
    {
        RemainingTimeChanged?.Invoke(RemainingTime);
        RescueCountChanged?.Invoke(RescuedStudentCount);
        CardKeyChanged?.Invoke(HasCardKey);
        StateChanged?.Invoke(State);
    }

    private void Update()
    {
        if (State != GameSessionState.Playing)
            return;

        RemainingTime = Mathf.Max(0f, RemainingTime - Time.deltaTime);
        RemainingTimeChanged?.Invoke(RemainingTime);

        if (RemainingTime == 0f)
            GameOver();
    }

    public bool TryRescueStudent()
    {
        if (State != GameSessionState.Playing || RescuedStudentCount >= MaxRescuedStudents)
            return false;

        RescuedStudentCount++;
        RescueCountChanged?.Invoke(RescuedStudentCount);
        return true;
    }

    public bool TryAcquireCardKey()
    {
        if (State != GameSessionState.Playing || HasCardKey)
            return false;

        HasCardKey = true;
        CardKeyChanged?.Invoke(true);
        return true;
    }

    public void GameOver()
    {
        SetState(GameSessionState.GameOver);
    }

    public void CompleteGame()
    {
        SetState(GameSessionState.Cleared);
    }

    private void SetState(GameSessionState nextState)
    {
        if (State != GameSessionState.Playing || nextState == GameSessionState.Playing)
            return;

        State = nextState;
        StateChanged?.Invoke(State);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
