using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabCanicaGrupo;
    public Transform puntoSpawn;
    public DetectorDeGrupos detectorDeGrupos;

    private bool puedeGenerar = false;

    void Start()
    {
        detectorDeGrupos = FindObjectOfType<DetectorDeGrupos>();
        // Generar el primer grupo automáticamente al inicio
        GenerarGrupo();
    }

    void Update()
    {
        if (puedeGenerar && NoHayCanicaGrupoActivo())
        {
            puedeGenerar = false;
            StartCoroutine(EsperarYGenerar());
        }
    }

    IEnumerator EsperarYGenerar()
    {
        if (detectorDeGrupos != null && detectorDeGrupos.huboDestruccion)
        {
            yield return new WaitForSeconds(5f);
        }
        
        GenerarGrupo();
    }

    public void PermitirGeneracion()
    {
        puedeGenerar = true;
    }

    public bool NoHayCanicaGrupoActivo()
    {
        return GameObject.FindObjectOfType<CanicaGrupoDetector>() == null;
    }

    void GenerarGrupo()
    {
        Instantiate(prefabCanicaGrupo, puntoSpawn.position, Quaternion.identity);
    }
}
