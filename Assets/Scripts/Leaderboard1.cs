using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Leaderboard1 : MonoBehaviour
{
  int MaxResultsCount = 10;
  public GameObject playerEntry;
  public Transform content;
    // Start is called before the first frame update
    void Start()
    {
      if(content == null){
        content = GameObject.Find("Content").transform;
      }
      if(playerEntry == null){
        playerEntry = Resources.Load<GameObject>("PlayerEntry");
      }
      FetchLeaderboard( );
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetLeaderBoard(){
      var request = new GetLeaderboardRequest
      {
        StatisticName = "Score",
        MaxResultsCount = MaxResultsCount
      };
      PlayFabClientAPI.GetLeaderboard(request,
      result =>
      {
        Debug.Log("Top Scores:");
        foreach (var entry in result.Leaderboard)
        {
          Debug.Log($"Rank: {entry.Position + 1}, Player:{entry.DisplayName}, Score: {entry.StatValue}");
        }
      },
      error =>
      {
          Debug.LogError($"Error getting Leaderboard {error.GenerateErrorReport()}");
      });
    }

    public void FetchLeaderboard()
   {
       var request = new GetLeaderboardRequest
       {
           StatisticName = "Score",
           StartPosition = 0,
           MaxResultsCount = MaxResultsCount
       };

       PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnLeaderboardError);
   }

   private void OnLeaderboardSuccess(GetLeaderboardResult result)
   {
       foreach (Transform child in content)
       {
           Destroy(child.gameObject);
       }
       foreach (var entry in result.Leaderboard)
       {
           GameObject newEntry = Instantiate(playerEntry, content);
           Text[] textFields = newEntry.GetComponentsInChildren<Text>();
           textFields[0].text = $"Rank: {entry.Position + 1}";
            if (entry.DisplayName == null)
            {
                textFields[1].text = $"Player: Guest";
            }
            else
            {
                textFields[1].text = $"Player: {entry.DisplayName}";
            }
           textFields[2].text = $"Score: {entry.StatValue}";
       }
   }
   private void OnLeaderboardError(PlayFabError error)
   {
       Debug.LogError($"Error fetching leaderboard: {error.GenerateErrorReport()}");
   }
   public void ReturnToMainMenu(){
     SceneManager.LoadSceneAsync(1);
   }
}
