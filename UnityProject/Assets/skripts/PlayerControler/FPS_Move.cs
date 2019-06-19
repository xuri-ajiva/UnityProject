using UnityEngine;

public class FPS_Move : MonoBehaviour
{
    //movment
    [Header("Player Movment")]
    [Range(0, 10)]
    public float speedNormal = 5f;
    private float speed = 5f;
    [Range(1, 5)]
    public float sprintModifire = 2f;
    //jump
    private float verticalVelocity;
    [Range(0,10)]
    public float jumpForce = 4f;

    //speed modifire
    [Header("Mouse ScrollWheel")]
    public bool AllowWeel = true;
    private float weel;
    public float weel_max = 10;
    public float weel_min = 1;

    //util
    private CharacterController _charControl;
    [Range(1,20)]
    public float gracity = 9.8F;
    private bool fly = false;

    private void Start()
    {
        _charControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {

        //sprint
        if (Input.GetKey(KeyCode.LeftShift))
            speed = speedNormal * sprintModifire;
        else
            speed = speedNormal;

        //progress speed modifire
        if (AllowWeel)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            weel += scroll * 1;
        }
        weel = Mathf.Clamp(weel, weel_min, weel_max);

        speed *= weel;

        //move w a s d
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        //- Input.GetAxis("Croutch")

        if (Input.GetKeyDown(KeyCode.Tab))
            fly = !fly;

        if (!fly)
        {
            //jump
            if (_charControl.isGrounded)
            {
                verticalVelocity = -gracity * Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalVelocity = jumpForce;
                }
            }
            else
            {
                verticalVelocity -= gracity * Time.deltaTime;
            }
        }

        //make vektor
        Vector3 movment = new Vector3(deltaX, verticalVelocity, deltaZ);

        movment = Vector3.ClampMagnitude(movment, speed);


        if (fly)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                movment.y +=1* speed;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                movment.y -= 1* speed;
            }
        }

        movment *= Time.deltaTime;
        movment = transform.TransformDirection(movment);
        _charControl.Move(movment);
    }
}
