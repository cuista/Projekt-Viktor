using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{

    [SerializeField] private Transform target;

    public float rotSpeed = 15.0f;

    public float moveSpeed = 10.0f;
    private CharacterController _charController;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    private float _vertSpeed;

    private float _burstDriveTime=0.1f;
    private float _burstDriveSpeed=1.0f;
    private bool _canBurstDrive;

    public const float baseSpeed = 6.0f;

    private void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
    private void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    private ControllerColliderHit _contact; //to be precise on edge of objects

    // Start is called before the first frame update
    void Start()
    {
        _vertSpeed = minFall;
        _canBurstDrive = false;
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameEvent.isPaused) {
            Vector3 movement = Vector3.zero;
            float horInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");

            if (horInput != 0 || vertInput != 0) {
                movement.x = horInput * moveSpeed;
                movement.z = vertInput * moveSpeed;
                movement = Vector3.ClampMagnitude(movement,moveSpeed); //avoid diagonal speed-up
                Quaternion tmp = target.rotation;
                target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
                movement = target.TransformDirection(movement);
                target.rotation = tmp;
                Quaternion direction = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Lerp(transform.rotation,direction,rotSpeed * Time.deltaTime); //change rotation smoothly
            }

            bool hitGround = false;
            RaycastHit hit;
            if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
                float check = (_charController.height + _charController.radius) / 1.9f;
                hitGround = hit.distance <= check;
            }

            if (hitGround) {
                if (Input.GetButtonDown("Jump")){
                    _vertSpeed = jumpSpeed;
                    _canBurstDrive = true;
                } else {
                    _vertSpeed = minFall;
                }
            } else {
                _vertSpeed += gravity * 5 * Time.deltaTime;
                if (_vertSpeed < terminalVelocity) {
                    _vertSpeed = terminalVelocity;
                }

                if (Input.GetKeyDown(KeyCode.Space) && _canBurstDrive)
                {
                    //_charController.Move(movement.normalized * 500.0f * Time.deltaTime);
                    StartCoroutine(BurstDriveCoroutine(movement.normalized));
                    _canBurstDrive = false;
                }

                if (_charController.isGrounded) {
                    if (Vector3.Dot(movement, _contact.normal) < 0) {
                        movement = _contact.normal * moveSpeed;
                    } else {
                        movement += _contact.normal * moveSpeed;
                    }
                }
            }

            movement.y = _vertSpeed;
            movement *= Time.deltaTime;

            _charController.Move(movement);
        }
    }

    private IEnumerator BurstDriveCoroutine(Vector3 direction)
    {
        float startTime = Time.time; // how long to burst drive
        while(Time.time < startTime + _burstDriveTime)
        {
            _charController.Move(direction * _burstDriveSpeed);
            yield return null; // this will make Unity stop here and continue next frame
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        _contact = hit;
    }

    private void OnSpeedChanged(float value) {
        moveSpeed = baseSpeed * value;
    }
}
