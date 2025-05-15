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

        private async void GenerateNewBlock_Click(object sender, EventArgs e)
        {
            // Retrieve pending transactions to be added to the newly generated Block
            List<Transaction> transactions = blockchain.GetPendingTransactions();
           
            // Run the block generation and mining in a background thread
            Block newBlock = await Task.Run(() =>
            {
                return new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
            });


            blockchain.Blocks.Add(newBlock);

            UpdateText(blockchain.GetBlockString(blockchain.Blocks.Count - 1));
        }

        // Reads all blocks
        private void ReadAll_Click(object sender, EventArgs e)
        {
            UpdateText(blockchain.ToString());
        }

        private void PrintPendingTransactions_Click(object sender, EventArgs e)
        {
            UpdateText(String.Join("\n\n", blockchain.transactionPool));
        }

        // Validate the integrity of the state of the Blockchain
        private void Validate_Click(object sender, EventArgs e)
        {
            // If: Genesis Block - Check only hash as no transactions are currently present
            if (blockchain.Blocks.Count == 1)
            {
                if (!Blockchain.ValidateHash(blockchain.Blocks[0])) // Recompute Hash to check validity
                {
                    UpdateText("Blockchain is invalid");
                }

                else
                {
                    UpdateText("Blockchain is valid");
                }

                return;
            }

            for (int i = 1; i < blockchain.Blocks.Count - 1; i++)

            {

                if (

                blockchain.Blocks[i].previousHash != blockchain.Blocks[i - 1].blockHash || // Check hash "chain"

                !Blockchain.ValidateHash(blockchain.Blocks[i]) || // Check each blocks hash

                !Blockchain.ValidateMerkleRoot(blockchain.Blocks[i]) // Check transaction integrity using Merkle Root

                )

                {

                    UpdateText("Blockchain is invalid");

                    return;

                }

            }

            UpdateText("Blockchain is valid");

        }

        // Checks balance, adds more information then that provided for in the code provided
        private void CheckBalance_Click(object sender, EventArgs e)
        {
            UpdateText("Address: " + reciever.Text
                        + "\nBalance: " + blockchain.GetBalance(reciever.Text).ToString() + " Nova Coin\n"
                        + "\nTransactions:\n" + blockchain.GetIncludedTransactions(reciever.Text));   // Displays the balance and all transactions with reciver key
        }

        

        // Generate new block with the five largest fees
        private void Click_GenLargest(object sender, EventArgs e)
        {
            // Retrieve pending transactions to be added to the newly generated Block based on greedy
            List<Transaction> transactions = blockchain.GetTransactionsSpecifically("fee");

            // Create and append the new block - requires a reference to the previous block, a set of transactions and the miners public address (For the reward to be issued)
            Block newBlock = new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
            blockchain.Blocks.Add(newBlock);

            UpdateText(blockchain.GetBlockString(blockchain.Blocks.Count - 1));
        }

        // Generate new block with the five oldest transactions
        private void Click_GenOldest(object sender, EventArgs e)
        {
            // Retrieve pending transactions to be added based on their age
            List<Transaction> transactions = blockchain.GetTransactionsSpecifically("date");

            // Create and append the new block - requires a reference to the previous block, a set of transactions and the miners public address (For the reward to be issued)
            Block newBlock = new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
            blockchain.Blocks.Add(newBlock);

            UpdateText(blockchain.GetBlockString(blockchain.Blocks.Count - 1));
        }

        private void Cick_GenRandom(object sender, EventArgs e)
        {

        }

        private void Click_GenPersonal(object sender, EventArgs e)
        {
            // Retrieve pending transactions to be added based on if they are in the miner's friend list
            List<Transaction> transactions = blockchain.GetTransactionsPersonal();

            // Create and append the new block - requires a reference to the previous block, a set of transactions and the miners public address (For the reward to be issued)
            Block newBlock = new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
            blockchain.Blocks.Add(newBlock);

            UpdateText(blockchain.GetBlockString(blockchain.Blocks.Count - 1));
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void Process_Dynamic_Degree_Difficulty(object sender, EventArgs e)
        {

        }
    }
}