using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HitClose : MonoBehaviour {
    public float  damage;
    public float  rangeSqr = 3;
    public string Tag;
    public bool   hasWeapon = false;

    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        if ( this.agent.remainingDistance > this.agent.stoppingDistance ) return;
        foreach ( var meshAgent in GameObject.FindGameObjectsWithTag( this.Tag ) ) {
            var lAgent = meshAgent.GetComponent<Thaget>();
            if ( lAgent == null ) return;

            var distanceSqr = ( meshAgent.transform.position - this.transform.position ).sqrMagnitude;
            if ( !( distanceSqr < this.rangeSqr ) ) continue;


            if ( !this.hasWeapon )
                lAgent.TagDamage( this.damage );
            else {
                this.transform.LookAt( meshAgent.transform.position );
                this.GetComponent<WeaponControler>().Schoot_Hit();
            }
        }
    }
}