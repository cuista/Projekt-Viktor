using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    public float rotationSpeed=3.0f;
    public float range=30.0f;
    public float fireDelay = 1f;

    private Quaternion _defaultRotation;
    private float _shootTimer;
    private bool _canShoot;

    private EnemyCharacter _enemyCharacter;

    public GameObject bulletCreationPoint;

    // Start is called before the first frame update
    void Start()
    {
        _defaultRotation = transform.rotation;
        _shootTimer = 0;

        _enemyCharacter=GetComponent<EnemyCharacter>();
        SetMoving(true);
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,range);
        _canShoot = false;
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
                if(hitOverlapSphere.tag == "Player")
                {
                    RaycastHit hitLinecast;
                    // if there are NO obstacles between turret and player
                    if(Physics.Linecast(transform.position, hitOverlapSphere.transform.position, out hitLinecast) && hitLinecast.transform.gameObject.tag == "Player")
                    {
                        if(IsMoving())
                        {
                            Vector3 direction = (hitOverlapSphere.transform.position - transform.position).normalized;
                            Quaternion toRotation = Quaternion.LookRotation(direction);
                            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                            transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                        }
                        _canShoot = true;
                    }
                }
            }
        }

        if(_canShoot)
        {
            _shootTimer += Time.deltaTime;
            if(_shootTimer > fireDelay)
            {
                GameObject bullet=Instantiate(bulletPrefab) as GameObject;
                bullet.transform.position=(bulletCreationPoint!=null)?bulletCreationPoint.transform.position:transform.TransformPoint(Vector3.forward*2.5f);
                bullet.transform.rotation=transform.rotation;
                _shootTimer = 0;
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _defaultRotation, rotationSpeed * Time.deltaTime);
            _shootTimer = 0;
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

    public void SetMoving(bool moving){
        _enemyCharacter.SetMoving(moving);
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

    public void OnSpeedChanged(float value){
        
    }
}
