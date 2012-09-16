using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OAuth;
using System.Configuration;
using System.Web;
using System.Net;

namespace Netflix
{
    public partial class Form1 : Form
    {
        string m_strNormalizedUrl;
        string m_strNormalizedRequestParameters;

        public string NormalizedUrl { get { return m_strNormalizedUrl; } }
        public string NormalizedRequestParameters { get { return m_strNormalizedRequestParameters; } }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OAuthBase oAuth = new OAuthBase();
            string token = oAuth.GenerateSignature(new Uri(ConfigurationManager.AppSettings["netflixUrl"]),
                ConfigurationManager.AppSettings["consumerKey"],
                ConfigurationManager.AppSettings["sharedSecret"],
                null, null, "GET", oAuth.GenerateTimeStamp(), oAuth.GenerateNonce(),
                out m_strNormalizedUrl, out m_strNormalizedRequestParameters);
            edToken.Text = token;
            edNormalizedURL.Text = NormalizedUrl;
            edParameters.Text = NormalizedRequestParameters;

            string request;
            using (WebClient w = new WebClient())
            {
                request = w.DownloadString(NormalizedUrl + "?" + NormalizedRequestParameters +
                    "&oauth_signature=" + HttpUtility.UrlEncode(token));
            }
            edRequest.Text = request;


/*
            var parameters = HttpUtility.ParseQueryString(NormalizedRequestParameters);
            string requestString = ConfigurationManager.AppSettings["netFlixLoginUrl"] + "?" +
                "oauth_token=" + parameters["oauth_token"] +
                "&oauth_consumer_key=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["consumerKey"]) +
                "&application_name=" + parameters["application_name"] +
                "&oauth_callback=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["OAuthCallback"]);
            edURL.Text = requestString;
 */
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(edURL.Text);
        }
    }
}
