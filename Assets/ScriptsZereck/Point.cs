//Creado por Zereck
// Para cualquier duda o sugerencia puedes contactarme en www.Zereck.net o en youtube "Zereck - Desarrollo de Videojuegos"
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class Point : MonoBehaviour {

    public void AdjustToLayer()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 500.0f))
        {
            transform.position = hit.point;
        }
    }
}


[CustomEditor(typeof(Point))]
public class PointAjustCustomInspector : Editor
{
    private Point targetComponent;

    private void OnEnable()
    {
        targetComponent = (Point)target; 
    }
    

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Ajustar al Objeto Inferior"))
        {
            targetComponent.AdjustToLayer();
        }                
    }
}

#endif