using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCanicaGrupo : MonoBehaviour
{
    [SerializeField] private float distanciaMovimiento = 1f;
    //public float distanciaMovimiento = 1f;
    [SerializeField] private float tiempoEntreMovimientos = 0.15f;
    //public float tiempoEntreMovimientos = 0.15f;

    private float tiempoProximoMovimientoX = 0f;

    void Update()
    {
        float direccionX = Input.GetAxisRaw("Horizontal");

        if (direccionX != 0 && Time.time > tiempoProximoMovimientoX)
        {
            MoverHorizontal(direccionX);
            tiempoProximoMovimientoX = Time.time + tiempoEntreMovimientos;
        }
    }

    void MoverHorizontal(float direccion)
    {
        Vector3 nuevaPosicion = transform.position + new Vector3(direccion * distanciaMovimiento, 0, 0);
        transform.position = nuevaPosicion;
    }
}