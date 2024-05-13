using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 20f;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke_Gernade
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;    
            if(countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch(throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke_Gernade:
                SmokeGrenadeEffect();
                break;
        }
    }

    private void SmokeGrenadeEffect()
    {
        GameObject smokeEffect = GlobalReferences.Instance.smokeGreanadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.grenadeSound);
        //AudioManager.Instance.PlaySFX("Grenade Sound");

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();

            if (rb != null)
            {
               // apply blind to enemies
            }
        }
    }

    private void GrenadeEffect()
    {
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect,transform.position,transform.rotation);

        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.grenadeSound);
        //AudioManager.Instance.PlaySFX("Grenade Sound");

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach(Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();

            if(rb != null )
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            if(objectInRange.gameObject.GetComponent<Enemy>())
            {
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(100);
            }
        }    
    }
}
