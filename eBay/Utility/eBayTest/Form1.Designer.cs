namespace eBayTest
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnEBayTime = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.edItemId_Get = new System.Windows.Forms.TextBox();
            this.btnGetItemTransactions = new System.Windows.Forms.Button();
            this.edPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.edItemId = new System.Windows.Forms.TextBox();
            this.btnDoSale = new System.Windows.Forms.Button();
            this.cbCompleted = new System.Windows.Forms.CheckBox();
            this.cbActive = new System.Windows.Forms.CheckBox();
            this.cnDateTo = new System.Windows.Forms.DateTimePicker();
            this.cnDateFrom = new System.Windows.Forms.DateTimePicker();
            this.btnGetMinimum = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.log = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClearLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.log);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(686, 389);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(314, 389);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnEBayTime);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.edItemId_Get);
            this.tabPage1.Controls.Add(this.btnGetItemTransactions);
            this.tabPage1.Controls.Add(this.edPrice);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.edItemId);
            this.tabPage1.Controls.Add(this.btnDoSale);
            this.tabPage1.Controls.Add(this.cbCompleted);
            this.tabPage1.Controls.Add(this.cbActive);
            this.tabPage1.Controls.Add(this.cnDateTo);
            this.tabPage1.Controls.Add(this.cnDateFrom);
            this.tabPage1.Controls.Add(this.btnGetMinimum);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.btnGo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(306, 363);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Operations";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnEBayTime
            // 
            this.btnEBayTime.Location = new System.Drawing.Point(8, 7);
            this.btnEBayTime.Name = "btnEBayTime";
            this.btnEBayTime.Size = new System.Drawing.Size(137, 23);
            this.btnEBayTime.TabIndex = 16;
            this.btnEBayTime.Text = "eBay time";
            this.btnEBayTime.UseVisualStyleBackColor = true;
            this.btnEBayTime.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(151, 279);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Item Id";
            // 
            // edItemId_Get
            // 
            this.edItemId_Get.Location = new System.Drawing.Point(200, 276);
            this.edItemId_Get.Name = "edItemId_Get";
            this.edItemId_Get.Size = new System.Drawing.Size(100, 20);
            this.edItemId_Get.TabIndex = 14;
            // 
            // btnGetItemTransactions
            // 
            this.btnGetItemTransactions.Location = new System.Drawing.Point(3, 274);
            this.btnGetItemTransactions.Name = "btnGetItemTransactions";
            this.btnGetItemTransactions.Size = new System.Drawing.Size(142, 23);
            this.btnGetItemTransactions.TabIndex = 13;
            this.btnGetItemTransactions.Text = "GetItemTransactions";
            this.btnGetItemTransactions.UseVisualStyleBackColor = true;
            this.btnGetItemTransactions.Click += new System.EventHandler(this.btnGetItemTransactions_Click);
            // 
            // edPrice
            // 
            this.edPrice.Location = new System.Drawing.Point(200, 234);
            this.edPrice.Name = "edPrice";
            this.edPrice.Size = new System.Drawing.Size(100, 20);
            this.edPrice.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(151, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Price";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 214);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Item Id";
            // 
            // edItemId
            // 
            this.edItemId.Location = new System.Drawing.Point(200, 211);
            this.edItemId.Name = "edItemId";
            this.edItemId.Size = new System.Drawing.Size(100, 20);
            this.edItemId.TabIndex = 9;
            // 
            // btnDoSale
            // 
            this.btnDoSale.Location = new System.Drawing.Point(3, 209);
            this.btnDoSale.Name = "btnDoSale";
            this.btnDoSale.Size = new System.Drawing.Size(142, 23);
            this.btnDoSale.TabIndex = 8;
            this.btnDoSale.Text = "Make sales (x200)";
            this.btnDoSale.UseVisualStyleBackColor = true;
            this.btnDoSale.Click += new System.EventHandler(this.btnDoSale_Click);
            // 
            // cbCompleted
            // 
            this.cbCompleted.AutoSize = true;
            this.cbCompleted.Checked = true;
            this.cbCompleted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCompleted.Location = new System.Drawing.Point(174, 86);
            this.cbCompleted.Name = "cbCompleted";
            this.cbCompleted.Size = new System.Drawing.Size(76, 17);
            this.cbCompleted.TabIndex = 6;
            this.cbCompleted.Text = "Completed";
            this.cbCompleted.UseVisualStyleBackColor = true;
            // 
            // cbActive
            // 
            this.cbActive.AutoSize = true;
            this.cbActive.Checked = true;
            this.cbActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActive.Location = new System.Drawing.Point(174, 71);
            this.cbActive.Name = "cbActive";
            this.cbActive.Size = new System.Drawing.Size(56, 17);
            this.cbActive.TabIndex = 5;
            this.cbActive.Text = "Active";
            this.cbActive.UseVisualStyleBackColor = true;
            // 
            // cnDateTo
            // 
            this.cnDateTo.Location = new System.Drawing.Point(174, 46);
            this.cnDateTo.Name = "cnDateTo";
            this.cnDateTo.Size = new System.Drawing.Size(126, 20);
            this.cnDateTo.TabIndex = 4;
            // 
            // cnDateFrom
            // 
            this.cnDateFrom.Location = new System.Drawing.Point(32, 46);
            this.cnDateFrom.Name = "cnDateFrom";
            this.cnDateFrom.Size = new System.Drawing.Size(113, 20);
            this.cnDateFrom.TabIndex = 3;
            // 
            // btnGetMinimum
            // 
            this.btnGetMinimum.Location = new System.Drawing.Point(3, 139);
            this.btnGetMinimum.Name = "btnGetMinimum";
            this.btnGetMinimum.Size = new System.Drawing.Size(142, 23);
            this.btnGetMinimum.TabIndex = 2;
            this.btnGetMinimum.Text = "Get minimum data";
            this.btnGetMinimum.UseVisualStyleBackColor = true;
            this.btnGetMinimum.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 110);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Get # of Orders";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(3, 168);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(142, 23);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "GetOrders";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(306, 363);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Utility";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.log.Location = new System.Drawing.Point(0, 0);
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.Size = new System.Drawing.Size(368, 357);
            this.log.TabIndex = 3;
            this.log.Text = "";
            this.log.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClearLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 357);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 32);
            this.panel1.TabIndex = 4;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(265, 5);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(91, 23);
            this.btnClearLog.TabIndex = 0;
            this.btnClearLog.Text = "Clear";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 389);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.RichTextBox log;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGetMinimum;
        private System.Windows.Forms.DateTimePicker cnDateTo;
        private System.Windows.Forms.DateTimePicker cnDateFrom;
        private System.Windows.Forms.CheckBox cbCompleted;
        private System.Windows.Forms.CheckBox cbActive;
        private System.Windows.Forms.Button btnDoSale;
        private System.Windows.Forms.TextBox edItemId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGetItemTransactions;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox edItemId_Get;
        private System.Windows.Forms.Button btnEBayTime;
    }
}

