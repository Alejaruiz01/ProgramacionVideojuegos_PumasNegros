using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorDeGrupos : MonoBehaviour
{
    public Action<bool> OnDeteccionTerminada;
    [SerializeField] private float tiempoAntesDeDestruir = 0.5f;
    [SerializeField] private float radioBusqueda = 0.55f;
    [SerializeField] private LayerMask capaCanicasFija;
    public bool huboDestruccion = false;

    private List<List<GameObject>> gruposParaDestruir = new List<List<GameObject>>();

    void Start()
    {
    }

    public void IniciarDeteccion()
    {
        StartCoroutine(DetectarYDestruirGrupos());
    }

    public IEnumerator DetectarYDestruirGrupos()
    {
        huboDestruccion = false;
        yield return null;

        GameObject[] canicas = GameObject.FindGameObjectsWithTag("VERDE");
        RevisarColor(canicas, "VERDE");

        canicas = GameObject.FindGameObjectsWithTag("ROJO");
        RevisarColor(canicas, "ROJO");

        canicas = GameObject.FindGameObjectsWithTag("AZUL");
        RevisarColor(canicas, "AZUL");

        canicas = GameObject.FindGameObjectsWithTag("AMARILLO");
        RevisarColor(canicas, "AMARILLO");

        canicas = GameObject.FindGameObjectsWithTag("MORADO");
        RevisarColor(canicas, "MORADO");

        if (gruposParaDestruir.Count > 0)
        {
            StartCoroutine(DestruirTodosLosGrupos(gruposParaDestruir));
        }


        yield return new WaitForSeconds(tiempoAntesDeDestruir + 0.1f);

        OnDeteccionTerminada?.Invoke(huboDestruccion);

        if (huboDestruccion)
            {
                FindObjectOfType<GravedadDeCanicas>()?.AcomodarCanicas();
            }

    }

    void RevisarColor(GameObject[] canicas, string colorTag)
    {
        HashSet<GameObject> visitadas = new HashSet<GameObject>();

        foreach (GameObject canica in canicas)
        {
            if (!visitadas.Contains(canica))
            {
                List<GameObject> grupo = new List<GameObject>();
                BuscarConectadas(canica, grupo, colorTag, visitadas);

                if (grupo.Count == 6)
                {
                    gruposParaDestruir.Add(grupo);
                    Debug.Log("Se agrego al grupo");
                    huboDestruccion = true;
                }
            }
        }
    }

    void BuscarConectadas(GameObject origen, List<GameObject> grupo, string colorTag, HashSet<GameObject> visitadas)
    {
        grupo.Add(origen);
        visitadas.Add(origen);

        Collider2D[] cercanas = Physics2D.OverlapCircleAll(origen.transform.position, radioBusqueda, capaCanicasFija);
        foreach (Collider2D col in cercanas)
        {
            if (col != null && col.CompareTag(colorTag))
            {
                GameObject vecina = col.gameObject;
                if (!visitadas.Contains(vecina))
                {
                    BuscarConectadas(vecina, grupo, colorTag, visitadas);
                }
            }
        }
    }

    IEnumerator DestruirTodosLosGrupos(List<List<GameObject>> grupos)
    {
        yield return new WaitForSeconds(tiempoAntesDeDestruir);

        foreach (List<GameObject> grupo in grupos)
        {
            foreach (GameObject canica in grupo)
            {
                SpriteRenderer sr = canica.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = Color.white; // Efecto visual opcional
            }
        }

        yield return new WaitForSeconds(0.2f); // Espera para el efecto visual

        foreach (List<GameObject> grupo in grupos)
        {
            foreach (GameObject canica in grupo)
            {
                Destroy(canica);
                Debug.Log("Se destruyeron las canicas");
            }

            // ✅ Aquí puedes sumar puntos por grupo destruido
            // GameManager.Instance?.SumarPuntos(grupo.Count); // En tu caso, siempre será 6
        }

        gruposParaDestruir.Clear();
    }

    public void IniciarDeteccionConGravedad()
    {
        StartCoroutine(DeteccionEnCadena());
    }

    private IEnumerator DeteccionEnCadena()
    {
        bool destruccionRealizada = false;

        // Paso 1: Detección inicial
        yield return StartCoroutine(DetectarYDestruirGruposConResultado(r => destruccionRealizada = r));

        if (!destruccionRealizada)
        {
            // No hubo destrucción, notifica para generar nuevo grupo
            FindObjectOfType<Spawner>()?.ManejarResultadoDeDeteccion(false);
            yield break;
        }

        // Paso 2: Esperar y aplicar gravedad
        var gravedad = FindObjectOfType<GravedadDeCanicas>();
        if (gravedad != null)
        {
            gravedad.AcomodarCanicas();

            // Espera a que todas las canicas estén estables
            yield return new WaitUntil(() => gravedad.CanicasEstablesWithFrames(5));
        }

        // Paso 3: Espera un momento y revisa si se formaron nuevos grupos
        yield return new WaitForSeconds(0.1f);

        if (HayGruposParaDestruir())
        {
            // Repetir la detección en cadena
            yield return DeteccionEnCadena();
        }
        else
        {        
            // Ahora sí se puede generar nuevo grupo
            FindObjectOfType<Spawner>()?.ManejarResultadoDeDeteccion(false);
        }
    }



    private bool HayGruposParaDestruir()
    {
        string[] colores = new string[] { "ROJO", "AZUL", "AMARILLO", "VERDE", "MORADO" };
        foreach (string color in colores)
        {
            GameObject[] canicas = GameObject.FindGameObjectsWithTag(color);
            HashSet<GameObject> visitadas = new HashSet<GameObject>();

            foreach (GameObject canica in canicas)
            {
                if (!visitadas.Contains(canica))
                {
                    List<GameObject> grupo = new List<GameObject>();
                    BuscarConectadas(canica, grupo, color, visitadas);

                    if (grupo.Count == 6)
                        return true;
                }
            }
        }
        return false;
    }

    private IEnumerator DetectarYDestruirGruposConResultado(Action<bool> callback)
    {
        yield return StartCoroutine(DetectarYDestruirGrupos());
        callback?.Invoke(huboDestruccion);
    }
}