using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        public List<Block> Blocks = new List<Block>();
        private int transactionsPerBlock = 5;                               // Maximum number of transactions per block
        public List<Transaction> transactionPool = new List<Transaction>();	// List of pending transactions to be mined 

        public Blockchain()
        {
            Blocks.Add(new Block());
        }

        // Gets a block by its index and returns its string
        public string GetBlockString(int index)
        {
            string str = Blocks[index].ToString();    // Gets the string for a block by its index
            return str;
        }

        // Retrieves the most recently appended block in the blockchain
        public Block GetLastBlock() 

        {
            return Blocks[Blocks.Count - 1];
        }

        // Retrieve pending transactions and remove from pool
        public List<Transaction> GetPendingTransactions() 

        {
            int n = Math.Min(transactionsPerBlock, transactionPool.Count);    // Determine the number of transactions to retrieve dependent on the number of pending transactions and the limit specified
            List<Transaction> transactions = transactionPool.GetRange(0, n); // "Pull" transactions from the transaction list (modifying the original list)
            transactionPool.RemoveRange(0, n);     // Remove from the pool
            return transactions; // Return the extracted transactions
        }

        // Check validity of a blocks hash by recomputing the hash and comparing with the mined value
        public static bool ValidateHash(Block block) 

        {
            string rehash = block.CreateHash();

            return rehash.Equals(block.blockHash);
        }

        // Check validity of the merkle root by recalculating the root and comparing with the mined value 
        public static bool ValidateMerkleRoot(Block block)      
        {
            String reMerkle = Block.MerkleRoot(block.transactionList);

            return reMerkle.Equals(block.merkleRoot);
        }

        // Check the balance associated with a wallet based on the public key
        public double GetBalance(string address) 
        {

            double balance = 0; // Accumulator value for current Wallet

            // Loop through all approved transactions in order to assess account balance
            foreach (Block block in Blocks) 
            {
                foreach (Transaction transaction in block.transactionList)
                {

                    if (transaction.recipientAddress.Equals(address))

                    {
                        balance += transaction.amount; // Credit funds received
                    }

                    if (transaction.senderAddress.Equals(address))

                    {
                        balance -= (transaction.amount + transaction.fee); // Debit payments placed
                    }

                }

            }

            return balance;

        }

        // Function to get all transactions in the blockchain involving a reciver key
        public string GetIncludedTransactions(string rk)
        {
            string tl = "";     // String to store the transactions

            // Search through all the blocks and transactions
            foreach (Block block in Blocks)
            {
                foreach (Transaction transaction in block.transactionList)
                {
                    // If the recipient address is there then add the address
                    if (transaction.recipientAddress == rk)
                    {
                        tl = tl + transaction.ToString() + "\n\n";
                    }
                }
            }

            return tl;
        }


        // This procedure swaps two transactions with the arguments n1 and n2 been the index in the pending transaction list
        // of where the card is. The procedure puts the card at n1 into index n2 and the card n2 into the index at n1.
        void swapTransactions(int n1, int n2)
        {
            Transaction tempN2 = transactionPool[n2]; // creates a temporary storage for n2 so we can put into n1 when it gets overwritten by n1's value
            transactionPool[n2] = transactionPool[n1];  // stores the value of n1 into n2
            transactionPool[n1] = tempN2;       // stores the value of n2 into n1
        }


        // This procedure sorts the pending transaction list through bubble sort
        // It puts the largest or oldest at the beginning of the list
        void bubbleSort(string sortBy)
        {

            // this will go through the pending transactions times to make sure each item is in the correct position
            for (int i = 0; i < transactionPool.Count - 1; i++)
            {
                // goes through the transaction list comparing each card to the one next to it
                // and swapping it if there is a bigger one to the left of a smaller one
                for (int j = 0; j < transactionPool.Count - 1; j++)
                {

                    // for fees get the largest ones at the front and for date get the earliest ones at the front
                    if (sortBy == "fee")
                    {
                        // if it is one that means that the first one is smaller and they need to be swapped
                        if (transactionPool[j].fee < transactionPool[j + 1].fee)
                        {
                            swapTransactions(j, j + 1);
                        }
                    }

                    if (sortBy == "date")
                    {
                        // if it is one that means that the first one is smaller and they need to be swapped
                        if (transactionPool[j].timestamp > transactionPool[j + 1].timestamp)
                        {
                            swapTransactions(j, j + 1);
                        }
                    }

                }
            }

            

        }


        // Method that returns the five largest or five oldest transactions from the transacrtion pool
        public List<Transaction> GetTransactionsSpecifically(string sortby)
        {

            List<Transaction> largestTransactions = new List<Transaction>();       // Value to store the five largest transactions

            // If there is less then five then it will automatically have the five biggest ones 
            if (transactionPool.Count < 5)
            {
                largestTransactions = transactionPool;
                transactionPool.Clear();        // Clear transaction pool
                return transactionPool;
            }

            // Sort the list so the largest fees or oldest ones are at the beginning of the list
            if (sortby == "fee") bubbleSort("fee");
            else if (sortby == "date") bubbleSort("date");

            Console.WriteLine($"Checking transactionpool\n {String.Join("\n\n", transactionPool)}");

            // After ordering get the five largest or oldest
            for (int i=0; i<4; i++)
            {
                // Add the first one and then remove it from transacction pool
                largestTransactions.Add(transactionPool[0]);
                transactionPool.Remove(transactionPool[0]);
            }

            return largestTransactions;       // Return the largest transactions
        }

        // Method that returns the transactions from the miner's friend list
        public List<Transaction> GetTransactionsPersonal()
        {
            List<Transaction> personalTransactions = new List<Transaction>();       // Value to store the transactions from the friends list

            List<String> friendList = new List<String> { "PyQMaqziE/Cv8miNIlFCpa/uJi0A69feLEp69ze5iQfB86jG90sWipStIPvtIE/iC1Rgw3bhdSw1nt5RKZKPrQ==",
                                                        "MDb4kTTRVR6tWZBC18bR9lzC/SReTOm0ZsNoUGus6TTWM/fc8thX6Pd+odAlFtU4H07KK3E8jYM0Gb6hcSIDmA==" ,
                                                        "MthpPmpJICFtuB4L9s9EpzlezzEsaA9DePmDXjzAuscHZT6SV8ZED9ILPQp3GKYhkZc8YLOeOsum3y+bdwweqA==",
                                                        "ANpZLIEBOKbK1Ho+MwPLngjPH8c11oX2ls4oAKXiQXd7HbdCnjCCaOYbQBr6XRTYJnPKuCWTMRa1P+AQ3lPADg==",
                                                        "2uvdAVmkITdZTL24ImNtlOiI92KhR9g8TnMIq1kcwlGmxPamN2bDxFhvNeIayFey3545ZVsMR7WYe1DNitLBAA=="};

            // Check each transaction if it is in the miner's friend list
            foreach (Transaction transaction in transactionPool.ToList())
            {
                foreach(String friendHash in friendList)
                {
                    if (transaction.senderAddress == friendHash || transaction.recipientAddress == friendHash)
                    {
                        personalTransactions.Add(transaction);
                        transactionPool.Remove(transaction);    // Remove it from the transaction pool
                        break;
                    }
                }
            }

            return personalTransactions;
        }

        public override string ToString() // Output all blocks of the blockchain as a string

        {
            return String.Join("\n\n", Blocks);
        }

    }

}
