using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    public float speed=3.0f;
    public float obstacleRange=1.0f;
    public const float baseSpeed = 3.0f;
    private GameObject _bullet;

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
        _enemyCharacter=GetComponent<EnemyCharacter>();
        SetAlive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAlive()){
        transform.Translate(0,0,speed*Time.deltaTime); //move continuosly enemy
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray,0.75f,out hit)){
            GameObject hitObject = hit.transform.gameObject;
            if(hitObject.GetComponent<PlayerCharacter>()){
                if(_bullet==null){
                    _bullet=Instantiate(bulletPrefab) as GameObject;
                    _bullet.transform.position=transform.TransformPoint(Vector3.forward*1.5f);
                    _bullet.transform.rotation=transform.rotation;
                }
            }else if(hit.distance < obstacleRange){
                float angle=Random.Range(-110,110);
                transform.Rotate(0,angle, 0);
            }
        }
    }

    public void RemoveLives(int livesToRemove){
        _enemyCharacter.RemoveLives(livesToRemove);
    }

    public int GetLives(){
        return _enemyCharacter.GetLives();
    }

    public bool IsAlive(){
        return _enemyCharacter.IsAlive();
    }

    public void SetAlive(bool alive){
        _enemyCharacter.SetAlive(alive);
    }

    public void OnSpeedChanged(float value){
        speed = baseSpeed * value;
    }
}
