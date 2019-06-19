using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent( typeof( AudioPeer ) )]
public class Insantiate512Cubes : MonoBehaviour {
    public GameObject                _SampleObject;
    GameObject[]                     _sampleCubes = new GameObject[512];
    [SerializeField] protected float _maxScale    = 10;
    [SerializeField][Range(10,100)]
    public int scale = 5;


    public AudioPeer _AudioPeer;

    void Start() {

        this._AudioPeer = GetComponent<AudioPeer>();
        for ( var i = 0; i < 512; i++ ) {
            var _instance = Instantiate( this._SampleObject );
            _instance.transform.position = transform.position;
            _instance.transform.parent   = transform;
            _instance.name               = "sampleCube" + i;
            transform.eulerAngles   = new Vector3( 0, -0.703125F * i, 0 );
            _instance.transform.position = Vector3.forward * 1 * this.scale;
            this._sampleCubes[i]         = _instance;
        }
    }

    // Update is called once per frame
    void Update() {
        for ( var i = 0; i < 512; i++ ) {
            if ( this._sampleCubes != null ) {
                this._sampleCubes[i].transform.localScale = new Vector3( 0.01F * this.scale, this._AudioPeer.SamplesBuffer[i]                                * this._maxScale, 0.01F * this.scale );
            }
        }
    }
}