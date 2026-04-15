using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Buttons")]
    public Button playButton, optionsButton, creditsButton, exitButton;
    public Button backOptionsButton, backCreditsButton;

    [Header("Main Panels")]
    public GameObject optionsPanel, creditsPanel;

    [Header("Sub-Menús de Opciones")]
    public Button soundButton;        
    public Button backSoundButton;   
    public GameObject soundPanel;   

    void Start()
    {
        
        optionsPanel?.SetActive(false);
        creditsPanel?.SetActive(false);
        soundPanel?.SetActive(false);

        // Listeners de los botones principales
        playButton?.onClick.AddListener(() => SceneManager.LoadScene("GameplayScene"));
        optionsButton?.onClick.AddListener(() => TogglePanel(optionsPanel, true));
        creditsButton?.onClick.AddListener(() => TogglePanel(creditsPanel, true));
        exitButton?.onClick.AddListener(Application.Quit);

        
        backOptionsButton?.onClick.AddListener(() => TogglePanel(optionsPanel, false));
        backCreditsButton?.onClick.AddListener(() => TogglePanel(creditsPanel, false));




        soundButton?.onClick.AddListener(() => { TogglePanel(soundPanel, true); TogglePanel(optionsPanel, false); });

        backSoundButton?.onClick.AddListener(() => { TogglePanel(soundPanel, false); TogglePanel(optionsPanel, true); });
    }

    void TogglePanel(GameObject panel, bool state)
    {
        if (panel != null) panel.SetActive(state);
    }
}