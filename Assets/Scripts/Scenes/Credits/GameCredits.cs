using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCredits : MonoBehaviour
{

    [SerializeField] private Text _finalScoreValue;

    private void Awake() {
        Messenger.AddListener(GameEvent.CREDITS_STOPPED, OnCreditsStopped);
        Messenger.AddListener(GameEvent.CREDITS_ENDED, OnCreditsEnded);

        _finalScoreValue.text = DontDestroyOnLoadManager.GetController().GetComponent<UIController>().GetScore().ToString("D4");

        Cursor.lockState = CursorLockMode.Locked;
        DontDestroyOnLoadManager.GetPlayer().SetActive(false);
        DontDestroyOnLoadManager.GetMainCamera().SetActive(false);
        DontDestroyOnLoadManager.GetHUD().SetActive(false);
        DontDestroyOnLoadManager.GetSkipMessage().SetActive(true);
        DontDestroyOnLoadManager.GetAudioManager().StopCurrentSoundtrack();
        StartCoroutine(StartSoundtrackWithDelay());
        Messenger.Broadcast(GameEvent.CREDITS_STARTED);
    }

    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.CREDITS_STOPPED, OnCreditsStopped);
        Messenger.RemoveListener(GameEvent.CREDITS_ENDED, OnCreditsEnded);
    }

    private void Update() {
        
    }

    private void OnCreditsStopped()
    {
        LoadingScenesManager.LoadingScenes("InitialMenu");
        DontDestroyOnLoadManager.DestroyAll();
    }

    private void OnCreditsEnded()
    {
        OnCreditsStopped();
    }

    private IEnumerator StartSoundtrackWithDelay()
    {
        yield return new WaitForSeconds(6f);
        DontDestroyOnLoadManager.GetAudioManager().PlaySoundtrackCredits();

        yield return new WaitForSeconds(DontDestroyOnLoadManager.GetAudioManager().GetCreditsClipLength()-1);
        OnCreditsStopped();
    }
}
