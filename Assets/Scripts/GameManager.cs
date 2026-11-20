using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Start,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public int best;
    public int score;
    public int currentStage = 0;
    public static GameManager singleton;
    public GameState gameState = GameState.Start;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    public AudioSource music;


    private InterstitialAds interstitialAds;

    void Update()
    {
        if (gameState == GameState.Start && Input.GetMouseButtonDown(0))
        {
            StartGame();
            FindObjectOfType<BallController>().ActivateBall();
            
            pausePanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
    }


    void Awake()
    {
        music = Camera.main.GetComponent<AudioSource>();
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        best = PlayerPrefs.GetInt("Highscore");
        // interstitialAds = FindObjectOfType<InterstitialAds>();
    }

    public void NextLevel()
    {
        BallController ballController = FindObjectOfType<BallController>();
        ballController.isSuperSpeedActive = false;
        ballController.superSpeedBreakCount = 0;
        
        VibrationManager.VibrateLight();
        HelixController helixController = FindObjectOfType<HelixController>();

        currentStage++;

        // Loop back to stage 0 if we exceed the number of available stages
        if (currentStage >= helixController.allStages.Count)
        {
            currentStage = 0;
        }

        FindObjectOfType<BallController>().RestartBall();
        helixController.LoadStage(currentStage);
        Debug.Log("Next Level Called - Current Stage: " + currentStage);
    }

    public void GameOver()
    {
        music.Stop();
        VibrationManager.VibrateHeavy();
        gameState = GameState.GameOver;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        // interstitialAds.ShowInterstitialAd();
    }

    public void RestartLevel()
    {
        music.Play();
        VibrationManager.VibrateLight();
        gameOverPanel.SetActive(false);
        //Reload Stage
        gameState = GameState.Start;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        currentStage = 0;
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

    public void StartGame()
    {
        VibrationManager.VibrateLight();
        gameState = GameState.Playing;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        music.Pause();
        VibrationManager.VibrateLight();
        gameState = GameState.Paused;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        music.UnPause();
        VibrationManager.VibrateLight();
        gameState = GameState.Playing;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void HomeScreen()
    {
        VibrationManager.VibrateLight();
        SceneManager.LoadScene("HomeScene");
    }
    public void togglePause()
    {
        if (gameState == GameState.Playing)
        {
            PauseGame();
        }
        else if (gameState == GameState.Paused)
        {
            ResumeGame();
        }
    }
}
