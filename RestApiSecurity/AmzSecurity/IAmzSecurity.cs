using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RestApiSecurity.AmzSecurity
{
    public interface IAmzSecurity
    {
        string GetAuthorisation(string accessKey, string accessSecretKey, WebRequest request, string domain = "");
        bool VerifySignature(HttpRequestMessage request, string domain = "");
    }
}