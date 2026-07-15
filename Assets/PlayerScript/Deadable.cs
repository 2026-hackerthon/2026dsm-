using UnityEngine;

public class Deadable : MonoBehaviour, Ideadable
{
    public void Dead()
    {
        Dead("위험 요소에 닿았습니다.");
    }

    public void Dead(string reason)
    {
        if (GameSession.Instance == null)
        {
            Debug.LogError("Deadable could not find GameSession.", this);
            return;
        }

        GameSession.Instance.GameOver(reason);
    }
}
