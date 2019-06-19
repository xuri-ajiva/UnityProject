using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RoteteCam : MonoBehaviour {
    [NotNull] public AudioPeer _audioPeer;

    private bool freecam = false;

    public           Vector3 _rotateAxis, _rotateSpeedMultyplier;
    private          Vector3 _rotateSpeed;
    [NotNull] public int[]   _audioBands;

    void Start() { }

    void Update() {
        if ( Input.anyKeyDown && Input.GetKeyDown( KeyCode.Tab ) ) ChangeEnabled();

        if ( !this.freecam ) {
            this._rotateSpeed = new Vector3( this._audioPeer.BandBuffer[this._audioBands[0]], this._audioPeer.BandBuffer[this._audioBands[1]],
                this._audioPeer.BandBuffer[this._audioBands[2]] );
            //if ( !float.IsNaN( this._rotateSpeed.x ) ) {
            transform.GetChild( 0 ).transform.LookAt( transform );
            var tmp = new Vector3(
                (float) this._rotateAxis.x * this._rotateSpeed.x * this._rotateSpeedMultyplier.x * Time.deltaTime * this._audioPeer._AmplitudeBuffer,
                (float) this._rotateAxis.y * this._rotateSpeed.y * this._rotateSpeedMultyplier.y * Time.deltaTime * this._audioPeer._AmplitudeBuffer,
                (float) this._rotateAxis.z    *
                this._rotateSpeed.z           *
                this._rotateSpeedMultyplier.z *
                Time.deltaTime                *
                this._audioPeer._AmplitudeBuffer );

            transform.Rotate( tmp );

            //var local = transform.GetChild( 0 ).transform.localPosition;
            //tmp = new Vector3( local.x, local.y, this._CameraBaseTransform.y * this._audioPeer._AmplitudeBuffer );
            //
            //transform.GetChild( 0 ).transform.localPosition = tmp;
            ////}
        }
        else { }
    }

    private bool Enabled;

    private void ChangeEnabled() {
        this.freecam = !this.freecam;

        var comp = this.transform.GetComponentsInChildren<Fps_Camara>();

        foreach ( var c in comp ) {
            c.enabled = this.freecam;
        }
    }
}