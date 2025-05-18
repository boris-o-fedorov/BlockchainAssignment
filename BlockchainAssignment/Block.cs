using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ILGPU;
using ILGPU.Runtime;


namespace BlockchainAssignment
{
    class Block
    {
        private DateTime timestamp;             // The time the block is made at
        private int index;                      // The position of the block in the blockchain
        public string blockHash;                // The hash of the current block
        public string previousHash;             //  The hash of the previous block

        private static int difficulty = 4;      // An arbitrary number of 0's to proceed a hash value
        public string minerAddress;             // Public Key (Wallet Address) of the Miner
        public string merkleRoot;  	            // The merkle root of all transactions in the block

        public List<Transaction> transactionList; // List of transactions in this block

        // Proof-of-work
        public long nonce; // Number used once for Proof-of-Work and mining

        // Rewards
        public double reward; // Simple fixed reward established by "Coinbase"
        
        private static readonly object lockObject = new object();  // Lock object for thread safety in parallel mining

        private static int threadNumber = 512;   // The thread number for parallel mining

        public Stopwatch stopwatch;        // Stopwatch for recording time taken to generate block

        private static int whichMine = 0;

        // Buffer for hash
        private static readonly uint[] K = new uint[]
    {
        0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
        0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
        0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
        0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
        0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
        0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
        0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
        0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
        0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
        0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
        0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
        0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
        0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
        0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
        0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    };



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

            this.minerAddress = minerAddress; // The wallet to be credited the reward for the mining effort
            this.reward = 1.0; // Assign a simple fixed value reward

            transactions.Add(createRewardTransaction(transactions)); // Create and append the reward transaction
            this.transactionList = new List<Transaction>(transactions); // Assign provided transactions to the block

            this.merkleRoot = MerkleRoot(transactionList); 		// Calculate the merkle root of the blocks transactions

            this.blockHash = MineHash();

        }


        // Does the mineing depending on which way of mining is currently being done
        public String MineHash()
        {
            String hash = "";    // The hash to return

            // Switch depending on which version of mining is currently being done
            switch(whichMine)
            {
                case 0:
                    hash = Mine();
                    break;
                case 1:
                    hash = MineParallel();
                    break;
                case 2:
                    hash = GPUMine();
                    break;
            }

            return hash;
        }
   

        //  Function for creating the hash
        public String CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            String input = index.ToString() + timestamp.ToString() + previousHash + nonce.ToString() + merkleRoot;
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

            stopwatch = Stopwatch.StartNew(); // Start timing

            while (!hash.StartsWith(re)) // Check the resultant hash against the "re" string
            {
                nonce++; // Increment the nonce if the difficulty level not be satisfied
                hash = CreateHash(); // Rehash with the new nonce as to generate a different hash
            }

            stopwatch.Stop();  // Stop timeing

            return hash;   // Return the hash meeting the difficulty requirement

        }
        

        // Multi-threaded mining
        public String MineParallel()
        {
            nonce = 0; // Initialise nonce
            String hash = null; // Store the first valid hash found
            String re = new string('0', difficulty);  // A string representing the “difficulty” for analysing the PoW requirement

            stopwatch = Stopwatch.StartNew(); // Start timing

            // Use Multi-thread processing
            Parallel.For(0, threadNumber, (threadIndex, state) =>
            {
                if (threadNumber < threadIndex) threadNumber = threadIndex;   // Update threadIndex if needed

                long enonce = threadIndex * 1000000; // unique range per thread

                long maxEnonce = enonce + 1000000;

                // Keep going through while not found
                while (!state.ShouldExitCurrentIteration)
                {
                    long localNonce = enonce++;     // Get the local nonce in the thread
                    nonce = localNonce;             // Assing it to the nonce to hash with

                    String newHash = CreateHash(); // Generate hash for current nonce

                    // Check if the hash meets difficulty requirement
                    if (newHash.StartsWith(re))
                    {
                        // Ensure only one thread writes the valid hash
                        lock (lockObject) 
                        {
                            if (hash == null) // First valid hash wins
                            {
                                hash = newHash;
                                nonce = localNonce; // Store the valid nonce
                                state.Stop(); // Stop all other threads
                            }
                        }
                    }

                    // If reached limit then go to the next range and keep searching
                    if (enonce >= maxEnonce)
                    {
                        threadIndex = threadNumber + threadIndex;
                        enonce = threadIndex * 1000000;
                        maxEnonce = enonce + 1000000;
                    }

                } 
            });


            stopwatch.Stop();  // Stop timing


            return hash; // Return the first valid hash found
        }

        // Add padding to ensure bytes match the required length
        byte[] PadToFixedSize(byte[] input, int size)
        {
            byte[] padded = new byte[size];
            Array.Copy(input, padded, Math.Min(input.Length, size));
            return padded;
        }

        // GPU mining with parallel processing
        public string GPUMine()
        {
            nonce = 0; // Initialise nonce
            string hash = null; // Store the first valid hash found
            string re = new string('0', difficulty); // A string representing the “difficulty” for analysing the PoW requirement

            stopwatch = Stopwatch.StartNew(); // Start timing

            // Initialise ILGPU context and accelerator for GPU use
            using (var context = ILGPU.Context.CreateDefault())
            using (var accelerator = context.GetPreferredDevice(preferCPU: false).CreateAccelerator(context))
            {
                // Define max lengths
                const int MaxIndexLength = 20;   
                const int MaxTimestampLength = 64;
                const int MaxPrevHashLength = 64;  
                const int MaxMerkleRootLength = 64;


                // Prepare data for hashing for the gpu
                byte[] indexBytes = Encoding.UTF8.GetBytes(index.ToString());
                byte[] timestampBytes = Encoding.UTF8.GetBytes(timestamp.ToString());
                byte[] prevHashBytes = Encoding.UTF8.GetBytes(previousHash);
                byte[] merkleRootBytes = Encoding.UTF8.GetBytes(merkleRoot.ToString());

                // Pad to match GPU buffer sizes
                indexBytes = PadToFixedSize(indexBytes, MaxIndexLength);
                timestampBytes = PadToFixedSize(timestampBytes, MaxTimestampLength);
                prevHashBytes = PadToFixedSize(prevHashBytes, MaxPrevHashLength);
                merkleRootBytes = PadToFixedSize(merkleRootBytes, MaxMerkleRootLength);

                // Allocate memory buffers on the GPU
                using (var outputBuffer = accelerator.Allocate1D<byte>(threadNumber))       // output buffer for storing if it is a valid hash
                using (var kBuffer = accelerator.Allocate1D<uint>(K))                       // buffer for hash
                using (var indexBuffer = accelerator.Allocate1D<byte>(MaxIndexLength)) 
                using (var timestampBuffer = accelerator.Allocate1D<byte>(MaxTimestampLength))
                using (var prevHashBuffer = accelerator.Allocate1D<byte>(MaxPrevHashLength))
                using (var merkleBuffer = accelerator.Allocate1D<byte>(MaxMerkleRootLength))
                {
                    // Load data from the cpu
                    kBuffer.CopyFromCPU(K);
                    indexBuffer.CopyFromCPU(indexBytes);
                    timestampBuffer.CopyFromCPU(timestampBytes);
                    prevHashBuffer.CopyFromCPU(prevHashBytes);
                    merkleBuffer.CopyFromCPU(merkleRootBytes);

                    /* Load and execute the kernel 
                     * As the threadNumber is being passed as an Index1D
                     * This means it will excecute all the values from 0 to 1023 instantaniously creating a seperate enonce for each nonce.
                     */
                    var kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<byte>, ArrayView<uint>,long, int,ArrayView<byte>, ArrayView<byte>, ArrayView<byte>, ArrayView<byte>>(Sha256GpuKernel.Sha256Kernel);


                    /* Process results to find the valid hash.
                     * This will continue going through different nonces until a valid one is found.
                     * Even if it is not in the first range of threads as the nonce is constantly updated to check each thread computed,
                     * If it gets to the end the nonce will be set to the last thread,
                     * Then the GPU kernal will be called again with the nonce now being nonce+1024.
                     * Meaning it will keep going until a valid hash is found and won't have an infinate loop.
                     */
                    while (hash == null)
                    {
                        kernel((int)threadNumber, outputBuffer.View, kBuffer.View, nonce, difficulty, indexBuffer.View, timestampBuffer.View, prevHashBuffer.View, merkleBuffer.View); // starting the parallel processing for the GPU
                        accelerator.Synchronize(); // Syncronise the GPU to ensure they finnish at the same time

                        var results = outputBuffer.GetAsArray1D(); // Gets the output buffer to use ont he GPU to store as results

                        /* Loops through the length of the output buffer on the GPU to do the mining for the space allocated for the thread on the GPU.
                         * Each buffer in the output buffer corresponds to a different enonce tried
                         */
                        for (int enonce = 0; enonce < results.Length; enonce++)
                        {
                            // Checks if the results is a 1, meaning we have found a valid hash on the gpu
                            if (results[enonce] == 1)
                            {
                                // If found recompute the hash on the CPU as we can't pass the hash from the GPU to the cpu
                                long foundNonce = nonce + enonce;   // The found nonce is the nonce that corresponds to the nonce that gives a hash with required degree of difficulty 
                                nonce = foundNonce;     // Set the nonce to the found nonce to recompute the found hash
                                string newHash = CreateHash();

                                // Doube check the hash is valid in case of any corruption or error on the gpu
                                if (newHash.StartsWith(re))
                                {
                                        // Once found lock it so there is no issue with other threads
                                        lock (lockObject)
                                        {
                                            if (hash == null) // First valid hash wins
                                            {
                                                hash = newHash;
                                                nonce = nonce = foundNonce; // Store the valid nonce
                                            }
                                        }
                                }
                            }
                        }

                        if (hash == null)  nonce = nonce + results.Length;      // If no valid hash found set the nonce to go to the next range
                    }
                }
            }

            stopwatch.Stop(); // Stop timing

            return hash; // Return the first valid hash found
        }


        // Create reward for incentivising the mining of block
        public Transaction createRewardTransaction(List<Transaction> transactions) 

        {
            double fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee); // Sum all transaction fees
            return new Transaction("Mine Rewards", minerAddress, (reward + fees), 0, ""); // Issue reward as a transaction in the new block
        }

        // Encodes transactions within a block into a single hash 
        public static String MerkleRoot(List<Transaction> transactionList)			
        {
            List<String> hashes = transactionList.Select(t => t.hash).ToList();             // Get a list of transaction hashes for "combining" 
            
            // Handle Blocks based on the has count. 
            if (hashes.Count == 0) 							// No transactions 
            {
                return String.Empty;
            }

            if (hashes.Count == 1) 							// One transaction - hash with "self" 

            {
                return HashCode.HashTools.CombineHash(hashes[0], hashes[0]);
            }

            while (hashes.Count != 1) 							// Multiple transactions - Repeat until tree has been traversed 

            {

                List<String> merkleLeaves = new List<String>();                     // Keep track of current "level" of the tree 

                // Step over neighbouring pair combining each 
                for (int i = 0; i < hashes.Count; i += 2) 						
                {
                    if (i == hashes.Count - 1)

                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i])); 	// Handle an odd number of leaves 
                    }

                    else
                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i + 1]));	// Hash neighbours leaves 
                    }

                }

                hashes = merkleLeaves;							 // Update the working "layer" 

            }

            return hashes[0];								 // Return the root node 
        }

        // Gets the Difficulty
        public static int GetDifficulty()
        {
            return difficulty;
        }


        // Sets the Difficulty
        public static void SetDifficulty(int newDifficulty)
        {
            difficulty = newDifficulty;
        }


        // Gets the thread number
        public static int GetThreadNumber()
        {
            return threadNumber;
        }


        // Sets the thread number
        public static void SetThreadNumber(int newThreadNumber)
        {
            threadNumber = newThreadNumber;
        }

        // Sets which mining is done
        public static void SetWhichMine(int newWhichMine)
        {
            whichMine = newWhichMine;
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
                + "\nMerkle Root: " + merkleRoot
                + "\n-- " + transactionList.Count + " Transactions --"
                + "\n\n-- " + transactionList.Count + " Transactions --"
                + "\n\n" + string.Join("\n\n", transactionList)
                + "\n\n Time ten to generate block " + stopwatch.Elapsed
                + "\n[BLOCK END]";

            return str;
        }

    }
}
