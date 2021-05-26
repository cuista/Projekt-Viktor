using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalysisTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<EnemyCharacter>() != null)
        {
            Debug.Log("Paralysis: " + other.gameObject.name);
        }
    }
}
