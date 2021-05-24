using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ReactiveObject
{
    void ReactToHit();

    void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius);
}
