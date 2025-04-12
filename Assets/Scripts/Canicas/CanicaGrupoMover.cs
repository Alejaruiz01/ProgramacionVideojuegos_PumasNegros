using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoMover : MonoBehaviour
{
    public float distanciaMovimiento = 1f;
    public float tiempoEntreMovimientos = 0.2f;
    private float tiempoUltimoMovimiento = 0f;

    private Transform[] canicas;

    private void Start()
    {
        // Obtener las canicas hijas
        canicas = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            canicas[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        float tiempoActual = Time.time;

        if (tiempoActual - tiempoUltimoMovimiento > tiempoEntreMovimientos)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * distanciaMovimiento;
                tiempoUltimoMovimiento = tiempoActual;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * distanciaMovimiento;
                tiempoUltimoMovimiento = tiempoActual;
            }
        }

        // Rotar el grupo de canicas (jugador 1)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotarCanicas();
        }
    }

    private void RotarCanicas()
    {
        if (canicas.Length != 3) return;

        // Rotar posiciones: A -> B, B -> C, C -> A
        Vector3 temp = canicas[0].localPosition;
        canicas[0].localPosition = canicas[2].localPosition;
        canicas[2].localPosition = canicas[1].localPosition;
        canicas[1].localPosition = temp;
    }
}