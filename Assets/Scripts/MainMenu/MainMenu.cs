using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject loadingInterface;
    public Image loadingProgressBar;

    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void PlayGame()
    {
        HideMenu();
        ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("LoadingNewGame"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Level_0"));
        StartCoroutine(LoadingScreen());
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

    public void ShowLoadingScreen(){
        loadingInterface.gameObject.SetActive(true);
    }

    private IEnumerator LoadingScreen(){
        float totalProgress = 0;
        for(int i=0; i<scenesToLoad.Count; i++)
        {
            while(!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                loadingProgressBar.fillAmount = totalProgress/scenesToLoad.Count;
                yield return null;
            }
        }
    }
}
