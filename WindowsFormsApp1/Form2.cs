using League.Provider;
using MaterialSkin.Controls;
using System;

namespace WindowsFormsApp1
{
    public partial class Form2 : MaterialForm
    {
        private Provider provider = new Provider();
        private String summonerName;
        public Form2(string summonerName)
        {
            InitializeComponent();
            this.summonerName = summonerName;
        }


        private void Form2_Load(object sender, EventArgs e)
        {

            panel2.Hide();
            materialLabel1.Text = "Hello " + summonerName + "!";
            materialLabel2.Text = "Your LEVEL - " + provider.GetIds(summonerName).summonerLevel;
            materialLabel3.Text = "Your RANK - " + provider.GetRank(provider.GetIds(summonerName).id).Tier + " " + provider.GetRank(provider.GetIds(summonerName).id).Rank;
        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel2_Click(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {


            panel2.Show();

        }
    }
}
