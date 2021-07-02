using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveBox : MonoBehaviour, ReactiveObject
{
    [SerializeField] public GameObject item;
    [SerializeField] public GameObject explosionEffect;

    //start coroutine to open box
    public void ReactToHits(int numHits){
        StartCoroutine(Open());
    }

    //open the box with explosion effect
    private IEnumerator Open() {
        ExplosionController.MakeItBoom(explosionEffect, transform);

        yield return new WaitForSeconds(0.5f);

        Instantiate(item, new Vector3(transform.position.x, transform.position.y+1, transform.position.z), transform.rotation);
        Destroy(this.gameObject);
    }

    public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius) {
    }
}
