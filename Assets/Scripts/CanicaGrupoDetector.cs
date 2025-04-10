using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoDetector : MonoBehaviour
{
    private bool seDetuvo = false;
    private Spawner spawner;

    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaChequeo = 0.1f;

    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    void Update()
    {
        if (!seDetuvo && VerificarContactoConSuelo())
        {
            seDetuvo = true;
            SepararCanicas();
            Invoke(nameof(GenerarNuevoGrupo), 0.1f); // Un pequeño retraso para evitar conflictos
        }
    }

    private bool VerificarContactoConSuelo()
    {
        foreach (Transform canica in transform)
        {
            Vector2 posicion = canica.position;
            RaycastHit2D hit = Physics2D.Raycast(posicion, Vector2.down, distanciaChequeo, capaSuelo);
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private void SepararCanicas()
    {
        foreach (Transform canica in transform)
        {
            GameObject clon = Instantiate(canica.gameObject, canica.position, Quaternion.identity);
            clon.name = canica.name; // Opcional: mantener el nombre

            clon.tag = "CanicaFija";
            clon.layer = LayerMask.NameToLayer("Suelo");

            Rigidbody2D rb = clon.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }

        Destroy(gameObject); // Destruir el grupo padre original
    }

    private void GenerarNuevoGrupo()
    {
        if (spawner != null)
        {
            spawner.GenerarNuevoGrupo();
            Debug.Log("¡Nuevo grupo generado por CanicaGrupoDetector!");
        }
    }
}