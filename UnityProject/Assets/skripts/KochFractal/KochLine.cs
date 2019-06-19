using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;


[RequireComponent( typeof(LineRenderer) )]
public class KochLine : KochGenerrator {
    [HideInInspector] public LineRenderer _lineRenderer;

    [HideInInspector] public Vector3[] _leapPositions = new Vector3[1];
    [Range( 0, 1 )]   public float[]   _leroAudio     = new float[1];

    public float _Multiply = 1;

    [Header( "Audio" )] //
    public AudioPeer _AudioPeer = new AudioPeer();

    public int[] _audioBand;

    [Header( "Visualization" )] //
    public Material _Material;

    public                   Color    _Color;
    [HideInInspector] public Material _materialInscance;
    public                   int      _audioBandMaterial   = 2;
    public                   float    _emissionsMultiplier = 2F;

    //[Range( 0, 1 )] public
    float _lerpAmount;

    private string MyPath = "";

    // Start is called before the first frame update
    void Start() {
        var r = new System.Random();

        this.MyPath = XMLOPERATION.PathBase + "XMLKochLine_" + name + "_" + this.Axis + GetHashCode() + ".xml";

        this._leroAudio                  = new float[this._initiatorPointAmount];
        this._lineRenderer               = GetComponent<LineRenderer>();
        this._lineRenderer.enabled       = !this.IsProvider;
        this._lineRenderer.useWorldSpace = false;
        this._lineRenderer.loop          = true;
        this._lineRenderer.positionCount = this._position.Length;
        this._lineRenderer.SetPositions( this._position );
        this._leapPositions = new Vector3[this._position.Length];
        //aplay materials
        this._materialInscance      = new Material( this._Material );
        this._lineRenderer.material = this._materialInscance;

        //if ( File.Exists( MyPath ) && this._XmlDataManager.loadSettings ) {
        //    LoadSettings();
        //}
        //else if ( this._XmlDataManager.saveSettings ) {
        //    SaveSettings();
        //}
    }

    public void Initialisation() { }

    public void SaveSettings() {
        var save = new XMLKochLine {
            _leroAudio           = this._leroAudio,
            _audioBand           = this._audioBand,
            _emissionsMultiplier = this._emissionsMultiplier,
            _audioBandMaterial   = this._audioBandMaterial,
            _animationCurve      = this._generratorCurve,
            _Axis                = this.Axis,
            _startGens           = this._startGen,
            _Initiator           = this.Initiator,
            _BezierVertexCount   = this._besireVertexCount,
            _InitiatorSize       = this._initiatorSize,
            _Multiply            = this._Multiply,
            _UseBezierCurves     = this._useBezierCurve
        };
        var r = new System.Random();
        XMLOPERATION.SAVE( save, MyPath );
    }

    public void LoadSettings() {
        var data = XMLOPERATION.LOAD<XMLKochLine>( MyPath );
        _leroAudio              = data._leroAudio;
        _audioBand              = data._audioBand;
        _emissionsMultiplier    = data._emissionsMultiplier;
        _audioBandMaterial      = data._audioBandMaterial;
        _generratorCurve        = data._animationCurve;
        Axis                    = data._Axis;
        _startGen               = data._startGens;
        Initiator               = data._Initiator;
        this._besireVertexCount = data._BezierVertexCount;
        this._initiatorSize     = data._InitiatorSize;
        _Multiply               = data._Multiply;
        this._useBezierCurve    = data._UseBezierCurves;
    }

    // Update is called once per frame

    void Update() {
        this._materialInscance.SetColor( "_EmissionColor",
            this._Color * this._AudioPeer._audioBandBuffer[this._audioBandMaterial] * this._emissionsMultiplier );

        if ( this._generationCount != 0 ) {
            int count = 0;
            for ( int i = 0; i < this._initiatorPointAmount; i++ ) {
                this._leroAudio[i] = this._AudioPeer._audioBandBuffer[this._audioBand[i]];
                this._lerpAmount   = this._AudioPeer._audioBand[this._audioBand[i]];

                for ( int j = 0; j < ( this._position.Length - 1 ) / this._initiatorPointAmount; j++ ) {
                    this._leapPositions[count] = Vector3.Lerp( this._position[count], this._thagetPosition[count], this._leroAudio[i] );
                    count++;
                }
            }

            this._leapPositions[count] =
                Vector3.Lerp( this._position[count], this._thagetPosition[count], this._leroAudio[this._initiatorPointAmount - 1] );

            if ( this._useBezierCurve ) {
                this._bezirePositions            = BezireCuve( this._leapPositions, this._besireVertexCount );
                this._lineRenderer.positionCount = this._bezirePositions.Length;
                this._lineRenderer.SetPositions( this._bezirePositions );
            }
            else {
                this._lineRenderer.positionCount = this._leapPositions.Length;
                this._lineRenderer.SetPositions( this._leapPositions );
            }
        }
    }

    public bool           IsProvider = true;
}