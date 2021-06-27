using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start() {
        DontDestroyOnLoadManager.GetAudioManager().PlayMenuClip();
    }
    public void PlayGame()
    {
        HideMenu();
        LoadingScenesManager.LoadingScenes("Gameplay", "Level_0");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HideMenu(){
        for (int i = 0; i < transform.childCount; i++)
        {  
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
