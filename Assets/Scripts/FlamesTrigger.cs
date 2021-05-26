using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamesTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        EnemyCharacter enemy = other.gameObject.GetComponent<EnemyCharacter>();
        if(enemy != null)
        {
            SpecialEffectController.NapalmEffect(enemy);
        }
    }
}
