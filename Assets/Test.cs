using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public float time;
    // Start is called before the first frame update
    void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", WaypointController.GetPathRandom(), "time", time, "orienttopath",true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
