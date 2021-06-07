using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel_1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<PlayerCharacter>() != null){
            other.gameObject.GetComponent<BombShooter>().ResetBombsPlanted();
            LoadingScenesManager.LoadingScenes("Level_2");
        }
    }
}
