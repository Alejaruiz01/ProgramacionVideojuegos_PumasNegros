using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoControlador : MonoBehaviour
{
    private Spawner spawner;
    private bool detenido = false;

    public float distanciaChequeo = 0.1f;
    public LayerMask capaSuelo;

    private void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    private void Update()
    {
        if (!detenido)
        {
            if (VerificarDetencion())
            {
                detenido = true;

                foreach (Transform canica in transform)
                {
                    canica.tag = "CanicaFija";

                    Rigidbody2D rb = canica.GetComponent<Rigidbody2D>();
                    if (rb != null)
                        rb.bodyType = RigidbodyType2D.Static;
                }

                Destroy(gameObject);

                if (spawner != null)
                {
                    spawner.GenerarNuevoGrupo();
                }
            }
        }
    }

    private bool VerificarDetencion()
    {
        foreach (Transform canica in transform)
        {
            RaycastHit2D hit = Physics2D.Raycast(canica.position, Vector2.down, distanciaChequeo, capaSuelo);
            if (hit.collider != null)
            {
                return true;
            }
        }

        return false;
    }
}

