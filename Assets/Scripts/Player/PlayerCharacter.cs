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
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Image damageImage;
    private Color flashColor = new Color(1f,0f,0f,0.7f);
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

        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameEvent.isPaused)
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
    }

    public void Hurt(int damage){
        if(!GetComponent<BombShooter>().hasShield)
        {
            damaged=true;
            health-=damage;
            healthBar.value -= barValueDamage;
        }
    }

    public void Death(){
        fillImg.enabled=false;
        gameOver.SetActive(true);
        gameOver.transform.GetChild(1).gameObject.SetActive(false);
        healthBarBackground.color=Color.red;
        Messenger.Broadcast(GameEvent.GAMEOVER);

        GameEvent.isPaused = true;
        StartCoroutine(Die());
    }

    private IEnumerator Die() {
        GetComponent<Animator>().SetBool("Dying",true);

        Image gameOverLogo = gameOver.GetComponentInChildren<Image>();
        Vector3 finalPosition = gameOverLogo.transform.position;
        Color color = gameOverLogo.color;
        color.a = 0;
        gameOverLogo.color = color;

        float duration = 5f;
        float totalTime = 0;
        while(totalTime <= duration)
        {
            color.a = totalTime / duration;
            gameOverLogo.transform.position = new Vector3(finalPosition.x,finalPosition.y - (1-(totalTime / duration))*500,finalPosition.z);
            gameOverLogo.color = color;
            totalTime += Time.deltaTime;
            yield return null;
        }

        gameOver.transform.GetChild(1).gameObject.SetActive(true);

        Cursor.visible=true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0; // stop everything (PAUSE)
    }
}
