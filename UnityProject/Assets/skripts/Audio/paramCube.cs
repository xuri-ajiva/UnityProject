using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof( AudioPeer ) )]
public class paramCube : MonoBehaviour {
    public int _band;

    public float _startScale, _multiplierScale;

    public bool _UseBuffer;

    private Material _material;

    public AudioPeer _AudioPeer;

    // Start is called before the first frame update
    void Start() {
        this._material = GetComponent<MeshRenderer>().materials[0];
        this._AudioPeer = GetComponent<AudioPeer>();
    }

    // Update is called once per frame
    void Update() {
        float vaule;
        float _colorVaule;
        if ( this._UseBuffer) {
            vaule = this._AudioPeer.BandBuffer[this._band];
            _colorVaule = this._AudioPeer._audioBandBuffer[this._band];
        }
        else {
            vaule = this._AudioPeer.FreqBand[this._band];
            _colorVaule = this._AudioPeer._audioBand[this._band];
        }

        transform.localScale = new Vector3( transform.localScale.x, ( vaule * this._multiplierScale ) + this._startScale,
            transform.localScale.z );

        Color _color = new Color( _colorVaule, _colorVaule, _colorVaule );
        this._material.SetColor( "_EmissionColor", _color );
    }

    /*
     *
     *         float vaule;
        if ( this._UseBuffer )
            vaule = AudioPeer._audioBandBuffer[this._band];
        else
            vaule = AudioPeer._audioBand[this._band];

        transform.localScale = new Vector3( transform.localScale.x, ( vaule * _multiplierScale ) + this._startScale,
            transform.localScale.z );
        Color _color = new Color( vaule, vaule, vaule );
        //this._material.SetColor( "_EmissionColor", _color );
     */
}