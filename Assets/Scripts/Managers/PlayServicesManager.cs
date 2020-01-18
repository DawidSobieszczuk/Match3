using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class PlayServicesManager : Manager<PlayServicesManager>
{
    public bool IsSignIn { get; private set; } = false;

    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);

        if (Debug.isDebugBuild)
        {
            PlayGamesPlatform.DebugLogEnabled = true;
        }

        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) => 
        {
            IsSignIn = success;
        });
    }

    public void PostToLeaderboard(long score, float gameTime)
    {
        string GPGSId = "";
        switch(gameTime)
        {
            case 60:
                GPGSId = GPGSIds.leaderboard_high_score_1_min;
                break;
            case 60*2:
                GPGSId = GPGSIds.leaderboard_high_score_2_min;
                break;
            case 60*5:
                GPGSId = GPGSIds.leaderboard_high_score_5_min;
                break;
            case 60*10:
                GPGSId = GPGSIds.leaderboard_high_score_10_min;
                break;
        }

        if (!string.IsNullOrEmpty(GPGSId))
        {
            Social.ReportScore(score, GPGSId, (bool success) =>
            {
            // TODO
        });
        }
    }

    public void ShowLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
}
