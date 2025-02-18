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
        string previousHash;              //  The hash of the previous block


        /* Genesis block constructor */
        public Block()
        {
            timestamp = DateTime.Now;
            index = 0;
            previousHash = "N/A";
            blockHash = CreateHash();
        }

        // New Block Constructor
        public Block(Block lastBlock)
        {
            timestamp = DateTime.Now;
            index = lastBlock.index + 1;
            previousHash = lastBlock.blockHash;
            blockHash = CreateHash();
        }

        //  Function for creating the hash
        public String CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            String input = index.ToString() + timestamp.ToString() + previousHash;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            String hash = string.Empty;

            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);

            return hash;
        }


        // Function to return string containing block values
        public override string ToString()
        {
            string str;         // the string to display
            str = "Index: " + index + "                 Timestamp: " + timestamp +  "\nHash of current block: " + blockHash + "\nPrevious block hash: " + previousHash;

            return str;
        }

    }
}
