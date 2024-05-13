using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    [Header("Shooting")]
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    [Header("Spread")]
    public float spreadIntensity;
    public float hitSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    [Header("Loading")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;


    public GameObject muzzleEffect;
    internal Animator animator;

    bool isADS;

    public enum WeaponModel
    {
       M1911,
       AK74,
       M48
    }

    public WeaponModel thisWeaponModel;
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;
    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        bulletsLeft = magazineSize;

        spreadIntensity = hitSpreadIntensity;
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (isActiveWeapon)
        {
            if(Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }


            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyManagizeSound1911.Play();
                //AudioManager.Instance.PlaySFX("1911 Empty Magazine");
            }
            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single ||
                currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0 )
            {
                Reload();
            }

            // redload automatically
            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //Reload(); // tat di boi vi neu de lai se ko nghe thay am thanh trong bang dan

            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }

        } 
    }

    public void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }

    public void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hitSpreadIntensity;
    }
    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if(isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }
        

        // SoundManager.Instance.shootingChannel.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        // pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        // shooting the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Check if we are done shooting
        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst Mode

        if(currentShootingMode == ShootingMode.Burst && burstBulletLeft >1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        //SoundManager.Instance.reloadSound1911.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        
        if (isADS)
        {
            animator.SetTrigger("RELOAD_ADS");
        }
        else
        {
            animator.SetTrigger("RELOAD");
        }
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize )
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        isReloading = false;
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        // shooting from the middle of the screen to check where are we pointing at
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray,out hit))
        {
            // hiiting something
            targetPoint = hit.point;
        }
        else
        {
            // shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0,y,z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
