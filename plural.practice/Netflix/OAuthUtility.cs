﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth;
using System.Configuration;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using System.IO;

namespace Netflix
{
    class ProtectedRequest
    {
        public string Url { get; set; }
        public string Parameters { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public string SignInRequest { get; set; }
    }
    
    class OAuthUtility
    {
        public const string AccessTokenFilename = "accessToken";

        internal static ProtectedRequest GenerateTokenRequest()
        {
            OAuthBase oAuth = new OAuthBase();
            string strNormalizedUrl, strNormalizedRequestParameters;
            string token = oAuth.GenerateSignature(new Uri(ConfigurationManager.AppSettings["netflixUrl"]),
                ConfigurationManager.AppSettings["consumerKey"],
                ConfigurationManager.AppSettings["sharedSecret"],
                null, null, "GET", oAuth.GenerateTimeStamp(), oAuth.GenerateNonce(),
                out strNormalizedUrl, out strNormalizedRequestParameters);

            string strTokenResponse;
            using (WebClient w = new WebClient())
            {
                strTokenResponse = w.DownloadString(strNormalizedUrl + "?" + strNormalizedRequestParameters +
                    "&oauth_signature=" + HttpUtility.UrlEncode(token));
            }

            NameValueCollection TokenResponse = HttpUtility.ParseQueryString(strTokenResponse);
            string signInRequest = ConfigurationManager.AppSettings["netFlixLoginUrl"] + "?" +
                "oauth_token=" + TokenResponse["oauth_token"] +
                "&application_name=" + TokenResponse["application_name"] +
                "&oauth_callback=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["OAuthCallback"]) +
                "&oauth_consumer_key=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["consumerKey"]);

            return new ProtectedRequest() { Token = TokenResponse["oauth_token"],
                TokenSecret = TokenResponse["oauth_token_secret"],
                Url = strNormalizedUrl,
                Parameters = strNormalizedRequestParameters,
                SignInRequest = signInRequest
            };
        }

        internal static string GenerateToken(string OAuthToken, string OAuthTokenSecret)
        {
            OAuthBase oAuth = new OAuthBase();
            string m_strNormalizedUrl, m_strNormalizedRequestParameters;
            string accessToken = oAuth.GenerateSignature(new Uri(ConfigurationManager.AppSettings["netFlixAccessToken"]),
                ConfigurationManager.AppSettings["consumerKey"],
                ConfigurationManager.AppSettings["sharedSecret"],
                OAuthToken,
                OAuthTokenSecret,
                "GET", oAuth.GenerateTimeStamp(), oAuth.GenerateNonce(),
                out m_strNormalizedUrl, out m_strNormalizedRequestParameters);
            
            string accessTokenResponse = "";
            using (WebClient w = new WebClient())
            {
                string accessTokenRequest = m_strNormalizedUrl + "?" + m_strNormalizedRequestParameters +
                    "&oauth_signature=" + HttpUtility.UrlEncode(accessToken);
                accessTokenResponse = w.DownloadString(accessTokenRequest);
            }

            return accessTokenResponse;
        }

        internal static string MakeACall()
        {
            NameValueCollection accessResponse = HttpUtility.ParseQueryString(
                File.ReadAllText(AccessTokenFilename));
        }
    }
}
