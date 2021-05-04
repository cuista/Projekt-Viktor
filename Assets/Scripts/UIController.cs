using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text scoreValue;

    [SerializeField] private SettingsPopup settingsPopup;

    private int _score;

    void Awake() {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    // Start is called before the first frame update
    void Start()
    {
        _score=0;
        scoreValue.text= _score.ToString();
        settingsPopup.Close();
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

    public void OnOpenSettings() {
        settingsPopup.Open();
    }

    public void OnPointerDown(){
        Debug.Log("Pointer down");
    }
}
