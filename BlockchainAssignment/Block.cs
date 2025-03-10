using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Block
    {
        private DateTime timestamp;     // The time the block is made at
        private int index;              // The position of the block in the blockchain
        string blockHash;               // The hash of the current block
        string previousHash;            //  The hash of the previous block

        private int difficulty = 4;     // An arbitrary number of 0's to proceed a hash value
        public string minerAddress;     // Public Key (Wallet Address) of the Miner
        
        public List<Transaction> transactionList; // List of transactions in this block

        // Proof-of-work
        public long nonce; // Number used once for Proof-of-Work and mining

        // Rewards
        public double reward; // Simple fixed reward established by "Coinbase"


        /* Genesis block constructor */
        public Block()
        {
            this.timestamp = DateTime.Now;
            this.index = 0;
            this.previousHash = "N/A";
            this.blockHash = Mine();

            this.transactionList = new List<Transaction>();
        }

        // New Block Constructor
        public Block(Block lastBlock, List<Transaction> transactions, String minerAddress)
        {
            this.timestamp = DateTime.Now;
            this.index = lastBlock.index + 1;
            this.previousHash = lastBlock.blockHash;
            this.blockHash = Mine();

            this.minerAddress = minerAddress; // The wallet to be credited the reward for the mining effort
            this.reward = 1.0; // Assign a simple fixed value reward

            transactions.Add(createRewardTransaction(transactions)); // Create and append the reward transaction
            this.transactionList = new List<Transaction>(transactions); // Assign provided transactions to the block
        }

        //  Function for creating the hash
        public String CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            String input = index.ToString() + timestamp.ToString() + previousHash + nonce.ToString();
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            String hash = string.Empty;

            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);

            return hash;
        }

        // Create a Hash which satisfies the difficulty level required for PoW
        public String Mine() 

        {
            nonce = 0; // Initalise the nonce
            String hash = CreateHash(); // Hash the block
            String re = new string('0', difficulty); // A string representing the “difficulty” for analysing the PoW requirement

            while (!hash.StartsWith(re)) // Check the resultant hash against the "re" string
            {
                nonce++; // Increment the nonce if the difficulty level not be satisfied
                hash = CreateHash(); // Rehash with the new nonce as to generate a different hash
            }

            return hash;   // Return the hash meeting the difficulty requirement

        }

        // Create reward for incentivising the mining of block
        public Transaction createRewardTransaction(List<Transaction> transactions) 

        {
            double fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee); // Sum all transaction fees
            return new Transaction("Mine Rewards", minerAddress, (reward + fees), 0, ""); // Issue reward as a transaction in the new block
        }

        // Function to return string containing block values
        public override string ToString()
        {
            string str;         // the string to display
            str = "[BLOCK START]" 
                + "\nIndex: " + index
                + "\tTimestamp: " + timestamp
                + "\nHash of current block: " + blockHash
                + "\nPrevious block hash: " + previousHash
                + "\n-- PoW --"
                + "\nDifficulty Level: " + difficulty
                + "\nNonce: " + nonce
                + "\n-- Rewards --"
                + "\nReward: " + reward
                + "\nMiners Address: " + minerAddress
                + "\n-- " + transactionList.Count + " Transactions --"
                + "\n\n-- " + transactionList.Count + " Transactions --"
                + "\n\n" + string.Join("\n\n", transactionList)
                + "\n[BLOCK END]";

            return str;
        }

    }
}
