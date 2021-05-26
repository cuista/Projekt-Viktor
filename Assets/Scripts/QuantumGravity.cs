using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumGravity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<EnemyCharacter>() != null)
        {
            Debug.Log("Graviton: " + other.gameObject.name);
        }
    }
}
