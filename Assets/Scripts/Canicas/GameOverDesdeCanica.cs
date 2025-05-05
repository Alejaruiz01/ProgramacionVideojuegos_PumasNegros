using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDesdeCanica : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameOver"))
        {
            Debug.Log("¡Game Over! Canica alcanzó el GameOverTrigger.");

            // Aquí puedes añadir la lógica de fin del juego:
            Time.timeScale = 0f;
            // También podrías activar un panel de UI aquí si quieres.
        }
    }
}
