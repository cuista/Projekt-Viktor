﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public float speed=4.0f;
    public float obstacleRange=1.0f;
    public float rotationSpeed=5.0f;
    public float range=30.0f;
    public float fireDelay = 1f;
    private bool _alive;
    public const float baseSpeed = 3.0f;

    private GameObject _bullet;
    private Vector3 _defaultPosition;
    private float _shootTimer;
    private bool _canShoot;
    private bool _isFollowing;

    private Vector3 _targetPosition;


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

        _alive=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_alive){
        transform.Translate(0,0,speed*Time.deltaTime); //move continuosly enemy
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position,range);
        _canShoot = false;
        _isFollowing = false;
        foreach(var hitCollider in hitColliders)
        {
            GameObject hitOverlapSphere = hitCollider.transform.gameObject;
            // check if player is within range
            if(hitOverlapSphere.GetComponent<PlayerCharacter>() != null)
            {
                _isFollowing = true;
                RaycastHit hitLinecast;
                // if there are NO obstacles between this enemy and player
                if(Physics.Linecast(transform.position, hitOverlapSphere.transform.position, out hitLinecast) && hitLinecast.transform.gameObject.GetComponent<PlayerCharacter>() != null)
                {
                    Vector3 direction = (hitOverlapSphere.transform.position - transform.position).normalized;
                    Quaternion toRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                    transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
                    _targetPosition = hitOverlapSphere.transform.position;
                    _canShoot = true;
                }
            }
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray,0.75f,out hit)){
            GameObject hitObject = hit.transform.gameObject;
            if(hit.distance < obstacleRange)
            {
                float angle=Random.Range(-110,110);
                transform.Rotate(0,angle, 0);
            }
            else if(Vector3.Distance(transform.position, _defaultPosition) > range)
            {
                if(_isFollowing)
                {
                    transform.Translate(0,0,-speed*Time.deltaTime);
                }
                else
                {
                    Quaternion toRotation = Quaternion.LookRotation(_defaultPosition);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                    transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0); // only y rotation
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

    public void SetAlive(bool alive){
        _alive=alive;
    }

    private void OnSpeedChanged(float value){
        speed = baseSpeed * value;
    }

    private IEnumerator Shoot() {
        for(int i=0; i<3; i++)
        {
            GameObject bullet=Instantiate(bulletPrefab) as GameObject;
            bullet.transform.position=transform.TransformPoint(Vector3.forward*2.5f);
            bullet.transform.LookAt(_targetPosition);

            yield return new WaitForSeconds(0.5f);
        }
    }
}