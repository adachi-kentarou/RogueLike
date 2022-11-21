using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navagent : MonoBehaviour
{
    [SerializeField]
    public Transform m_target = null;

    private NavMeshAgent m_nav = null;
    // Start is called before the first frame update
    void Start()
    {

        m_nav = GetComponent<NavMeshAgent>();
        if (m_target != null)
        {
            m_nav.destination = m_target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}