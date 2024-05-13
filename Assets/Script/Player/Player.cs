using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;

    public TextMeshProUGUI playerHealth;
    public GameObject gameOverUI;

    public bool isDead;
    private void Start()
    {
        playerHealth.text = $"Health: {HP}";
        
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("Player died");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Player damaged");
            StartCoroutine(BloodyScreenEffect());
            playerHealth.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
            //AudioManager.Instance.PlaySFX("Player Hurt");
        }
    }

    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);
        //AudioManager.Instance.PlaySFX("Player Die");
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        // Dying Animation
        GetComponentInChildren<Animator>().enabled = true;
        playerHealth.gameObject.SetActive(false);

        GetComponent<ScreenBlackOut>().StartFade();

        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
        int waveSurvived = GlobalReferences.Instance.waveNumber;
        //SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);

        //if(waveSurvived-1 >SaveLoadManager.Instance.LoadHighScore())
        //{
        //    SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
        //}

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Menu Scene");
    }

    private IEnumerator BloodyScreenEffect()
    {
        if(bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // set the initial alpha value to 1 ( fully visible)
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // calculate the new alpha value using Lerp
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // update the color with  the new alpha value
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // wait for the next frame
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ZombieHand"))
        {
            Enemy zombie = other.GetComponentInParent<Enemy>();
            if(isDead == false)
            {
                int damageAmount = zombie.GetDamage();
                TakeDamage(damageAmount);
            }
           
        }
    }
}
