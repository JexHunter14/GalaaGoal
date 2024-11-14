public void GetLeaderboard()
{
    var request = GetLeaderboardRequest
    {
        StatisticName = "Wins",
        StartPosition = 0,
        MaxResultsCount = 100 // adjust based on number of players
    };

    PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardRetrieved, OnError)
}

