using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenManager : MonoBehaviour
{
    public string gameSceneName = "MainGame";
    public GameObject inquiryPanel;
    public GameObject inquiryButton;

    public void PlayGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowInquiryPanel()
    {
        inquiryPanel.SetActive(true);
        inquiryButton.SetActive(false);
    }

    public void HideInquiryPanel()
    {
        inquiryPanel.SetActive(false);
        inquiryButton.SetActive(true);
    }

}

