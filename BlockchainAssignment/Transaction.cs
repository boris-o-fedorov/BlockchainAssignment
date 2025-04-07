using System;
using System.Collections.Generic;
using System.Linq; 
using System.Security.Cryptography; 
using System.Text; 
using System.Threading.Tasks; 

 

namespace BlockchainAssignment

{
    class Transaction
    {

        /* Transaction Variables */
        public DateTime timestamp; 			    // Time of creation 
        public string senderAddress;            // Sender's address
        public string recipientAddress; 		// Participants public key addresses 
        public double amount;                   // The amount of currency being sent to the receiver
        public double fee; 		                // The fee added to the transaction 
        public string hash;                     // The hash of the transaction
        public string signature;                // The hash signed with the private key of the sender


        /* Transaction Constructor */
        public Transaction(string from, string to, double amount, double fee, string privateKey)
        {

            // Assign variables
            this.timestamp = DateTime.Now;
            this.senderAddress = from;
            this.recipientAddress = to;
            this.amount = amount;
            this.fee = fee;

            this.hash = CreateHash();						// Hash the transaction attributes 
            this.signature = Wallet.Wallet.CreateSignature(from, privateKey, hash); 		// Sign the hash with the senders private key ensuring validity 

        }



        /* Transaction Functions */
        public String CreateHash()						// Hash the transaction attributes using SHA256 

        {

            String hash = String.Empty;

            SHA256 hasher = SHA256Managed.Create();



            String input = timestamp + senderAddress + recipientAddress + amount + fee; 	// Concatenate all transaction properties 

            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));	// Apply the hash function to the "input" string 

            foreach (byte x in hashByte)

                hash += String.Format("{0:x2}", x);                 // Reformat to a string 



            return hash;

        }

        public override string ToString()						// Represent a transaction as a string for output to UI 

        {

            return "  [TRANSACTION START]"               + "\n  Timestamp: " + timestamp                + "\n  -- Verification --"                + "\n  Hash: " + hash                + "\n  Signature: " + signature               + "\n  -- Quantities --"                + "\n  Transferred: " + amount + " Nova Coin"                + "\t  Fee: " + fee               + "\n  -- Participants --"                + "\n  Sender: " + senderAddress                + "\n  Reciever: " + recipientAddress                + "\n  [TRANSACTION END]";

        }

    }

}