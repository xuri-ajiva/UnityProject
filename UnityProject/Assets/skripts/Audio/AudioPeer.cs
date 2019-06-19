using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent( typeof(AudioSource) )]
public class AudioPeer : MonoBehaviour {
    private AudioSource _audioSource;

    public bool            useMicrophone = false;
    public string          _SelectetDevice;
    public AudioMixerGroup _MixerGroupMicrophone, _MixerGroupMaster;

    public _channel channel = new _channel();

    [HideInInspector] public int FreqBands;

    [SerializeField] private                         int     SampleAmmount                   = 512;
    private                                          float[] SamplesLeft                     = new float[1];
    private                                          float[] SamplesRight                    = new float[1];
    public                                           float[] SamplesBuffer                   = new float[1];
    [SerializeField]                         private float[] _samplesbufferDecrese           = new float[1];
    [SerializeField] [Range( 0.001f, 0.1F )] private float   _SamplesbufferDecreseStart      = 0.005F;
    [SerializeField] [Range( 1.0F,   2F )]   private float   _SamplesbufferDecreseMultiplier = 1.005F;

    public                                         float[] FreqBand                 = new float[1];
    public                                         float[] BandBuffer               = new float[1];
    [SerializeField]                       private float[] _bufferDecrese           = new float[1];
    [SerializeField] [Range( 0.001f, 1F )] private float   _bufferDecreseStart      = 0.1F;
    [SerializeField] [Range( 1.0F,   2F )] private float   _bufferDecreseMultiplier = 1.005F;


    private float[] _freqBandHighest = new float[1];
    public  float[] _audioBand       = new float[1];
    public  float[] _audioBandBuffer = new float[1];
    public  float   _audioProfile    = 4;

    public  float _Amplitude, _AmplitudeBuffer;
    private float _AmplitudeHighest;

    public AudioClip[] Clip;

    private string MyPath = "";

    // Start is called before the first frame update
    void Start() {
        this._audioSource        = GetComponent<AudioSource>();
        this._audioSource.volume = .5F;

        this.SamplesLeft           = new float[this.SampleAmmount];
        this.SamplesRight          = new float[this.SampleAmmount];
        this.SamplesBuffer         = new float[this.SampleAmmount];
        this._samplesbufferDecrese = new float[this.SampleAmmount];

        this.FreqBand       = new float[this.FreqBands];
        this.BandBuffer     = new float[this.FreqBands];
        this._bufferDecrese = new float[this.FreqBands];

        this._freqBandHighest = new float[this.FreqBands];
        this._audioBand       = new float[this.FreqBands];
        this._audioBandBuffer = new float[this.FreqBands];

        AudioProfile( this._audioProfile );

        if ( this.useMicrophone ) {
            if ( Microphone.devices.Length > 0 ) {
                this._SelectetDevice                    = Microphone.devices[0].ToString();
                this._audioSource.outputAudioMixerGroup = this._MixerGroupMicrophone;
                this._audioSource.clip                  = Microphone.Start( this._SelectetDevice, true, 10, AudioSettings.outputSampleRate );
                this._audioSource.Play();
                return;
            }
        }

        this._audioSource.outputAudioMixerGroup = this._MixerGroupMaster;
        if ( this.Clip == null ) throw new NullReferenceException();
        this._audioSource.clip = this.Clip[this._playIndex];
    }


    void AudioProfile(float profile) {
        for ( int i = 0; i < this.FreqBands; i++ ) {
            this._freqBandHighest[i] = profile;
        }
    }

    private int _playIndex = 0;
    private int _tmpIndex  = -1;

    private bool _play = false;

    private bool play {
        get => this._play;
        set {
            this._play = value;

            if ( this._play ) {
                this._audioSource.pitch = this._currentPlaySpeed;

                return;
            }

            this._currentPlaySpeed  = this._audioSource.pitch;
            this._audioSource.pitch = 0;

            this.SamplesLeft           = new float[this.SampleAmmount];
            this.SamplesRight          = new float[this.SampleAmmount];
            this.SamplesBuffer         = new float[this.SampleAmmount];
            this._samplesbufferDecrese = new float[this.SampleAmmount];

            this.FreqBand       = new float[this.FreqBands];
            this.BandBuffer     = new float[this.FreqBands];
            this._bufferDecrese = new float[this.FreqBands];

            this._freqBandHighest = new float[this.FreqBands];
            this._audioBand       = new float[this.FreqBands];
            this._audioBandBuffer = new float[this.FreqBands];
        }
    }

    private float _currentPlaySpeed = 1f;

    // Update is called once per frame
    void Update() {
        if ( Input.anyKey ) {
            if ( Input.GetKeyDown( KeyCode.Clear ) || Input.GetKeyDown( KeyCode.P ) ) this.play = !this.play;
        }

        if ( this.play ) {
            GetSpectrumAudioSource();
            MakeFrequencyBands();
            CalculateBandBuffer();
            CalculateSpectrumBuffer();
            CreateAudioBands();
            GetAmplitude();

            if ( Input.anyKey ) {
                if ( Input.GetKey( KeyCode.DownArrow ) || Input.GetKey( KeyCode.S ) ) this._audioSource.volume -= 0.01F;
                if ( Input.GetKey( KeyCode.UpArrow )   || Input.GetKey( KeyCode.W ) ) this._audioSource.volume += 0.01F;

                if ( Input.GetKey( KeyCode.Home )   || Input.GetKey( KeyCode.Q ) ) this._audioSource.pitch -= 0.01F;
                if ( Input.GetKey( KeyCode.PageUp ) || Input.GetKey( KeyCode.E ) ) this._audioSource.pitch += 0.01F;
            }

            if ( this.useMicrophone ) {
                if ( !this._audioSource.isPlaying ) {
                    this._audioSource.Play();
                }

                return;
            }

            if ( !this._audioSource.isPlaying ) {
                this._playIndex++;
            }

            if ( Input.anyKeyDown ) {
                if ( Input.GetKeyDown( KeyCode.RightArrow ) || Input.GetKeyDown( KeyCode.D ) ) this._playIndex++;
                if ( Input.GetKeyDown( KeyCode.LeftArrow )  || Input.GetKeyDown( KeyCode.A ) ) this._playIndex--;
            }

            if ( this._playIndex != this._tmpIndex ) {
                if ( this._playIndex < 0 ) this._playIndex                 = this.Clip.Length - 1;
                if ( this._playIndex >= this.Clip.Length ) this._playIndex = 0;
                this._tmpIndex = this._playIndex;

                this._audioSource.clip = this.Clip[this._playIndex];
            }

            if ( !this._audioSource.isPlaying ) {
                this._audioSource.Play();
            }
        }
    }

    private void GetAmplitude() {
        var currentAmplitude       = 0F;
        var currentAmplitudeBuffer = 0F;

        for ( int i = 0; i < this.FreqBands; i++ ) {
            currentAmplitude       += this._audioBand[i];
            currentAmplitudeBuffer += this._audioBandBuffer[i];
        }

        if ( currentAmplitude > this._AmplitudeHighest ) this._AmplitudeHighest = currentAmplitude;

        this._Amplitude       = currentAmplitude       / this._AmplitudeHighest;
        this._AmplitudeBuffer = currentAmplitudeBuffer / this._AmplitudeHighest;
    }

    private void CreateAudioBands() {
        for ( var i = 0; i < this.FreqBands; i++ ) {
            if ( this.FreqBand[i] > this._freqBandHighest[i] ) this._freqBandHighest[i] = this.FreqBand[i];
            this._audioBand[i]       = ( this.FreqBand[i]   / this._freqBandHighest[i] );
            this._audioBandBuffer[i] = ( this.BandBuffer[i] / this._freqBandHighest[i] );
        }
    }

    private void CalculateSpectrumBuffer() {
        for ( var i = 0; i < this.SampleAmmount; i++ ) {
            if ( ( this.SamplesLeft[i] + this.SamplesRight[i] ) / 2 > this.SamplesBuffer[i] ) {
                this.SamplesBuffer[i]         = ( this.SamplesLeft[i] + this.SamplesRight[i] ) / 2;
                this._samplesbufferDecrese[i] = this._SamplesbufferDecreseStart;
            }
            else if ( ( this.SamplesLeft[i] + this.SamplesRight[i] ) / 2 < this.SamplesBuffer[i] ) {
                var oset              = this.SamplesBuffer[i] - this._samplesbufferDecrese[i];
                if ( oset <= 0 ) oset = 0;
                this.SamplesBuffer[i]         =  oset;
                this._samplesbufferDecrese[i] *= this._SamplesbufferDecreseMultiplier;
            }
        }
    }

    void GetSpectrumAudioSource() {
        this._audioSource.GetSpectrumData( this.SamplesLeft,  0, FFTWindow.Blackman );
        this._audioSource.GetSpectrumData( this.SamplesRight, 1, FFTWindow.Blackman );
    }

    void MakeFrequencyBands() {
        /*
         *0 - 2     = 86    Hz
         *1 - 4     = 172   Hz  - 87-258
         *2 - 8     = 344   Hz  - 259-602
         *3 - 16    = 688   Hz  - 603-1290
         *4 - 32    = 1376  Hz  - 1291-2666
         *5 - 64    = 2752  Hz  - 2667-541FreqBands
         *6 - 128   = 5504  Hz  - 5419-11922
         *7 - 256   = 11008 Hz  - 10923-21930
         *
         *    510
         */

        var count = 0;
        for ( var i = 0; i < this.FreqBands; i++ ) {
            var avage       = 0F;
            var sampleCount = (int) Mathf.Pow( 2, i ) * 2;

            if ( i == this.FreqBands - 1 ) {
                sampleCount += 2;
            }

            for ( var j = 0; j < sampleCount; j++ ) {
                if ( count >= this.SamplesLeft.Length || count >= this.SamplesRight.Length ) continue;
                switch (this.channel) {
                    case _channel.Left:
                        avage += this.SamplesLeft[count] * ( count + 1 );
                        break;
                    case _channel.Right:
                        avage += this.SamplesRight[count] * ( count + 1 );
                        break;
                    case _channel.Stereo:
                        avage += ( this.SamplesLeft[count] + this.SamplesRight[count] ) * ( count + 1 );
                        break;
                    default: break;
                }

                count++;
            }

            avage            /= count;
            this.FreqBand[i] =  avage * 10;
        }
    }

    void CalculateBandBuffer() {
        for ( var i = 0; i < this.FreqBands; i++ ) {
            if ( this.FreqBand[i] > this.BandBuffer[i] ) {
                this.BandBuffer[i]     = this.FreqBand[i];
                this._bufferDecrese[i] = this._bufferDecreseStart;
            }
            else if ( this.FreqBand[i] < this.BandBuffer[i] ) {
                this.BandBuffer[i]     -= this._bufferDecrese[i];
                this._bufferDecrese[i] *= this._bufferDecreseMultiplier;
            }
        }
    }
}

public enum _channel { Stereo, Left, Right }