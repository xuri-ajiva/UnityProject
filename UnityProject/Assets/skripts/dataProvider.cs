using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class dataProvider : MonoBehaviour {
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}

public class XMLOPERATION {
    public const string PathBase = ".\\DataSave\\";

    public static void SAVE(object dataLayout, string path) {
        System.IO.FileInfo file = new System.IO.FileInfo( path );
        file.Directory.Create();

        var        sr     = new XmlSerializer( dataLayout.GetType() );
        TextWriter writer = new StreamWriter( path );
        sr.Serialize( writer, dataLayout );
        writer.Close();
        var x = Path.GetDirectoryName( path );
        Debug.Log( x );
    }

    public static T LOAD <T>(string path) {
        if ( !File.Exists( path ) ) throw new TypeAccessException();

        var xs = new XmlSerializer( typeof(T) );
        var fs = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
        var r  = (T) xs.Deserialize( fs );
        fs.Close();
        return r;
    }
}

public class XMLKochLine {
    public _axis          _Axis;
    public _initiator     _Initiator;
    public float          _InitiatorSize;
    public AnimationCurve _animationCurve;
    public bool           _UseBezierCurves;
    public int            _BezierVertexCount;
    public StartGen[]     _startGens;

    public float[] _leroAudio;
    public float   _Multiply;
    public int[]   _audioBand;

    public int   _audioBandMaterial;
    public float _emissionsMultiplier;
}

public class XMLAudioPeer {
    public float _SamplesbufferDecreseStart;
    public float _SamplesbufferDecreseMultiplier;
    public float _bufferDecreseStart;
    public float _bufferDecreseMultiplier;
    public float _Amplitude, _AmplitudeBuffer;
    private float _AmplitudeHighest;
    public AudioClip[] Clip;
}


public class XMLLayout {
    public XMLKochLine kl;
}