using UnityEngine;
using TMPro;

public class SoundController : MonoBehaviour
{
    public AudioSource musicSource; // el componente que reproduce la m�sica
    public TextMeshProUGUI buttonText; // el texto del bot�n

    private bool isMuted = false;

    void Start()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        UpdateButtonText();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = isMuted ? "Activar Sonido" : "Silenciar";
        }
    }
}

