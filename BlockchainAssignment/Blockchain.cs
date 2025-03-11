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

        public override string ToString() // Output all blocks of the blockchain as a string

        {
            return String.Join("\n\n", Blocks);
        }

    }

}
