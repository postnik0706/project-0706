namespace Netflix
{
    partial class Form1
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
            this.btnGetToken = new System.Windows.Forms.Button();
            this.edToken = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.edNormalizedURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edParameters = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.edURL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.edRequest = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGetToken
            // 
            this.btnGetToken.Location = new System.Drawing.Point(0, 0);
            this.btnGetToken.Name = "btnGetToken";
            this.btnGetToken.Size = new System.Drawing.Size(75, 23);
            this.btnGetToken.TabIndex = 0;
            this.btnGetToken.Text = "Get Token";
            this.btnGetToken.UseVisualStyleBackColor = true;
            this.btnGetToken.Click += new System.EventHandler(this.button1_Click);
            // 
            // edToken
            // 
            this.edToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edToken.Location = new System.Drawing.Point(206, 0);
            this.edToken.Name = "edToken";
            this.edToken.Size = new System.Drawing.Size(357, 20);
            this.edToken.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Token";
            // 
            // edNormalizedURL
            // 
            this.edNormalizedURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edNormalizedURL.Location = new System.Drawing.Point(206, 26);
            this.edNormalizedURL.Name = "edNormalizedURL";
            this.edNormalizedURL.Size = new System.Drawing.Size(357, 20);
            this.edNormalizedURL.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Normalized URL";
            // 
            // edParameters
            // 
            this.edParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edParameters.Location = new System.Drawing.Point(206, 52);
            this.edParameters.Name = "edParameters";
            this.edParameters.Size = new System.Drawing.Size(357, 20);
            this.edParameters.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Parameters";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(104, 150);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(84, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Sign In to Netflix";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // edURL
            // 
            this.edURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edURL.Location = new System.Drawing.Point(206, 147);
            this.edURL.Name = "edURL";
            this.edURL.ReadOnly = true;
            this.edURL.Size = new System.Drawing.Size(357, 20);
            this.edURL.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Request";
            // 
            // edRequest
            // 
            this.edRequest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edRequest.Location = new System.Drawing.Point(206, 78);
            this.edRequest.Name = "edRequest";
            this.edRequest.Size = new System.Drawing.Size(357, 20);
            this.edRequest.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 262);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.edRequest);
            this.Controls.Add(this.edURL);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edParameters);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edNormalizedURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edToken);
            this.Controls.Add(this.btnGetToken);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetToken;
        private System.Windows.Forms.TextBox edToken;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edNormalizedURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edParameters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox edURL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edRequest;
    }
}

