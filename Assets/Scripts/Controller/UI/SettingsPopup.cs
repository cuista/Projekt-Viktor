using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Text nameLabel;

    public void Open() {
        gameObject.SetActive(true);
        PauseGame();
    }

    public void Close() {
        gameObject.SetActive(false);
        UnPauseGame();
    }

    public void ExitGame() {
        SceneManager.LoadSceneAsync("InitialMenu");
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
}
