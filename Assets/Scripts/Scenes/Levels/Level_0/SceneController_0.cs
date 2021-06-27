using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_0 : MonoBehaviour
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

    [SerializeField] private GameObject obstacle_1;
    [SerializeField] private GameObject obstacle_2;
    [SerializeField] private GameObject obstacle_3;
    
    private List<GameObject> _targets;

    public float speed;

    public GameObject timeline1;

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

        endLevel.gameObject.SetActive(false);

        /*ENEMIES*/
        _enemies=new List<GameObject>();
        //first enemies
        AddEnemy(drone, new Vector3(-120, 5, 75), Quaternion.Euler(0,90,0));
        AddEnemy(robot, new Vector3(-119, 1.1f, 192), Quaternion.Euler(0,180,0));
        AddEnemy(robot, new Vector3(-78, 1.1f, 194), Quaternion.Euler(0,-90,0));

        //square enemies
        AddEnemy(drone, new Vector3(-10, 5, 160), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(15, 5, 180), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(15, 5, 210), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(-10, 5, 230), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(10, 5, 130), Quaternion.Euler(0,-90,0));
        AddEnemy(drone, new Vector3(10, 5, 260), Quaternion.Euler(0,-90,0));

        AddEnemy(robot, new Vector3(33, 2.1f, 192), Quaternion.Euler(0,-90,0));
        AddEnemy(robot, new Vector3(33, 2.1f, 203), Quaternion.Euler(0,-90,0));
        AddEnemy(robot, new Vector3(33, 2.1f, 213), Quaternion.Euler(0,-90,0));
        AddEnemy(robot, new Vector3(28, 2.1f, 268), Quaternion.Euler(0,-90,0));
        AddEnemy(robot, new Vector3(28, 2.1f, 138), Quaternion.Euler(0,-90,0));

        /*TARGETS*/
        _targets=new List<GameObject>();
        AddTarget(obstacle_1, new Vector3(0,0.1f,0), Quaternion.Euler(0,0,0));
        AddTarget(obstacle_2, new Vector3(-23.7f,0.1f,74.6f), Quaternion.Euler(0,-90,0));
        AddTarget(obstacle_3, new Vector3(-119.8f,0.1f,106.9f), Quaternion.Euler(0,0,0));
        
        Messenger<int>.Broadcast(GameEvent.TARGET_TOTAL, (_enemies.Count + _targets.Count));
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
                Messenger.Broadcast(GameEvent.TARGET_ELIMINATED);
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

        if((_enemies.Count + _targets.Count) <= 0 && !endLevel.gameObject.activeInHierarchy)
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

        DontDestroyOnLoadManager.GetMainCamera().SetActive(false);
        DontDestroyOnLoadManager.GetHUD().SetActive(false);
        DontDestroyOnLoadManager.GetSkipMessage().SetActive(true);

        Messenger.Broadcast(GameEvent.CUTSCENE_STARTED);

        DontDestroyOnLoadManager.GetAudioManager().StopCurrentSoundtrack();
    }

    public void AfterCutscene() {
        GameEvent.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

        timeline1.SetActive(false);
        DontDestroyOnLoadManager.GetMainCamera().SetActive(true);
        DontDestroyOnLoadManager.GetHUD().SetActive(true);
        DontDestroyOnLoadManager.GetSkipMessage().SetActive(false);

        Messenger.Broadcast(GameEvent.CUTSCENE_ENDED);

        //To make special_bombs animators works after cutscene
        Messenger<int>.Broadcast(GameEvent.SPECIALBOMB_CHANGED, 0);

        //if skip before cutscene makes level soundtrack playing
        AudioManager audioManager = DontDestroyOnLoadManager.GetAudioManager();
        if(!audioManager.isPlayingClip(audioManager.level0_soundtrack))
            audioManager.PlaySoundtrackLevel_0();
    }

    public void PlaySoundtrack()
    {
        DontDestroyOnLoadManager.GetAudioManager().PlaySoundtrackLevel_0();
    }
}
