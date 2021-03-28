using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightTarget : MonoBehaviour
{

    private GameObject targetEnemy;
    //private 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {
        EnemyCharacter enemy=collider.GetComponent<EnemyCharacter>();
        if(enemy!=null){
            Debug.Log("enemy hit");
            targetEnemy=enemy.gameObject;
        }
    }

    public GameObject GetTargetEnemy() {
        return targetEnemy;
    }
}
