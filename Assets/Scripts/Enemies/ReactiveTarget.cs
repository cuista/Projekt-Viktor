using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{

    [SerializeField] public GameObject explosionEffect;

    public void ReactToHit(){
        WanderingAI behavior=GetComponent<WanderingAI>();
        if(behavior!=null){
            behavior.SetAlive(false);
        }
        StartCoroutine(Die());
    }

    private IEnumerator Die() {
        this.transform.Rotate(-75, 0, 0);

        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion,explosion.GetComponent<ParticleSystem>().main.duration);

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
