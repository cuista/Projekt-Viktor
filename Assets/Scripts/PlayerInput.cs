using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] //grants CharacterController component is present on the object 
[AddComponentMenu("Control Script/Player Input")] //adds a folder with your script inside the add component menu
public class PlayerInput : MonoBehaviour
{
    public float speed = 10.0f;
    private CharacterController _charController; //variable for referencing CharacterController
    public float downForce = 40.0f;
    private GameObject _player;

    public float jumpForce = 20.0f;
    private float _verticalVelocity;

    private bool _canBurstDrive;

    Vector3 prevLocation;
    Vector3 directionMovement;

    // Start is called before the first frame update
    void Start()
    {
        _player = gameObject;

        _canBurstDrive = false;

        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        directionMovement = transform.position - prevLocation;
        prevLocation = transform.position;

        if (_charController.isGrounded)
        {
            _verticalVelocity = -downForce * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _verticalVelocity = jumpForce;
                _canBurstDrive = true;
            }
        }

        else
        {
            _verticalVelocity -= downForce * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && _canBurstDrive)
            {
                directionMovement.y=0;
                _charController.Move(directionMovement.normalized * 500.0f * Time.deltaTime);
                _canBurstDrive = false;
            }
        }

        Vector3 moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * speed;
        moveVector.y = _verticalVelocity;
        moveVector.z = Input.GetAxis("Vertical") * speed;
        moveVector = Vector3.ClampMagnitude(moveVector, speed); // avoid diagonal overspeed
        moveVector = transform.TransformDirection(moveVector);
        _charController.Move(moveVector * Time.deltaTime);
    }
}