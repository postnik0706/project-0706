using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using Utility;
using System.IO;
using System.Configuration;

namespace eBayTest
{
    public delegate void RefreshControlEvent(string Contents);

    public partial class Form1 : Form
    {
        LogWatcher logWatcher;
        public RefreshControlEvent OnRefresh;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ApiContext apiCtxSeller = eBayClass.GetContext(eBayClass.SellerOrBuyer.typeSELLER);
            GeteBayOfficialTimeCall timeCall = new GeteBayOfficialTimeCall(apiCtxSeller);
            DateTime eBayTime = timeCall.GeteBayOfficialTime();
            TimeSpan timeDiff = DateTime.Now - eBayTime;
            edeBayTime.Text = String.Format("{0:F}", eBayTime);
            eBayClass.Metrics.GenerateReport(eBayClass.LogManager.ApiLoggerList[0]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OnRefresh = new RefreshControlEvent(RefreshControl);
            logWatcher = new LogWatcher(ConfigurationManager.AppSettings["eBayLogger"],
                this, log);
        }

        private void RefreshControl(string Contents)
        {
            if ((log.Text.Length < Contents.Length) && (log.Text.Length != 0))
            {
                log.AppendText(Contents.Substring(log.Text.Length));
            }
            else
                log.Text = Contents;

            log.SelectionStart = log.Text.Length;
            log.SelectionLength = 0;
            log.ScrollToCaret();
        }
        
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            logWatcher.Pause();
            try
            {
                File.WriteAllText(ConfigurationManager.AppSettings["eBayLogger"], "");
            }
            finally
            {
                logWatcher.Resume();
            }
        }
    }
}
