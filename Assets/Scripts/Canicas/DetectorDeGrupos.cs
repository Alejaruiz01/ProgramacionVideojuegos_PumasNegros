using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorDeGrupos : MonoBehaviour
{
    [SerializeField] private float tiempoAntesDeDestruir = 0.5f;
    [SerializeField] private float radioBusqueda = 0.55f;
    [SerializeField] private LayerMask capaCanicasFija;
    public bool huboDestruccion = false;

    void Start()
    {
        // Iniciar la detección después de un pequeño retraso
        StartCoroutine(DetectarYDestruirGrupos());
    }

    public IEnumerator DetectarYDestruirGrupos()
    {
        yield return new WaitForSeconds(0.5f); // Espera tras caída
        huboDestruccion = false;

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

        yield return null;
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

                if (grupo.Count >= 6)
                {
                    huboDestruccion = true;
                    StartCoroutine(DestruirGrupo(grupo));
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

    IEnumerator DestruirGrupo(List<GameObject> grupo)
    {
        // Efecto visual antes de destruir
        foreach (GameObject canica in grupo)
        {
            SpriteRenderer sr = canica.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Color.white; // Color temporal antes de destruir
        }

        yield return new WaitForSeconds(tiempoAntesDeDestruir);

        foreach (GameObject canica in grupo)
        {
            Destroy(canica);
        }

        huboDestruccion = true;
    }
}