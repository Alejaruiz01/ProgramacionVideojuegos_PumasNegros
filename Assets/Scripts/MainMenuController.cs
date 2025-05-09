using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject instructionsPanel;  // Panel de instrucciones
    public GameObject settingsPanel;      // Panel de configuración de sonido

    // Función para iniciar el juego
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");  // Cambia "GameScene" por el nombre de tu escena de juego.
    }

    // Función para mostrar las instrucciones
    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);  // Muestra el panel de instrucciones
    }

    // Función para ocultar las instrucciones
    public void HideInstructions()
    {
        instructionsPanel.SetActive(false); // Oculta el panel de instrucciones
    }

    // Función para abrir la configuración de sonido
    public void OpenSoundSettings()
    {
        settingsPanel.SetActive(true);  // Muestra el panel de configuración de sonido
    }

    // Función para cerrar la configuración de sonido
    public void CloseSoundSettings()
    {
        settingsPanel.SetActive(false);  // Oculta el panel de configuración de sonido
    }

    // Función para salir del juego
    public void ExitGame()
    {
        Application.Quit();  // Cierra la aplicación cuando está compilada
    }
}


