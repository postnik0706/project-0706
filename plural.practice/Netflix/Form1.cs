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

namespace Netflix
{
    public partial class Form1 : Form
    {
        string m_strNormalizedUrl;
        string m_strNormalizedRequestParameters;

        public string NormalizedUrl { get { return m_strNormalizedUrl; } }
        public string NormalizedRequestParameters { get { return m_strNormalizedRequestParameters; } }
        public NameValueCollection TokenResponse { get; set; }

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

            string strTokenResponse;
            using (WebClient w = new WebClient())
            {
                strTokenResponse = w.DownloadString(NormalizedUrl + "?" + NormalizedRequestParameters +
                    "&oauth_signature=" + HttpUtility.UrlEncode(token));
            }
            edRequest.Text = strTokenResponse;
            
            TokenResponse = HttpUtility.ParseQueryString(strTokenResponse);
            edSecretToken.Text = TokenResponse["oauth_token_secret"];

            string signInRequest = ConfigurationManager.AppSettings["netFlixLoginUrl"] + "?" +
                "oauth_token=" + TokenResponse["oauth_token"] +
                "&application_name=" + TokenResponse["application_name"] +
                "&oauth_callback=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["OAuthCallback"]) +
                "&oauth_consumer_key=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["consumerKey"]);
            edURL.Text = signInRequest;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(edURL.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string accessSecret = ConfigurationManager.AppSettings["sharedSecret"] +
                TokenResponse["oauth_token_secret"];
            
            OAuthBase oAuth = new OAuthBase();
            string m_strNormalizedUrl, m_strNormalizedRequestParameters;
            string accessToken = oAuth.GenerateSignature(new Uri(ConfigurationManager.AppSettings["netFlixAccessToken"]),
                ConfigurationManager.AppSettings["consumerKey"],
                accessSecret,
                TokenResponse["oauth_token"],
                TokenResponse["oauth_token_secret"],
                "GET", oAuth.GenerateTimeStamp(), oAuth.GenerateNonce(),
                out m_strNormalizedUrl, out m_strNormalizedRequestParameters);

            string accessTokenResponse = "";
            using (WebClient w = new WebClient())
            {
                accessTokenResponse = w.DownloadString(NormalizedUrl + "?" + NormalizedRequestParameters +
                    "&oauth_signature=" + HttpUtility.UrlEncode(accessToken));
            }
            edAccessToken.Text = accessTokenResponse;
        }
    }
}
