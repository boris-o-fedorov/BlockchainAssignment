using ILGPU;
using ILGPU.Runtime;
using System;

public static class Sha256GpuKernel
{
    /* 
     * Compute sha256 hashes for the GPU.
     * This gives a simplified version of Sha256 for demonstration purposes,
     * as using GPU mining requires seperate functions as it can not excecute CPU functions.
     * 
     * index is the thread index for each thread
     * output is the output return value indicating if the hash returns the needed degree of difficulty
     * kconstants returns the SHA-256 constants
     * The nonce is the staring main nonce for each of the threads
     * difficulty is the required degree of difficulty
     */
    public static void Sha256Kernel(Index1D threadId, ArrayView<byte> output, ArrayView<uint> kConstants, long nonce, int difficulty, ArrayView<byte> indexBytes, ArrayView<byte> timestampBytes, ArrayView<byte> prevHashBytes, ArrayView<byte> merkleBytes)
    {
        long enonce = nonce + threadId;   // gets the enonce for each thread

        // Convert enonce to ASCII bytes 
        byte[] enonceAscii = new byte[20];
        int enonceLength = 0;
        long temp = enonce;

        // Convert enonce to ASCII bytes
        do
        {
            enonceAscii[enonceLength++] = (byte)('0' + (temp % 10));
            temp /= 10;
        } while (temp > 0);

        // Reverse to fix endianness 
        for (int i = 0; i < enonceLength / 2; i++)
        {
            byte tmp = enonceAscii[i];
            enonceAscii[i] = enonceAscii[enonceLength - 1 - i];
            enonceAscii[enonceLength - 1 - i] = tmp;
        }

        // Calculate total length
        long totalLength = indexBytes.Length +
                         timestampBytes.Length +
                         prevHashBytes.Length +
                         enonceLength +
                         merkleBytes.Length;

        // Combine all bytes
        byte[] combined = new byte[256];
        int pos = 0;

        // Copy index
        for (int i = 0; i < indexBytes.Length; i++)
            combined[pos++] = indexBytes[i];

        // Copy timestamp
        for (int i = 0; i < timestampBytes.Length; i++)
            combined[pos++] = timestampBytes[i];

        // Copy previous hash
        for (int i = 0; i < prevHashBytes.Length; i++)
            combined[pos++] = prevHashBytes[i];

        // Copy enonce
        for (int i = 0; i < enonceLength; i++)
            combined[pos++] = enonceAscii[i];

        // Copy merkle root
        for (int i = 0; i < merkleBytes.Length; i++)
            combined[pos++] = merkleBytes[i];


        // Begin SHA-256 Padding (simplified for ≤ 55-byte messages)
        int messageLen = combined.Length;
        int paddedLen = ((messageLen + 9 + 63) / 64) * 64;
        byte[] padded = new byte[256];

        for (int i = 0; i < messageLen; i++)
            padded[i] = combined[i];

        padded[messageLen] = 0x80;
        ulong bitLen = (ulong)messageLen * 8;

        // Append original length in bits (big-endian)
        for (int i = 0; i < 8; i++)
            padded[paddedLen - 1 - i] = (byte)((bitLen >> (8 * i)) & 0xFF);

        // SHA-256 initial hash values
        uint a = 0x6a09e667;
        uint b = 0xbb67ae85;
        uint c = 0x3c6ef372;
        uint d = 0xa54ff53a;
        uint e = 0x510e527f;
        uint f = 0x9b05688c;
        uint g = 0x1f83d9ab;
        uint h = 0x5be0cd19;

        // Message schedule array
        uint[] w = new uint[64];

        // Process the message in successive 64-byte chunks
        for (int chunkStart = 0; chunkStart < paddedLen; chunkStart += 64)
        {
            // Break the chunk into sixteen 32-bit big-endian words
            for (int i = 0; i < 16; i++)
            {
                int j = chunkStart + i * 4;
                w[i] = (uint)(padded[j] << 24 | padded[j + 1] << 16 | padded[j + 2] << 8 | padded[j + 3]);
            }

            // Extend the first 16 words into the remaining 48 words
            for (int i = 16; i < 64; i++)
            {
                uint s0 = RotateRight(w[i - 15], 7) ^ RotateRight(w[i - 15], 18) ^ (w[i - 15] >> 3);
                uint s1 = RotateRight(w[i - 2], 17) ^ RotateRight(w[i - 2], 19) ^ (w[i - 2] >> 10);
                w[i] = w[i - 16] + s0 + w[i - 7] + s1;
            }

            // Initialise working variables with current hash value
            uint A = a, B = b, C = c, D = d, E = e, F = f, G = g, H = h;

            // Compression loop loop
            for (int i = 0; i < 64; i++)
            {
                uint S1 = RotateRight(E, 6) ^ RotateRight(E, 11) ^ RotateRight(E, 25);      // Bitwise operations for diffusion
                uint ch = (E & F) ^ (~E & G);                                               // If E then F else G
                uint temp1 = H + S1 + ch + kConstants[i] + w[i];                            // The current round constant and message schedule
                uint S0 = RotateRight(A, 2) ^ RotateRight(A, 13) ^ RotateRight(A, 22);      // Bitwise operations for diffusion
                uint maj = (A & B) ^ (A & C) ^ (B & C);                                     // Majority function of A, B, C
                uint temp2 = S0 + maj;                                                      // Combine S0 and majority

                // Update working variables through rotation pipeline
                H = G;
                G = F;
                F = E;
                E = D + temp1;
                D = C;
                C = B;
                B = A;
                A = temp1 + temp2;
            }

            // Add the compressed chunk to the current hash value
            a += A;
            b += B;
            c += C;
            d += D;
            e += E;
            f += F;
            g += G;
            h += H;
        }

        // Check for difficulty: count leading zero bits in the final hash (starting with 'a')
        int zeroBits = CountLeadingZeros(a);
        if (zeroBits == 32) zeroBits += CountLeadingZeros(b);
        if (zeroBits == 64) zeroBits += CountLeadingZeros(c);
        if (zeroBits == 96) zeroBits += CountLeadingZeros(d);
        if (zeroBits == 128) zeroBits += CountLeadingZeros(e);
        if (zeroBits == 160) zeroBits += CountLeadingZeros(f);
        if (zeroBits == 192) zeroBits += CountLeadingZeros(g);
        if (zeroBits == 224) zeroBits += CountLeadingZeros(h);

        output[threadId] = (byte)(zeroBits >= difficulty ? 1 : 0);
    }

    // Peforms a rotate right
    private static uint RotateRight(uint x, int n)
    {
        return (x >> n) | (x << (32 - n));
    }

    // Count the leading zeroes to see if it satisfies the degree of difficulty
    private static int CountLeadingZeros(uint x)
    {
        int count = 0;
        for (int i = 31; i >= 0; i--)
        {
            if ((x & (1u << i)) == 0)
                count++;
            else
                break;
        }
        return count;
    }
}



