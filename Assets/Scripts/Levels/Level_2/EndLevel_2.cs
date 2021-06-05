using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel_2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<PlayerCharacter>() != null){
            other.gameObject.GetComponent<BombShooter>().ResetBombsPlanted();
            SceneManager.LoadScene("Level_1");
        }
    }
}
