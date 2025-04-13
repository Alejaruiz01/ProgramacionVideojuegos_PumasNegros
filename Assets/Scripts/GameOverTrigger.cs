using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    private bool juegoTerminado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (juegoTerminado) return;

        // Verificamos si el objeto tiene el layer "CanicaFija"
        if (collision.gameObject.layer == LayerMask.NameToLayer("CanicaFija"))
        {
            juegoTerminado = true;
            Debug.Log("¡Game Over! Canica fija alcanzó la parte superior.");

            // Pausar el juego o activar el menú de Game Over
            Time.timeScale = 0f;

            // Aquí puedes mostrar un panel de Game Over si quieres
        }
    }
}
