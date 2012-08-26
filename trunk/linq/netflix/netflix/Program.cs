using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Web;
using System.IO;
using System.Xml.Linq;

namespace netflix
{
    class Program
    {
        static void Main(string[] args)
        {
            string Uri_base = "http://api-public.netflix.com/catalog/titles";
            
            Uri uri = new Uri(String.Format("{0}/{1}?oauth_consumer_key={2}&term={3}",
                Uri_base, "autocomplete",
                HttpUtility.UrlEncode(ConfigurationManager.AppSettings["consumerKey"]),
                "a"));
            /*Console.WriteLine("Host: {0}; LocalPath: {1}; Query: {2}", uri.Host, uri.LocalPath,
                uri.Query);
            */
            WebRequest request = WebRequest.Create(uri);
            ((HttpWebRequest)request).UserAgent = ".Net";
            WebResponse response = request.GetResponse();
            try
            {
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string strResponse = sr.ReadToEnd();

                FileStream f = new FileStream(@".\out.txt", FileMode.OpenOrCreate);
                StreamWriter w = new StreamWriter(f);
                w.Write(strResponse);
                w.Close();

                XDocument doc = new XDocument();
                doc = XDocument.Parse(strResponse);
                Console.WriteLine(doc);
                stream.Close();
            }
            finally
            {
                response.Close();
            } 
        }
    }
}
