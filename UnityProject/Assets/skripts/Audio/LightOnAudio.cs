using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( AudioPeer ) )]
public class LightOnAudio : MonoBehaviour {
    public int _band;

    public float _maxIntensity, _minIntensity;

    private Light _light;

    public AudioPeer _AudioPeer;
    // Start is called before the first frame update
    void Start() {
        this._light = GetComponent<Light>();
        this._AudioPeer = GetComponent<AudioPeer>();
    }

    // Update is called once per frame
    void Update() {
        this._light.intensity =
            ( this._AudioPeer._audioBandBuffer[this._band] * ( this._maxIntensity - this._minIntensity ) ) +
            this._minIntensity;
    }
}