using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace League.Provider
{


    public class Provider
    {

        public List<SpecificSummonerStats> matchHistory = new List<SpecificSummonerStats>();
        string apiKey = "?&api_key=RGAPI-b7228e3a-c2b2-4377-8f08-101c5283ba51";
        public bool userExists = true;
        public AllChampions allChampions;
        public List<MasteryChamps> masteryChamps;
        public int totalKills = 0;
        public int totalAssits = 0;
        public int totalDeaths = 0;


        public AccountIds GetIds(string summonerName)
        {

            try
            {
                var clientBaseAdress = "https://euw1.api.riotgames.com/tft/summoner/v1/summoners/by-name/";
                var accountIds = JsonConvert.DeserializeObject<AccountIds>(JsonStringReturner(summonerName, clientBaseAdress));
                return accountIds;
            }
            catch (AggregateException ex)
            {
                Console.Write(ex);
                userExists = false;
                return null;
            }

        }
        public string JsonStringReturner(string parameter, string clientBaseAdress)
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(clientBaseAdress);
                string s = client.GetStringAsync(parameter + apiKey).Result;
                return s;

            }
        }

        public RankInfo GetRank(string id)
        {

            var clientBaseAdress = "https://euw1.api.riotgames.com/lol/league/v4/entries/by-summoner/";
            var summonerRankInfo = JsonConvert.DeserializeObject<List<RankInfo>>(JsonStringReturner(id, clientBaseAdress));
            try
            {
                return summonerRankInfo[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                RankInfo rankEmpty = new RankInfo();
                return rankEmpty;
            }
        }

        public void GetSummonerMatchIds(string accountId, string numberOfGames, int championId)
        {
            using (HttpClient client = new HttpClient())
            {
                matchHistory.Clear();
                string idToPutInLink;
                client.BaseAddress = new Uri("https://euw1.api.riotgames.com/lol/match/v4/matchlists/by-account/");
                if (championId == 0)
                    idToPutInLink = "";
                else
                    idToPutInLink = championId.ToString();
                try
                {
                    string s = client.GetStringAsync(accountId + $"?champion={idToPutInLink}&endIndex={numberOfGames}&api_key={apiKey}").Result;
                    var gameIdsList = JsonConvert.DeserializeObject<GameIdsList>(s);
                    
                    foreach (var element in gameIdsList.matches)
                        GetSpecificMatchDataForSpecificSummoner(element.gameId, accountId);
                }
                catch(AggregateException ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }



        public void GetSpecificMatchDataForSpecificSummoner(long gameId, string accountId)
        {

            var clientBaseAdress = "https://euw1.api.riotgames.com/lol/match/v4/matches/";
            var rankInfoList = JsonConvert.DeserializeObject<MatchInfoWithAllParticipants>(JsonStringReturner(gameId.ToString(), clientBaseAdress));

            matchHistory.Add(ProvideDataToDisplay(rankInfoList,accountId));

        }

        public SpecificSummonerStats ProvideDataToDisplay(MatchInfoWithAllParticipants matchInfoWithAllParticipants, string accountId)
        {

            string winOrLoss;
            var participantId = matchInfoWithAllParticipants.participantIdentities.Find(x => x.player.accountId == accountId).participantId;
            var champId = matchInfoWithAllParticipants.participants.Find(x => x.participantId == participantId).championId;
            var teamId = matchInfoWithAllParticipants.participants.Find(x => x.participantId == participantId).teamId;
            if (matchInfoWithAllParticipants.teams.Find(x => x.teamId == teamId).win == "Fail")
                winOrLoss = "Lost";
            else
                winOrLoss = "Won";
            var stats = matchInfoWithAllParticipants.participants.Find(x => x.participantId == participantId).stats;
            SpecificSummonerStats specificSummonerGameStats = new SpecificSummonerStats
            {
                champName = GetChampName(champId),
                winOrLoss = winOrLoss,
                stats=stats

            };
            return specificSummonerGameStats;
        }
        public SummonerAverages GetAverages(List<SpecificSummonerStats> matchHistory)
        {

            AddStatsTotals(matchHistory);
            SummonerAverages summonerAverages = new SummonerAverages()
            {

                killsAverage = float.IsNaN((float)totalKills / matchHistory.Count) ? 0 : (float)totalKills / matchHistory.Count,
                deathsAverage = float.IsNaN((float)totalDeaths / matchHistory.Count) ? 0 : (float)totalDeaths / matchHistory.Count,
                assistsAverage = float.IsNaN((float)totalAssits / matchHistory.Count) ? 0 : (float)totalAssits / matchHistory.Count
            };
            return summonerAverages;
        }

        public void AddStatsTotals(List<SpecificSummonerStats> matchHistory)
        {
            totalKills = 0;
            totalAssits = 0;
            totalDeaths = 0;
            foreach (var element in matchHistory)
            {
                totalKills += element.stats.kills;
                totalAssits += element.stats.assists;
                totalDeaths += element.stats.deaths;
            }

        }

        public void GetAllChamps()
        {


                var baseAddress = "http://ddragon.leagueoflegends.com/";
                allChampions = JsonConvert.DeserializeObject<AllChampions>(JsonStringReturner("cdn/10.25.1/data/en_US/champion.json", baseAddress));

            
        }
        public string GetChampName(int champId)
        {
            string champName = null;
            foreach (var element in allChampions.data.Values)
                if (element.key == champId)
                {
                    champName = element.name;
                    break;
                }

            return champName;
        }

        public void GetMasteryChampsForSpecificSummoner(string accountId)
        {
            var clientBaseAdress = "https://euw1.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-summoner/";
            var masteryChamps = JsonConvert.DeserializeObject<List<MasteryChamps>>(JsonStringReturner(accountId, clientBaseAdress));
            this.masteryChamps = masteryChamps;
        }



    }

}
