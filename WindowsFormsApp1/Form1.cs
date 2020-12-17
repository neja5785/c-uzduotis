using League.Provider;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : MaterialForm
    {
        private bool matchesLoaded = false;
        Provider provider = new Provider();
        private string id;
        private string accountId;
        private bool championsLoaded = false;

        public Form1()
        {

            InitializeComponent();
            panel1.Hide();
            panel3.Hide();
        }


        private void MaterialButton1_Click(object sender, EventArgs e)
        {
            provider.GetIds(materialTextBox1.Text);

            if (provider.userExists)
            {

                provider.GetAllChamps();
                id = provider.GetIds(materialTextBox1.Text).id;
                accountId = provider.GetIds(materialTextBox1.Text).accountId;
                matchesLoaded = false;
                championsLoaded = false;
                FillMainChampionsToListView();
                FillSummonerRankTierLevel(provider.GetRank(id).Tier, materialTextBox1.Text, provider.GetRank(id).Rank, provider.GetIds(materialTextBox1.Text).summonerLevel);
                panel2.Hide();
                panel1.Show();
                materialTextBox2.Text = "5";
            }
            else
            {
                errorProvider1.SetError(materialTextBox1, "There is no such summoner");
                provider.userExists = true;
            }
        }


        private void FillSummonerRankTierLevel(string tier, string summonerName, string rank, int level)
        {
            label1.Font = new Font("Arial", 24, FontStyle.Bold);
            label1.Text = $"Hello {summonerName} !";
            materialLabel2.Text = $"Your LEVEL - {level}";
            materialLabel3.Text = $"Your RANK - {tier} {rank}";
        }
        public void FillMainChampionsToListView()
        {
            
            listView1.Items.Clear();
            provider.GetMasteryChampsForSpecificSummoner(id);
            int numberToLoad = provider.masteryChamps.Count() < 5 ? provider.masteryChamps.Count() : 5;
        
            for (int i = 0; i < numberToLoad; i++)
            {
                var item = new ListViewItem(new[] { provider.GetChampName(provider.masteryChamps[i].championId), provider.masteryChamps[i].championPoints.ToString(), provider.masteryChamps[i].championLevel.ToString() });
                listView1.Items.Add(item);
            }
        }
        private void MaterialButton2_Click(object sender, EventArgs e)
        {
            if (matchesLoaded == false)
            {
                LoadMatchDataToListView();
            }
            panel1.Hide();
            panel3.Show();
        }

        private void FillMatchHistoryDataToListView()
        {

            foreach (var element in provider.matchHistory)
            {
                var item = new ListViewItem(new[] { element.winOrLoss, element.champName, $"{element.stats.kills}/{element.stats.deaths}/{element.stats.assists}", element.stats.totalMinionsKilled.ToString() });
                materialListView1.Items.Add(item);
            }

            
            matchesLoaded = true;
        }
        private void FillAverages()
        {
            var averages = provider.GetAverages(provider.matchHistory);
            materialLabel8.Text = $"{ Math.Round(averages.killsAverage,1)}/{Math.Round(averages.deathsAverage,1)}/{Math.Round(averages.assistsAverage,1)}";

        }
        private void LoadMatchDataToListView()
        {
            materialListView1.Items.Clear();
            if (championsLoaded == false)
                LoadChampionsToComboBox();
            provider.GetSummonerMatchIds(accountId, materialTextBox2.Text, (int)materialComboBox1.SelectedValue);
            
            FillMatchHistoryDataToListView();
            FillAverages();
        }


        private void LoadChampionsToComboBox()
        {
            Dictionary<int, string> champIdAndString = new Dictionary<int, string>
            {
                { 0, "" }
            };
            foreach (var element in provider.masteryChamps)
            {
                champIdAndString.Add(element.championId, provider.GetChampName(element.championId));
            }

            materialComboBox1.DataSource = new BindingSource(champIdAndString, null);
            materialComboBox1.DisplayMember = "Value";
            materialComboBox1.ValueMember = "Key";
            championsLoaded = true;
        }
        private void materialButton5_Click(object sender, EventArgs e)
        {

            
            LoadMatchDataToListView();

        }
        private void materialButton3_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Show();
        }

        private void materialButton4_Click_1(object sender, EventArgs e)
        {
            panel3.Hide();
            panel1.Show();
        }


    }
}
