using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public int best;
    public int score;
    public int currentStage = 0;
    public static GameManager singleton;
    private InterstitialAds interstitialAds;

    

    // Start is called before the first frame update

    void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        best = PlayerPrefs.GetInt("Highscore");
        interstitialAds = FindObjectOfType<InterstitialAds>();
    }

    public void NextLevel()
    {
        currentStage++;
        FindObjectOfType<BallController>().RestartBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
        Debug.Log("Next Level Called");
    }

    public void RestartLevel()
    {
        Debug.Log("Game Over");

        // Show adds
        interstitialAds.ShowInterstitialAd();
        
        //Reload Stage
        singleton.score = 0;
        FindObjectOfType<BallController>().RestartBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);

    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > best)
        {
            best = score;

            //score highscore/best in playerpref
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

   
}
