using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingAI : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    public float speed=4.0f;
    public float obstacleRange=1.0f;
    public float rotationSpeed=5.0f;
    public float range=30.0f;
    public float fireDelay = 1f;
    public const float baseSpeed = 3.0f;

    private GameObject _bullet;
    private Vector3 _defaultPosition;
    private float _shootTimer;
    private bool _canShoot;
    private bool _isFollowing;
    private bool _isPatrolling;

    private EnemyCharacter _enemyCharacter;


    private void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    private void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        _defaultPosition=transform.position;
        _shootTimer = 0;

        _enemyCharacter=GetComponent<EnemyCharacter>();
        SetMoving(true);

        _isPatrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMoving()){
        transform.Translate(0,0,speed*Time.deltaTime); //move continuosly enemy
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position,range);
        _canShoot = false;
        _isFollowing = false;
        Vector3 graviton = GetGraviton();
        if(graviton != Vector3.zero)
        {
            Vector3 direction = (graviton - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
        }
        else
        {
            foreach(var hitCollider in hitColliders)
            {
                GameObject hitOverlapSphere = hitCollider.transform.gameObject;
                // check if player is within range
                if(hitOverlapSphere.GetComponent<PlayerCharacter>() != null)
                {
                    _isFollowing = true;
                    _isPatrolling = false;
                    RaycastHit hitLinecast;
                    // if there are NO obstacles between this enemy and player
                    if(Physics.Linecast(transform.position, hitOverlapSphere.transform.position, out hitLinecast) && hitLinecast.transform.gameObject.GetComponent<PlayerCharacter>() != null)
                    {
                        Vector3 direction = (hitOverlapSphere.transform.position - transform.position).normalized;
                        Quaternion toRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                        _canShoot = true;
                    }
                }
            }

            // really close to default position
            if(Vector3.Distance(transform.position, _defaultPosition)<5f){
                _isPatrolling=true;
            }
            // returning to default position
            if(!_isFollowing && !_isPatrolling){
                Vector3 forwardDir = (_defaultPosition - transform.position).normalized;
                Vector3 rightDir = Quaternion.Euler(0,90,0) * forwardDir;
                Vector3 leftDir = Quaternion.Euler(0,-90,0) * forwardDir;
                RaycastHit forwardHit, rightHit, leftHit;
                if (!Physics.Raycast(transform.position,forwardDir, out forwardHit) || forwardHit.distance>4f)
                {
                    Quaternion toRotation = Quaternion.LookRotation(forwardDir);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                    transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                }
                else
                {
                    bool hasHittedRight = Physics.Raycast(transform.position,rightDir, out rightHit);
                    bool hasHittedLeft = Physics.Raycast(transform.position,leftDir, out leftHit);
                    if(!hasHittedRight || (hasHittedLeft && rightHit.distance >= leftHit.distance && rightHit.distance>4f))
                    {
                        Quaternion toRotation = Quaternion.LookRotation(rightDir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                    }
                    else if(!hasHittedLeft || (hasHittedRight && leftHit.distance >= rightHit.distance && leftHit.distance>4f))
                    {
                        Quaternion toRotation = Quaternion.LookRotation(leftDir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                    }
                    else
                    {
                        if(forwardHit.distance >= rightHit.distance && forwardHit.distance >= leftHit.distance)
                        {
                            Quaternion toRotation = Quaternion.LookRotation(forwardDir);
                            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                        }
                        else if(rightHit.distance >= forwardHit.distance && rightHit.distance >= leftHit.distance)
                        {
                            Quaternion toRotation = Quaternion.LookRotation(rightDir);
                            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                        }
                        else
                        {
                            Quaternion toRotation = Quaternion.LookRotation(leftDir);
                            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                        }
                    }

                }       
            }

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray,0.75f,out hit)){
                GameObject hitObject = hit.transform.gameObject;
                if(hit.distance < obstacleRange)
                {
                    float patrolAngle=Random.Range(-110,110);
                    transform.Rotate(0,patrolAngle, 0);
                }
                else if(Vector3.Distance(transform.position, _defaultPosition) > range)
                {
                    if(_isFollowing)
                    {
                        transform.Translate(0,0,-speed*Time.deltaTime);
                    }
                    else
                    {
                        _isPatrolling = false; // trigger return to default position
                    }
                }
            }
        }

        if(_canShoot)
        {
            //Debug.Log(_canShoot + " " + _shootTimer);
            _shootTimer += Time.deltaTime;
            if(_shootTimer > fireDelay)
            {
                StartCoroutine(Shoot());
                _shootTimer = -2;
            }
        }
    }

    public void RemoveLives(int livesToRemove){
        _enemyCharacter.RemoveLives(livesToRemove);
    }

    public int GetLives(){
        return _enemyCharacter.GetLives();
    }

    public bool IsMoving(){
        return _enemyCharacter.IsMoving();
    }

    public Vector3 GetGraviton(){
        return _enemyCharacter.GetGraviton();
    }

    public void AddGravitonAddiction(Vector3 graviton)
    {
        _enemyCharacter.AddGravitonAddiction(graviton);
    }

    public void RemoveGravitonAddiction()
    {
        _enemyCharacter.RemoveGravitonAddiction();
    }

    public void SetMoving(bool moving){
        _enemyCharacter.SetMoving(moving);
    }

    public void OnSpeedChanged(float value){
        speed = baseSpeed * value;
    }

    private IEnumerator Shoot() {
        for(int i=0; i<3; i++)
        {
            GameObject bullet=Instantiate(bulletPrefab) as GameObject;
            bullet.transform.position=transform.TransformPoint(Vector3.forward*2.5f);
            bullet.transform.rotation=transform.rotation;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
