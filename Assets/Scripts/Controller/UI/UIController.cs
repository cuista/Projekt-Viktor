using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private int _score;
    private int _bombsCapacity;
    private int _bombsPlanted;
    [SerializeField] private Text scoreValue;
    [SerializeField] private GameObject scoreMultiplier;

    private int _multiplier = 1;

    public float _multiplierDuration = 5f;


    [SerializeField] private SettingsPopup settingsPopup;

    [SerializeField] private GameObject[] specialBomb_quantityValues;
    [SerializeField] private GameObject[] bomb_countValues;

    [SerializeField] private Animator[] specialBombsAnimator_slots;

    [SerializeField] private Sprite red_crossImage;

    [SerializeField] private Sprite bombImage;
    [SerializeField] private Sprite tetradoxImage;
    [SerializeField] private Sprite napalmImage;
    [SerializeField] private Sprite gravitonImage;


    void Awake() {
        Messenger.AddListener(GameEvent.ENEMY_KILLED, OnEnemyHit);
        Messenger<int>.AddListener(GameEvent.BOMBS_CAPACITY_CHANGED, OnBombCapacityChanged);
        Messenger<int>.AddListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.AddListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
        Messenger<int>.AddListener(GameEvent.SPECIALBOMB_CHANGED, OnSpecialBombChanged);
        Messenger<int>.AddListener(GameEvent.LIQUID_COLLECTED, OnLiquidCollectedChanged);
        Messenger<int>.AddListener(GameEvent.LIQUID_CONSUMED, OnLiquidConsumedChanged);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.ENEMY_KILLED, OnEnemyHit);
        Messenger<int>.RemoveListener(GameEvent.BOMBS_CAPACITY_CHANGED, OnBombCapacityChanged);
        Messenger<int>.RemoveListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.RemoveListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
        Messenger<int>.RemoveListener(GameEvent.SPECIALBOMB_CHANGED, OnSpecialBombChanged);
        Messenger<int>.RemoveListener(GameEvent.LIQUID_COLLECTED, OnLiquidCollectedChanged);
        Messenger<int>.RemoveListener(GameEvent.LIQUID_CONSUMED, OnLiquidConsumedChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        _score=0;
        _bombsCapacity=0;
        _bombsPlanted=0;
        scoreValue.text= _score.ToString();
        settingsPopup.Close();

        scoreValue.text=_score.ToString("D4");

        for (int i = 0; i < specialBombsAnimator_slots.Length; i++)
        {
            specialBomb_quantityValues[i].SetActive(i==0?true:false);
            specialBombsAnimator_slots[i].SetInteger("currentSpecialBomb",i);
        }

        scoreMultiplier.SetActive(false);
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
        _score+=10*_multiplier;
        if(_multiplier>1)
            _multiplier+=1;
        else
            StartCoroutine(ScoreMultiplier());
        scoreValue.text=_score.ToString("D4");
    }

    private void OnBombCapacityChanged(int capacity){
        int oldCapacity = _bombsCapacity;
        _bombsCapacity=capacity;

        for (int i = 0; i < bomb_countValues.Length; i++)
        {
            if(i < _bombsCapacity && i >= _bombsCapacity - (_bombsCapacity - oldCapacity))
            {
                bomb_countValues[i].SetActive(false);
            }
            else if(i>=_bombsCapacity)
            {
                bomb_countValues[i].SetActive(true);
                bomb_countValues[i].GetComponent<Image>().sprite = red_crossImage;
            }
        }
    }

    private void OnBombPlanted(int type){
        bomb_countValues[_bombsPlanted].SetActive(true);
        switch(type){
            case 0: bomb_countValues[_bombsPlanted].GetComponent<Image>().sprite = tetradoxImage;break;
            case 1: bomb_countValues[_bombsPlanted].GetComponent<Image>().sprite = napalmImage;break;
            case 2: bomb_countValues[_bombsPlanted].GetComponent<Image>().sprite = gravitonImage;break;
            default: bomb_countValues[_bombsPlanted].GetComponent<Image>().sprite = bombImage;break;
        }
        _bombsPlanted++;
    }

    private void OnBombsDetonated(){
        _bombsPlanted=0;
        for (int i = 0; i < _bombsCapacity; i++)
        {
            bomb_countValues[i].SetActive(false);
        }
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

    public void OnLiquidCollectedChanged(int liquidType){
        Text textComponent = specialBomb_quantityValues[liquidType].GetComponent<Text>();
        int currentQuantity = int.Parse(textComponent.text);
        textComponent.text = (currentQuantity+1).ToString();
    }

    public void OnLiquidConsumedChanged(int liquidType){
        Text textComponent = specialBomb_quantityValues[liquidType].GetComponent<Text>();
        int currentQuantity = int.Parse(textComponent.text);
        textComponent.text = (currentQuantity-1).ToString();
    }

    private IEnumerator ScoreMultiplier(){

        _multiplier+=1;
        Text multiplierValue = scoreMultiplier.transform.GetChild(1).GetComponent<Text>();
        multiplierValue.text = _multiplier.ToString();
        scoreMultiplier.SetActive(true);

        while(_multiplier>1)
        {
            float time = 0;
            int currentMultiplier = _multiplier;
            while(time <= _multiplierDuration)
            {
                time += Time.deltaTime / _multiplierDuration;
                if(_multiplier>currentMultiplier)
                {
                    time=0;
                    multiplierValue.text = _multiplier.ToString();
                }
                yield return null;
            }
            _multiplier-=1;
        }

        scoreMultiplier.SetActive(false);
    }

    private int MathMod(int a, int b){
        return (Mathf.Abs(a * b) + a) % b;
    }
}
