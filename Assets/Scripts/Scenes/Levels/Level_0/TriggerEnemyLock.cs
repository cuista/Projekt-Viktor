using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemyLock : MonoBehaviour
{

    [SerializeField] private SceneController_0 controller;

    [SerializeField] private GameObject blockedArea;

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player")
        {
            controller.ShowEnemyLockTutorial(blockedArea);
            Destroy(this.gameObject);
        }
    }
}
