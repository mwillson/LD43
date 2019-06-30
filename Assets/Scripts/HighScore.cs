using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public int score;

    private string gameDataFileName = "data.json";

    public static GameObject highScoreTracker;

    void Awake()
    {
        if (highScoreTracker == null)
        {
            highScoreTracker = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (highScoreTracker != gameObject)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadHighScore();
        GameObject.Find("HighScore").GetComponent<Text>().text = "High Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            LoadHighScore();
            GameObject.Find("HighScore").GetComponent<Text>().text = "High Score: " + score;
        }
    }

    public void SubmitNewScore(int newScore)
    {
        if (newScore > score)
        {
            score = newScore;
            SaveHighScore();
        }
    }

    private void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("highestScore"))
        {
            score = PlayerPrefs.GetInt("highestScore");
        }
        else
        {
            score = 0;
        }
    }

    private void SaveHighScore()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", score);
    }
}
