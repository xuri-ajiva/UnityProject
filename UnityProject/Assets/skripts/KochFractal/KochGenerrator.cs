#region using

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

public class KochGenerrator : MonoBehaviour {
    [Header( "Initiator" )] //
    [SerializeField]
    protected _axis Axis = new _axis();

    [SerializeField] //
    protected _initiator Initiator = new _initiator();

    public float _initiatorSize = 2;
    public float _lengthOfSides;

    [Header( "Curve" )] //
    [SerializeField]
    protected AnimationCurve _generratorCurve;

    public                  bool _useBezierCurve;
    [Range( 8, 24 )] public int  _besireVertexCount;

    [Header( "Generator" )] public StartGen[] _startGen;


    public void OnDrawGizmos() {
        GetInitiatorPoints();
        this._initiatorPoint = new Vector3[this._initiatorPointAmount];

        this._rotateVector = Quaternion.AngleAxis( this._initialRotation, this._rotateAxis ) * this._rotateVector;
        for ( var i = 0; i < this._initiatorPointAmount; i++ ) {
            this._initiatorPoint[i] = this._rotateVector * this._initiatorSize;

            this._rotateVector = Quaternion.AngleAxis( 360 / this._initiatorPointAmount, this._rotateAxis ) * this._rotateVector;
        }

        for ( var i = 0; i < this._initiatorPointAmount; i++ ) {
            Gizmos.color = Color.white;
            var rotationMatrix = Matrix4x4.TRS( transform.position, transform.rotation, transform.lossyScale );
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawLine( this._initiatorPoint[i], i < this._initiatorPointAmount - 1 ? this._initiatorPoint[i + 1] : this._initiatorPoint[0] );
        }

        this._lengthOfSides = Vector3.Distance( this._initiatorPoint[0], this._initiatorPoint[1] ) * .5F;
    }

    public void GetInitiatorPoints() {
        switch (this.Initiator) {
            case _initiator.Triangle:
                this._initiatorPointAmount = 3;
                this._initialRotation      = 0;
                break;
            case _initiator.Square:
                this._initiatorPointAmount = 4;
                this._initialRotation      = 45;
                break;
            case _initiator.Pentagon:
                this._initiatorPointAmount = 5;
                this._initialRotation      = 36;
                break;
            case _initiator.Hexagon:
                this._initiatorPointAmount = 6;
                this._initialRotation      = 30;
                break;
            case _initiator.Heptagon:
                this._initiatorPointAmount = 7;
                this._initialRotation      = 25.71428f;
                break;
            case _initiator.Octagon:
                this._initiatorPointAmount = 8;
                this._initialRotation      = 22.5F;
                break;
            default:
                this._initiatorPointAmount = 3;
                this._initialRotation      = 0;
                break;
        }

        switch (this.Axis) {
            case _axis.XAxis:
                this._rotateVector = new Vector3( 1, 0, 0 );
                this._rotateAxis   = new Vector3( 0, 0, 1 );
                break;
            case _axis.YAxis:
                this._rotateVector = new Vector3( 0, 1, 0 );
                this._rotateAxis   = new Vector3( 1, 0, 0 );
                break;
            case _axis.ZAxis:
                this._rotateVector = new Vector3( 0, 0, 1 );
                this._rotateAxis   = new Vector3( 0, 1, 0 );
                break;
            default:
                this._rotateVector = new Vector3( 0, 0, 1 );
                this._rotateAxis   = new Vector3( 0, 1, 0 );
                break;
        }
    }

    private void Awake() {
        GetInitiatorPoints();
        //assassin list an arrays
        this._position       = new Vector3[this._initiatorPointAmount + 1];
        this._thagetPosition = new Vector3[this._initiatorPointAmount + 1];
        this._keyframes      = this._generratorCurve.keys;
        this._lineSegments   = new List<LineSegment>();

        this._rotateVector = Quaternion.AngleAxis( this._initialRotation, this._rotateAxis ) * this._rotateVector;
        for ( var i = 0; i < this._initiatorPointAmount; i++ ) {
            this._position[i] = this._rotateVector * this._initiatorSize;

            this._rotateVector = Quaternion.AngleAxis( 360 / this._initiatorPointAmount, this._rotateAxis ) * this._rotateVector;
        }

        this._position[this._initiatorPointAmount] = this._position[0];
        this._thagetPosition                       = this._position;

        for ( int i = 0; i < this._startGen.Length; i++ ) {
            KochGene( this._thagetPosition, this._startGen[i].outwarts, this._startGen[i].Scale );
        }
    }

    protected void KochGene(Vector3[] positions, bool outwards, float generatorMultiplier) {
        //create line segments
        this._lineSegments.Clear();
        for ( int i = 0; i < positions.Length - 1; i++ ) {
            LineSegment line = new LineSegment { StartPostiton = positions[i] };
            if ( i == positions.Length - 1 ) {
                line.EndPosition = positions[0];
            }
            else {
                line.EndPosition = positions[i + 1];
            }

            line.Direction = ( line.EndPosition - line.StartPostiton ).normalized;
            line.Length    = Vector3.Distance( line.EndPosition, line.StartPostiton );
            this._lineSegments.Add( line );
        }

        //add the line segment to a point array
        List<Vector3> newPos    = new List<Vector3>();
        List<Vector3> thagetPos = new List<Vector3>();

        for ( int i = 0; i < this._lineSegments.Count; i++ ) {
            newPos.Add( this._lineSegments[i].StartPostiton );
            thagetPos.Add( this._lineSegments[i].StartPostiton );

            for ( int j = 1; j < this._keyframes.Length - 1; j++ ) {
                float moveAmount = this._lineSegments[i].Length *
                                       this._keyframes[j].time;
                float heigthAmount = ( this._lineSegments[i].Length * this._keyframes[j].value ) *
                                       generatorMultiplier;
                Vector3 movePos = this._lineSegments[i].StartPostiton + ( this._lineSegments[i].Direction * moveAmount );
                Vector3 dir;
                if ( outwards ) {
                    dir = Quaternion.AngleAxis( -90, this._rotateAxis ) * this._lineSegments[i].Direction;
                }
                else {
                    dir = Quaternion.AngleAxis( 90, this._rotateAxis ) * this._lineSegments[i].Direction;
                }

                newPos.Add( movePos );
                thagetPos.Add( movePos + ( dir * heigthAmount ) );
            }
        }

        newPos.Add( this._lineSegments[0].StartPostiton );
        thagetPos.Add( this._lineSegments[0].StartPostiton );

        this._position       = new Vector3[newPos.Count];
        this._thagetPosition = new Vector3[thagetPos.Count];

        this._position        = newPos.ToArray();
        this._thagetPosition  = thagetPos.ToArray();
        this._bezirePositions = BezireCuve( this._thagetPosition, this._besireVertexCount );
        this._generationCount++;
    }

    [HideInInspector]
    public Vector3[] BezireCuve(Vector3[] points, int vertexCount) {
        var pointlist = new List<Vector3>();
        for ( var i = 0; i < points.Length; i += 2 ) {
            if ( i + 2 <= points.Length - 1 ) {
                for ( var ratio = 0F; ratio <= 1; ratio += 1.0F / vertexCount ) {
                    var tangentLineVertex1 = Vector3.Lerp( points[i], points[i + 1], ratio );
                    var tangentLineVertex2 = Vector3.Lerp( points[i            + 1], points[i + 2], ratio );

                    var bezierpoint = Vector3.Lerp( tangentLineVertex1, tangentLineVertex2, ratio );

                    pointlist.Add( bezierpoint );
                }
            }
        }

        return pointlist.ToArray();
    }

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    [HideInInspector] public Vector3 _rotateAxis;
    [HideInInspector] public Vector3 _rotateVector;

    [HideInInspector] public Vector3[]         _initiatorPoint;
    [HideInInspector] public Vector3[]         _thagetPosition;
    [HideInInspector] public Vector3[]         _position;
    [HideInInspector] public Vector3[]         _bezirePositions;
    private                  List<LineSegment> _lineSegments;

    protected Keyframe[] _keyframes;

    [HideInInspector] public int   _generationCount;
    [HideInInspector] public int   _initiatorPointAmount;
    [HideInInspector] public float _initialRotation;
}

public enum _initiator {
    Triangle,
    Square,
    Pentagon,
    Hexagon,
    Heptagon,
    Octagon
}

public enum _axis {
    ZAxis,
    XAxis,
    YAxis
}

[System.Serializable]
public struct StartGen {
    public                  bool  outwarts;
    [Range( 0, 10 )] public float Scale;
}

public struct LineSegment {
    public Vector3 StartPostiton { get; set; }
    public Vector3 EndPosition   { get; set; }
    public Vector3 Direction     { get; set; }
    public float   Length        { get; set; }
}