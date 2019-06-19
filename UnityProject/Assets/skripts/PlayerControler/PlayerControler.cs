using UnityEngine;
using UnityEngine.AI;

public class PlayerControler : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;
    private bool folow;

    // Update is called once per frame
    private void Update()
    {
        agent.stoppingDistance = 0f;
        if (Input.GetKey("q"))
        {
            Ray ray = cam.ScreenPointToRay(GameObject.Find("Image").transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                this.agent.SetDestination(hit.point);
            }
        }
        if (Input.GetKeyDown("f"))
        {
            folow = !folow;
        }
        if (folow)
        {
            agent.stoppingDistance = 2.5f;
            agent.SetDestination(cam.transform.position);
        }

    }
}
