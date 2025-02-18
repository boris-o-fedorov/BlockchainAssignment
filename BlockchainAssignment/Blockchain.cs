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
        List<Block> Blocks = new List<Block>();

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

    }
}
