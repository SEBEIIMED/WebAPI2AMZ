using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestApiSecurity.AmzSecurity
{
    /// <summary>
    /// Classe qui contient les données échangés.
    /// </summary>
    public class AmzSecurityData
    {
        /// <summary>
        /// Méthode d'appel de wenservice.
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// type de contenue échangé.
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// md5 content type
        /// </summary>
        public string MD5_ContentType { get; set; }
        /// <summary>
        /// Date d'appel.
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// CanonicalizedHeaders
        /// </summary>
        public string CanonicalizedHeaders { get; set; }
        /// <summary>
        /// CanonicalizedRessources
        /// </summary>
        public string CanonicalizedRessources { get; set; }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="request"></param>
        /// <param name="domain"></param>
        public AmzSecurityData(WebRequest request, string domain)
        {
            this.InitialiseData(request, domain);
        }
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="domain"></param>
        public AmzSecurityData(HttpRequestMessage request, string domain)
        {
            this.InitialiseData(request, domain);
        }
        /// <summary>
        /// Méthode d'intialisation des données
        /// à partir d'un request de type <see cref="WebRequest"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="domain"></param>
        private void InitialiseData (WebRequest request, string domain)
        {
           this.Date = AmzSecurityUtility.GetHeaderValue(request, "date").Replace("GMT", "+0000"); // a voir aprés pour modifier
            string xamzdate = AmzSecurityUtility.GetHeaderValue(request, "x-amz-date");
            if (!string.IsNullOrEmpty(xamzdate))
                this.Date = xamzdate;

            this.CanonicalizedHeaders = AmzSecurityUtility.PrepareCanonicalizedHeaders(request);
            this.CanonicalizedRessources = AmzSecurityUtility.PrepareCanonicalizedRessources(request, domain);
            this.MD5_ContentType = AmzSecurityUtility.GetHeaderValue(request, "Content-MD5");
            this.ContentType = request.ContentType;
            this.Method = request.Method;
        }
        /// <summary>
        /// Méthode d'initialisation des données à partir
        /// d'un type <see cref="HttpRequestMessage"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="domain"></param>
        private void InitialiseData(HttpRequestMessage request, string domain)
        {
            this.Date = AmzSecurityUtility.GetHeaderValue(request, "date").Replace("GMT", "+0000"); // a voir aprés pour modifier
            string xamzdate = AmzSecurityUtility.GetHeaderValue(request, "x-amz-date");
            if (!string.IsNullOrEmpty(xamzdate))
                this.Date = xamzdate;

            this.CanonicalizedHeaders = AmzSecurityUtility.PrepareCanonicalizedHeaders(request);
            this.CanonicalizedRessources = AmzSecurityUtility.PrepareCanonicalizedRessources(request, domain);
            this.MD5_ContentType = AmzSecurityUtility.GetHeaderValue(request, "ContentMD5");
            this.Method = request.Method.Method;
            this.ContentType = AmzSecurityUtility.GetHeaderValue(request, "ContentType");
        }
    }
}
