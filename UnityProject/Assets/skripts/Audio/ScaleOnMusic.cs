using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ScaleOnMusic : MonoBehaviour {
    [NotNull] public AudioPeer _audioPeer;
    
    public                   Vector2 ScaleMinMax = new Vector2( .5F, 1.5F );
    [SerializeField] private float   baseScale;
    [SerializeField] private float   ammount;


    private bool Enabled = true;


    // Start is called before the first frame update
    void Start() { this.baseScale = transform.localScale.x; }

    // Update is called once per frame
    void Update() {
        this.ammount = Mathf.Lerp( this.ScaleMinMax.x, this.ScaleMinMax.y, this._audioPeer._AmplitudeBuffer );

        transform.localScale = new Vector3( this.ammount, this.ammount, this.ammount ) * this.baseScale;
    }
}