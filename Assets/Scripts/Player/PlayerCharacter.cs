using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{

    private int health;
    private int healthPackValueA;
    private int healthPackValueB;
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
        health = Managers.Player.health;
        healthBar.maxValue = Managers.Player.maxHealth;
        healthPackValueA = Managers.Player.healthPackValueA;
        healthPackValueB = Managers.Player.healthPackValueB;
        barValueDamage = Managers.Player.barValueDamage;
        healthBarBackground = healthBar.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Managers.Inventory.GetItemCount("Heal Ampule-A") != 0)
        {
            health += healthPackValueA;
            healthBar.value += (barValueDamage * healthPackValueA);

            if(health > Managers.Player.health) {
                health = Managers.Player.health;
                healthBar.value = healthBar.maxValue;
            }

            Managers.Inventory.ConsumeItem("Heal Ampule-A");
        }
        else if(Managers.Inventory.GetItemCount("Heal Ampule-B") != 0)
        {
            health += healthPackValueB;
            healthBar.value += (barValueDamage * healthPackValueB);

            if(health > Managers.Player.health) {
                health = Managers.Player.health;
                healthBar.value = healthBar.maxValue;
            }

            Managers.Inventory.ConsumeItem("Heal Ampule-B");
        }

        if(health <= 0){
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
        health-=damage;
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
