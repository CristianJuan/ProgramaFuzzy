//Creado por Zereck
// Para cualquier duda o sugerencia puedes contactarme en www.Zereck.net o en youtube "Zereck - Desarrollo de Videojuegos"

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaypointController : MonoBehaviour {

    public static Dictionary<string, WaypointController> pathDictionary = new Dictionary<string, WaypointController>();
    public static Dictionary<string, List<WaypointController>> pathTagDictionary = new Dictionary<string, List<WaypointController>>();

    public string nombreRuta;
    public string[] listaTag;
    public List<GameObject> listaPuntos = new List<GameObject>();

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(nombreRuta))
        {
            Debug.LogWarning(string.Concat("El GameObject '", gameObject.name, "' no tiene asignado un nombre"));
        }
        if (!pathDictionary.ContainsKey(nombreRuta))
        {
            pathDictionary.Add(nombreRuta.ToLower(), this);
        }

        foreach(string path in listaTag)
        {
            string tempTag = path.ToLower();
            if (!pathTagDictionary.ContainsKey(tempTag))
            {
                pathTagDictionary.Add(tempTag, new List<WaypointController>());
                pathTagDictionary[tempTag].Add(this);
            }
            else
            {
                pathTagDictionary[tempTag].Add(this);
            }
        }
    }

    private void OnDisable()
    {
        pathDictionary.Remove(nombreRuta.ToLower());

        foreach (string path in listaTag)
        {
            string tempTag = path.ToLower();
            if (pathTagDictionary.ContainsKey(tempTag))
            {
                if (pathTagDictionary[tempTag].Contains(this))
                {
                    pathTagDictionary[tempTag].Remove(this);
                }

                if (pathTagDictionary[tempTag].Count == 0)
                    pathTagDictionary.Remove(tempTag);
            }
        }
    }
    
    public static Vector3[] GetPath(string pathName)
    {
        pathName = pathName.ToLower();
        if (pathDictionary.ContainsKey(pathName))
        {
            List<GameObject> pathSelected = pathDictionary[pathName].listaPuntos;
            Vector3[] pathArray = new Vector3[pathSelected.Count];
            for (int i = 0; i < pathSelected.Count; i++)
            {
                pathArray[i] = pathSelected[i].transform.position;
            }
            return pathArray;
        }
        else
        {
            Debug.LogWarning(string.Concat("No se ha encontrado la ruta con nombre '", pathName, "'"));
            return null;
        }
    }

    public static Vector3[] GetPathReversed(string pathName)
    {
        pathName = pathName.ToLower();
        if (pathDictionary.ContainsKey(pathName))
        {
            List<GameObject> pathSelected = pathDictionary[pathName].listaPuntos;
            Vector3[] pathArray = new Vector3[pathSelected.Count];
            for (int i = pathSelected.Count - 1; i > 0; i--)
            {
                pathArray[i] = pathSelected[i].transform.position;
            }
            return pathArray;
        }
        else
        {
            Debug.LogWarning(string.Concat("No se ha encontrado la ruta con nombre '", pathName, "'"));
            return null;
        }
    }

    public static Vector3[] GetPathRandom()
    {        
        if (pathDictionary.Count > 0)
        {
            int random = Random.Range(0, pathDictionary.Count);
            int counter = 0;
            foreach (WaypointController p in pathDictionary.Values)
            {
                if(counter == random)
                {
                    Vector3[] pathArray = new Vector3[p.listaPuntos.Count];
                    for (int i = 0; i < p.listaPuntos.Count; i++)
                    {
                        pathArray[i] = p.listaPuntos[i].transform.position;
                    }
                    return pathArray;
                }
                counter++;
            }
                
        }
        else
        {
            Debug.LogWarning("No se ha creado ninguna ruta todavía");
        }
        return null;
    }

    public static Vector3[] GetPathRandomByTag(string tag)
    {
        tag = tag.ToLower();
        if (pathTagDictionary.Count > 0 && pathTagDictionary.ContainsKey(tag) && pathTagDictionary[tag].Count > 0)
        {
            List<GameObject> pointSelected;
            if (pathTagDictionary[tag].Count == 1)
                pointSelected = pathTagDictionary[tag][0].listaPuntos;
            else
            {
                int random = Random.Range(0, pathTagDictionary[tag].Count);
                pointSelected = pathTagDictionary[tag][random].listaPuntos;
            }

            foreach (GameObject go in pointSelected)
            {
                Vector3[] pathArray = new Vector3[pointSelected.Count];
                for (int i = 0; i < pointSelected.Count; i++)
                {
                    pathArray[i] = pointSelected[i].transform.position;
                }
                return pathArray;
            }
        }
        else
        {
            Debug.LogWarning(string.Concat("No se ha encontrado el tag '", tag, "'"));
        }
        return null;
    }



#if UNITY_EDITOR
    
    [HideInInspector]
    public Color sphereColor = Color.red;
    [HideInInspector]
    public Color lineColor = Color.blue;
    [HideInInspector]
    public float sphereSize = 0.1f;
    [HideInInspector]
    public UnityEvent onDisableEditMode = new UnityEvent();
    [HideInInspector]
    public bool isPointCreatorActived = false;
    [HideInInspector]
    public bool isVisible = true;

    void OnDrawGizmos()
    {

        if (isVisible && listaPuntos.Count > 0)
        {

            Gizmos.color = sphereColor;

            foreach (GameObject obj in listaPuntos)
            {
                if (obj == null)
                    continue;
                else
                    Gizmos.DrawSphere(obj.transform.position, sphereSize);
            }


            if (listaPuntos.Count > 1)
            {
                Vector3[] pathArray = new Vector3[listaPuntos.Count];
                for (int i = 0; i < listaPuntos.Count; i++)
                {
                    if (listaPuntos[i] == null)
                        continue;
                    else
                        pathArray[i] = listaPuntos[i].transform.position;
                }

                iTween.DrawPath(pathArray, lineColor);
            }
        }
    }
    

#endif
}
