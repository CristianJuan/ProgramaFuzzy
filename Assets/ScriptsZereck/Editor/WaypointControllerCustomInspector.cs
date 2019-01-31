//Creado por Zereck
// Para cualquier duda o sugerencia puedes contactarme en www.Zereck.net o en youtube "Zereck - Desarrollo de Videojuegos"
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointController))]
public class WaypointControllerCustomInspector : Editor
{
    private bool switchButton;
    private WaypointController scriptMaster;

    private void OnEnable()
    {
        scriptMaster = (WaypointController)target;
        scriptMaster.gameObject.isStatic = true;
        scriptMaster.onDisableEditMode.AddListener(DisablePointGenerator);
    }

    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            CreatePoint();
        }
        if (scriptMaster.isVisible && scriptMaster.listaPuntos.Count > 0)
        {
            Handles.Label(scriptMaster.listaPuntos[0].transform.position, "'" + scriptMaster.nombreRuta + "' INICIO");
        }
        if (scriptMaster.isVisible && scriptMaster.listaPuntos.Count > 1)
        {
            Handles.Label(scriptMaster.listaPuntos[scriptMaster.listaPuntos.Count - 1].transform.position, "'" + scriptMaster.nombreRuta + "' FIN");
        }
    }
    
    public override void OnInspectorGUI()
    {        
        DrawDefaultInspector();

        if (scriptMaster.isVisible)
        {
            GUI.color = Color.grey;
            if (GUILayout.Button("OCULTAR EDITOR"))
            {                
                scriptMaster.isVisible = false;
            }
        }
        else
        {
            GUI.color = Color.cyan;
            if (GUILayout.Button("MOSTRAR EDITOR"))
            {
                scriptMaster.isVisible = true;
            }
        }

        GUI.color = Color.white;

        if (scriptMaster.isVisible)
        {
            scriptMaster.sphereSize = EditorGUILayout.Slider("Tamaño Esferas", scriptMaster.sphereSize, 0.1f, 20f);
            scriptMaster.sphereColor = EditorGUILayout.ColorField("Color Esferas", scriptMaster.sphereColor);
            scriptMaster.lineColor = EditorGUILayout.ColorField("Color Líneas", scriptMaster.lineColor);

            if (GUILayout.Button(new GUIContent("Ajustar al Objeto Inferior", "Ajusta la posición de todos los puntos de la ruta al objeto que tengan debajo")))
            {
                if (EditorUtility.DisplayDialog("Ajustar al objeto inferior", "¿Quieres cambiar la posición de todos los puntos?", "sí", "NO"))
                {
                    AdjustToLayer();
                }
            }

            if (!switchButton)
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Activar Modo Creación", GUILayout.MinHeight(50)))
                {
                    RemoveUnsudedPoints();
                    ActivatePointCreator();
                    switchButton = true;
                }

            }
            else
            {
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Desactivar Modo Creación", GUILayout.MinHeight(50)))
                {
                    DisablePointGenerator();
                    switchButton = false;
                }
            }

            GUI.backgroundColor = Color.white;
            
            if (GUILayout.Button("Eliminar Puntos No Usados"))
            {
                RemoveUnsudedPoints();
            }
            GUI.backgroundColor = Color.yellow;

            SceneView.RepaintAll();
        }

    }
    
    public void ActivatePointCreator()
    {
        scriptMaster.onDisableEditMode.Invoke();
        ActiveEditorTracker.sharedTracker.isLocked = true;
        scriptMaster.isPointCreatorActived = true;
    }

    public void DisablePointGenerator()
    {
        ActiveEditorTracker.sharedTracker.isLocked = false;
        scriptMaster.isPointCreatorActived = false;
    }

    public void CreatePoint()
    {
        if (scriptMaster.isPointCreatorActived)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = new GameObject();
                obj.transform.parent = scriptMaster.transform;
                obj.name = string.Concat("Punto", scriptMaster.listaPuntos.Count);
                obj.transform.position = hit.point;
                obj.AddComponent<Point>();
                obj.isStatic = true;
                scriptMaster.listaPuntos.Add(obj);
            }
        }
    }


    public void AdjustToLayer()
    {
        RaycastHit hit = new RaycastHit();
        for (int a = 0; a < scriptMaster.listaPuntos.Count; a++)
        {
            if (Physics.Raycast(scriptMaster.listaPuntos[a].transform.position, Vector3.down, out hit, 500.0f))
            {
                scriptMaster.listaPuntos[a].transform.position = hit.point;
            }
        }
    }

    private void RemoveUnsudedPoints()
    {
        while (HaveUnsudedPoints());
        SceneView.RepaintAll();
    }

    private bool HaveUnsudedPoints()
    {

        if (scriptMaster.transform.childCount > 0)
        {
            bool found;
            for (int i = 0; i < scriptMaster.transform.childCount; i++)
            {
                found = false;
                for (int a = 0; a < scriptMaster.listaPuntos.Count; a++)
                {
                    if (scriptMaster.listaPuntos[a] == null)
                    {
                        scriptMaster.listaPuntos.RemoveAt(a);
                        return true;
                    }

                    if (scriptMaster.transform.GetChild(i).name.Equals(scriptMaster.listaPuntos[a].name))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    DestroyImmediate(scriptMaster.transform.GetChild(i).gameObject);
                    return true;
                }

            }
        }
        else
        {
            for (int a = 0; a < scriptMaster.listaPuntos.Count; a++)
            {
                if (scriptMaster.listaPuntos[a] == null)
                {
                    scriptMaster.listaPuntos.RemoveAt(a);
                    return true;
                }
            }
        }       
        
        return false;
    }
}

#endif