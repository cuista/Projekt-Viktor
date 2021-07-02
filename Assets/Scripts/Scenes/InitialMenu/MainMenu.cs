using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private AudioManager audioManager;

    [SerializeField] private AudioClip clickSound;

    [SerializeField] private GameObject gameModeMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject videoMenu;

    private void Awake() {
        audioManager = DontDestroyOnLoadManager.GetAudioManager();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DontDestroyOnLoadManager.GetAudioManager().PlaySoundtrackMenuClip();

        gameModeMenu.SetActive(false);
        audioMenu.SetActive(false);
        videoMenu.SetActive(false);
    }
    public void PlayGame()
    {
        audioManager.PlaySound(clickSound);
        HideMenu();
        LoadingScenesManager.LoadingScenes("Gameplay", "Level_0");
    }

    public void PlayVR_Mode()
    {
        audioManager.PlaySound(clickSound);
        HideMenu();
        LoadingScenesManager.LoadingScenes("Gameplay", "VR_Mode");
    }

    public void QuitGame()
    {
        audioManager.PlaySound(clickSound);
        Application.Quit();
    }

    public void HideMenu(){
        for (int i = 0; i < transform.childCount; i++)
        {  
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnSoundToggle() {
        audioManager.soundMute = !audioManager.soundMute;
        audioManager.PlaySound(clickSound);
    }

    public void OnSoundValue(float volume) {
        audioManager.soundVolume = volume;
    }

    public void OnMusicToggle() {
        audioManager.musicMute = !audioManager.musicMute;
        audioManager.PlaySound(clickSound);
    }

    public void OnMusicValue(float volume) {
        audioManager.musicVolume = volume;
    }

    public void SetActiveGameModeMenu(bool active){
        audioManager.PlaySound(clickSound);
        gameModeMenu.SetActive(active);
    }

    public void SetActiveAudioMenu(bool active){
        audioManager.PlaySound(clickSound);
        audioMenu.SetActive(active);
    }

    public void SetActiveVideoMenu(bool active){
        audioManager.PlaySound(clickSound);
        videoMenu.SetActive(active);
    }

    public void OnFullScreenToggle()
    {
        audioManager.PlaySound(clickSound);
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetScreenSize(int width)
    {
        audioManager.PlaySound(clickSound);
        bool fullscreen = Screen.fullScreen;
        switch(width)
        {
            case 1920:Screen.SetResolution(width,1080,fullscreen);break;
            case 1280:Screen.SetResolution(width,720,fullscreen);break;
            case 1024:Screen.SetResolution(width,576,fullscreen);break;
            default:Screen.SetResolution(1920,1080,fullscreen);break;
        }
    }

    public void ClickedButton()
    {
        audioManager.PlaySound(clickSound);
    }
}
