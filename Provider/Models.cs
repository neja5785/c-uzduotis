using System.Collections.Generic;

namespace League.Provider
{
    public class AccountIds
    {
        public string accountId { get; set; }
        public string id { get; set; }
        public int summonerLevel { get; set; }
    }

    public class RankInfo
    {
        public RankInfo()
        {
            Tier = "";
            Rank = "Unranked";

        }

        public string Tier { get; set; } = null;
        public string Rank { get; set; } = null;

    }

    public class GameId
    {

        public long gameId { get; set; }
    }
    public class GameIdsList
    {
        public List<GameId> matches { get; set; }
    }
    public class Player
    {
        public string accountId { get; set; }
    }

    public class ParticipantIdentity
    {
        public int participantId { get; set; }
        public Player player { get; set; }
    }

    public class MatchInfoWithAllParticipants
    {
        public List<Team> teams { get; set; }

        public List<Participant> participants { get; set; }
        public List<ParticipantIdentity> participantIdentities { get; set; }
    }
    public class Team
    {
        public int teamId { get; set; }
        public string win { get; set; }
    }
    public class Participant
    {
        public int participantId { get; set; }
        public int teamId { get; set; }
        public int championId { get; set; }
        public Stats stats { get; set; }
    }


    public class AllChampions
    {

        public Dictionary<string, Champion> data { get; set; }
    }
    public class Champion
    {

        public int key { get; set; }
        public string name { get; set; }
    }
    public class SpecificSummonerStats
    {
        public string winOrLoss { get; set; }
        public string champName { get; set; }

        public Stats stats;
/*        public int kills { get; set; }
        public int assists { get; set; }
        public int deaths { get; set; }

        public int totalMinionsKilled { get; set; }*/
    }
    public class Stats
    {
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int totalMinionsKilled { get; set; }
    }

    public class MasteryChamps
    {
        public int championId { get; set; }
        public int championLevel { get; set; }
        public int championPoints { get; set; }
    }
    public class SummonerAverages
    {
        public float killsAverage { get; set; }
        public float assistsAverage { get; set; }
        public float deathsAverage { get; set; }
    }

}