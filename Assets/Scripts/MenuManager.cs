using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject stopMenu;
    public GameObject mainButtons;
    public GameObject optionsPanel;

    [Header("Cursor")]
    public Texture2D menuCursor;
    public Vector2 cursorHotspot = Vector2.zero;

    [Header("Audio")]
    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    bool isPaused = false;


    [Header("Settings")]
    public Toggle soundFXToggle;
    public Toggle visualFXToggle;


    void Start()
    {
        ResumeGame();

        if (soundFXToggle != null)
        {
            soundFXToggle.isOn = AudioListener.volume > 0f;
            soundFXToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        }
        if (visualFXToggle != null)
        {
            visualFXToggle.isOn = true;
            visualFXToggle.onValueChanged.AddListener(OnVisualFXToggleChanged);
        }

    }
    public void OnSoundToggleChanged(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
    }
    public void OnVisualFXToggleChanged(bool isOn)
    {
        if (VFXManager.Instance != null)
            VFXManager.Instance.SetVFX(isOn);
    }



    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (optionsPanel.activeSelf)
        {
            CloseOptions();
            return;
        }

        TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        stopMenu.SetActive(isPaused);
        mainButtons.SetActive(isPaused);
        optionsPanel.SetActive(false);

        Time.timeScale = isPaused ? 0f : 1f;
        SetCursor(isPaused);
    }

    public void ResumeGame()
    {
        PlayClick();

        isPaused = false;

        stopMenu.SetActive(false);
        mainButtons.SetActive(false);
        optionsPanel.SetActive(false);

        Time.timeScale = 1f;
        SetCursor(false);
    }

    public void OpenOptions()
    {
        PlayClick();

        mainButtons.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        PlayClick();

        optionsPanel.SetActive(false);
        mainButtons.SetActive(true);
    }

    void SetCursor(bool menuOpen)
    {
        if (menuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Cursor.SetCursor(menuCursor, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    void PlayClick()
    {
        if (uiAudioSource != null && clickSound != null)
            uiAudioSource.PlayOneShot(clickSound);
    }

    public void QuitGame()
    {
        PlayClick();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
