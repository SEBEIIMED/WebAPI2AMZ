using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RestApiSecurity;
using RestApiSecurity.AmzSecurity;

namespace RestAPI.Test
{
    [TestClass]
    public class AmzSecurityTest
    {
        [TestMethod]
        public void SignatureGetInvoke()
        {
            string access_key = "AKIAIOSFODNN7EXAMPLE";
            string access_secret_key = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";
           
           
            var request = (HttpWebRequest)WebRequest.Create("https://johnsmith.s3.amazonaws.com/photos/puppy.jpg");
            request.Method = "GET";
            request.ContentType = "";
            request.Date = DateTime.Parse("Tue, 27 Mar 2007 19:36:42 +0000").ToUniversalTime();
            IAmzSecurity AmzSecurity = new AmzSecurity();
            string authorisation =  AmzSecurity.GetAuthorisation(access_key, access_secret_key, request, "s3.amazonaws.com");
            string mockautorisation = string.Format("AWS {0}:bWq2s1WEIj+Ydj0vQ697zp+IXMU=", access_key);
            Assert.AreEqual(authorisation, mockautorisation);
        }
        [TestMethod]
        public void SignaturePutInvoke()
        {
            string access_key = "AKIAIOSFODNN7EXAMPLE";
            string access_secret_key = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";


            var request = (HttpWebRequest)WebRequest.Create("https://johnsmith.s3.amazonaws.com/photos/puppy.jpg");
            request.Method = "PUT";
            request.ContentType = "image/jpeg";
            IAmzSecurity AmzSecurity = new AmzSecurity();
            request.Date = DateTime.Parse("Tue, 27 Mar 2007 21:15:45 +0000").ToUniversalTime();
            string authorisation = AmzSecurity.GetAuthorisation(access_key, access_secret_key, request, "s3.amazonaws.com");
            string mockautorisation = string.Format("AWS {0}:MyyxeRY7whkBe+bq8fHCL/2kKUg=", access_key);
            Assert.AreEqual(authorisation, mockautorisation);
        }
        [TestMethod]
        public void SignatureListInvoke()
        {
            string access_key = "AKIAIOSFODNN7EXAMPLE";
            string access_secret_key = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";


            var request = (HttpWebRequest)WebRequest.Create("https://johnsmith.s3.amazonaws.com/?prefix=photos&max-keys=50&marker=puppy");
            request.Method = "GET";
            request.ContentType = "";
            request.Date = DateTime.Parse("Tue, 27 Mar 2007 19:42:41 +0000").ToUniversalTime();
            IAmzSecurity AmzSecurity = new AmzSecurity();
            string authorisation = AmzSecurity.GetAuthorisation(access_key, access_secret_key, request, "s3.amazonaws.com");
            string mockautorisation = string.Format("AWS {0}:htDYFYduRNen8P9ZfE/s9SuKy0U=", access_key);
            Assert.AreEqual(authorisation, mockautorisation);
        }

        [TestMethod]
        public void SignatureFetchInvoke()
        {
            string access_key = "AKIAIOSFODNN7EXAMPLE";
            string access_secret_key = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";


            var request = (HttpWebRequest)WebRequest.Create("https://johnsmith.s3.amazonaws.com/?acl");
            request.Method = "GET";
            request.ContentType = "";
            IAmzSecurity AmzSecurity = new AmzSecurity();
            request.Date = DateTime.Parse("Tue, 27 Mar 2007 19:44:46 +0000").ToUniversalTime();
            string authorisation = AmzSecurity.GetAuthorisation(access_key, access_secret_key, request, "s3.amazonaws.com");
            string mockautorisation = string.Format("AWS {0}:c2WLPFtWHVgbEmeEG93a4cG37dM=", access_key);
            Assert.AreEqual(authorisation, mockautorisation);
        }
        [TestMethod]
        public void SignatureDeleteInvoke()
        {
            string access_key = "AKIAIOSFODNN7EXAMPLE";
            string access_secret_key = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";


            var request = (HttpWebRequest)WebRequest.Create("https://johnsmith.s3.amazonaws.com/photos/puppy.jpg");
            request.Method = "DELETE";
            request.ContentType = "";
            request.Date = DateTime.Parse("Tue, 27 Mar 2007 21:20:27 +0000").ToUniversalTime();
            request.Headers.Add("x-amz-date", "Tue, 27 Mar 2007 21:20:26 +0000");
            IAmzSecurity AmzSecurity = new AmzSecurity();
            string authorisation = AmzSecurity.GetAuthorisation(access_key, access_secret_key, request, "s3.amazonaws.com");
            string mockautorisation = string.Format("AWS {0}:lx3byBScXR6KzyMaifNkardMwNk=", access_key);
            Assert.AreEqual(authorisation, mockautorisation);
        }

        [TestMethod]
        public void SignatureUploadInvoke()
        {
            string access_key = "AKIAIOSFODNN7EXAMPLE";
            string access_secret_key = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";


            var request = (HttpWebRequest)WebRequest.Create("https://static.johnsmith.net:8080/db-backup.dat.gz");
            request.Method = "PUT";
            request.ContentType = "application/x-download";
            request.Headers.Add("x-amz-acl", "public-read");
            request.Headers.Add("X-Amz-Meta-ReviewedBy", "joe@johnsmith.net");
            request.Headers.Add("X-Amz-Meta-ReviewedBy", "jane@johnsmith.net");
            request.Headers.Add("X-Amz-Meta-FileChecksum", "0x02661779");
            request.Headers.Add("X-Amz-Meta-ChecksumAlgorithm", "crc32");
            request.Headers.Add("Content-MD5", "4gJE4saaMU4BqNR0kLY+lw==");
            request.Date = DateTime.Parse("Tue, 27 Mar 2007 21:06:08 +0000").ToUniversalTime();
            IAmzSecurity AmzSecurity = new AmzSecurity();
            string authorisation = AmzSecurity.GetAuthorisation(access_key, access_secret_key, request);
            string mockautorisation = string.Format("AWS {0}:ilyl83RwaSoYIEdixDQcA4OnAnc=", access_key);
            Assert.AreEqual(authorisation, mockautorisation);
        }
    }
}
