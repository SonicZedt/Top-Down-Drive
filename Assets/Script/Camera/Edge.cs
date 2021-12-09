using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    [HideInInspector] public float[] world = new float[4];

    void Awake()
    {
        SetWorldEdge();    
    }
    
    void Update()
    {
        SetWorldEdge();
    }

    private void SetWorldEdge()
    {
        world[0] = Camera.main.ViewportToWorldPoint(Vector3.one).y;
        world[1] = Camera.main.ViewportToWorldPoint(Vector3.one).x;
        world[2] = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        world[3] = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
    }
}
