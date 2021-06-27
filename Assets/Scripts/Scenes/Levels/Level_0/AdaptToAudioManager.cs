using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptToAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = DontDestroyOnLoadManager.GetAudioManager().GetVolume();
    }
}
