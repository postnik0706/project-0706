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
            if (File.Exists(OAuthUtility.AccessTokenFilename))
                File.Delete(OAuthUtility.AccessTokenFilename);

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
            Directory.CreateDirectory(Path.GetDirectoryName(OAuthUtility.AccessTokenFilename));
            using (FileStream fs = new FileStream(OAuthUtility.AccessTokenFilename, FileMode.OpenOrCreate))
            {
                using (TextWriter tr = new StreamWriter(fs))
                {
                    tr.Write(accessToken);
                }
            }
            edAccessToken.Text = accessToken;
        }

        private void btnMakeACall_Click(object sender, EventArgs e)
        {
            NameValueCollection accessResponse = HttpUtility.ParseQueryString(
                File.ReadAllText(OAuthUtility.AccessTokenFilename));

            //string command = "users/current";
            //string command = "users/" + accessResponse["user_id"];
            string command = "catalog/titles?term=Jackie Chan&start_index=0&max_results=100";
            //string command = "catalog/people?term=Jackie Chan&start_index=0&max_results=100";
            //string command = "catalog/people/30004299/filmography";
            
            string result = OAuthUtility.MakeACall(command);
            edResponse.Text = result;
        }
    }
}
