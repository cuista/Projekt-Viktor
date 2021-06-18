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
    private Text _multiplierValue;

    private int _multiplier = 1;

    private float _comboTimer;

    public float _comboDuration = 5f;


    [SerializeField] private SettingsPopup settingsPopup;

    [SerializeField] private GameObject[] specialBomb_quantityValues;
    [SerializeField] private GameObject[] bomb_countValues;

    [SerializeField] private Animator[] specialBombsAnimator_slots;

    [SerializeField] private Sprite red_crossImage;

    [SerializeField] private Sprite bombImage;
    [SerializeField] private Sprite tetradoxImage;
    [SerializeField] private Sprite napalmImage;
    [SerializeField] private Sprite gravitonImage;

    private bool isPlayingCutscene = false;

    [SerializeField] private GameObject target;

    [SerializeField] private Text targetCount;

    [SerializeField] private Text targetTotal;


    void Awake() {
        Messenger.AddListener(GameEvent.ENEMY_KILLED, OnEnemyHit);
        Messenger<int>.AddListener(GameEvent.BOMBS_CAPACITY_CHANGED, OnBombCapacityChanged);
        Messenger<int>.AddListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.AddListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
        Messenger<int>.AddListener(GameEvent.BOMBS_DETONATED_N, OnBombsDetonatedN);
        Messenger<int>.AddListener(GameEvent.SPECIALBOMB_CHANGED, OnSpecialBombChanged);
        Messenger<int>.AddListener(GameEvent.LIQUID_COLLECTED, OnLiquidCollectedChanged);
        Messenger<int>.AddListener(GameEvent.LIQUID_CONSUMED, OnLiquidConsumedChanged);
        Messenger.AddListener(GameEvent.CUTSCENE_STARTED, OnCutsceneStarted);
        Messenger.AddListener(GameEvent.CUTSCENE_ENDED, OnCutsceneEnded);
        Messenger<int>.AddListener(GameEvent.TARGET_TOTAL, OnTargetTotal);
        Messenger.AddListener(GameEvent.TARGET_ELIMINATED, OnTargetEliminated);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.ENEMY_KILLED, OnEnemyHit);
        Messenger<int>.RemoveListener(GameEvent.BOMBS_CAPACITY_CHANGED, OnBombCapacityChanged);
        Messenger<int>.RemoveListener(GameEvent.BOMB_PLANTED, OnBombPlanted);
        Messenger.RemoveListener(GameEvent.BOMBS_DETONATED, OnBombsDetonated);
        Messenger<int>.RemoveListener(GameEvent.BOMBS_DETONATED_N, OnBombsDetonatedN);
        Messenger<int>.RemoveListener(GameEvent.SPECIALBOMB_CHANGED, OnSpecialBombChanged);
        Messenger<int>.RemoveListener(GameEvent.LIQUID_COLLECTED, OnLiquidCollectedChanged);
        Messenger<int>.RemoveListener(GameEvent.LIQUID_CONSUMED, OnLiquidConsumedChanged);
        Messenger.RemoveListener(GameEvent.CUTSCENE_STARTED, OnCutsceneStarted);
        Messenger.RemoveListener(GameEvent.CUTSCENE_ENDED, OnCutsceneEnded);
        Messenger<int>.RemoveListener(GameEvent.TARGET_TOTAL, OnTargetTotal);
        Messenger.RemoveListener(GameEvent.TARGET_ELIMINATED, OnTargetEliminated);
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
        _multiplierValue = scoreMultiplier.transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPlayingCutscene)
                Messenger.Broadcast(GameEvent.CUTSCENE_STOPPED);
            else
                settingsPopup.Open();
        }

        if(_multiplier>1)
        {
            scoreMultiplier.SetActive(true);
            if((Time.timeSinceLevelLoad - _comboTimer) > _comboDuration)
            {
                _multiplier-=1;
                if(_multiplier>1)
                {
                    _comboTimer = Time.timeSinceLevelLoad;
                    _multiplierValue.text = _multiplier.ToString();
                }
            }
        }
        else
        {
            scoreMultiplier.SetActive(false);
        }
    }

    private void OnEnemyHit(){
        _score+=10*_multiplier;
        scoreValue.text = _score.ToString("D4");

        _multiplier+=1;
        _comboTimer = Time.timeSinceLevelLoad;
        _multiplierValue.text = _multiplier.ToString();
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

    private void OnBombsDetonatedN(int numOfBombs){
        List<int> indexToAdjust = new List<int>();
        for (int i = _bombsPlanted-1; i >= 0; i--)
        {
            if(bomb_countValues[i].GetComponent<Image>().sprite == bombImage)
            {
                if(indexToAdjust.Count>=numOfBombs)
                    break;
                else
                    indexToAdjust.Add(i);
            }
        }

        foreach(int index in indexToAdjust)
        {
            for (int i = index; i < _bombsPlanted-1; i++)
            {
                bomb_countValues[i].GetComponent<Image>().sprite = bomb_countValues[i+1].GetComponent<Image>().sprite;
            }
        }

        for (int i = (_bombsPlanted-numOfBombs); i < _bombsPlanted; i++)
        {
            bomb_countValues[i].SetActive(false);
        }
        _bombsPlanted-=numOfBombs;
    }

    public void OnOpenSettings() {
        settingsPopup.Open();
    }

    public void OnPointerDown(){
        Debug.Log("Pointer down");
    }

    public void OnSpecialBombChanged(int selectedSpecialBomb){
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

    public void OnCutsceneStarted(){
        isPlayingCutscene = true;
    }

    public void OnCutsceneEnded(){
        isPlayingCutscene = false;
    }

    public void OnTargetTotal(int total){
        target.SetActive(true);
        targetTotal.text=total.ToString();
    }

    public void OnTargetEliminated(){
        targetCount.text=(int.Parse(targetCount.text)+1).ToString();
    }

    private int MathMod(int a, int b){
        return (Mathf.Abs(a * b) + a) % b;
    }
}
