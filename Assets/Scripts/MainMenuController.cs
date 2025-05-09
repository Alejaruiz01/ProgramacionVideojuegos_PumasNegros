using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject instructionsPanel;  // Panel de instrucciones
    public GameObject settingsPanel;      // Panel de configuraci�n de sonido

    // Funci�n para iniciar el juego
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");  // Cambia "GameScene" por el nombre de tu escena de juego.
    }

    // Funci�n para mostrar las instrucciones
    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);  // Muestra el panel de instrucciones
    }

    // Funci�n para ocultar las instrucciones
    public void HideInstructions()
    {
        instructionsPanel.SetActive(false); // Oculta el panel de instrucciones
    }

    // Funci�n para abrir la configuraci�n de sonido
    public void OpenSoundSettings()
    {
        settingsPanel.SetActive(true);  // Muestra el panel de configuraci�n de sonido
    }

    // Funci�n para cerrar la configuraci�n de sonido
    public void CloseSoundSettings()
    {
        settingsPanel.SetActive(false);  // Oculta el panel de configuraci�n de sonido
    }

    // Funci�n para salir del juego
    public void ExitGame()
    {
        Application.Quit();  // Cierra la aplicaci�n cuando est� compilada
    }
}


