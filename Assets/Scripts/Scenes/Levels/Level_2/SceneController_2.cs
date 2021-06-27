using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_2 : MonoBehaviour
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

    [SerializeField] private GameObject capacitor_1;
    [SerializeField] private GameObject capacitor_2;
    [SerializeField] private GameObject capacitor_3;
    [SerializeField] private GameObject capacitor_4;
    
    private List<GameObject> _targets;

    public GameObject timeline2;

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
        AddEnemy(turret, new Vector3(19.5f, 3.8f, -39), Quaternion.Euler(0,-90,0));
        AddEnemy(turret, new Vector3(4.5f, 3.8f, -39), Quaternion.Euler(0,90,0));
        AddEnemy(turret, new Vector3(41.5f, 3.8f, -65.5f), Quaternion.Euler(0,0,0));
        AddEnemy(turret, new Vector3(41.5f, 3.8f, -51), Quaternion.Euler(0,180,0));
        AddEnemy(turret, new Vector3(77.5f, 3.8f, -65.5f), Quaternion.Euler(0,0,0));
        AddEnemy(turret, new Vector3(77.5f, 3.8f, -51), Quaternion.Euler(0,180,0));
        AddEnemy(turret, new Vector3(103, 3.8f, -58), Quaternion.Euler(0,-90,0));
        AddEnemy(turret, new Vector3(48, 3.8f, -89.5f), Quaternion.Euler(0,0,0));
        AddEnemy(turret, new Vector3(48, 3.8f, -74.5f), Quaternion.Euler(0,180,0));
        //soldiers
        AddEnemy(soldier, new Vector3(48, 4.2f, -58), Quaternion.Euler(0,-90,0));
        AddEnemy(soldier, new Vector3(82, 4.2f, -58), Quaternion.Euler(0,-90,0));
        AddEnemy(soldier, new Vector3(96, 4.2f, -70), Quaternion.Euler(0,0,0));
        AddEnemy(soldier, new Vector3(77, 4.2f, -82), Quaternion.Euler(0,90,0));
        AddEnemy(soldier, new Vector3(43, 4.2f, -82), Quaternion.Euler(0,90,0));
        AddEnemy(soldier, new Vector3(30, 4.2f, -82), Quaternion.Euler(0,90,0));
        AddEnemy(soldier, new Vector3(12, 4.2f, -100), Quaternion.Euler(0,0,0));
        AddEnemy(soldier, new Vector3(-30, 4.2f, -106), Quaternion.Euler(0,90,0));
        AddEnemy(soldier, new Vector3(-8, 4.2f, -130), Quaternion.Euler(0,0,0));
        AddEnemy(soldier, new Vector3(-50, 4.2f, -130), Quaternion.Euler(0,90,0));
        AddEnemy(soldier, new Vector3(12, 7.4f, -185), Quaternion.Euler(0,0,0));
        AddEnemy(soldier, new Vector3(12, 7.4f, -250), Quaternion.Euler(0,0,0));
        AddEnemy(soldier, new Vector3(-12, 7.4f, -215), Quaternion.Euler(0,-90,0));
        AddEnemy(soldier, new Vector3(32, 7.4f, -215), Quaternion.Euler(0,90,0));
        AddEnemy(soldier, new Vector3(12, 7.4f, -340), Quaternion.Euler(0,0,0));
        //miniDrones
        AddEnemy(miniDrone, new Vector3(-7, 8f, -106), Quaternion.Euler(0,90,0));
        AddEnemy(miniDrone, new Vector3(-50, 8f, -106), Quaternion.Euler(0,90,0));
        AddEnemy(miniDrone, new Vector3(-9, 8f, -130), Quaternion.Euler(0,-90,0));
        AddEnemy(miniDrone, new Vector3(-55, 8f, -130), Quaternion.Euler(0,-90,0));
        AddEnemy(miniDrone, new Vector3(12, 10f, -185), Quaternion.Euler(0,0,0));
        AddEnemy(miniDrone, new Vector3(12, 10f, -250), Quaternion.Euler(0,0,0));
        AddEnemy(miniDrone, new Vector3(-12, 10f, -215), Quaternion.Euler(0,-90,0));
        AddEnemy(miniDrone, new Vector3(32, 10f, -215), Quaternion.Euler(0,90,0));
        AddEnemy(miniDrone, new Vector3(55, 10f, -245), Quaternion.Euler(0,0,0));
        AddEnemy(miniDrone, new Vector3(55, 10f, -191), Quaternion.Euler(0,0,0));
        AddEnemy(miniDrone, new Vector3(-32, 10f, -191), Quaternion.Euler(0,0,0));
        AddEnemy(miniDrone, new Vector3(-32, 10f, -245), Quaternion.Euler(0,0,0));
        AddEnemy(miniDrone, new Vector3(12, 10f, -320), Quaternion.Euler(0,0,0));

        /*TARGETS*/
        _targets=new List<GameObject>();
        AddTarget(capacitor_1, new Vector3(0,0.1f,50), Quaternion.Euler(0,0,0));
        AddTarget(capacitor_2, new Vector3(0,0.1f,50), Quaternion.Euler(0,0,0));
        AddTarget(capacitor_3, new Vector3(0,0.1f,50), Quaternion.Euler(0,0,0));
        AddTarget(capacitor_4, new Vector3(0,0.1f,50), Quaternion.Euler(0,0,0));
        
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

        timeline2.SetActive(false);
        DontDestroyOnLoadManager.GetMainCamera().SetActive(true);
        DontDestroyOnLoadManager.GetHUD().SetActive(true);
        DontDestroyOnLoadManager.GetSkipMessage().SetActive(false);

        Messenger.Broadcast(GameEvent.CUTSCENE_ENDED);

        //To make special_bombs animators works after cutscene
        Messenger<int>.Broadcast(GameEvent.SPECIALBOMB_CHANGED, 0);

        AudioManager audioManager = DontDestroyOnLoadManager.GetAudioManager();
        if(!audioManager.isPlayingClip(audioManager.level2_soundtrack))
            audioManager.PlaySoundtrackLevel_2();
    }
}
