using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShooter : MonoBehaviour
{
    private int _score=0;
    private Camera _camera;

    [SerializeField] private GameObject bombPrefab;
    public float bombsRadius=5f;
    public int bombsCount=7;
    private int _bombsPlantedCount=0;
    private GameObject[] _bombsPlanted;

    [SerializeField] public GameObject sight;

    //rapid click bomb on the ground, holding click to attach it
    private const float _minimumHeldDuration = 0.17f;
    private float _spacePressedTime = 0;
    private bool _spaceHeld = false;

    private void OnGUI() {
        int sizeX = _camera.pixelWidth;
        int sizeY = _camera.pixelHeight;
        float posX = _camera.pixelWidth/25;
        float posY = _camera.pixelHeight/25;
        GUI.Label(new Rect(posX, posY, sizeX, sizeY),"VIKTOR SCORE:" + _score);
        GUI.Label(new Rect(posX, posY + 20, sizeX, sizeY),"VIKTOR LIFE:" + GetComponent<PlayerCharacter>().Life());
        GUI.Label(new Rect(posX, posY + 40, sizeX, sizeY),"BOMBS AVAILABLE:" + (bombsCount-_bombsPlantedCount));
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _camera = GetComponentInChildren<Camera>(); //Prima controlla il chiamante e dopo inizia a controllare i figli (restituisce il primo che trova)

        _bombsPlanted=new GameObject[bombsCount];

        //sight.gameObject.SetActive(false); //--> I can't disable it because the line won't work
        sight.GetComponent<MeshRenderer>().enabled=false;
        sight.GetComponent<MeshCollider>().enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // Keep track if the button will be released or holded
            _spacePressedTime = Time.timeSinceLevelLoad;
            _spaceHeld = false;
        } else if(Input.GetMouseButtonUp(0)) {
            // Player has released the button without holding it
            if(!_spaceHeld && sight.GetComponent<SightTarget>().GetTargetEnemy()==null)
                {
                Vector3 point = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Ray ray = new Ray(transform.position,-transform.up);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo)) { //out è un passaggio per riferimento
                    //Debug.Log("Hit " + hit.point); //for DEBUG print of what hitted

                    if(_bombsPlantedCount < bombsCount)
                    {
                        //GameObject bomb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        GameObject bomb = Instantiate(bombPrefab) as GameObject;
                        bomb.transform.position = hitInfo.point;
                        bomb.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        // Removing collider
                        Collider bombCollider = bomb.GetComponent<Collider>();
                        DestroyImmediate(bombCollider);

                        _bombsPlanted[_bombsPlantedCount]=bomb;
                        _bombsPlantedCount++;
                    }
                }
            } else { // Player has holded and released the button
                //sight.gameObject.SetActive(false); //--> I can't disable it because the line won't work
                sight.GetComponent<MeshRenderer>().enabled=false;
                sight.GetComponent<MeshCollider>().enabled=false;
                GameObject targetEnemy = sight.GetComponent<SightTarget>().GetTargetEnemy();
                if(targetEnemy!=null && _bombsPlantedCount < bombsCount) {
                    //GameObject bomb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    GameObject bomb = Instantiate(bombPrefab) as GameObject;
                    bomb.transform.position = targetEnemy.transform.position;
                    bomb.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    // Removing collider
                    Collider bombCollider = bomb.GetComponent<Collider>();
                    DestroyImmediate(bombCollider);

                    //parenting bomb to targetEnemy
                    bomb.transform.parent=targetEnemy.transform;

                    _bombsPlanted[_bombsPlantedCount]=bomb;
                    _bombsPlantedCount++;
                }
            }
        } else if (Input.GetMouseButton(0)) {
            // Player has held the button for more than _minimumHeldDuration, consider it "held"
            if (Time.timeSinceLevelLoad - _spacePressedTime > _minimumHeldDuration) {
                //Debug.Log("Button helded");
                //sight.gameObject.SetActive(true); //--> I can't disable it because the line won't work
                sight.GetComponent<MeshRenderer>().enabled=true;
                sight.GetComponent<MeshCollider>().enabled=true;
                _spaceHeld = true;
            }
        } else if (Input.GetMouseButtonDown(1)) {
            int bombPlanted_N=_bombsPlantedCount;
            for(int i = 0; i<bombPlanted_N;i++)
            {
                Vector3 point = new Vector3(_bombsPlanted[i].transform.position.x, _bombsPlanted[i].transform.position.y, _bombsPlanted[i].transform.position.z);
                Collider[] hitColliders = Physics.OverlapSphere(point, bombsRadius);
                foreach (var hitCollider in hitColliders) { //out è un passaggio per riferimento
                    //Debug.Log("Hit " + hit.point); //for DEBUG print of what hitted

                    GameObject hitObject = hitCollider.transform.gameObject;
                    ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
                    if(target != null){
                        Debug.Log("Target hit");
                        target.ReactToHit();
                        _score+=1;
                    }
                }

                Destroy(_bombsPlanted[i]);
                _bombsPlantedCount--;
            }
        }
    }
}
