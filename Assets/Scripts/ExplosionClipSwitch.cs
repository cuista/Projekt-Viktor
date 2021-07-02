using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionClipSwitch : MonoBehaviour
{

    [SerializeField] private AudioClip[] explosionsSounds;

    //For having different kind of explosion sounds
    private void Awake() {
        GetComponent<AudioSource>().PlayOneShot(explosionsSounds[(int) Random.Range(0,explosionsSounds.Length)]);
    }
}
