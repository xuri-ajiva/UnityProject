using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FractalMaster : MonoBehaviour {
    public ComputeShader fractalShader;

    [Range( 1, 100 )] public float FractalPower = 10;
    public                   float darkness     = 70;

    [Header( "Colour mixing" )] [Range( 0, 1 )] public float blackAndWhite;
    [Range(                             0, 1 )] public float redA;
    [Range(                             0, 1 )] public float greenA;
    [Range(                             0, 1 )] public float blueA = 1;
    [Range(                             0, 1 )] public float redB  = 1;
    [Range(                             0, 1 )] public float greenB;
    [Range(                             0, 1 )] public float blueB;

    RenderTexture target;
    Camera        cam;
    Light         directionalLight;

    [Header( "Animation Settings" )] public float powerIncreaseSpeed = 0.2f;

    void Start() {
        Application.targetFrameRate = 60;
    }

    void Init() {
        this.cam              = Camera.current;
        this.directionalLight = FindObjectOfType<Light>();
    }

    // Animate properties
    void Update() {
        if ( Application.isPlaying ) {
            this.FractalPower += this.powerIncreaseSpeed * Time.deltaTime;
        }

        if ( Input.anyKey )
            if ( Input.GetKey( KeyCode.Q ) ) {
                this.FractalPower -= 0.2F;
            }
            else if ( Input.GetKey( KeyCode.E ) ) {
                this.FractalPower += 0.2F;
            }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Init();
        InitRenderTexture();
        SetParameters();

        int threadGroupsX = Mathf.CeilToInt( this.cam.pixelWidth  / 8.0f );
        int threadGroupsY = Mathf.CeilToInt( this.cam.pixelHeight / 8.0f );
        this.fractalShader.Dispatch( 0, threadGroupsX, threadGroupsY, 1 );

        Graphics.Blit( this.target, destination );
    }

    void SetParameters() {
        this.fractalShader.SetTexture( 0, "Destination", this.target );
        this.fractalShader.SetFloat( "power",         Mathf.Max( this.FractalPower, 1.01f ) );
        this.fractalShader.SetFloat( "darkness",      this.darkness );
        this.fractalShader.SetFloat( "blackAndWhite", this.blackAndWhite );
        this.fractalShader.SetVector( "colourAMix", new Vector3( this.redA, this.greenA, this.blueA ) );
        this.fractalShader.SetVector( "colourBMix", new Vector3( this.redB, this.greenB, this.blueB ) );

        this.fractalShader.SetMatrix( "_CameraToWorld",           this.cam.cameraToWorldMatrix );
        this.fractalShader.SetMatrix( "_CameraInverseProjection", this.cam.projectionMatrix.inverse );
        this.fractalShader.SetVector( "_LightDirection", this.directionalLight.transform.forward );
    }

    void InitRenderTexture() {
        if ( this.target == null || this.target.width != this.cam.pixelWidth || this.target.height != this.cam.pixelHeight ) {
            if ( this.target != null ) {
                this.target.Release();
            }

            this.target = new RenderTexture( this.cam.pixelWidth, this.cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat,
                RenderTextureReadWrite.Linear );
            this.target.enableRandomWrite = true;
            this.target.Create();
        }
    }
}