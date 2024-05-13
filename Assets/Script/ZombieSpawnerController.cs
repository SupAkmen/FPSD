using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnerController : MonoBehaviour
{
    public int initialZombiePerWave = 5;
    public int currentZombiePerWave;

    public float spawnDelay = 0.5f; // Delay btw spawning each zombie in a wave

    public int currentWave = 0;
    public float waveCooldown = 10.0f; // Time in seconds btw wave

    public bool inCooldown;
    public float cooldownCounter = 0; // we only use this for testing ang the ui

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        currentZombiePerWave = initialZombiePerWave;
        GlobalReferences.Instance.waveNumber = 0;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;
        GlobalReferences.Instance.waveNumber = currentWave;
        currentWaveUI.text ="Wave : " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for(int i = 0;i < currentZombiePerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie = Instantiate(zombiePrefab,spawnPosition,Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();
            
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        // get all dead zombie
        List<Enemy> zombiesToMove = new List<Enemy> ();
        foreach(Enemy zombie in currentZombiesAlive)
        {
            if(zombie.isDead)
            {
                zombiesToMove.Add(zombie);
            }
        }

        // actually remove all dead zombie
        foreach(Enemy zombie in zombiesToMove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombiesToMove.Clear();

        // start cooldown if all zombies are dead
        if(currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if(inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }

        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);

        currentZombiePerWave *= 2;

        StartNextWave();
    }
}
