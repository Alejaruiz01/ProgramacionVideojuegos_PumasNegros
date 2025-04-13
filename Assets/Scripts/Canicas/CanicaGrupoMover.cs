using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoMover : MonoBehaviour
{
    public float distanciaMovimiento = 1f;
    public float tiempoEntreMovimientos = 0.2f;
    private float tiempoUltimoMovimiento = 0f;

    private Transform[] canicas = new Transform[3];
    private Vector3[] posiciones = new Vector3[3];

    private void Start()
    {
        // Obtener las canicas hijas automáticamente
        for (int i = 0; i < 3; i++)
        {
            canicas[i] = transform.Find("Canica_" + i);
        }

        // Guardar las posiciones iniciales (triángulo normal)
        for (int i = 0; i < 3; i++)
        {
            posiciones[i] = canicas[i].localPosition;
        }

        // Obtener las canicas hijas
        // canicas = new Transform[transform.childCount];
        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     canicas[i] = transform.GetChild(i);
        // }
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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotarEnTriangulo();
        }

        // Rotar el grupo de canicas (jugador 1)
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     RotarCanicas();
        // }
    }

    void AplicarPosiciones()
    {
        for (int j = 0; j < canicas.Length; j++)
        {
            canicas[j].localPosition = posiciones[j];
        }
    }

    void RotarEnTriangulo()
    {
        Vector3 temp = canicas[0].localPosition;

        canicas[0].localPosition = canicas[1].localPosition;
        canicas[1].localPosition = canicas[2].localPosition;
        canicas[2].localPosition = temp;
    }

    // private void RotarCanicas()
    // {
    //     if (canicas.Length != 3) return;

    //     // Rotar posiciones: A -> B, B -> C, C -> A
    //     Vector3 temp = canicas[0].localPosition;
    //     canicas[0].localPosition = canicas[2].localPosition;
    //     canicas[2].localPosition = canicas[1].localPosition;
    //     canicas[1].localPosition = temp;
    // }
}