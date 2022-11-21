using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(-103)]
public class navbake : MonoBehaviour
{
    private void Awake()
    {
        var t_nav = GetComponent<NavMeshSurface>();
		t_nav.BuildNavMesh();
		
        var t_model = GetComponent<Map.Model.Cell.BaseInitModel>();    
		Debug.Log(0b0110);
    }
}
