using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using Utility;
using System.IO;
using System.Configuration;
using eBay.Service.Util;
using System.Threading;

namespace eBayTest
{
    public delegate void RefreshControlEvent(string Contents);

    public partial class Form1 : Form
    {
        LogWatcher logWatcher;
        public RefreshControlEvent OnRefresh;
        private List<Transaction> tranList;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eBayDate = "";
            string timeDiff = "";
            
            ThreadPool.QueueUserWorkItem(t =>
            {
                eBayDate = String.Format("{0:F}", eBayClass.EBayDate);
                timeDiff = eBayClass.TimeDiff.Hours.ToString();
                eBayClass.LogManager.RecordMessage(String.Format("eBayDate = {0}, timeDiff = {1}",
                    eBayDate, timeDiff));
                eBayClass.LogFileAccess.Set();
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OnRefresh = new RefreshControlEvent(RefreshControl);
            logWatcher = new LogWatcher(ConfigurationManager.AppSettings["eBayLogger"],
                this, log);
            tranList = new List<Transaction>();
            eBayClass.LogFileAccess.Set();
        }

        private void RefreshControl(string Contents)
        {
            if ((log.Text.Length < Contents.Replace("\r\n", "\n").Length)
                && (log.Text.Length != 0))
            {
                log.AppendText(Contents.Replace("\r\n", "\n").Substring(log.Text.Length));
            }
            else if ((log.Text.Length == 0) || (Contents == ""))
                log.Text = Contents;

            log.SelectionStart = log.Text.Length;
            log.SelectionLength = 0;
            log.ScrollToCaret();
        }
        
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(ConfigurationManager.AppSettings["eBayLogger"], "");
            }
            finally
            {
                eBayClass.LogFileAccess.Set();
            }
        }

        private void NormalizeDates(out DateTime DateFrom, out DateTime DateTo)
        {
            DateFrom = new DateTime(cnDateFrom.Value.Year, cnDateFrom.Value.Month,
                cnDateFrom.Value.Day);
            DateTo = new DateTime(cnDateTo.Value.Year, cnDateTo.Value.Month,
                cnDateTo.Value.Day, 23, 59, 59);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DateTime dtFrom, dtTo;
            NormalizeDates(out dtFrom, out dtTo);
            ThreadPool.QueueUserWorkItem(t =>
                {
                    int numOfItems = eBayClass.GetNumberOfItems(eBayClass.SellerContext,
                        dtFrom, dtTo, cbActive.Checked, cbCompleted.Checked);
                    eBayClass.LogManager.RecordMessage(String.Format("Number of Items = {0}",
                        numOfItems));
                    eBayClass.LogFileAccess.Set();
                });
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DateTime dtFrom, dtTo;
            NormalizeDates(out dtFrom, out dtTo);
            ThreadPool.QueueUserWorkItem(t =>
                {
                    tranList = eBayClass.GetOrders(new GetOrdersCall_(eBayClass.SellerContext),
                        dtFrom, dtTo, cbActive.Checked, cbCompleted.Checked);
                });
        }

        private void btnDoSale_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 200; i++)
            {
                eBayClass.SellerContext.ApiLogManager.RecordMessage(String.Format("Buying cycle... {0} - START", i), MessageType.Information, MessageSeverity.Informational);
                eBayClass.PlaceOffer(eBayClass.BuyerContext, edItemId.Text, Double.Parse(edPrice.Text));
                eBayClass.SellerContext.ApiLogManager.RecordMessage(String.Format("Buying cycle... {0} - SUCCESS", i), MessageType.Information, MessageSeverity.Informational);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dtFrom, dtTo;
            NormalizeDates(out dtFrom, out dtTo);
            ThreadPool.QueueUserWorkItem(t =>
                {
                    tranList = eBayClass.GetOrders(new GetOrdersCall_(eBayClass.SellerContext),
                        dtFrom, dtTo, cbActive.Checked, cbCompleted.Checked,
                        true);
                });
        }

        private void btnGetItemTransactions_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(t =>
                {
                    eBayClass.GetItemTransactions(eBayClass.SellerContext, edItemId_Get.Text);
                });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logWatcher.Quit = true;
        }

        private void btnGetSellingManagerSaleRecord_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(t =>
                {
                    eBayClass.GetSellingManagerSaleRecord(eBayClass.SellerContext, edRecordNumber.Text);
                });
        }

        private void btnGetOrderTransactions_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(t =>
                {
                    eBayClass.GetOrderTransactions(eBayClass.SellerContext, edTranItemID.Text, edTransactionID.Text);
                });
        }
    }
}
