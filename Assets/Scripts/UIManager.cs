using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestText;

    // Update is called once per frame
    void Update()
    {
        bestText.text = $"Best: {GameManager.singleton.best}";
        scoreText.text = $"Score: {GameManager.singleton.score}";
    }
}
