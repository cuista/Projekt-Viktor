using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFinalBoss : MonoBehaviour
{
    [SerializeField] private SceneController_2 controller;

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player")
        {
            controller.StartFinalBossCutscene();
            Destroy(this.gameObject);
        }
    }
}
