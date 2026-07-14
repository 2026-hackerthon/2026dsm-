using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(GameStart);
        if (quitButton != null)
            quitButton.onClick.AddListener(GameQuit);
    }

    private void GameStart()
    {
        SceneManager.LoadScene("MapLoading");
    }
    
    private void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
}
