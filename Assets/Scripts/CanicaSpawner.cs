using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanicaSpawner : MonoBehaviour
{
    public GameObject canicaPrefab;

    public void SpawnCanica()
    {
        GameObject nuevaCanica = Instantiate(canicaPrefab, transform.position, Quaternion.identity);
        nuevaCanica.transform.localScale *= 1.5f;

        // Pasa la referencia del spawner a la nueva canica
        CanicaController controller = nuevaCanica.GetComponent<CanicaController>();
        controller.spawner = this;
    }

    void Start()
    {
        SpawnCanica();
    }
}
