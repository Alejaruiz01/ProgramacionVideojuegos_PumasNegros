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
        if (detectorDeGrupos == null)
        {
            detectorDeGrupos = FindObjectOfType<DetectorDeGrupos>();
        }

        detectorDeGrupos.OnDeteccionTerminada += ManejarResultadoDeDeteccion;
        // Generar el primer grupo automáticamente al inicio
        GenerarGrupo();
    }

    void Update()
    {
        
    }

    void ManejarResultadoDeDeteccion(bool huboDestruccion)
    {
        StartCoroutine(EsperarYGenerar(huboDestruccion));
    }

    IEnumerator EsperarYGenerar(bool huboDestruccion)
    {
        if (huboDestruccion)
        {
            yield return new WaitForSeconds(1f);
        }

        // Prevenir generación múltiple
        if (NoHayCanicaGrupoActivo())
        {
            GenerarGrupo();
        }
        
    }

    public void PermitirGeneracion()
    {
        puedeGenerar = true;
    }

    public void RevisarDestruccion()
    {
        if (detectorDeGrupos != null)
        {
            detectorDeGrupos.IniciarDeteccion();
        }
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
