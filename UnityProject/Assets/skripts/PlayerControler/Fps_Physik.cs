using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fps_Physik : MonoBehaviour
{

    public float jumpForce =5f;

    private CharacterController _charControl;

    private float gracity = 9.8F;
    private float verticalVelocity;

    private void Start()
    {
        _charControl = GetComponent<CharacterController>();
    }

    private void Update()
    {
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

        Vector3 moveVector = new Vector3(0, verticalVelocity, 0);
        _charControl.Move(moveVector * Time.deltaTime);
    }
}
