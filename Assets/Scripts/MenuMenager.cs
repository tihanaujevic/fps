using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMenager : MonoBehaviour
{
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button quitGameBtn;

    private void Awake()
    {
        startGameBtn.onClick.AddListener(() => StartGame());
        quitGameBtn.onClick.AddListener(() => QuitGame());
    }

    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
