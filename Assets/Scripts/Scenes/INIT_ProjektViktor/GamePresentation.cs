using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePresentation : MonoBehaviour
{
    public Image[] presentationImages;

    public Animator crossfade;
    private float _crossfadeTime = 1f;

    private void Start()
    {
        StartCoroutine(Presentation());
    }

    private IEnumerator Presentation()
    {
        foreach(Image image in presentationImages)
        {
            image.gameObject.SetActive(true);
            yield return new WaitForSeconds(5.7f);

            crossfade.SetTrigger("Start");
            yield return new WaitForSeconds(_crossfadeTime);

            image.gameObject.SetActive(false);
            crossfade.SetTrigger("End");
        }
        
        LoadingScenesManager.LoadingScenes("InitialMenu");
    }
}
