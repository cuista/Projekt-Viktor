using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulFlames : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<EnemyCharacter>() != null)
        {
            Debug.Log("Set on Fire: " + other.gameObject.name);
        }
    }
}
