using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Thaget : MonoBehaviour {
    public float      Health       = 100F;
    public Vector2    HealthMinMax = new Vector2( 0, 100 );
    public bool       IsDead       = false;
    public bool       Invulnerable = false;
    public bool       Respawn      = false;
    public GameObject DeadObj;
    public Vector3    Spawn = Vector3.zero;

    private void Start() { this.Spawn = transform.localPosition; }

    public void TagDamage(float amount) {
        if ( this.IsDead ) Die();
        if ( this.Invulnerable ) return;

        this.Health -= amount;

        Debug.Log( gameObject.name + " : Health:" + this.Health );
        if ( this.Health <= this.HealthMinMax.x ) {
            Die();
        }
    }


    private void Die() {
        if ( !this.Respawn ) {
            var obj = Instantiate( this.DeadObj );
            obj.transform.position = this.Spawn;
            obj.transform.parent   = this.transform.parent;

            Destroy( gameObject );
            this.IsDead = true;
        }
        else {
            transform.position = this.Spawn;
            this.Health        = this.HealthMinMax.y;
        }
    }
}