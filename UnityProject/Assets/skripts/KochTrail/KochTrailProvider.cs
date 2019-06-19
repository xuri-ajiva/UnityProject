using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochTrailProvider : MonoBehaviour {
    [HideInInspector] public List<TrailObject> _trails = new List<TrailObject>();
    public                   KochTrail         KochTrailSource;

    // Start is called before the first frame update
    void Start() { }

    private bool init = false;

    // Update is called once per frame
    void Update() {
        if ( !init && this.KochTrailSource._trails.Count != 0 ) _initiator();

        this.KochTrailSource.Movment( this.KochTrailSource, this._trails );

        this.KochTrailSource.AudioBehavior( this.KochTrailSource,this._trails);
    }

    private void _initiator() {
        for ( var i = 0; i < this.KochTrailSource._trails.Count; i++ ) {
            var trailInstate = (GameObject) Instantiate( this.KochTrailSource._trailPrefab, transform.position, Quaternion.identity, this.transform );
            var trailObject = new TrailObject {
                Go            = trailInstate,
                Trail         = trailInstate.GetComponent<TrailRenderer>(),
                EmissionColor = this.KochTrailSource._trailColor.Evaluate( i * ( 1.0F / this.KochTrailSource._initiatorPointAmount ) )
            };

            trailObject.Trail.material       = new Material( this.KochTrailSource._trailMaterial );
            trailObject.Trail.widthCurve     = this.KochTrailSource._trailWidthCurve;
            trailObject.Trail.numCapVertices = this.KochTrailSource._trailEndCapVertices;

            Vector3 instancePos;
            if ( this.KochTrailSource._generationCount > 0 ) {
                int step;
                if ( this.KochTrailSource._useBezierCurve ) {
                    step                         = this.KochTrailSource._bezirePositions.Length / this.KochTrailSource._initiatorPointAmount;
                    instancePos                  = this.KochTrailSource._bezirePositions[i * step];
                    trailObject.CurrentTargetNum = ( i * step ) + 1;
                    trailObject.TargetPosition   = this.KochTrailSource._bezirePositions[trailObject.CurrentTargetNum];
                }
                else {
                    step                         = this.KochTrailSource._position.Length / this.KochTrailSource._initiatorPointAmount;
                    instancePos                  = this.KochTrailSource._position[i * step];
                    trailObject.CurrentTargetNum = ( i * step ) + 1;
                    trailObject.TargetPosition   = this.KochTrailSource._position[trailObject.CurrentTargetNum];
                }
            }
            else {
                instancePos                  = this.KochTrailSource._position[i];
                trailObject.CurrentTargetNum = i + 1;
                trailObject.TargetPosition   = this.KochTrailSource._position[trailObject.CurrentTargetNum];
            }

            trailObject.Go.transform.localPosition = instancePos;
            this._trails.Add( trailObject );
        }

        this.init = true;
    }

    void OnDrawGizmos() {
        for ( var i = 0; i < this.KochTrailSource._initiatorPointAmount; i++ ) {
            Gizmos.color = Color.white;
            var rotationMatrix = Matrix4x4.TRS( transform.position, transform.rotation, transform.lossyScale );
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawLine( this.KochTrailSource._initiatorPoint[i],
                i < this.KochTrailSource._initiatorPointAmount - 1
                    ? this.KochTrailSource._initiatorPoint[i + 1]
                    : this.KochTrailSource._initiatorPoint[0] );
        }
    }
}