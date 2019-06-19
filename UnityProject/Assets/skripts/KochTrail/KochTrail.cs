using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochTrail : KochGenerrator {
    [HideInInspector] public List<TrailObject> _trails;

    [Header( "Trail Properties" )] public GameObject     _trailPrefab;
    public                                AnimationCurve _trailWidthCurve;
    [Range( 0, 8 )] public                int            _trailEndCapVertices;
    public                                Material       _trailMaterial;
    public                                Gradient       _trailColor;


    [Header( "Audio" )] public AudioPeer _AudioPeer;

    public int[]   _audioBand;
    public Vector2 _speedMinMax, _widthMinMax, _trailTimeMinMax;
    public float   _colorMultiplier;


    //private
    [HideInInspector] public float _lerpPosSpeed;
    [HideInInspector] public float _distancesnap;
    private                  Color _startColor, _endColor;


    void Start() {
        this._startColor = new Color( 0, 0, 0, 0 );
        this._endColor   = new Color( 0, 0, 0, 1 );

        this._trails = new List<TrailObject>();

        for ( var i = 0; i < this._initiatorPointAmount; i++ ) {
            var trailInstate = (GameObject) Instantiate( this._trailPrefab, transform.position, Quaternion.identity, this.transform );
            var trailObject = new TrailObject {
                Go            = trailInstate,
                Trail         = trailInstate.GetComponent<TrailRenderer>(),
                EmissionColor = this._trailColor.Evaluate( i * ( 1.0F / this._initiatorPointAmount ) )
            };

            trailObject.Trail.material       = new Material( this._trailMaterial );
            trailObject.Trail.widthCurve     = this._trailWidthCurve;
            trailObject.Trail.numCapVertices = this._trailEndCapVertices;

            Vector3 instancePos;
            if ( this._generationCount > 0 ) {
                int step;
                if ( this._useBezierCurve ) {
                    step                         = this._bezirePositions.Length / this._initiatorPointAmount;
                    instancePos                  = this._bezirePositions[i * step];
                    trailObject.CurrentTargetNum = ( i * step ) + 1;
                    trailObject.TargetPosition   = this._bezirePositions[trailObject.CurrentTargetNum];
                }
                else {
                    step                         = this._position.Length / this._initiatorPointAmount;
                    instancePos                  = this._position[i * step];
                    trailObject.CurrentTargetNum = ( i * step ) + 1;
                    trailObject.TargetPosition   = this._position[trailObject.CurrentTargetNum];
                }
            }
            else {
                instancePos                  = this._position[i];
                trailObject.CurrentTargetNum = i + 1;
                trailObject.TargetPosition   = this._position[trailObject.CurrentTargetNum];
            }

            trailObject.Go.transform.localPosition = instancePos;
            this._trails.Add( trailObject );
        }

        this.enabled = !this.IsProvider;
    }

    public void Movment(KochTrail obj, List<TrailObject> trail) {
        obj._lerpPosSpeed = Mathf.Lerp( obj._speedMinMax.x, obj._speedMinMax.y, obj._AudioPeer._Amplitude );

        foreach ( var t in trail ) {
            obj._distancesnap = Vector3.Distance( t.Go.transform.localPosition, t.TargetPosition );
            if ( obj._distancesnap < 0.05 ) {
                t.Go.transform.localPosition = t.TargetPosition;

                if ( obj._useBezierCurve && obj._generationCount > 0 ) {
                    if ( t.CurrentTargetNum < obj._bezirePositions.Length - 1 )
                        t.CurrentTargetNum += 1;
                    else
                        t.CurrentTargetNum = 1;
                    t.TargetPosition = obj._bezirePositions[t.CurrentTargetNum];
                }
                else {
                    if ( t.CurrentTargetNum < obj._position.Length - 1 )
                        t.CurrentTargetNum += 1;
                    else
                        t.CurrentTargetNum = 1;
                    t.TargetPosition = obj._thagetPosition[t.CurrentTargetNum];
                }
            }

            t.Go.transform.localPosition = Vector3.MoveTowards( t.Go.transform.localPosition, t.TargetPosition,
                Time.deltaTime * obj._lerpPosSpeed );
        }
    }

    public void AudioBehavior(KochTrail obj, List<TrailObject> trail) {
        for ( var i = 0; i < obj._initiatorPointAmount; i++ ) {
            var colorLeper = Color.Lerp( obj._startColor, trail[i].EmissionColor * obj._colorMultiplier,
                obj._AudioPeer._audioBand[obj._audioBand[i]] );
            trail[i].Trail.material.SetColor( "_EmissionColor", colorLeper );

            colorLeper = Color.Lerp( obj._startColor, obj._endColor, obj._AudioPeer._audioBand[obj._audioBand[i]] );
            trail[i].Trail.material.SetColor( "_Color", colorLeper );

            var widthLeper = Mathf.Lerp( obj._widthMinMax.x, obj._widthMinMax.y, obj._AudioPeer._audioBandBuffer[obj._audioBand[i]] );
            trail[i].Trail.widthMultiplier = widthLeper;

            var timeLeper = Mathf.Lerp( obj._trailTimeMinMax.x, obj._trailTimeMinMax.y, obj._AudioPeer._audioBandBuffer[obj._audioBand[i]] );
            trail[i].Trail.time = timeLeper;
        }
    }

    void Update() {
        Movment( this,this._trails );
        AudioBehavior( this ,this._trails);
    }

    public bool IsProvider = true;
}

public class TrailObject {
    public GameObject    Go               { get; set; }
    public TrailRenderer Trail            { get; set; }
    public int           CurrentTargetNum { get; set; }
    public Vector3       TargetPosition   { get; set; }
    public Color         EmissionColor    { get; set; }
}