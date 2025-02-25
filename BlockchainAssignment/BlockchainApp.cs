using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
       

        private void PrintBlock_Click(object sender, EventArgs e)
        {
            // Display Block string
            if (Int32.TryParse(blockNo.Text, out int index))
                UpdateText(blockchain.GetBlockString(index));
            else
                UpdateText("Invalid Block number.");
        }

        private void GenWallet_Click(object sender, EventArgs e)
        {
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out string privKey);

            publicKey.Text = myNewWallet.publicID;

            privateKey.Text = privKey;
        }

        private void ValKeys_Click(object sender, EventArgs e)
        {
            if (Wallet.Wallet.ValidatePrivateKey(privateKey.Text, publicKey.Text))
            {
                UpdateText("Keys are valid");
            }

            else
             {
                UpdateText("Keys are invalid");
            }
        }



        private void CreateTransaction_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(publicKey.Text, reciever.Text, Double.Parse(amount.Text), Double.Parse(fee.Text), privateKey.Text);

            blockchain.transactionPool.Add(transaction);

            UpdateText(transaction.ToString());
        }
    }
}
