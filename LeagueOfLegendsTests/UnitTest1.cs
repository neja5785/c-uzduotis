using League.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LeagueOfLegendsTests
{
    [TestClass]
    public class UnitTest1
    {
        List<SpecificSummonerStats> matchHistory = new List<SpecificSummonerStats>() { new SpecificSummonerStats { stats=new Stats{kills = 5, assists = 6, deaths = 10 } },
                                                                                       new SpecificSummonerStats { stats=new Stats{ kills = 5, assists = 4, deaths = 12} },
                                                                                       new SpecificSummonerStats { stats=new Stats{ kills = 5, assists = 2, deaths = 11 } } };


        [TestMethod]
        public void findingChampNameTest()
        {
            var testProv = new Provider();
            MasteryChamps masteryChamps = new MasteryChamps() { championId = 1 };
            Champion champion = new Champion() { key = 1, name = "Annie" };
            Champion champion1 = new Champion() { key = 2, name = "Zed" };
            /*AllChampions champRoot = new AllChampions() { data = { { "Annie", champion } } };*/
            testProv.allChampions = new AllChampions() { data = new Dictionary<string, Champion>() { { "Annie", champion }, { "Zed", champion1 } } };

            Assert.AreEqual("Annie", testProv.GetChampName(masteryChamps.championId));
        }
        [TestMethod]
        public void averageTest()
        {
            var testProv = new Provider();
            var averages = testProv.GetAverages(matchHistory);
            Assert.AreEqual(4, averages.assistsAverage);
            Assert.AreEqual(5, averages.killsAverage);
            Assert.AreEqual(11, averages.deathsAverage);
        }
        [TestMethod]
        public void testSums()
        {
            var testProv = new Provider();
            testProv.GetAverages(matchHistory);
            Assert.AreEqual(33, testProv.totalDeaths);
            Assert.AreEqual(12, testProv.totalAssits);
            Assert.AreEqual(15, testProv.totalKills);
        }
        [TestMethod]
        public void checkIfSummonerDataIsCorrectlyFound()
        {
            
            var testProv = new Provider();
            List<Team> teams = new List<Team>() {new Team { teamId = 1, win = "Fail"},
                                                 new Team { teamId = 2, win = "Win"} };
            List<Participant> participants = new List<Participant>() { new Participant { participantId=1,teamId=1,championId=1, stats=new Stats { kills = 1, assists = 2, deaths = 4, totalMinionsKilled = 212 } },
                                                                       new Participant { participantId=2,teamId=2,championId=3, stats=new Stats { kills = 5, assists = 2, deaths = 55, totalMinionsKilled = 22 } },
                                                                       new Participant { participantId=3,teamId=1,championId=5, stats=new Stats { kills = 55, assists = 3, deaths = 2, totalMinionsKilled = 21 } }};
            List<ParticipantIdentity> participantIdentities = new List<ParticipantIdentity>() { new ParticipantIdentity() {participantId=1, player= new Player { accountId="abc"} },
                                                                                                new ParticipantIdentity() {participantId=2, player= new Player { accountId="def"} }};
            MatchInfoWithAllParticipants matchInfoWithAllParticipants = new MatchInfoWithAllParticipants() { teams = teams, participants = participants, participantIdentities = participantIdentities };
            Champion champion = new Champion() { key = 1, name = "Annie" };
            Champion champion1 = new Champion() { key = 1, name = "Annie" };
            testProv.allChampions = new AllChampions() { data = new Dictionary<string, Champion>() { { "Annie", champion }, { "Zed", champion1 } } };

            var testData = testProv.ProvideDataToDisplay(matchInfoWithAllParticipants, "abc");


            Assert.AreEqual("Annie", testData.champName);
            Assert.AreEqual(2, testData.stats.assists);
            Assert.AreEqual(4, testData.stats.deaths);
            Assert.AreEqual(1, testData.stats.kills);
            Assert.AreEqual(212, testData.stats.totalMinionsKilled);
            Assert.AreEqual("Lost", testData.winOrLoss);
        }
        /*   public List<Team> teams { get; set; }

           public List<Participant> participants { get; set; }
           public List<ParticipantIdentity> participantIdentities { get; set; }
                public int participantId { get; set; }
           public int teamId { get; set; }
           public int championId { get; set; }
           public Stats stats { get; set; }*/

    }
}


