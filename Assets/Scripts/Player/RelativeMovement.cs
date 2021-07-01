using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{

    [SerializeField] private Transform target;

    public float rotSpeed = 15.0f;

    private float _moveSpeed = 10.0f;
    private CharacterController _charController;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    private float _vertSpeed;
    private bool _isJumping;

    [SerializeField] public GameObject burstEffect;
    private float _burstDriveTime=0.2f;
    private float _burstDriveSpeed=4f;
    private bool _canBurstDrive;

    public const float baseSpeed = 6.0f;
    private ControllerColliderHit _contact; //to be precise on edge of objects

    private Animator _animator;
    [SerializeField] private Animator _animatorShadow;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip footStepSound;
    private float _walkStepSoundLength;
    private float _runStepSoundLength;
    private bool _step;

    [SerializeField] private AudioClip jumpSound;


    private void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
    private void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        _vertSpeed = minFall;
        _canBurstDrive = false;
        _isJumping = false;
        _charController = GetComponent<CharacterController>();

        burstEffect.SetActive(false);

        _animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
        _step = true;
        _walkStepSoundLength = 0.336f;
        _runStepSoundLength = 0.261f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameEvent.isPaused) {
            Vector3 movement = Vector3.zero;
            float horInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");

            if (horInput != 0 || vertInput != 0) {
                if(Input.GetKey(KeyCode.LeftShift)){
                    _moveSpeed=9f;
                } else {
                    _moveSpeed=6f;
                }
                movement.x = horInput * _moveSpeed;
                movement.z = vertInput * _moveSpeed;
                movement = Vector3.ClampMagnitude(movement,_moveSpeed); //avoid diagonal speed-up
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

            
            if(_charController.velocity.magnitude > 1f && _step && !_isJumping){
                _audioSource.PlayOneShot(footStepSound);
                StartCoroutine(WaitForFootSteps(_charController.velocity.magnitude));
            }
            _animator.SetFloat("Speed", movement.magnitude);
            _animatorShadow.SetFloat("Speed", movement.magnitude);

            if (hitGround) {
                if (Input.GetButtonDown("Jump")){
                    _vertSpeed = jumpSpeed;
                    _canBurstDrive = true;
                    _audioSource.PlayOneShot(jumpSound);
                } else {
                    _vertSpeed = minFall;
                    _isJumping=false;
                    _animator.SetBool("Jumping",false);
                    _animatorShadow.SetBool("Jumping",false);
                }
            } else {
                _vertSpeed += gravity * 5 * Time.deltaTime;
                if (_vertSpeed < terminalVelocity) {
                    _vertSpeed = terminalVelocity;
                }

                _isJumping=true;
                _animator.SetBool("Jumping",true);
                _animatorShadow.SetBool("Jumping",true);

                if (Input.GetKeyDown(KeyCode.Space) && _canBurstDrive)
                {
                    if(movement!=Vector3.zero)
                    {
                        StartCoroutine(BurstDriveCoroutine(movement));
                        _canBurstDrive = false;
                        _audioSource.PlayOneShot(jumpSound);
                    }
                }

                if (_charController.isGrounded) {
                    if (Vector3.Dot(movement, _contact.normal) < 0) // Dot if they point same is 1 (same direction) to -1 (opposite)
                    {
                        movement = _contact.normal * _moveSpeed;
                        _isJumping=false;
                        _animator.SetBool("Jumping",false);
                        _animatorShadow.SetBool("Jumping",false);
                    } else {
                        movement += _contact.normal * _moveSpeed * 10;
                    }
                }
            }
            movement.y = _vertSpeed;

            _charController.Move(movement * Time.deltaTime);
        }
    }

    private IEnumerator BurstDriveCoroutine(Vector3 direction)
    {
        float startTime = Time.time; // how long to burst drive
        while(Time.time < startTime + _burstDriveTime)
        {
            Vector3 movement = Vector3.ClampMagnitude(direction,_burstDriveSpeed);
            _charController.Move(movement * _burstDriveSpeed * Time.deltaTime);
            burstEffect.SetActive(true);
            yield return null; // this will make Unity stop here and continue next frame
        }
        burstEffect.SetActive(false);
    }

    public bool isJumping(){
        return _isJumping;
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        _contact = hit;
    }

    private IEnumerator WaitForFootSteps(float movementSpeed){
        _step = false;
        yield return new WaitForSeconds(movementSpeed>7?_runStepSoundLength:_walkStepSoundLength);
        _step = true;
    }

    private void OnSpeedChanged(float value) {
        _moveSpeed = baseSpeed * value;
    }
}
