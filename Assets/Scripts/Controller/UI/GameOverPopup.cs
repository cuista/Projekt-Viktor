using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : MonoBehaviour
{
    //Restart level
    public void Retry() {
        DontDestroyOnLoadManager.DestroyAll();
        LoadingScenesManager.LoadingScenes("Gameplay", SceneManager.GetActiveScene().name);
    }

    //Back to Initial Menu
    public void Exit() {
        LoadingScenesManager.LoadingScenes("InitialMenu");
        DontDestroyOnLoadManager.DestroyAll();
    }
}
