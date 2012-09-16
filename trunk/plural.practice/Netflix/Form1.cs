using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.IO;

namespace Netflix
{
    public partial class Form1 : Form
    {
        public string Token { get; set; }
        public string TokenSecret { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(accessTokenFilename))
                File.Delete(accessTokenFilename);

            var req = OAuthUtility.GenerateTokenRequest();
            Token = req.Token;
            edToken.Text = Token;
            TokenSecret = req.TokenSecret;
            edTokenSecret.Text = TokenSecret;
            edNormalizedURL.Text = req.Url;
            edParameters.Text = req.Parameters;
            edRequest.Text = req.Token;
            edURL.Text = req.SignInRequest;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(edURL.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string accessToken = OAuthUtility.GenerateToken(Token, TokenSecret);
            File.WriteAllText(OAuthUtility.AccessTokenFilename, accessToken);
        }

        private void btnMakeACall_Click(object sender, EventArgs e)
        {
            //string command = "current";
            string command = accessResponse["user_id"];

            OAuthBase oAuth = new OAuthBase();
            string m_strNormalizedUrl, m_strNormalizedRequestParameters;
            string accessToken = oAuth.GenerateSignature(
                new Uri(ConfigurationManager.AppSettings["netFlixEndpoint"] + command),
                ConfigurationManager.AppSettings["consumerKey"],
                ConfigurationManager.AppSettings["sharedSecret"],
                accessResponse["oauth_token"],
                accessResponse["oauth_token_secret"],
                "GET", oAuth.GenerateTimeStamp(), oAuth.GenerateNonce(),
                out m_strNormalizedUrl, out m_strNormalizedRequestParameters);

            using (WebClient w = new WebClient())
            {
                string callRequest = m_strNormalizedUrl + "?" + m_strNormalizedRequestParameters +
                    "&oauth_signature=" + HttpUtility.UrlEncode(accessToken);
                string strCallResponse = w.DownloadString(callRequest);
                edResponse.Text = strCallResponse;
            }
        }
    }
}
