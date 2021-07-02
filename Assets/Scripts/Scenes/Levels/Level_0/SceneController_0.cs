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
    [SerializeField] private GameObject obstacle_6;
    
    private List<GameObject> _targets;

    public float speed;

    public GameObject timeline1;

    [SerializeField] private GameObject plantTutorial;
    [SerializeField] private GameObject detonateTutorial;
    [SerializeField] private GameObject plantSpecialTutorial;
    [SerializeField] private GameObject moveTutorial;
    [SerializeField] private GameObject sprintTutorial;
    [SerializeField] private GameObject jumpTutorial;
    [SerializeField] private GameObject itemsTutorial;
    [SerializeField] private GameObject echipTutorial;
    [SerializeField] private GameObject healthAmpulesTutorial;
    [SerializeField] private GameObject liquidsTutorial;
    [SerializeField] private GameObject switchTutorial;
    [SerializeField] private GameObject targetsHealthTutorial;
    [SerializeField] private GameObject enemyTutorial;
    [SerializeField] private GameObject enemy1Tutorial;
    [SerializeField] private GameObject enemy2Tutorial;
    [SerializeField] private GameObject enemy3Tutorial;
    private GameObject _blockedArea;
    [SerializeField] private GameObject killEnemyTutorial;
    [SerializeField] private GameObject defenceTutorial;
    [SerializeField] private GameObject shieldTutorial;
    [SerializeField] private GameObject burstDriveTutorial;
    [SerializeField] private GameObject finishLevelTutorial;

    private void Awake() {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, UpdateNewEnemiesSpeed);
        Messenger.AddListener(GameEvent.CUTSCENE_STOPPED, AfterCutscene);
        Messenger<int>.AddListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.AddListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
        Messenger.AddListener(GameEvent.ENEMY_KILLED, OnEnemyKilled);
        BeforeCutscene();
    }

    private void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, UpdateNewEnemiesSpeed);
        Messenger.RemoveListener(GameEvent.CUTSCENE_STOPPED, AfterCutscene);
        Messenger<int>.RemoveListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.RemoveListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
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
        //square targets
        AddTarget(obstacle_6, new Vector3(-18,0.9f,137), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_6, new Vector3(-18,0.9f,268), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_6, new Vector3(-44,0.9f,137), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_6, new Vector3(-44,0.9f,268), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_6, new Vector3(-2,0.9f,212), Quaternion.Euler(0,90,0));
        AddTarget(obstacle_6, new Vector3(-2,0.9f,192), Quaternion.Euler(0,90,0));
        
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

        StartCoroutine(ShowBasicTutorial());
    }

    public void PlaySoundtrack()
    {
        DontDestroyOnLoadManager.GetAudioManager().PlaySoundtrackLevel_0();
    }

    public void ShowPlantTutorial()
    {
        plantTutorial.SetActive(true);
    }

    public void ShowDetonateTutorial()
    {
        detonateTutorial.SetActive(true);
    }

    public void ShowItemsTutorial()
    {
        StartCoroutine(ShowCollectiblesTutorial());
    }

    public void OnBombPlanted(int n)
    {
        if(plantTutorial.activeInHierarchy)
        {
            plantTutorial.SetActive(false);
            ShowDetonateTutorial();
        }
    }

    public void OnBombsDetonated()
    {
        if(detonateTutorial.activeInHierarchy)
        {
            detonateTutorial.SetActive(false);
        }
    }

    private IEnumerator ShowBasicTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        GameEvent.isPaused=true;

        moveTutorial.SetActive(true);
        yield return new WaitForSeconds(3f);
        moveTutorial.SetActive(false);
        sprintTutorial.SetActive(true);
        yield return new WaitForSeconds(3f);
        sprintTutorial.SetActive(false);
        jumpTutorial.SetActive(true);
        yield return new WaitForSeconds(3f);
        jumpTutorial.SetActive(false);
        
        GameEvent.isPaused = false;
    }

    private IEnumerator ShowCollectiblesTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        GameEvent.isPaused=true;

        itemsTutorial.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        itemsTutorial.SetActive(false);
        echipTutorial.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        echipTutorial.SetActive(false);
        healthAmpulesTutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        healthAmpulesTutorial.SetActive(false);
        liquidsTutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        liquidsTutorial.SetActive(false);
        switchTutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        switchTutorial.SetActive(false);
        plantSpecialTutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        plantSpecialTutorial.SetActive(false);
        
        GameEvent.isPaused = false;
    }

    public void ShowTargetsHealthTutorial()
    {
        StartCoroutine(ShowTargetsInfoTutorial());
    }

    private IEnumerator ShowTargetsInfoTutorial()
    {
        targetsHealthTutorial.SetActive(true);
        yield return new WaitForSeconds(5f);
        targetsHealthTutorial.SetActive(false);
    }

    public void ShowEnemyLockTutorial(GameObject blockedArea)
    {
        _blockedArea = blockedArea;
        StartCoroutine(ShowEnemyTutorial());
    }

    private IEnumerator ShowEnemyTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        GameEvent.isPaused=true;

        enemyTutorial.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        enemyTutorial.SetActive(false);
        enemy1Tutorial.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        enemy1Tutorial.SetActive(false);
        enemy2Tutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        enemy2Tutorial.SetActive(false);
        enemy3Tutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        enemy3Tutorial.SetActive(false);

        killEnemyTutorial.SetActive(true);
        
        GameEvent.isPaused = false;
    }

    public void OnEnemyKilled()
    {
        if(killEnemyTutorial.activeInHierarchy)
        {
            killEnemyTutorial.SetActive(false);
            Destroy(_blockedArea);
        }
    }

    public void ShowDefenceTutorial()
    {
        StartCoroutine(ShowShieldBurstDriveTutorial());
    }

    private IEnumerator ShowShieldBurstDriveTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        GameEvent.isPaused=true;

        defenceTutorial.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        defenceTutorial.SetActive(false);
        shieldTutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        shieldTutorial.SetActive(false);
        burstDriveTutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        burstDriveTutorial.SetActive(false);
        
        GameEvent.isPaused = false;
    }

    public void ShowFinishLevelTutorial(){
        StartCoroutine(ShowLevelTargetsTutorial());
    }

    private IEnumerator ShowLevelTargetsTutorial(){
        finishLevelTutorial.SetActive(true);
        yield return new WaitForSeconds(4f);
        finishLevelTutorial.SetActive(false);
    }
}
