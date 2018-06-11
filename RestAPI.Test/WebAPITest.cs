using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using RestApiSecurity.AmzSecurity;
using System.IO;

namespace RestAPI.Test
{
    /// <summary>
    /// Description résumée pour ConfidentialsTest
    /// </summary>
    [TestClass]
    public class WebAPITest
    {
        /// <summary>
        /// Méthode pour tester l'authetification basique
        /// </summary>
        [TestMethod]
        public void AuthenticateInvoke()
        {
           
            string url = "http://localhost:55521/api/authenticate/x1@test.com/x1passwordtest";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            string result = "";

            WebResponse webResp = request.GetResponse();
            using (var reader = new StreamReader(webResp.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }

            Assert.AreEqual(result, "true");
        }

        /// <summary>
        /// Méthode pour tester l'authentification avec signature.
        /// </summary>
        [TestMethod]
        public void ConfidentialsInvoke()
        {
            string access_key = "AKIAIOSFODNN7EXAMPLE";
            string access_secret_key = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
            string result = "";

            string url = "http://localhost:55521/api/confidentials/x1@test.com/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Date = DateTime.Now.ToUniversalTime();
            request.ContentType = "";
            IAmzSecurity AmzSecurity = new AmzSecurity();
            string authorisation = AmzSecurity.GetAuthorisation(access_key, access_secret_key, request);

            request.Headers.Add("Authorization", authorisation);
            
            WebResponse webResp = request.GetResponse();
            using (var reader = new StreamReader(webResp.GetResponseStream()))
            {
                 result = reader.ReadToEnd(); 
            }

            Assert.AreEqual(result, "true");
        }
    }
}
