using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCambiarLayer : MonoBehaviour
{
    private string nuevoLayer;
    private float tiempoEspera;

    public void IniciarCambio(string layerDestino, float espera)
    {
        nuevoLayer = layerDestino;
        tiempoEspera = espera;
        StartCoroutine(CambiarLayerDespues());
    }

    private System.Collections.IEnumerator CambiarLayerDespues()
    {
        yield return new WaitForSeconds(tiempoEspera);
        gameObject.layer = LayerMask.NameToLayer(nuevoLayer);

        // Opcional: cambiar bodyType para dejarlo estático
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Destroy(this); // Elimina este script después de cumplir su propósito
    }
}
