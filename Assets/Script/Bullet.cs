using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit" + objectWeHit.gameObject.name);
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);

        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            print("hit wall");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            if( objectWeHit.gameObject.GetComponent<Enemy>().isDead == false )
            {
                objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }
            
            Destroy(gameObject);
            CreatBloodSprayEffect(objectWeHit);
        }
    }

    private void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

    private void CreatBloodSprayEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
