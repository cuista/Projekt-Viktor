using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private int _counter;

    private bool isExploded;

    [SerializeField] GameObject counterLabel;

    [SerializeField] public GameObject explosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        _counter = 1;
        isExploded = false;
        counterLabel.GetComponent<TMPro.TextMeshPro>().text=_counter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isExploded)
        {
            counterLabel.transform.rotation = Quaternion.LookRotation( counterLabel.transform.position - Camera.main.transform.position );
        }
    }

    private void OnDestroy() {
       Boom();
    }

    private void Boom(){
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion,explosion.GetComponent<ParticleSystem>().main.duration);
    }

    public float GetRadius() {
        return GetComponent<Renderer>().bounds.size.x;
    }

    public void AddBombOver(){
        _counter++;
        counterLabel.GetComponent<TMPro.TextMeshPro>().text=_counter.ToString();
    }

    public int GetCounter(){
        return _counter;
    }
}
