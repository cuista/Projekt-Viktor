using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        EnemyCharacter enemy = other.gameObject.GetComponent<EnemyCharacter>();
        if(enemy != null)
        {
            SpecialEffectController.GravitonEffect(enemy, transform.position);
        }
    }
}
