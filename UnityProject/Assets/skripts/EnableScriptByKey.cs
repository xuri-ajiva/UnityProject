using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableScriptByKey : MonoBehaviour {
    public Keystate      _KeyState = new Keystate();
    public List<KeyCode> _Key      = new List<KeyCode>();


    // Start is called before the first frame update
    void Start() { getAllChiles( this.transform ); }

    private                 bool   Enabled = true;
    [SerializeField] public string Tag     = "change";

    // Update is called once per frame
    void Update() {
        if ( Input.anyKey ) {
            switch (this._KeyState) {
                case Keystate.Down: break;
                case Keystate.Up:
                    foreach ( var code in this._Key ) {
                        if ( !Input.GetKeyUp( code ) ) continue;

                        ChangeEnabled();
                        return;
                    }

                    break;
                case Keystate.Pres:
                    foreach ( var code in this._Key ) {
                        if ( !Input.GetKey( code ) ) continue;

                        ChangeEnabled();
                        return;
                    }

                    break;
                default: return;
            }
        }

        if ( !Input.anyKeyDown ) return;
        foreach ( var code in this._Key ) {
            if ( !Input.GetKeyDown( code ) ) continue;

            ChangeEnabled();
            return;
        }
    }

    public List<GameObject> childs = new List<GameObject>();

    void getAllChiles(Transform t) => this.childs = new List<GameObject>( GameObject.FindGameObjectsWithTag( Tag ) );

    private void ChangeEnabled() {
        this.Enabled = !this.Enabled;
        foreach ( var c in this.childs ) {
            c.GetComponent<ScaleOnMusic>().enabled = this.Enabled;
        }
    }

    public enum Keystate {
        Down, Up, Pres
    }
}