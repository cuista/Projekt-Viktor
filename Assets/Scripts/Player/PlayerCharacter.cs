using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{

    private int _health;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image fillImg;
    [SerializeField] private Text gameOver;
    [SerializeField] private Image damageImage;
    private Color flashColor = new Color(1f,0f,0f,0.1f);
    private float flashSpeed = 5f;
    private float barValueDamage;
    private Image healthBarBackground;
    private bool damaged;

    // Start is called before the first frame update
    void Start()
    {
        _health=5;
        barValueDamage = healthBar.maxValue / _health;
        healthBarBackground = healthBar.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_health <= 0){
            Death();
        }
        if(damaged) {
            damageImage.color = flashColor;
        } else {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged=false;
    }

    public void Hurt(int damage){
        damaged=true;
        _health-=damage;
        healthBar.value -= barValueDamage;
    }

    public void Death(){
        fillImg.enabled=false;
        gameOver.enabled=true;
        healthBarBackground.color=Color.red;

        Cursor.visible=true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0; // stop everything (PAUSE)
        GameEvent.isPaused = true;
    }
}
