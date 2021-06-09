using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_0 : MonoBehaviour
{

    private GameObject _player;

    [SerializeField] public GameObject playerSpawn;

    [SerializeField] private GameObject dummy;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject robot;
    [SerializeField] private GameObject drone;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject miniDrone;
    private List<GameObject> _enemies;
    private int _enemiesCount = 0;

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

        _player = DontDestroyOnLoadManager.GetPlayer();
        _player.transform.position = playerSpawn.transform.position;

        _enemies=new List<GameObject>();

        //first enemy
        AddEnemy(drone, new Vector3(-120, 5, 75), Quaternion.Euler(0,90,0));

        //square enemies
        AddEnemy(drone, new Vector3(-10, 5, 160), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(15, 5, 180), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(15, 5, 210), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(-10, 5, 230), Quaternion.Euler(0,-90,0));
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var enemy in _enemies){
            if(enemy==null){
                Messenger.Broadcast(GameEvent.ENEMY_KILLED);
            }
        }
    }

    private void AddEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation){
        _enemies.Add(Instantiate(enemyPrefab, position, rotation));
        _enemiesCount++;
    }

    private void UpdateNewEnemiesSpeed(float value){
        speed = WanderingAI.baseSpeed * value;
    }
}
