using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravedadDeCanicas : MonoBehaviour
{
    public float velocidadCaida = 2f;
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

        yield return new WaitForSeconds(1f); // Tiempo para que caigan

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

    private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("Canica tocó: " + collision.gameObject.name);
}

}
