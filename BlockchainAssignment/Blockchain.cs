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

        public override string ToString() // Output all blocks of the blockchain as a string

        {
            return String.Join("\n\n", Blocks);
        }

    }

}
