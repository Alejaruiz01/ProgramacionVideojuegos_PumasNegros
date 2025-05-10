using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravedadDeCanicas : MonoBehaviour
{
    public float velocidadCaida = 8f;
    public LayerMask capaCanicaFija;

    public void AcomodarCanicas()
    {
        StartCoroutine(MoverCanicas());
    }

    IEnumerator MoverCanicas()
    {
        GameObject[] canicas = GameObject.FindGameObjectsWithTag("VERDE")
            .Concat(GameObject.FindGameObjectsWithTag("ROJO"))
            .Concat(GameObject.FindGameObjectsWithTag("AZUL"))
            .Concat(GameObject.FindGameObjectsWithTag("AMARILLO"))
            .Concat(GameObject.FindGameObjectsWithTag("MORADO")).ToArray();

        foreach (GameObject canica in canicas)
        {
            if (canica.layer == LayerMask.NameToLayer("CanicaFija"))
            {
                RaycastHit2D hit = Physics2D.Raycast(canica.transform.position, Vector2.down, 1f, capaCanicaFija);

                if (hit.collider == null)
                {
                    canica.layer = LayerMask.NameToLayer("CanicasDistribuidas"); // Temporal
                    Rigidbody2D rb = canica.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        rb.gravityScale = 1f;
                    }
                }
            }
        }

        // Esperar hasta que todas las canicas distribuidas estén estables
        yield return new WaitUntil(() => CanicasEstables());
        // Confirmar estabilidad durante varios frames (evitar falsos positivos)
        for (int i = 0; i < 5; i++) // Esperar 5 frames estables
        {
            yield return new WaitForSeconds(0.05f);
            if (!CanicasEstables())
            {
                i = 0; // Reinicia la cuenta si alguna canica aún se mueve
            }
        }

        // Al final, volver a colocarlas como "CanicaFija"
        foreach (GameObject canica in GameObject.FindObjectsOfType<GameObject>())
        {
            if (canica.layer == LayerMask.NameToLayer("CanicasDistribuidas"))
            {
                Rigidbody2D rb = canica.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Static;
                    rb.gravityScale = 0f;
                }

                canica.layer = LayerMask.NameToLayer("CanicaFija");
            }
        }
    }

    public bool CanicasEstables()
    {
        // Verifica que ninguna canica tenga velocidad significativa (ya no está cayendo)
        Rigidbody2D[] cuerpos = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in cuerpos)
        {
            if (rb.gameObject.layer == LayerMask.NameToLayer("CanicasDistribuidas"))
            {
                if (rb.velocity.magnitude > 0.05f || rb.IsAwake()) // Umbral de movimiento
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool CanicasEstablesWithFrames(int framesSeguidos)
    {
        // int contador = 0;
        Rigidbody2D[] cuerpos = FindObjectsOfType<Rigidbody2D>();
        foreach (var rb in cuerpos)
        {
            if (rb.gameObject.layer == LayerMask.NameToLayer("CanicasDistribuidas"))
            {
                if (rb.velocity.magnitude > 0.05f || rb.IsAwake())
                    return false;
            }
        }
        // Si llegas aquí, son estables este frame
        return true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Canica tocó: " + collision.gameObject.name);
    }

}
