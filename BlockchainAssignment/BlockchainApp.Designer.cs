﻿namespace BlockchainAssignment
{
    partial class BlockchainApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.PrintBlock = new System.Windows.Forms.Button();
            this.blockNo = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.publicKey = new System.Windows.Forms.TextBox();
            this.privateKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.amount = new System.Windows.Forms.TextBox();
            this.fee = new System.Windows.Forms.TextBox();
            this.reciever = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ReciverKey = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBox1.Location = new System.Drawing.Point(16, 15);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(875, 386);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // PrintBlock
            // 
            this.PrintBlock.Location = new System.Drawing.Point(12, 421);
            this.PrintBlock.Name = "PrintBlock";
            this.PrintBlock.Size = new System.Drawing.Size(105, 31);
            this.PrintBlock.TabIndex = 1;
            this.PrintBlock.Text = "Display Block";
            this.PrintBlock.UseVisualStyleBackColor = true;
            this.PrintBlock.Click += new System.EventHandler(this.PrintBlock_Click);
            // 
            // blockNo
            // 
            this.blockNo.Location = new System.Drawing.Point(123, 425);
            this.blockNo.Name = "blockNo";
            this.blockNo.Size = new System.Drawing.Size(33, 22);
            this.blockNo.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(785, 408);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 56);
            this.button2.TabIndex = 3;
            this.button2.Text = "Generate Wallet";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.GenWallet_Click);
            // 
            // publicKey
            // 
            this.publicKey.Location = new System.Drawing.Point(336, 408);
            this.publicKey.Name = "publicKey";
            this.publicKey.Size = new System.Drawing.Size(416, 22);
            this.publicKey.TabIndex = 4;
            // 
            // privateKey
            // 
            this.privateKey.Location = new System.Drawing.Point(336, 442);
            this.privateKey.Name = "privateKey";
            this.privateKey.Size = new System.Drawing.Size(416, 22);
            this.privateKey.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 411);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Public Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 445);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Private Key";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(785, 472);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 27);
            this.button3.TabIndex = 8;
            this.button3.Text = "Validate Keys";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.ValKeys_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 534);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 46);
            this.button1.TabIndex = 9;
            this.button1.Text = "Create Transaction";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CreateTransaction_Click);
            // 
            // amount
            // 
            this.amount.Location = new System.Drawing.Point(187, 528);
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(51, 22);
            this.amount.TabIndex = 10;
            // 
            // fee
            // 
            this.fee.Location = new System.Drawing.Point(187, 558);
            this.fee.Name = "fee";
            this.fee.Size = new System.Drawing.Size(51, 22);
            this.fee.TabIndex = 11;
            // 
            // reciever
            // 
            this.reciever.Location = new System.Drawing.Point(356, 561);
            this.reciever.Name = "reciever";
            this.reciever.Size = new System.Drawing.Size(414, 22);
            this.reciever.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(129, 528);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Amount";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 561);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Fee";
            // 
            // ReciverKey
            // 
            this.ReciverKey.AutoSize = true;
            this.ReciverKey.Location = new System.Drawing.Point(273, 565);
            this.ReciverKey.Name = "ReciverKey";
            this.ReciverKey.Size = new System.Drawing.Size(77, 16);
            this.ReciverKey.TabIndex = 15;
            this.ReciverKey.Text = "ReciverKey";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(146, 469);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 45);
            this.button4.TabIndex = 16;
            this.button4.Text = "Generate New Block";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.GenerateNewBlock_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(162, 421);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(81, 31);
            this.button5.TabIndex = 17;
            this.button5.Text = "Read All";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.ReadAll_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(16, 469);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(124, 47);
            this.button6.TabIndex = 18;
            this.button6.Text = "Read Pending Transactions";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.PrintPendingTransactions_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(653, 472);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(117, 25);
            this.button7.TabIndex = 19;
            this.button7.Text = "Check Balance";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.CheckBalance_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(785, 505);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(106, 63);
            this.button8.TabIndex = 20;
            this.button8.Text = "Full Blockchain Validation";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Validate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Location = new System.Drawing.Point(253, 468);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 87);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generate Blocks with Criteria";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(7, 52);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(123, 30);
            this.button12.TabIndex = 22;
            this.button12.Text = "Use Your friends";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.Click_GenPersonal);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(154, 17);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(123, 30);
            this.button11.TabIndex = 21;
            this.button11.Text = "Use Oldest";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Click_GenOldest);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(7, 17);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(123, 30);
            this.button9.TabIndex = 19;
            this.button9.Text = "Use Largest";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Click_GenLargest);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(653, 505);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(117, 45);
            this.button10.TabIndex = 23;
            this.button10.Text = "Adapt Block Mining";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.adapt_block_mining);
            // 
            // BlockchainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(908, 592);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.ReciverKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.reciever);
            this.Controls.Add(this.fee);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.privateKey);
            this.Controls.Add(this.publicKey);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.blockNo);
            this.Controls.Add(this.PrintBlock);
            this.Controls.Add(this.richTextBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BlockchainApp";
            this.Text = "Blockchain App";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button PrintBlock;
        private System.Windows.Forms.TextBox blockNo;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox publicKey;
        private System.Windows.Forms.TextBox privateKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox amount;
        private System.Windows.Forms.TextBox fee;
        private System.Windows.Forms.TextBox reciever;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label ReciverKey;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
    }
}

