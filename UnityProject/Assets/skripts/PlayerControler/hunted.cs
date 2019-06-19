using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class hunted : MonoBehaviour {
    private bool folow;

    public string Tag;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        foreach ( var meshAgent in GameObject.FindGameObjectsWithTag( Tag ) ) {
            var agent = meshAgent.GetComponent<NavMeshAgent>();
            if ( agent == null || !agent.isOnNavMesh ) return;
            agent?.SetDestination( this.transform.localPosition );
        }
    }
}