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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

        if (points >= 200)
        {
            level++;
            points = 0;
            StopAllCoroutines();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowMessage()
    {
        StartCoroutine(ShowMessageCoroutine());
    }

    IEnumerator ShowMessageCoroutine()
    {
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        messagePanel.SetActive(false);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        reinicioButton.SetActive(true);
    }

    public void OnReinicioButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        pauseMenu.SetActive(false); // Oculta el menú de pausa
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        pauseMenu.SetActive(true); // Vuelve a mostrar el menú de pausa
    }

    void UpdateUI()
    {
        pointsText.text = "Puntos: " + points;
        //levelText.text = "Nivel: " + level;
    }
}

