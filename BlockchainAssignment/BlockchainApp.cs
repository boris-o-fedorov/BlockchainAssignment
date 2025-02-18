using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {

        Blockchain blockchain;
        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
            UpdateText("New Blockchsin Initialised!");
        }

        // Method for updating output text
        public void UpdateText(string text)
        {
            richTextBox1.Text = text;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            // Display Block string
            if (Int32.TryParse(textBox1.Text, out int index))
                UpdateText(blockchain.GetBlockString(index));
            else
                UpdateText("Invalid Block number.");
        }

    }
}
