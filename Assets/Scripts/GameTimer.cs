using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameTimer : MonoBehaviour
{
    public Text timerText; 
    public GameObject endGamePanel; 
    public Button mainMenuButton; 
    public Text finalScoreText; 
    public DiamondCollector diamondCollector;

    public PlayerStatistics playerStatistics;

    public PlayerMovement Movement;

    private float countdownTime = 3f;

    void Start()
    {
        UpdateTimerText();
        endGamePanel.SetActive(false); 
        StartCoroutine(StartCountdown());
        mainMenuButton.onClick.AddListener(ReturnToMainMenu); 
    }

    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            countdownTime--;
            UpdateTimerText();
        }

        EndGame();
    }

    void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.Ceil(countdownTime).ToString() + "s";
    }

    void EndGame()
    {
        Movement.enabled = false;

        endGamePanel.SetActive(true);
        finalScoreText.text = "Diamonds Collected: " + diamondCollector.GetScore();
        playerStatistics.UpdatePlayerStatistics(1, diamondCollector.GetScore())


    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
