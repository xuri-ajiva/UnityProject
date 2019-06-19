using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class draw_creator : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    private Queue<Vector3> pathpoints = new Queue<Vector3>();

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        FindObjectOfType<draw_point>().OnNewPathCreated += SetPoints;
    }

    private void SetPoints(IEnumerable<Vector3> points)
    {
        pathpoints = new Queue<Vector3>(points);
    }

    private bool ShouldSetDestination()
    {
        if (pathpoints.Count == 0)
        {
            return false;
        }
        if (NavMeshAgent.hasPath == false || NavMeshAgent.remainingDistance < 0.5F)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (ShouldSetDestination())
        {
            NavMeshAgent.SetDestination(pathpoints.Dequeue());
        }
    }
}
