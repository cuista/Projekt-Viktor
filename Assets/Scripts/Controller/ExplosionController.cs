using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public void MakeItBoom(GameObject explosionEffect, Transform bombTransform){
        GameObject explosion = Instantiate(explosionEffect, bombTransform.position, bombTransform.rotation);
        Destroy(explosion,explosion.GetComponent<ParticleSystem>().main.duration);
    }
}
