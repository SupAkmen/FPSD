using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    private string highScorekey = "BestWaveSavedValue";
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

        DontDestroyOnLoad(this);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScorekey, score);
    }

    public  int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScorekey))
        {
            return PlayerPrefs.GetInt(highScorekey);
        }
        else
        {
            return 0;
        }
    }
}
