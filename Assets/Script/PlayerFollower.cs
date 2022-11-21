using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Map;
public class PlayerFollower : MonoBehaviour
{
    // Start is called before the first frame update

    private enum State
    {
        InitWait,
        BakeWait,
        Move,
        TargetChange
    }

    [SerializeField]
    private State m_state = State.InitWait;
    private NavMeshAgent m_agent;
    private navbake m_nav;
    
    [SerializeField]
    private Transform[] m_target;
    private int m_target_index = 0;
    //private int m_count = 0;
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        //m_target = GameObject.Find("target").transform;
        m_nav = GameObject.Find("nav").GetComponent<navbake>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (++m_count > 30)
        {
            m_count = 0;
            m_agent.enabled = !m_agent.enabled;
        }
        */
        switch (m_state)
        {
            case State.InitWait:
                if(m_agent.isOnNavMesh)
                {
                    m_agent.SetDestination(m_target[m_target_index].position);
                    m_state = State.BakeWait;
                }
                break;
            case State.BakeWait:
                if(m_agent.isActiveAndEnabled)
                {
                    m_state = State.Move;
                }
                break;
            case State.Move:
                //m_agent.velocity = (m_agent.steeringTarget - this.transform.position).normalized * m_agent.speed;
                //transform.up = m_agent.steeringTarget - transform.position;
                var t_distance = m_agent.remainingDistance;
                
                if (float.IsInfinity(t_distance) == false && t_distance <= 0.001f)
                {
                    m_state = State.TargetChange;
                }
                break;
            case State.TargetChange:

                m_target_index = (m_target_index += 1) % m_target.Length;
                m_agent.SetDestination(m_target[m_target_index].position);
                m_state = State.BakeWait;

                break;
        }
        
        

    }
}