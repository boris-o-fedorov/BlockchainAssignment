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
    public static void Sha256Kernel(Index1D index, ArrayView<byte> output, ArrayView<uint> kConstants, long nonce, int difficulty)
    {
        long enonce = nonce + index;   // gets the enonce for each thread
        byte[] message = new byte[8]; // Convert the nonce to an 8-byte array in big-endian format.
        for (int i = 0; i < 8; i++)
        {
            message[7 - i] = (byte)((enonce >> (8 * i)) & 0xFF);
        }

        uint a = 0x6a09e667;
        uint b = 0xbb67ae85;
        uint c = 0x3c6ef372;
        uint d = 0xa54ff53a;
        uint e = 0x510e527f;
        uint f = 0x9b05688c;
        uint g = 0x1f83d9ab;
        uint h = 0x5be0cd19;

        uint[] w = new uint[64];
        for (int i = 0; i < 16; i++)
        {
            w[i] = (uint)(message[i * 4] << 24 |
                          message[i * 4 + 1] << 16 |
                          message[i * 4 + 2] << 8 |
                          message[i * 4 + 3]);
        }
        for (int i = 16; i < 64; i++)
        {
            uint s0 = RotateRight(w[i - 15], 7) ^ RotateRight(w[i - 15], 18) ^ (w[i - 15] >> 3);
            uint s1 = RotateRight(w[i - 2], 17) ^ RotateRight(w[i - 2], 19) ^ (w[i - 2] >> 10);
            w[i] = w[i - 16] + s0 + w[i - 7] + s1;
        }

        for (int i = 0; i < 64; i++)
        {
            uint S1 = RotateRight(e, 6) ^ RotateRight(e, 11) ^ RotateRight(e, 25);
            uint ch = (e & f) ^ ((~e) & g);
            uint temp1 = h + S1 + ch + kConstants[i] + w[i];
            uint S0 = RotateRight(a, 2) ^ RotateRight(a, 13) ^ RotateRight(a, 22);
            uint maj = (a & b) ^ (a & c) ^ (b & c);
            uint temp2 = S0 + maj;

            h = g;
            g = f;
            f = e;
            e = d + temp1;
            d = c;
            c = b;
            b = a;
            a = temp1 + temp2;
        }

        a += 0x6a09e667;
        b += 0xbb67ae85;
        c += 0x3c6ef372;
        d += 0xa54ff53a;
        e += 0x510e527f;
        f += 0x9b05688c;
        g += 0x1f83d9ab;
        h += 0x5be0cd19;

        int leadingZeros = CountLeadingZeros(a);
        output[index] = (byte)(leadingZeros >= difficulty ? 1 : 0);
    }

    private static uint RotateRight(uint x, int n)
    {
        return (x >> n) | (x << (32 - n));
    }

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