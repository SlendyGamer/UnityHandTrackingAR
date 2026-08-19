using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;

    [Header("Audio")]
    public AudioSource backgroundMusic;
    public AudioSource clickSound;

    private void Start()
    {
        // Make sure options panel is hidden at start
        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        // Play background music
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    // =====================
    // Button Functions
    // =====================

    public void PlayGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("Level1");
    }

    public void OpenOptions()
    {
        PlayClickSound();
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        PlayClickSound();
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        PlayClickSound();

        // Works only in build, not in editor
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // =====================
    // Helper
    // =====================

    private void PlayClickSound()
    {
        if (clickSound != null)
            clickSound.Play();
    }
}