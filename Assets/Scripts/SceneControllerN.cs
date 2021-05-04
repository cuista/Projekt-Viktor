using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerN : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
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
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<_enemies.Length; i++){
            /* //Alternativa all'if sotto:
            if(_enemies[i]==null){
                _enemies[i]=Instantiate(enemyPrefab) as GameObject;
                _enemies[i].transform.position=new Vector3(Random.Range(1f,5f),1f,Random.Range(1f,5f));
                float angle = Random.Range(0,360f);
                _enemies[i].transform.Rotate(0,angle,0);
            }
            */
            if(_enemies[i]==null)
            _enemies[i]=Instantiate(enemyPrefab,new Vector3(Random.Range(-70f,70f),1f,Random.Range(-70f,70f)),Quaternion.Euler(0,Random.Range(0, 360f),0));
        }
    }

    private void UpdateNewEnemiesSpeed(float value){
        speed = WanderingAI.baseSpeed * value;
    }
}
