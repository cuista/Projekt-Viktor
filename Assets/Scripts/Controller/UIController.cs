using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text scoreValue;

    [SerializeField] private Text bombsCapacityValue;
    [SerializeField] private Text bombsValue;

    [SerializeField] private SettingsPopup settingsPopup;

    [SerializeField] private GameObject[] specialBomb_quantityValues;

    [SerializeField] private Animator[] specialBombsAnimator_slots;

    private int _score;
    private int _bombsCapacity;
    private int _bombsPlanted;

    void Awake() {
        Messenger.AddListener(GameEvent.ENEMY_KILLED, OnEnemyHit);
        Messenger<int>.AddListener(GameEvent.BOMBS_CAPACITY_CHANGED, OnBombCapacityChanged);
        Messenger.AddListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.AddListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
        Messenger<int>.AddListener(GameEvent.SPECIALBOMB_CHANGED, OnSpecialBombChanged);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.ENEMY_KILLED, OnEnemyHit);
        Messenger<int>.RemoveListener(GameEvent.BOMBS_CAPACITY_CHANGED, OnBombCapacityChanged);
        Messenger.RemoveListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.RemoveListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
    }

    // Start is called before the first frame update
    void Start()
    {
        _score=0;
        _bombsPlanted=0;
        scoreValue.text= _score.ToString();
        settingsPopup.Close();

        for (int i = 0; i < specialBombsAnimator_slots.Length; i++)
        {
            specialBomb_quantityValues[i].SetActive(i==0?true:false);
            specialBombsAnimator_slots[i].SetInteger("currentSpecialBomb",i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            settingsPopup.Open();
        }
    }

    private void OnEnemyHit(){
        _score+=1;
        scoreValue.text=_score.ToString();
    }

    private void OnBombCapacityChanged(int capacity){
        _bombsCapacity=capacity;
        bombsCapacityValue.text=_bombsCapacity.ToString();
    }

    private void OnBombPlanted(){
        _bombsPlanted++;
        bombsValue.text=_bombsPlanted.ToString();
    }

    private void OnBombsDetonated(){
        _bombsPlanted=0;
        bombsValue.text=_bombsPlanted.ToString();
    }

    public void OnOpenSettings() {
        settingsPopup.Open();
    }

    public void OnPointerDown(){
        Debug.Log("Pointer down");
    }

    public void OnSpecialBombChanged(int selectedSpecialBomb){
         //FIXME on new load scene it's null, HUD must be DontDestroyOnLoad and created one time
        for (int i = 0; i < specialBombsAnimator_slots.Length; i++)
        {
            specialBomb_quantityValues[i].SetActive(i==selectedSpecialBomb?true:false);
            specialBombsAnimator_slots[i].SetInteger("currentSpecialBomb",MathMod(selectedSpecialBomb+i,specialBombsAnimator_slots.Length));
        }
    }

    private int MathMod(int a, int b){
        return (Mathf.Abs(a * b) + a) % b;
    }
}
