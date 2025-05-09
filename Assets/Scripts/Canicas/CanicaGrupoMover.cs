using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoMover : MonoBehaviour
{
    public float distanciaMovimiento = 1f;
    private float retardoInicial = 0.2f;
    private float velocidadRepeticion = 0.05f;
    private bool moviendo = false;
    private float tiempoProximaRepeticion;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            moviendo = true;
            tiempoProximaRepeticion = Time.time + retardoInicial;
            MoverGrupo(Input.GetKey(KeyCode.LeftArrow) ? Vector3.left : Vector3.right);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moviendo = false;
        }

        if (moviendo && Time.time >= tiempoProximaRepeticion)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoverGrupo(Vector3.left);
                tiempoProximaRepeticion = Time.time + velocidadRepeticion;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                MoverGrupo(Vector3.right);
                tiempoProximaRepeticion = Time.time + velocidadRepeticion;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * distanciaMovimiento * 10f * Time.deltaTime; // Más rápido que el normal
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotarEnTriangulo();
        }
    }

    private void MoverGrupo(Vector3 direccion)
    {
        transform.position += direccion * distanciaMovimiento;
    }

    void RotarEnTriangulo()
    {
        Vector3 temp = canicas[0].localPosition;

        canicas[0].localPosition = canicas[1].localPosition;
        canicas[1].localPosition = canicas[2].localPosition;
        canicas[2].localPosition = temp;
    }
}