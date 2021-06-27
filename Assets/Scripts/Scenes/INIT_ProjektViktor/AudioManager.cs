using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource _audioSource;

    public AudioClip intro_init;
    public AudioClip menu_soundtrack;
    public AudioClip level0_soundtrack;
    public AudioClip level1_soundtrack;
    public AudioClip level2_soundtrack;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(intro_init);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMenuClip()
    {
        _audioSource.clip = menu_soundtrack;
        _audioSource.Play();
    }

    public void PlaySoundtrackLevel_0()
    {
        _audioSource.clip = level0_soundtrack;
        _audioSource.Play();
    }

    public void PlaySoundtrackLevel_1()
    {
        _audioSource.clip = level1_soundtrack;
        _audioSource.Play();
    }

    public void PlaySoundtrackLevel_2()
    {
        _audioSource.clip = level2_soundtrack;
        _audioSource.Play();
    }

    public void StopCurrentSoundtrack(){
        _audioSource.Stop();
    }

    public bool isPlayingClip(AudioClip audio)
    {
        return _audioSource.clip==audio?true:false;
    }

    public float GetVolume(){
        return _audioSource.volume;
    }
}
