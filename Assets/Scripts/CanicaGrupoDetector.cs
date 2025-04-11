using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoDetector : MonoBehaviour
{
    private bool seDetuvo = false;
    private Spawner spawner;

    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaChequeo = 0.3f;

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
            Debug.Log("¡Contacto con el suelo detectado!");
            seDetuvo = true;

            SepararCanicas();

            // ✅ Mover esto antes del Destroy
            if (spawner != null)
            {
                spawner.GenerarNuevoGrupo();
                Debug.Log("¡Nuevo grupo generado por CanicaGrupoDetector!");
            }

            Destroy(gameObject);
        }
    }

    private bool VerificarContactoConSuelo()
    {
        foreach (Transform canica in transform)
        {
            Vector2 posicion = canica.position;
            RaycastHit2D hit = Physics2D.Raycast(posicion, Vector2.down, distanciaChequeo, capaSuelo);
            Debug.DrawRay(posicion, Vector2.down * distanciaChequeo, Color.red, 0.2f);

            if (hit.collider != null)
            {
                Debug.Log($"Raycast impactó con: {hit.collider.name}");
                return true;
            }
        }
        return false;
    }

    private void SepararCanicas()
    {
        Debug.Log("Separando canicas...");

        foreach (Transform canica in transform)
        {
            GameObject clon = Instantiate(canica.gameObject, canica.position, Quaternion.identity);
            clon.name = canica.name;
            clon.tag = "CanicaFija";
            clon.layer = LayerMask.NameToLayer("Suelo");

            Rigidbody2D rb = clon.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}