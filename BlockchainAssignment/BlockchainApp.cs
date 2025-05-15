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
using System.Diagnostics;

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
  
        // This gets a dynamic degree of difficulty and thread mining using a hill climbing approach
        private void adapt_block_mining(object sender, EventArgs e)
        {

            // Initialise variables for analysis
            bool stillSearching = true;                              // Says if the algorithm is still searching through
            bool optimumDifficultyFound = false;                     // Says if the optimum difficulty has been found
            bool optimumThreadNumberFound = false;                   // Says if the optimum number of threads has been found 
            int optimalDifficulty = Block.GetDifficulty();           // The optimal degree of difficulty
            int optimalThreadNumber = 1;                             // The optimal thread number 
            double idealBlockTime = 13.0;                            // The ideal block time
            double optimumTimeDifferenceSquared = double.MaxValue;   // The smallest difference between recorded time and ideal blocktime
            TimeSpan optimalBlockTime = TimeSpan.Zero;               // Keep track of the closest block time to the ideal blocktime           

            // If not enough transactions for minimum update then return saying there is not enough transactions to peform mining analysis
            if (blockchain.transactionPool.Count < 30)
            {
                UpdateText("Not enough transactions in transaction pool to peform mining analysis");
                return;
            }

            // If there is enough then apply hill climbing approch until an optium soution is found
            while (stillSearching == true)
            {
                // Update the transactions so the user knows it is still analysing
                UpdateText(blockchain.GetBlockString(blockchain.Blocks.Count - 1));

                // Retrieve five pending transactions to be added based on their age
                List<Transaction> transactions = blockchain.GetTransactionsSpecifically("date");

                // Create and append the new block
                Block newBlock = new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
                blockchain.Blocks.Add(newBlock);

                // Display the new block so that the user knows it is working and hasn't crashed
                

                // Get the block time
                TimeSpan mineTime = newBlock.stopwatch.Elapsed;

                // Calculates the squared values of the time difference and optimul time to ensure they are both positive
                double timeDrifference = mineTime.TotalSeconds - idealBlockTime;
                double timeDrifferenceSquared = Math.Pow(timeDrifference, 2);

                // If the optimum degree of difficulty has not been found yet
                if (optimumDifficultyFound == false)
                {
                    if (timeDrifferenceSquared < optimumTimeDifferenceSquared)
                    {
                        // If faster then set the new optimal time and difficulty
                        optimumTimeDifferenceSquared = timeDrifferenceSquared;
                        optimalBlockTime = mineTime;
                        optimalDifficulty = Block.GetDifficulty();
                    }

                    // If it has gone over the ideal block time then the solution has found the ideal difficulty
                    if (mineTime.TotalSeconds > idealBlockTime)
                    {
                        Block.SetDifficulty(optimalDifficulty); // Set the ideal difficulty
                        Block.SetWhichMine(1);   // Set to parallell mining to compare
                        optimumDifficultyFound = true;
                    }

                    // If it is not done yet keep going until it has gone over the block time
                    else Block.SetDifficulty(Block.GetDifficulty() + 1);
                }

                // Once the optimum difficulty has been locked begin to check thread numbers
                else if (optimumDifficultyFound == true && optimumThreadNumberFound == false)
                {
                    if (timeDrifferenceSquared < optimumTimeDifferenceSquared)
                    {
                        // If faster then set the new optimal time and difficulty
                        optimumTimeDifferenceSquared = timeDrifferenceSquared;
                        optimalBlockTime = mineTime;
                        optimalThreadNumber = Block.GetThreadNumber();
                        Block.SetThreadNumber(Block.GetThreadNumber() + 512);
                    }

                    // If it has not improved and the time has got further away then increasing thread number does not help and is going further away from the ideal block time so stop
                    else if (timeDrifferenceSquared > optimumTimeDifferenceSquared)
                    {
                        Block.SetThreadNumber(optimalThreadNumber); // Set the ideal difficulty
                        Block.SetWhichMine(2);                     // Set mining to GPU mine
                        optimumThreadNumberFound = true;

                        // If optimal thread number is one, no lead to compare on GPU due to how long it takes
                        if (optimalThreadNumber == 1)
                        {
                            Block.SetWhichMine(0);  // Switch to CPU with single thread
                            UpdateText("Finnished Analysis:\nClosest Blocktime Found: " + idealBlockTime.ToString() + "\nIdeal Difficulty:" + optimalDifficulty + "\nIdeal number of threads:" + optimalThreadNumber + "\nProcessor: CPU\n\nThe difficulty, thread number and processor have now been switched to the values above to get the closest mining time to the ideal mining time of 13 seconds.");
                            stillSearching = false;
                        }
                    }

                }

                // Once the difficulty and thread number is done then compare CPU vs GPU time
                else
                {
                    // If better on GPU stay on GPU
                    if (timeDrifferenceSquared < optimumTimeDifferenceSquared)
                    {
                        optimalBlockTime = mineTime;
                        UpdateText("Finnished Analysis:\nClosest Blocktime Found: " + optimalBlockTime.ToString() +  "\nIdeal Difficulty:" + optimalDifficulty + "\nIdeal number of threads:" + optimalThreadNumber + "\nProcessor: GPU\n\nThe difficulty, thread number and processor has now been switched to the values above to get the closest mining time to the ideal mining time of 13 seconds.");
                    }

                    // Otherwise switch to parallel processing on the CPU
                    else
                    {
                        Block.SetWhichMine(1);  // Switch back to CPU
                        UpdateText("Finnished Analysis:\nClosest Blocktime Found: " + optimalBlockTime.ToString() + "\nIdeal Difficulty:" + optimalDifficulty + "\nIdeal number of threads:" + optimalThreadNumber + "\nProcessor: CPU\n\nThe difficulty, thread number and processor has now been switched to the values above to get the closest mining time to the ideal mining time of 13 seconds.");
                    }

                    stillSearching = false;     // Finnished analysis
                }

            }          
        }
    }
}