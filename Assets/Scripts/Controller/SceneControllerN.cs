using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerN : MonoBehaviour
{
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;
    [SerializeField] private GameObject enemy3Prefab;
    [SerializeField] private GameObject enemy4Prefab;
    private GameObject[] _enemies;
    public int enemiesCount;

    public float speed;

    private void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, UpdateNewEnemiesSpeed);
    }

    private void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, UpdateNewEnemiesSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemies=new GameObject[enemiesCount];

        //Instantiate enemy for debugging
        for(int i=0; i<_enemies.Length; i++){
            if(_enemies[i]==null){
                if(i==_enemies.Length-1){
                    _enemies[i]=Instantiate(enemy2Prefab,new Vector3(-9f,1f,9f),Quaternion.Euler(0,90f,0));
                }
                else if(i==_enemies.Length-2){
                    _enemies[i]=Instantiate(enemy2Prefab,new Vector3(9f,1f,9f),Quaternion.Euler(0,-90f,0));
                }
                else if(i==_enemies.Length-3){
                    _enemies[i]=Instantiate(enemy2Prefab,new Vector3(52f,1f,23f),Quaternion.Euler(0,-90f,0));
                }
                else if(i==_enemies.Length-4){
                    _enemies[i]=Instantiate(enemy2Prefab,new Vector3(-23f,1f,48f),Quaternion.Euler(0,-180f,0));
                }
                else if(i==_enemies.Length-5){
                    _enemies[i]=Instantiate(enemy2Prefab,new Vector3(48f,1f,-27f),Quaternion.Euler(0,0,0));
                }
                else if(i==_enemies.Length-6){
                    _enemies[i]=Instantiate(enemy2Prefab,new Vector3(68f,1f,62.5f),Quaternion.Euler(0,-90f,0));
                }
                else if(i==_enemies.Length-7){
                    _enemies[i]=Instantiate(enemy3Prefab,new Vector3(-22f,1f,42f),Quaternion.Euler(0,-180f,0));
                }
                else if(i==_enemies.Length-8){
                    _enemies[i]=Instantiate(enemy3Prefab,new Vector3(42f,1f,-22f),Quaternion.Euler(0,0,0));
                }
                else if(i==_enemies.Length-9){
                    _enemies[i]=Instantiate(enemy3Prefab,new Vector3(22f,1f,20f),Quaternion.Euler(0,-90f,0));
                }
                else if(i==_enemies.Length-10){
                    _enemies[i]=Instantiate(enemy4Prefab,new Vector3(30f,6f,20f),Quaternion.Euler(0,-90f,0));
                }
                else if(i==_enemies.Length-11){
                    _enemies[i]=Instantiate(enemy4Prefab,new Vector3(-20f,6f,30f),Quaternion.Euler(0,-90f,0));
                }
                else if(i==_enemies.Length-12){
                    _enemies[i]=Instantiate(enemy4Prefab,new Vector3(5f,6f,-20f),Quaternion.Euler(0,-90f,0));
                }
                else
                {
                    // Alternative to gameObject creation with more lines
                    _enemies[i]=Instantiate(enemy1Prefab) as GameObject;
                    _enemies[i].transform.position=new Vector3(Random.Range(-70f,70f),1f,Random.Range(-70f,70f));
                    float angle = Random.Range(0,360f);
                    _enemies[i].transform.Rotate(0,angle,0);
                }
            }
        }
        
    }

    // Update is called once per frame and instantiate an enemy for every dead
    void Update()
    {
        for(int i=0; i<_enemies.Length; i++){
            if(_enemies[i]==null){
                Messenger.Broadcast(GameEvent.ENEMY_KILLED);
                _enemies[i]=Instantiate(enemy1Prefab,new Vector3(Random.Range(-70f,70f),1f,Random.Range(-70f,70f)),Quaternion.Euler(0,Random.Range(0, 360f),0));
            }
        }
    }

    private void UpdateNewEnemiesSpeed(float value){
        speed = WanderingAI.baseSpeed * value;
    }
}
