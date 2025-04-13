using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaGrupoController : MonoBehaviour
{
    [Header("Prefabs de canicas por color")]
    public GameObject[] canicaColores;

    [Header("Posiciones locales de las 3 canicas")]
    private Vector3[] posiciones = new Vector3[3]
    {
        new Vector3(0.02860022f, 0.521f, 0.01302223f),       // Canica de arriba
        new Vector3(-0.18f, 0.13f, 0.01302223f),    // Izquierda abajo
        new Vector3(0.25f, 0.13f, 0.01302223f)      // Derecha abajo
    };

    private void Start()
    {
        GenerarCanicasAleatorias();
    }

    void GenerarCanicasAleatorias()
    {
        for (int i = 0; i < 3; i++)
        {
            int indexColor = Random.Range(0, canicaColores.Length);
            GameObject nuevaCanica = Instantiate(canicaColores[indexColor], transform);

            nuevaCanica.transform.localPosition = posiciones[i];
            nuevaCanica.name = "Canica_" + i;
        }
    }
}