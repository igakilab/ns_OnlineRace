using System.Collections.Generic;
using System.Linq;

public static class RankingData
{
    private static Dictionary<string, string> rankingMap = new Dictionary<string, string>();

    public static string GetRanking()
    {
        var rankingData = rankingMap.OrderBy((x) => x.Value);
        string ranking = "Ranking\n";
        foreach (var rank in rankingData)
        {
            ranking = ranking + rank.Key + " " + rank.Value + "s\n";
        }
        return ranking;
    }

    public static void SetRanking(string name, string time)
    {
        rankingMap.Add(name, time);
    }

    public static void ResetRanking()
    {
        rankingMap.Clear();
    }

}
