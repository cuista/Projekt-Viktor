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

    public float speed;

    public GameObject timeline1;

    private void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, UpdateNewEnemiesSpeed);
        BeforeCutscene();
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
        for(int i = _enemies.Count - 1; i >= 0; --i)
        {
            if(_enemies[i] == null)
            {
                _enemies.RemoveAt(i);
                Messenger.Broadcast(GameEvent.ENEMY_KILLED);
            }
        }
    }

    private void AddEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation){
        _enemies.Add(Instantiate(enemyPrefab, position, rotation));
    }

    private void UpdateNewEnemiesSpeed(float value){
        speed = WanderingAI.baseSpeed * value;
    }

    public void BeforeCutscene() {
        GameEvent.isPaused=true;
        Cursor.lockState = CursorLockMode.None;

        DontDestroyOnLoadManager.GetMainCamera().SetActive(false);
        DontDestroyOnLoadManager.GetHUD().SetActive(false);
    }

    public void AfterCutscene() {
        GameEvent.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

        timeline1.SetActive(false);
        DontDestroyOnLoadManager.GetMainCamera().SetActive(true);
        DontDestroyOnLoadManager.GetHUD().SetActive(true);

        //To make special_bombs animators works after cutscene
        Messenger<int>.Broadcast(GameEvent.SPECIALBOMB_CHANGED, 0);
    }
}
