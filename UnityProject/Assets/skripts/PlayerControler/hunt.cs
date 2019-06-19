using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class hunt : MonoBehaviour
{
    public NavMeshAgent agent;
    
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameObject.Find("thaget").transform.position);
    }
}
