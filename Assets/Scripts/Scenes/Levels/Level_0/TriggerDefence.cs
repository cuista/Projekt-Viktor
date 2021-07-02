using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDefence : MonoBehaviour
{
    [SerializeField] private SceneController_0 controller;

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player")
        {
            controller.ShowDefenceTutorial();
            Destroy(this.gameObject);
        }
    }
}
