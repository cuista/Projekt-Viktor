using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BombShooter : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    public float bombsRadius=15f;
    private int _bombsCapacity;
    private int _maxCapacity=8;
    private int _bombsPlantedCount=0;
    private List<GameObject> _bombsPlanted;

    [SerializeField] private GameObject[] specialBombPrefabs;
    private int _currentSpecialBomb;

    [SerializeField] public GameObject sight;
    [SerializeField] public GameObject sightOnTop;

    [SerializeField] public GameObject shield;
    [SerializeField] public Slider shieldSlider;
    private MeshRenderer _shieldMeshRenderer;
    private GameObject _shieldEffect;
    public bool hasShield = false;
    public float shieldDuration = 5f;
    private bool _canUseShield = true;
    public float shieldRecoveryTime = 15f;

    //rapid click bomb on the ground, holding click to attach it
    private const float _minimumHeldDuration = 0.17f;
    private float _bombButtonPressedTime = 0;
    private bool _bombButtonHeld = false;

    private AudioSource _audioSource;

    [SerializeField] private AudioClip plantBombSound;
    [SerializeField] private AudioClip swampSpecialBombSound;
    [SerializeField] private AudioClip shieldSound;
    [SerializeField] private AudioClip rechargeShieldSound;
    [SerializeField] private AudioClip shieldReadySound;

    void Awake() {
        Messenger<Bomb>.AddListener(GameEvent.BOMBS_DETONATED_BECAUSE_ENEMY_DEATH, OnBombsDetonateBecauseEnemyDeath);
    }

    void OnDestroy() {
        Messenger<Bomb>.RemoveListener(GameEvent.BOMBS_DETONATED_BECAUSE_ENEMY_DEATH, OnBombsDetonateBecauseEnemyDeath);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _bombsCapacity=2;
        _bombsPlanted=new List<GameObject>(_bombsCapacity);
        Messenger<int>.Broadcast(GameEvent.BOMBS_CAPACITY_CHANGED, _bombsCapacity);

        //sight.gameObject.SetActive(false); //--> I can't disable it because the line won't work
        sightOnTop.GetComponent<MeshRenderer>().enabled=false;
        sight.GetComponent<MeshCollider>().enabled=false;

        _currentSpecialBomb = 0;

        _shieldMeshRenderer = shield.GetComponent<MeshRenderer>();
        _shieldMeshRenderer.enabled = false;
        _shieldEffect = shield.transform.GetChild(0).gameObject;
        _shieldEffect.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameEvent.isPaused) {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                // Keep track if the button will be released or holded
                _bombButtonPressedTime = Time.timeSinceLevelLoad;
                _bombButtonHeld = false;
            } else if(Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
                // Player has released the button without holding it
                if(!_bombButtonHeld && sight.GetComponent<SightTarget>().GetTargetEnemy()==null)
                    {
                    Vector3 point = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Ray ray = new Ray(transform.position,-transform.up);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo)) { //out è un passaggio per riferimento
                        if(_bombsPlantedCount < _bombsCapacity)
                        {
                            if(!IncrementIfOverlappingBomb(hitInfo.point) && !GetComponent<RelativeMovement>().isJumping()) {
                                //GameObject bomb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                GameObject bomb = Instantiate(bombPrefab) as GameObject;
                                bomb.transform.position = hitInfo.point;
                                bomb.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                                // Removing collider
                                Collider bombCollider = bomb.GetComponent<Collider>();
                                DestroyImmediate(bombCollider);

                                _bombsPlanted.Add(bomb);
                                _bombsPlantedCount++;
                                Messenger<int>.Broadcast(GameEvent.BOMB_PLANTED,-1);
                                _audioSource.PlayOneShot(plantBombSound);
                            }
                        }
                    }
                } else if (!EventSystem.current.IsPointerOverGameObject()) { // Player has holded and released the button
                    //sight.gameObject.SetActive(false); //--> I can't disable it because the line won't work
                    sightOnTop.GetComponent<MeshRenderer>().enabled=false;
                    sight.GetComponent<MeshCollider>().enabled=false;
                    GameObject targetEnemy = sight.GetComponent<SightTarget>().GetTargetEnemy();
                    if(targetEnemy!=null && _bombsPlantedCount < _bombsCapacity) {
                        if(!IncrementIfOverlappingBomb(targetEnemy.transform.position)) {
                            //GameObject bomb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            GameObject bomb = Instantiate(bombPrefab) as GameObject;
                            bomb.transform.position = targetEnemy.transform.position;
                            bomb.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                            // Removing collider
                            Collider bombCollider = bomb.GetComponent<Collider>();
                            DestroyImmediate(bombCollider);

                            //parenting bomb to targetEnemy
                            bomb.transform.parent=targetEnemy.transform;

                            _bombsPlanted.Add(bomb);
                            _bombsPlantedCount++;
                            Messenger<int>.Broadcast(GameEvent.BOMB_PLANTED, -1);
                            _audioSource.PlayOneShot(plantBombSound);
                        }
                    }
                }
            } else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
                // Player has held the button for more than _minimumHeldDuration, consider it "held"
                if (Time.timeSinceLevelLoad - _bombButtonPressedTime > _minimumHeldDuration) {
                    //sight.gameObject.SetActive(true); //--> I can't disable it because the line won't work
                    sightOnTop.GetComponent<MeshRenderer>().enabled=true;
                    sight.GetComponent<MeshCollider>().enabled=true;
                    _bombButtonHeld = true;
                }
            } else if (Input.GetMouseButtonDown(1)) {
                foreach(GameObject bomb in _bombsPlanted)
                {
                    Vector3 point = new Vector3(bomb.transform.position.x, bomb.transform.position.y, bomb.transform.position.z);
                    Collider[] hitColliders = Physics.OverlapSphere(point, bombsRadius);
                    foreach (var hitCollider in hitColliders) { //out is a passage by reference

                        GameObject hitObject = hitCollider.transform.gameObject;
                        ReactiveObject target = hitObject.GetComponent<ReactiveObject>();
                        if(target != null){
                            if(bomb.tag != "SpecialBomb")
                            {
                                target.ReactToHits(bomb.GetComponent<Bomb>().GetCounter());
                                //adding explosion force
                                target.AddExplosionForce(1000f, bomb.transform.position, bombsRadius);
                            }
                        }
                        else // player get hurt too
                        {
                            PlayerCharacter playerCharacter = hitObject.GetComponent<PlayerCharacter>();
                            if(playerCharacter != null)
                                playerCharacter.Hurt(1);
                        }
                    }

                    Bomb bombToDetonate = bomb.GetComponent<Bomb>();
                    _bombsPlantedCount-=bombToDetonate.GetCounter();
                    bombToDetonate.Detonate();
                }
                _bombsPlanted.Clear();
                Messenger.Broadcast(GameEvent.BOMBS_DETONATED);
            } else if (Input.GetKeyUp(KeyCode.Mouse2)){ //adding special bomb
                Vector3 point = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Ray ray = new Ray(transform.position,-transform.up);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo)) { //out is a passage by reference
                    if(_bombsPlantedCount < _bombsCapacity)
                    {
                        if(!IncrementIfOverlappingBomb(hitInfo.point) && !GetComponent<RelativeMovement>().isJumping()) {
                            if(Managers.Inventory.GetItemCount("Liquid " + _currentSpecialBomb)>0)
                            {
                                GameObject bomb = Instantiate(specialBombPrefabs[_currentSpecialBomb]) as GameObject;
                                bomb.transform.position = hitInfo.point;
                                bomb.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                                // Removing collider
                                Collider bombCollider = bomb.GetComponent<Collider>();
                                DestroyImmediate(bombCollider);

                                _bombsPlanted.Add(bomb);
                                _bombsPlantedCount++;
                                Messenger<int>.Broadcast(GameEvent.BOMB_PLANTED, _currentSpecialBomb);
                                Managers.Inventory.ConsumeSpecialBomb(_currentSpecialBomb);
                                _audioSource.PlayOneShot(plantBombSound);
                            }
                        }
                    }
                }
            }

            // Change special bomb type
            if(Input.GetKeyUp(KeyCode.Q))
            {
                _currentSpecialBomb=MathMod(_currentSpecialBomb-1,specialBombPrefabs.Length);
                Messenger<int>.Broadcast(GameEvent.SPECIALBOMB_CHANGED,_currentSpecialBomb);
                _audioSource.PlayOneShot(swampSpecialBombSound);
            }
            else if(Input.GetKeyUp(KeyCode.E))
            {
                _currentSpecialBomb=MathMod(_currentSpecialBomb+1,specialBombPrefabs.Length);
                Messenger<int>.Broadcast(GameEvent.SPECIALBOMB_CHANGED,_currentSpecialBomb);
                _audioSource.PlayOneShot(swampSpecialBombSound);
            }

            //activate shield
            if(Input.GetKeyUp(KeyCode.F) && _canUseShield)
            {
                StartCoroutine(UseShield());
            }

            if(Managers.Inventory.GetItemCount("E-Chip") != 0)
            {
                _bombsCapacity = (_bombsCapacity+1<=_maxCapacity)?_bombsCapacity+1:_maxCapacity;
                Managers.Inventory.ConsumeItem("E-Chip");
                Messenger<int>.Broadcast(GameEvent.BOMBS_CAPACITY_CHANGED, _bombsCapacity);
            }
        } else {
            _bombButtonHeld=true;
        }

    }

    //if the new bomb is closer to another, it increase the counter of that bomb
    private bool IncrementIfOverlappingBomb(Vector3 point){
        foreach(GameObject bombPlanted in _bombsPlanted) { 
            Bomb bomb = bombPlanted.GetComponent<Bomb>();
            if(Vector3.Distance(bombPlanted.transform.position, point) < bomb.GetRadius()){
                bomb.AddBombOver();
                _bombsPlantedCount++;
                Messenger<int>.Broadcast(GameEvent.BOMB_PLANTED, -1);
                _audioSource.PlayOneShot(plantBombSound);
                return true;
            }
        }
        return false;
    }

    public void ResetBombsPlanted(){
        _bombsPlantedCount = 0;
        _bombsPlanted.Clear();
        Messenger.Broadcast(GameEvent.BOMBS_DETONATED);
    }

    public void OnBombsDetonateBecauseEnemyDeath(Bomb bomb){
        _bombsPlantedCount-=bomb.GetCounter();
        Messenger<int>.Broadcast(GameEvent.BOMBS_DETONATED_N,bomb.GetCounter());
        _bombsPlanted.Remove(bomb.gameObject);
    }

    private IEnumerator UseShield()
    {
        _shieldMeshRenderer.enabled = true;
        _shieldEffect.SetActive(true);

        AudioSource shieldAudioSource = shield.GetComponent<AudioSource>();
        shieldAudioSource.clip=shieldSound;
        shieldAudioSource.Play();

        float totalTime = 0;
        hasShield = true;
        _canUseShield = false;
        while(totalTime <= shieldDuration)
        {
            shieldSlider.value = 1 - (totalTime / shieldDuration);
            totalTime += Time.deltaTime;
            yield return null;
        }
        hasShield = false;

        _shieldMeshRenderer.enabled = false;
        _shieldEffect.SetActive(false);
        shieldAudioSource.Stop();

        // Recharge shield
        shieldAudioSource.clip=rechargeShieldSound;
        shieldAudioSource.Play();
        totalTime = 0;
        while(totalTime <= shieldRecoveryTime)
        {
            shieldSlider.value = totalTime / shieldRecoveryTime;
            totalTime += Time.deltaTime;
            yield return null;
        }
        _canUseShield = true;

        shieldAudioSource.Stop();
        shieldAudioSource.PlayOneShot(shieldReadySound);
    }

    private int MathMod(int a, int b){
        return (Mathf.Abs(a * b) + a) % b;
    }

}
