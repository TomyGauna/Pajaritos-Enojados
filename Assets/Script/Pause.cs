using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject playerPanel;

    public void Pausa()
    {
        Time.timeScale = 0f;

        pausePanel.SetActive(true);

        playerPanel.SetActive(false);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);

        playerPanel.SetActive(true);
    }
}
