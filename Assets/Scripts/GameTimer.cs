using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    public GameObject endGamePanel;
    public Button mainMenuButton;
    public Text winnerText; 
    public PlayerMovement Movement;

    private float countdownTime = 60f;

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

        if (DiamondCollector.player1Score > DiamondCollector.player2Score)
            winnerText.text = "Winner: Player 1!";
        else if (DiamondCollector.player2Score > DiamondCollector.player1Score)
            winnerText.text = "Winner: Player 2!";
        else
            winnerText.text = "It's a tie!";

        DiamondCollector.player1Score = 0;
        DiamondCollector.player2Score = 0;
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
