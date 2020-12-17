using League.Provider;
using MaterialSkin.Controls;
using System;
using System.ComponentModel;

namespace WindowsFormsApp1
{
    public partial class Form3 : MaterialForm
    {
        private Provider provider;

        public Form3(Provider provider)
        {
            InitializeComponent();
            this.provider = provider;


            /*        foreach(var element in provider.GetMatchInfo())
                        {
                                    materialListView1.Items.Add(element.winOrLoss, element.champName, element.kda);
                        }*/
        }

        private void textBoxContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
