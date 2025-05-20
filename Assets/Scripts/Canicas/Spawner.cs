using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabCanicaGrupo;
    public Transform puntoSpawn;
    public DetectorDeGrupos detectorDeGrupos;

    void Start()
    {
        detectorDeGrupos = FindObjectOfType<DetectorDeGrupos>();

        detectorDeGrupos.OnDeteccionTerminada += ManejarResultadoDeDeteccion;
        // Generar el primer grupo automáticamente al inicio
        GenerarGrupo();
    }

    public void ManejarResultadoDeDeteccion(bool huboDestruccion)
    {
        StartCoroutine(EsperarYGenerar(huboDestruccion));
    }

    IEnumerator EsperarYGenerar(bool huboDestruccion)
    {
        if (huboDestruccion)
        {
            GravedadDeCanicas gravedad = FindObjectOfType<GravedadDeCanicas>();
            if (gravedad != null)
            {
                yield return new WaitUntil(() => gravedad.CanicasEstables());
            }
        }

        yield return new WaitForSeconds(0.1f);

        // Prevenir generación múltiple
        if (NoHayCanicaGrupoActivo())
        {
            GenerarGrupo();
        }
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
