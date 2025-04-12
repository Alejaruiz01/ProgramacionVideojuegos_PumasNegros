using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaIndividualDetector : MonoBehaviour
{
    [SerializeField] private LayerMask capaSuelo;
    public LayerMask capaCanicasFija;
    private Rigidbody2D rb2D;

    private bool yaSeDetuvo = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        
        if (capaSuelo == 0)
        {
            capaSuelo = LayerMask.GetMask("Suelo");
        }
        
        if (capaCanicasFija == 0)
        {
            capaCanicasFija = LayerMask.GetMask("CanicaFija");
        }

        InvokeRepeating("IntentarCaer", 0.1f, 0.05f); // más rápido
    }


    void IntentarCaer()
    {
        if (yaSeDetuvo) return;

        Vector2 posicionActual = transform.position;
        Vector2 abajo = Vector2.down;
        float distancia = 0.6f;

        // Verificar si hay suelo o canica fija abajo
        RaycastHit2D hitAbajo = Physics2D.Raycast(posicionActual, abajo, distancia, capaSuelo | capaCanicasFija);

        if (hitAbajo.collider == null)
        {
            if (rb2D.bodyType == RigidbodyType2D.Kinematic)
            {
                rb2D.gravityScale = 1f;
                rb2D.bodyType = RigidbodyType2D.Dynamic;
            }
            return;
        }

        // Ya hay algo abajo, detenerse
        if (rb2D.bodyType != RigidbodyType2D.Static)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.gravityScale = 0f;
            rb2D.bodyType = RigidbodyType2D.Kinematic;
        }

        yaSeDetuvo = true;


        // Intentar moverse a izquierda o derecha si hay espacio libre abajo
        Vector2[] direcciones = { Vector2.left, Vector2.right };

        foreach (Vector2 dir in direcciones)
        {
            Vector2 posicionLateral = posicionActual + (Vector2.down * 0.5f) + (dir * 0.5f);

            Collider2D col = Physics2D.OverlapCircle(posicionLateral, 0.2f, capaCanicasFija | capaSuelo);
            if (col == null)
            {
                // Hay espacio diagonal abajo a un lado, moverse hacia allá
                transform.position = Vector2.MoveTowards(transform.position, posicionLateral, 0.05f);
                return;
            }
        }

        // Si no se puede mover a los lados, quedarse en su sitio
    }
}
