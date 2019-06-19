using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(LineRenderer) )]
public class KochLineProvider : MonoBehaviour {
    public KochLine KochLineSource;

    private LineRenderer _lineRenderer;

    private int posCount;

    private Vector3[] pos;

    // Start is called before the first frame update
    void Start() {
        this._lineRenderer = GetComponent<LineRenderer>();

        this._lineRenderer.enabled       = true;
        this._lineRenderer.useWorldSpace = false;
        this._lineRenderer.loop          = true;
    }

    // Update is called once per frame
    void Update() {
        this._lineRenderer.material = this.KochLineSource._materialInscance;

        if ( this.KochLineSource._useBezierCurve ) {
            this.KochLineSource._bezirePositions =
                this.KochLineSource.BezireCuve( this.KochLineSource._leapPositions, this.KochLineSource._besireVertexCount );
            this._lineRenderer.positionCount = this.KochLineSource._bezirePositions.Length;
            this._lineRenderer.SetPositions( this.KochLineSource._bezirePositions );

            this.pos      = this.KochLineSource._bezirePositions;
            this.posCount = this.pos.Length;
        }
        else {
            this._lineRenderer.positionCount = this.KochLineSource._leapPositions.Length;
            this._lineRenderer.SetPositions( this.KochLineSource._leapPositions );
            this.pos      = this.KochLineSource._leapPositions;
            this.posCount = this.pos.Length;
        }
    }

    void OnDrawGizmos() {
        for ( var i = 0; i < this.KochLineSource._initiatorPointAmount; i++ ) {
            Gizmos.color = Color.white;
            var rotationMatrix = Matrix4x4.TRS( transform.position, transform.rotation, transform.lossyScale );
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawLine( this.KochLineSource._initiatorPoint[i],
                i < this.KochLineSource._initiatorPointAmount - 1
                    ? this.KochLineSource._initiatorPoint[i + 1]
                    : this.KochLineSource._initiatorPoint[0] );
        }
    }
}