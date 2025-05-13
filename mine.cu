#include "sha256.cuh"

extern "C"
__global__ void mine(long* nonces, unsigned char* results, int difficulty, long index, long timestamp, const char* previousHash, const char* merkleRoot)
{
    // Calculate the unique index based on the block and thread dimensions
    int idx = blockIdx.x * blockDim.x + threadIdx.x;

    // Retrieve the nonce that this thread is responsible for
    long nonce = nonces[idx];

    // Buffer to store the concatenated input string (nonce, index, timestamp, previousHash, merkleRoot)
    char input[256];
    int len = 0;

    // Concatenate all the input values into the buffer (index, timestamp, previousHash, nonce, merkleRoot)
    len += sprintf(input + len, "%ld", index); // Index
    len += sprintf(input + len, "%ld", timestamp); // Timestamp
    len += sprintf(input + len, "%s", previousHash); // Previous Hash
    len += sprintf(input + len, "%ld", nonce); // Nonce
    len += sprintf(input + len, "%s", merkleRoot); // Merkle Root

    // SHA256 hash output (32 bytes)
    unsigned char hash[32];

    // Compute the hash
    sha256_kernel((unsigned char*)input, len, hash);  // sha256_kernel is a function in cuSHA

    // Variable to count the number of leading zero bits in the hash
    int leadingZeros = 0;

    // Loop through the bytes of the hash
    for (int i = 0; i < 32; ++i)
    {
        // Loop through each bit of the current byte (8 bits per byte)
        for (int b = 7; b >= 0; --b)
        {
            // Check if the bit is 1
            if ((hash[i] >> b) & 1) {
                // If the bit is 1, stop counting leading zeros
                i = 32; // Exit the loop early
                break;
            }
            
            // If the bit is 0, increment the leading zero count
            leadingZeros++;

            // If the leading zeros count has reached the difficulty requirement, set the result to 1 (valid)
            if (leadingZeros >= difficulty) {
                results[idx] = 1; // Mark the result as valid (1)
                return; // Exit the kernel
            }
        }
    }

    // If we exit the loop and don't find enough leading zeros, mark the result as invalid (0)
    results[idx] = 0;
}