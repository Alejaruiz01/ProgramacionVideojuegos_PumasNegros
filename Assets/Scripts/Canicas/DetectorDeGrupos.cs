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

    public void IniciarDeteccion()
    {
        StartCoroutine(DetectarYDestruirGrupos());
    }

    public IEnumerator DetectarYDestruirGrupos()
    {
        huboDestruccion = false;
        yield return new WaitForSeconds(0.2f); // Espera tras caída

        string[] colores = new string[] { "VERDE", "ROJO", "AZUL", "AMARILLO", "MORADO" };
        List<IEnumerator> destruccionesPendientes = new List<IEnumerator>();

        foreach (string color in colores)
        {
            GameObject[] canicas = GameObject.FindGameObjectsWithTag(color);
            List<IEnumerator> corutinas = RevisarColor(canicas, color);
            destruccionesPendientes.AddRange(corutinas);
        }

        // Ejecutar las corutinas de destrucción secuencialmente
        foreach (var destruccion in destruccionesPendientes)
        {
            yield return StartCoroutine(destruccion);
        }

        // Llamar evento una vez que TODO se destruyó
        OnDeteccionTerminada?.Invoke(huboDestruccion);

        if (huboDestruccion)
        {
            // Activar gravedad
            FindObjectOfType<GravedadDeCanicas>()?.AcomodarCanicas();
        }
    }

    List<IEnumerator> RevisarColor(GameObject[] canicas, string colorTag)
    {
        HashSet<GameObject> visitadas = new HashSet<GameObject>();
        List<IEnumerator> corutinas = new List<IEnumerator>();

        foreach (GameObject canica in canicas)
        {
            if (!visitadas.Contains(canica))
            {
                List<GameObject> grupo = new List<GameObject>();
                BuscarConectadas(canica, grupo, colorTag, visitadas);

                if (grupo.Count >= 6)
                {
                    huboDestruccion = true;
                    GameManager.Instance?.AddPoints(10);
                    FindObjectOfType<RanitaCelebracion>()?.Celebrar();

                    corutinas.Add(DestruirGrupo(grupo)); // ← GUARDAMOS la corutina
                }
            }
        }

        return corutinas;
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

    private IEnumerator DestruirGrupo(List<GameObject> grupo)
    {
        // Espera el tiempo antes de destruir
        yield return new WaitForSeconds(tiempoAntesDeDestruir);

        // Efecto visual opcional
        foreach (GameObject canica in grupo)
        {
            SpriteRenderer sr = canica.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Color.white;
        }

        // Pequeño delay para el efecto
        yield return new WaitForSeconds(0.2f);

        // Destruye las canicas
        foreach (GameObject canica in grupo)
        {
            Destroy(canica);
        }
    }
    using UnityEngine;

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
    [SerializeField] private AudioClip sonidoDestruccion;
    public bool huboDestruccion = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void IniciarDeteccion()
    {
        StartCoroutine(DetectarYDestruirGrupos());
    }

    public IEnumerator DetectarYDestruirGrupos()
{
    huboDestruccion = false;
    yield return new WaitForSeconds(0.2f); // Espera tras caída

    string[] colores = new string[] { "VERDE", "ROJO", "AZUL", "AMARILLO", "MORADO" };
    List<IEnumerator> destruccionesPendientes = new List<IEnumerator>();

    foreach (string color in colores)
    {
        GameObject[] canicas = GameObject.FindGameObjectsWithTag(color);
        List<IEnumerator> corutinas = RevisarColor(canicas, color);
        destruccionesPendientes.AddRange(corutinas);
    }

    // Ejecutar las corutinas de destrucción secuencialmente
    foreach (var destruccion in destruccionesPendientes)
    {
        yield return StartCoroutine(destruccion);
    }

    // Llamar evento una vez que TODO se destruyó
    OnDeteccionTerminada?.Invoke(huboDestruccion);

    if (huboDestruccion)
    {
        // Activar gravedad
        FindObjectOfType<GravedadDeCanicas>()?.AcomodarCanicas();
    }
}

List<IEnumerator> RevisarColor(GameObject[] canicas, string colorTag)
{
    HashSet<GameObject> visitadas = new HashSet<GameObject>();
    List<IEnumerator> corutinas = new List<IEnumerator>();

    foreach (GameObject canica in canicas)
    {
        if (!visitadas.Contains(canica))
        {
            List<GameObject> grupo = new List<GameObject>();
            BuscarConectadas(canica, grupo, colorTag, visitadas);

            if (grupo.Count >= 6)
            {
                huboDestruccion = true;
                GameManager.Instance?.AddPoints(10);
                FindObjectOfType<RanitaCelebracion>()?.Celebrar();

                corutinas.Add(DestruirGrupo(grupo)); // ← GUARDAMOS la corutina
            }
        }
    }

    return corutinas;
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

    private IEnumerator DestruirGrupo(List<GameObject> grupo)
    {
        // Espera el tiempo antes de destruir
        yield return new WaitForSeconds(tiempoAntesDeDestruir);
        yield return new WaitForSeconds(0.2f);

        if (sonidoDestruccion != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDestruccion);
        }

        // Destruye las canicas
        foreach (GameObject canica in grupo)
        {
            Destroy(canica);
        }
    }
}

}
