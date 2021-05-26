using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectController : MonoBehaviour
{

    private static SpecialEffectController _specialEffectController;

    [SerializeField] private GameObject lightingEffect;
    [SerializeField] private GameObject flamesEffect;
    [SerializeField] private GameObject gravitonEffect;

    void Awake()
    {
        _specialEffectController = this;
    }

    public static void TetradoxEffect(EnemyCharacter enemy){
        _specialEffectController.StartCoroutine(Paralyze(enemy));
    }

    public static void NapalmEffect(EnemyCharacter enemy) {
        _specialEffectController.StartCoroutine(SetOnFire(enemy));
    }

    public static void GravitonEffect(EnemyCharacter enemy, Vector3 graviton) {
        _specialEffectController.StartCoroutine(GravitationalAttraction(enemy, graviton));
    }

    private static IEnumerator Paralyze(EnemyCharacter enemy){
        GameObject lightingOnEnemy = Instantiate(_specialEffectController.lightingEffect, enemy.transform.position, enemy.transform.rotation);
        lightingOnEnemy.transform.parent = enemy.transform;

        enemy.SetMoving(false);

        float paralysisDuration = 5f;
        float timePassed = 0;
        while(timePassed <= paralysisDuration)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        enemy.SetMoving(true);
        Destroy(lightingOnEnemy);
    }

    private static IEnumerator SetOnFire(EnemyCharacter enemy){
        GameObject flamesOnEnemy = Instantiate(_specialEffectController.flamesEffect, enemy.transform.position, enemy.transform.rotation);
        flamesOnEnemy.transform.parent = enemy.transform;
        Destroy(flamesOnEnemy, _specialEffectController.flamesEffect.GetComponent<ParticleSystem>().main.duration/2);

        ReactiveTarget reactiveTarget = enemy.GetComponent<ReactiveTarget>();
        while(flamesOnEnemy != null){
            reactiveTarget.ReactToHits(1);
            yield return new WaitForSeconds(2f);
        }
    }

    private static IEnumerator GravitationalAttraction(EnemyCharacter enemy, Vector3 graviton){
        GameObject gravitonOnEnemy = Instantiate(_specialEffectController.gravitonEffect, enemy.transform.position, enemy.transform.rotation);
        gravitonOnEnemy.transform.parent = enemy.transform;

        enemy.AddGravitonAddiction(graviton);

        float gravitonDuration = _specialEffectController.gravitonEffect.GetComponent<ParticleSystem>().main.duration;
        float timePassed = 0;
        while(timePassed <= gravitonDuration)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        enemy.RemoveGravitonAddiction();
        Destroy(gravitonOnEnemy);
    }
}
