using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource shootingChannel;
    public AudioClip M1911Shot;
    public AudioClip AK74Shot;
    
    public AudioSource reloadSoundAK74;
    public AudioSource reloadSound1911;
    public AudioSource emptyManagizeSound1911;

    public AudioSource throwableChannel;
    public AudioClip grenadeSound;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;
    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void PlayShootingSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.M1911:
                shootingChannel.PlayOneShot(M1911Shot);
                //AudioManager.Instance.PlaySFX("1911 Shot");
                break;
            case WeaponModel.AK74:
                shootingChannel.PlayOneShot(AK74Shot);
                break;
            case WeaponModel.M48:
                shootingChannel.PlayOneShot(AK74Shot);
                //AudioManager.Instance.PlaySFX("AK74 Shot");
                break;

        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                reloadSound1911.Play();
                //AudioManager.Instance.PlaySFX("1911 Reload");
                break;
            case WeaponModel.AK74:
                reloadSoundAK74.Play();
                break;
            case WeaponModel.M48:
                reloadSoundAK74.Play();
                //AudioManager.Instance.PlaySFX("AK74 Reload");
                break;

        }
    }
}
