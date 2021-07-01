using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController_VR_Mode : MonoBehaviour
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

    [SerializeField] private GameObject ampule_A;
    [SerializeField] private GameObject ampule_B;
    [SerializeField] private GameObject eChip;
    [SerializeField] private GameObject tetradox;
    [SerializeField] private GameObject napalm;
    [SerializeField] private GameObject graviton;

    public float speed;

    public GameObject directionalLight;

    private float _dropCount = 0;


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

        PlaySoundtrack();

        int sceneColor = (int) Random.Range(0, 6);
        switch(sceneColor)
        {
            case 0:directionalLight.GetComponent<Light>().color = new Color(0.8F,1F,0.8F);break;
            case 1:directionalLight.GetComponent<Light>().color = new Color(0.8F,0.8F,1F);break;
            case 2:directionalLight.GetComponent<Light>().color = new Color(1F,0.8F,0.8F);break;
            case 3:directionalLight.GetComponent<Light>().color = new Color(1F,1F,0.8F);break;
            case 4:directionalLight.GetComponent<Light>().color = new Color(0.8F,1F,1F);break;
            case 5:directionalLight.GetComponent<Light>().color = new Color(1F,0.8F,1F);break;
            default:directionalLight.GetComponent<Light>().color = new Color(0.8F,0.8F,0.8F);break;
        }

        /*ENEMIES*/
        _enemies=new List<GameObject>();

        _enemies.Add(Instantiate(drone, new Vector3(-30, 5, -30), Quaternion.Euler(0,0,0)));
        _enemies.Add(Instantiate(drone, new Vector3(45, 5, -40), Quaternion.Euler(0,-90,0)));
        _enemies.Add(Instantiate(drone, new Vector3(45, 5, 30), Quaternion.Euler(0,90,0)));
        _enemies.Add(Instantiate(drone, new Vector3(-30, 5, 40), Quaternion.Euler(0,0,0)));
        _enemies.Add(Instantiate(drone, new Vector3(30, 5, 30), Quaternion.Euler(0,-90,0)));
        _enemies.Add(Instantiate(drone, new Vector3(-40, 5, -40), Quaternion.Euler(0,-90,0)));

        _enemies.Add(Instantiate(robot, new Vector3(33, 2.1f, 50), Quaternion.Euler(0,90,0)));
        _enemies.Add(Instantiate(robot, new Vector3(-33, 2.1f, 40), Quaternion.Euler(0,0,0)));
        _enemies.Add(Instantiate(robot, new Vector3(33, 2.1f, -50), Quaternion.Euler(0,-90,0)));
        _enemies.Add(Instantiate(robot, new Vector3(28, 2.1f, -40), Quaternion.Euler(0,0,0)));
        _enemies.Add(Instantiate(robot, new Vector3(28, 2.1f, 30), Quaternion.Euler(0,90,0)));

        _enemies.Add(Instantiate(robot, new Vector3(-20, 0.7f, -50), Quaternion.Euler(0,90,0)));
        _enemies.Add(Instantiate(robot, new Vector3(33, 0.7f, 40), Quaternion.Euler(0,0,0)));
        _enemies.Add(Instantiate(robot, new Vector3(-33, 0.7f, -50), Quaternion.Euler(0,-90,0)));
        _enemies.Add(Instantiate(robot, new Vector3(-50, 0.7f, 40), Quaternion.Euler(0,0,0)));
        _enemies.Add(Instantiate(robot, new Vector3(28, 0.7f, -30), Quaternion.Euler(0,90,0)));
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = _enemies.Count - 1; i >= 0; --i)
        {
            if(_enemies[i] == null)
            {
                Messenger.Broadcast(GameEvent.ENEMY_KILLED);
                _dropCount++;

                int enemyType = (int) Random.Range(0, 5);
                switch(enemyType)
                {
                    case 0:AddEnemyAtIndex(i,turret, new Vector3(Random.Range(-75, 75),0.7f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                    case 1:AddEnemyAtIndex(i,robot, new Vector3(Random.Range(-75, 75),2.1f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                    case 2:AddEnemyAtIndex(i,drone, new Vector3(Random.Range(-75, 75),5,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                    case 3:AddEnemyAtIndex(i,soldier, new Vector3(Random.Range(-75, 75),2.1f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                    case 4:AddEnemyAtIndex(i,miniDrone, new Vector3(Random.Range(-75, 75),5,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                    default:AddEnemyAtIndex(i,dummy, new Vector3(Random.Range(-75, 75),2.1f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                }
            }
        }

        if(_dropCount>=3)
        {
            int boxType = (int) Random.Range(0, 6);
            switch(boxType)
            {
                case 0:Instantiate(ampule_A, new Vector3(Random.Range(-75, 75),0.3f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                case 1:Instantiate(ampule_B, new Vector3(Random.Range(-75, 75),0.3f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                case 2:Instantiate(eChip, new Vector3(Random.Range(-75, 75),0.3f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                case 3:Instantiate(tetradox, new Vector3(Random.Range(-75, 75),0.3f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                case 4:Instantiate(napalm, new Vector3(Random.Range(-75, 75),0.3f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                case 5:Instantiate(graviton, new Vector3(Random.Range(-75, 75),0.3f,Random.Range(-75, 75)), Quaternion.Euler(0,0,0));break;
                default:break;
            }

            _dropCount=0;
        }
    }

    private void AddEnemyAtIndex(int index,GameObject enemyPrefab, Vector3 position, Quaternion rotation){
        _enemies[index]=Instantiate(enemyPrefab, position, rotation);
    }

    private void AddBox(GameObject boxPrefab, Vector3 position, Quaternion rotation){
        Instantiate(boxPrefab, position, rotation);
    }

    private void UpdateNewEnemiesSpeed(float value){
        speed = WanderingAI.baseSpeed * value;
    }

    public void PlaySoundtrack()
    {
        DontDestroyOnLoadManager.GetAudioManager().PlaySoundtrackLevel_0();
    }

}
