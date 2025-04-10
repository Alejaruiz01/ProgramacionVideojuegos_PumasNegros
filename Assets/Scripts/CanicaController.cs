using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveDelay = 0.15f;
    public float moveTimer;
    public float fastFallSpeed = 10f;
    private Rigidbody2D rb;
    private bool isFallingFast = false;
    private bool isSettled = false;
    public CanicaSpawner spawner;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveTimer += Time.deltaTime;

        HandleInput();
    }

    void HandleInput()
    {
        // Movimiento continuo si se mantiene la flecha
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && moveTimer >= moveDelay)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.position += Vector3.left;

            if (Input.GetKey(KeyCode.RightArrow))
                transform.position += Vector3.right;

            moveTimer = 0f; // Reinicia el contador
        }

        // Caída rápida
        if (Input.GetKeyDown(KeyCode.DownArrow))
            rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);

        // Rotación (aún con GetKeyDown, para que sea solo por toque)
        if (Input.GetKeyDown(KeyCode.Alpha1))
            transform.Rotate(0, 0, 90);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            transform.Rotate(0, 0, -90);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isSettled)
        {
            // Si colisiona con el suelo o con otra canica, se considera "colocada"
            if (collision.collider.CompareTag("Canica") || collision.collider.CompareTag("Suelo"))
            {
                isSettled = true;
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static; // Ya no se mueve

                // Espera un pequeño momento y luego genera una nueva
                Invoke(nameof(GenerarNuevaCanica), 0.3f);
            }
        }
    }

    void GenerarNuevaCanica()
    {
        if (spawner != null)
            spawner.SpawnCanica();
    }
}