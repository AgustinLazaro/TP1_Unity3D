using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Paneles Principales")]
    public GameObject pausePanel; // Tu "Panel" principal
    public GameObject soundPanel; // Tu "PanelSound"

    // NUEVO: Agregamos el contenedor de botones
    public GameObject contenedorBotones;

    [Header("Botones de Navegación")]
    public Button resumeButton;
    public Button soundButton;
    public Button backSoundButton;

    private bool isPaused = false;

    void Start()
    {
        SetPauseState(false);
        soundPanel?.SetActive(false);

        resumeButton?.onClick.AddListener(() => SetPauseState(false));

        
        soundButton?.onClick.AddListener(() => {
            soundPanel?.SetActive(true);
            contenedorBotones?.SetActive(false);
        });


        backSoundButton?.onClick.AddListener(() => {
            soundPanel?.SetActive(false);
            contenedorBotones?.SetActive(true);
        });
    }

    void Update()
    {
        
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !soundPanel.activeSelf)
        {
            SetPauseState(!isPaused);
        }
    }

    public void SetPauseState(bool state)
    {
        isPaused = state;
        pausePanel?.SetActive(state);

        
        if (state) contenedorBotones?.SetActive(true);

        Time.timeScale = state ? 0f : 1f;
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }
}