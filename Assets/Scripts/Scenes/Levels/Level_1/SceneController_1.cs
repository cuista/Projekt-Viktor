using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_1 : MonoBehaviour
{

    private GameObject _player;

    [SerializeField] public GameObject playerSpawn;
    [SerializeField] public GameObject endLevel;

    [SerializeField] private GameObject dummy;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject robot;
    [SerializeField] private GameObject drone;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject miniDrone;
    private List<GameObject> _enemies;

    [SerializeField] private GameObject obstacle_4;
    [SerializeField] private GameObject obstacle_5;
    
    private List<GameObject> _targets;

    public GameObject timeline1;

    public float speed;

    private void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, UpdateNewEnemiesSpeed);
        Messenger.AddListener(GameEvent.CUTSCENE_STOPPED, AfterCutscene);
        BeforeCutscene();
    }

    private void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, UpdateNewEnemiesSpeed);
        Messenger.RemoveListener(GameEvent.CUTSCENE_STOPPED, AfterCutscene);
    }

    // Start is called before the first frame update
    void Start()
    {

        _player = DontDestroyOnLoadManager.GetPlayer();
        _player.transform.position = playerSpawn.transform.position;
        _player.transform.rotation = Quaternion.Euler(-transform.right);

        endLevel.gameObject.SetActive(false);

        GameObject camera = DontDestroyOnLoadManager.GetMainCamera();
        camera.transform.position=new Vector3(75,6.01f,112);
        camera.transform.rotation=Quaternion.Euler(30,-90,0);

        /*ENEMIES*/
        _enemies=new List<GameObject>();
        //turrets
        AddEnemy(turret, new Vector3(37, 0.54f, 106), Quaternion.Euler(0,0,0));
        AddEnemy(turret, new Vector3(-8, 0.54f, 105.5f), Quaternion.Euler(0,45,0));
        AddEnemy(turret, new Vector3(-8, 0.54f, 118), Quaternion.Euler(0,135,0));
        AddEnemy(turret, new Vector3(-8, 0.54f, 55.5f), Quaternion.Euler(0,90,0));
        AddEnemy(turret, new Vector3(10.5f, 0.54f, 55.5f), Quaternion.Euler(0,-90,0));
        AddEnemy(turret, new Vector3(7.5f, 0.54f, 35.5f), Quaternion.Euler(0,90,0));
        AddEnemy(turret, new Vector3(47.5f, 0.54f, 52), Quaternion.Euler(0,-90,0));
        AddEnemy(turret, new Vector3(47.5f, 0.54f, 36), Quaternion.Euler(0,-90,0));
        //robots
        AddEnemy(robot, new Vector3(3.5f, 1.1f, 90), Quaternion.Euler(0,0,0));
        AddEnemy(robot, new Vector3(-1.5f, 1.1f, 81), Quaternion.Euler(0,0,0));
        AddEnemy(robot, new Vector3(4, 1.1f, 64), Quaternion.Euler(0,0,0));
        AddEnemy(robot, new Vector3(-1.5f, 1.1f, 17), Quaternion.Euler(0,0,0));
        AddEnemy(robot, new Vector3(-16, 1.1f, 62), Quaternion.Euler(0,90,0));
        AddEnemy(robot, new Vector3(42, 1.1f, 10), Quaternion.Euler(0,180,0));
        //drones
        AddEnemy(drone, new Vector3(0, 5f, 84), Quaternion.Euler(0,90,0));
        AddEnemy(drone, new Vector3(0, 5f, 10), Quaternion.Euler(0,90,0));
        AddEnemy(drone, new Vector3(2.5f, 5f, 63), Quaternion.Euler(0,0,0));
        AddEnemy(drone, new Vector3(60, 5f, 46), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(56, 5f, 36), Quaternion.Euler(0,-90,0));
        //soldiers
        AddEnemy(soldier, new Vector3(26f, 1.1f, -15), Quaternion.Euler(0,0,0));
        AddEnemy(soldier, new Vector3(33, 1.1f, -20), Quaternion.Euler(0,0,0));
        AddEnemy(soldier, new Vector3(53, 1.1f, 32), Quaternion.Euler(0,-90,0));
        AddEnemy(soldier, new Vector3(43, 1.1f, 55), Quaternion.Euler(0,180,0));
        AddEnemy(soldier, new Vector3(55, 1.1f, 39), Quaternion.Euler(0,-90,0));
        AddEnemy(soldier, new Vector3(41, 1.1f, 43), Quaternion.Euler(0,180,0));
        AddEnemy(soldier, new Vector3(42, 1.1f, 47.5f), Quaternion.Euler(0,180,0));
        //miniDrones
        AddEnemy(miniDrone, new Vector3(42, 5f, 7), Quaternion.Euler(0,180,0));
        AddEnemy(miniDrone, new Vector3(42, 5f, 20), Quaternion.Euler(0,-90,0));

        /*TARGETS*/
        _targets=new List<GameObject>();
        AddTarget(obstacle_4, new Vector3(-6.6f,0.1f,22.7f), Quaternion.Euler(0,162,0));
        AddTarget(obstacle_5, new Vector3(11,0.1f,61.8f), Quaternion.Euler(0,-90,0));
        AddTarget(obstacle_5, new Vector3(11.7f,0.1f,111.9f), Quaternion.Euler(0,-90,0));
        AddTarget(obstacle_5, new Vector3(-21f,0.1f,61.8f), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_5, new Vector3(26f,0.1f,50.9f), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_5, new Vector3(20,0.1f,36), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_5, new Vector3(30,0.1f,62), Quaternion.Euler(0,90,0));
        
        Messenger<int>.Broadcast(GameEvent.TARGET_TOTAL, (_targets.Count));
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

        for(int i = _targets.Count - 1; i >= 0; --i)
        {
            if(_targets[i].GetComponentInChildren<ReactiveTarget>() == null)
            {
                _targets.RemoveAt(i);
                Messenger.Broadcast(GameEvent.TARGET_ELIMINATED);
            }
        }

        if((_targets.Count) <= 0 && !endLevel.gameObject.activeInHierarchy)
        {
            endLevel.gameObject.SetActive(true);
        }
    }

    private void AddEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation){
        _enemies.Add(Instantiate(enemyPrefab, position, rotation));
    }

    private void AddTarget(GameObject targetPrefab, Vector3 position, Quaternion rotation){
        _targets.Add(Instantiate(targetPrefab, position, rotation));
    }

    private void UpdateNewEnemiesSpeed(float value){
        speed = WanderingAI.baseSpeed * value;
    }

    public void BeforeCutscene() {
        GameEvent.isPaused=true;
        Cursor.lockState = CursorLockMode.None;

        DontDestroyOnLoadManager.GetPlayer().SetActive(false);

        DontDestroyOnLoadManager.GetMainCamera().SetActive(false);
        DontDestroyOnLoadManager.GetHUD().SetActive(false);
        DontDestroyOnLoadManager.GetSkipMessage().SetActive(true);

        Messenger.Broadcast(GameEvent.CUTSCENE_STARTED);

        DontDestroyOnLoadManager.GetAudioManager().StopCurrentSoundtrack();
    }

    public void AfterCutscene() {
        GameEvent.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

        DontDestroyOnLoadManager.GetPlayer().SetActive(true);

        timeline1.SetActive(false);
        DontDestroyOnLoadManager.GetMainCamera().SetActive(true);
        DontDestroyOnLoadManager.GetHUD().SetActive(true);
        DontDestroyOnLoadManager.GetSkipMessage().SetActive(false);

        Messenger.Broadcast(GameEvent.CUTSCENE_ENDED);

        //To make special_bombs animators works after cutscene
        Messenger<int>.Broadcast(GameEvent.SPECIALBOMB_CHANGED, 0);

        //if skip before cutscene makes level soundtrack playing
        AudioManager audioManager = DontDestroyOnLoadManager.GetAudioManager();
        if(!audioManager.isPlayingClip(audioManager.level1_soundtrack))
            audioManager.PlaySoundtrackLevel_1();
    }
}
