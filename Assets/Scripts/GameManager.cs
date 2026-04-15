using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Objetivos de Victoria (Modificables)")]
    public int civilianObjective = 5;  // Cuántos civiles hay que rescatar
    public int scoreObjective = 150;   // Cuántos puntos hay que conseguir

    [Header("Estadísticas Actuales")]
    public int rescuedCivilians = 0;
    public int deadEnemies = 0;
    private int score = 0;

    [Header("UI Textos")]
    public TextMeshProUGUI civiliansText;
    public TextMeshProUGUI scoreText;

    [Header("UI Paneles Finales")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Botones")]
    public Button[] restartButtons;
    public Button[] menuButtons;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI(); // Actualizamos los textos apenas arranca el nivel

        // Asignación automática de los botones
        foreach (var btn in restartButtons) btn?.onClick.AddListener(RestartGame);
        foreach (var btn in menuButtons) btn?.onClick.AddListener(GoToMenu);
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
        CheckWinCondition();
    }

    public void CivilianRescued()
    {
        rescuedCivilians++;
        AddScore(100); // Te da 100 puntos rescatar a uno
        UpdateUI();
        CheckWinCondition();
    }

    public void EnemyKilled()
    {
        deadEnemies++;
      
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
       
        if (rescuedCivilians >= civilianObjective && score >= scoreObjective)
        {
            EndGame(victoryPanel);
        }
    }

    public void GameOver() => EndGame(defeatPanel);

    private void EndGame(GameObject panel)
    {
        if (panel != null) panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void UpdateUI()
    {
       
        if (scoreText) scoreText.text = $"Score: {score} / {scoreObjective}";
        if (civiliansText) civiliansText.text = $"Civilians: {rescuedCivilians} / {civilianObjective}";
    }

    public void RestartGame() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void GoToMenu() { Time.timeScale = 1f; SceneManager.LoadScene("MainMenu"); }
}