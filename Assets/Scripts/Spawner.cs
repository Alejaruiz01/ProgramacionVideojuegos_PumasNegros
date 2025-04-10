using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject canicaGrupoPrefab;
    //[SerializeField] private GameObject canicaGrupoPrefab;

    private GameObject canicaActual;

    public void GenerarNuevoGrupo()
    {
        if (canicaGrupoPrefab != null)
        {
            canicaActual = Instantiate(canicaGrupoPrefab, transform.position, Quaternion.identity);
            Debug.Log("Nuevo grupo instanciado");
        }
        else
        {
            Debug.LogError("No se ha asignado el prefab del grupo de canicas.");
        }
    }

    void Start()
    {
        GenerarNuevoGrupo();
    }
}
