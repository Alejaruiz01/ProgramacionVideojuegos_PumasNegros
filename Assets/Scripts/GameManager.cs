using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int points = 0;
    public int level = 1;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI levelText;
    public GameObject pauseMenu;
    public GameObject messagePanel;
    public GameObject gameOverPanel;
    public GameObject reinicioButton; 
    public GameObject instructionsPanel;
    private bool isPaused = false;

    private int puntosParaSubirNivel = 20;

    void Awake()
    {
        if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        pauseMenu.SetActive(false);
        messagePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        reinicioButton.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            UnityEngine.Debug.LogError("No se encontró ningún Canvas en la escena.");
            return;
        }

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        UpdateUI();

        if (points >= puntosParaSubirNivel)
        {
            level++;
            points = 0;
            puntosParaSubirNivel += 10;

            StartCoroutine(LevelUpCoroutine());
        }
    }

    IEnumerator LevelUpCoroutine()
    {
        // Detenemos el juego
        Time.timeScale = 0f;
        // Mostrar tu panel de mensaje (ya lo tienes en messagePanel)
        messagePanel.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        // Lo volvemos a ocultar
        messagePanel.SetActive(false);
        // Volvemos a correr el tiempo
        Time.timeScale = 1f;

        // Recargamos la escena (o lo que quieras para resetear el tablero)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        reinicioButton.SetActive(true);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (!isPaused) pauseMenu.SetActive(false);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void RestartGame()
    {
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);

        // Resetear estado
        points = 0;
        level = 1;
        puntosParaSubirNivel = 20;
        UpdateUI();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReinicioButtonPressed()
    {
        RestartGame();
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        pauseMenu.SetActive(false); // Oculta el men� de pausa
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        pauseMenu.SetActive(true); // Vuelve a mostrar el men� de pausa
    }

    void UpdateUI()
    {
        if (pointsText != null)
        pointsText.text = "Puntos: " + points + "/" + puntosParaSubirNivel;

        if (levelText != null)
            levelText.text = "Nivel: " + level;

        UnityEngine.Debug.Log("Nivel: " + level + " | Gravedad: " + GetGravedadActual());
    }

    public float GetGravedadActual()
    {
        float escala = 0.1f + (level - 1) * 0.02f;
        return Mathf.Clamp(escala, 0.1f, 2f);
    }
}