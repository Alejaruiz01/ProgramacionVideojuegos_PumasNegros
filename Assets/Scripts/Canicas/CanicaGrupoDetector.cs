using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoDetector : MonoBehaviour
{
    private bool seDetuvo = false;
    private Spawner spawner;

    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaChequeo = 0.6f;

    void Start()
    {
        spawner = FindObjectOfType<Spawner>();

        if (spawner == null)
            Debug.LogError("¡Spawner no encontrado!");
    }

    void Update()
    {
        if (!seDetuvo && VerificarContactoConSuelo())
        {
            Debug.Log("¡Todas las canicas tocaron suelo o se detuvieron!");
            seDetuvo = true;

            SepararCanicas();

            if (spawner != null)
            {
                spawner.RevisarDestruccion();
            }

            Destroy(gameObject);
        }
    }

    private bool VerificarContactoConSuelo()
    {
        int canicasDetenidas = 0;

        foreach (Transform canica in transform)
        {
            CircleCollider2D collider = canica.GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                float radio = collider.radius * canica.localScale.y;
                Vector2 origen = (Vector2)canica.position + Vector2.down * radio;

                RaycastHit2D hit = Physics2D.Raycast(origen, Vector2.down, distanciaChequeo, capaSuelo | LayerMask.GetMask("CanicaFija"));
                Debug.DrawRay(origen, Vector2.down * distanciaChequeo, Color.red, 0.2f);

                if (hit.collider != null)
                {
                    canicasDetenidas++;
                }
            }
            else
            {
                Debug.LogWarning($"La canica {canica.name} no tiene CircleCollider2D");
            }
        }

        return canicasDetenidas >= 3;
    }


    private void SepararCanicas()
    {
        Debug.Log("Separando canicas...");

        foreach (Transform canica in transform)
        {
            GameObject clon = Instantiate(canica.gameObject, canica.position, Quaternion.identity);
            clon.name = canica.name;
            clon.tag = canica.tag; // ¡No cambiamos el tag a "CanicaFija"!

            clon.layer = LayerMask.NameToLayer("CanicasDistribuidas");
            // clon.layer = LayerMask.NameToLayer("CanicaFija");

            Rigidbody2D rb = clon.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 1f;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            if (clon.GetComponent<GravedadDeCanicas>() == null)
            {
                clon.AddComponent<GravedadDeCanicas>();
            }

            clon.AddComponent<AutoCambiarLayer>().IniciarCambio("CanicaFija", 1.0f);
        }

        FindObjectOfType<DetectorDeGrupos>()?.StartCoroutine("DetectarYDestruirGrupos");
    }
}
