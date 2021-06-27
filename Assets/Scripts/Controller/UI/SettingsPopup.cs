using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Text nameLabel;

    private bool _isGameOver = false;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip openPopupSound;

    private void Awake() {
        Messenger.AddListener(GameEvent.GAMEOVER, OnGameOver);
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.GAMEOVER, OnGameOver);
    }

    public void Open() {
        gameObject.SetActive(true);
        if(!_isGameOver)
        {
            PauseGame();
            _audioSource.PlayOneShot(openPopupSound);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Close() {
        gameObject.SetActive(false);
        if(!_isGameOver)
            UnPauseGame();
    }

    public void ExitGame() {
        LoadingScenesManager.LoadingScenes("InitialMenu");
        DontDestroyOnLoadManager.DestroyAll();
    }

    public void PauseGame() {
        GameEvent.isPaused=true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void UnPauseGame() {
        GameEvent.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void OnSubmitName(string name) {
        Debug.Log("Name:" + name);
        nameLabel.text=name;
    }

    public void OnSpeedValue(float speed) {
        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
    }

    public void OnGameOver(){
        _isGameOver = true;
    }
}
