using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject canicaGrupoPrefab;

    private GameObject canicaActual;

    public void GenerarNuevoGrupo()
    {
        if (canicaGrupoPrefab == null)
        {
            Debug.LogError("Prefab del grupo de canicas no asignado.");
            return;
        }

        // Instanciar el grupo (que ya tiene las 3 canicas hijas)
        canicaActual = Instantiate(canicaGrupoPrefab, transform.position, Quaternion.identity);
        Debug.Log("Nuevo grupo instanciado desde prefab.");
    }

    void Start()
    {
        GenerarNuevoGrupo();
    }
}
