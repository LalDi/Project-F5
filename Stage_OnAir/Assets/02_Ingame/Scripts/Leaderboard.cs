using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public void ShowLeaderboard()
    {
        Social.ReportScore((long)(GameManager.Instance.Best_Quality*10f), GPGSIds.BestQuality, isSuccess => { });
        Social.ReportScore((long)GameManager.Instance.Best_Audience, GPGSIds.BestAudience, isSuccess => { });
        Social.ReportScore((long)GameManager.Instance.Best_Profit, GPGSIds.BestProfit, isSuccess => { });
        Social.ShowLeaderboardUI();
    }

}
