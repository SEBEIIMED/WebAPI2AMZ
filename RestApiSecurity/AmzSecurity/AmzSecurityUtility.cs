using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestApiSecurity.AmzSecurity
{
    public static class AmzSecurityUtility
    {
        /// <summary>
        /// Liste des SubRessourceNames
        /// </summary>
        private static string[] SubRessourceNames = new string[] {"?acl", "?lifecycle" , "?logging" , "?notification", "?partNumber", "?polic" ,
                                                                   "?requestPayment" , "?torrent", "?uploadId", "?uploads" , "?versionId",
                                                                   "?versioning", "?website" };
        /// <summary>
        /// Liste des overrided ResponseHeader
        /// </summary>
        private static string[] ParamsOverrideResponseHeader = new string[] { "response-content-type", "response-content-language",
                                                                                "response-expires" , "response-cache-control", "response-content-disposition",
                                                                                "response-content-encoding"};

        /// <summary>
        /// Permet de retourner la valeur à part de header de request.
        /// </summary>
        /// <param name="request"><seealso cref="HttpRequestMessage"/></param>
        /// <param name="key">le clé</param>
        /// <returns></returns>
        public static string GetHeaderValue(HttpRequestMessage request, string key)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = request.Headers;

            if (headers.Contains(key))
                return headers.GetValues(key).FirstOrDefault();

            return "";
        }

        /// <summary>
        /// Permet de retourner la valeur à part de header de request.
        /// </summary>
        /// <param name="request"><seealso cref="WebRequest"/></param>
        /// <param name="key">le clé</param>
        /// <returns></returns>
        public static string GetHeaderValue(WebRequest request, string key)
        {
            WebHeaderCollection headers = request.Headers;
           
            return headers.Get(key);
           
        }
        /// <summary>
        /// Méthode qui permet de préparer
        /// la partie CanonicalizedHeaders.
        /// </summary>
        /// <param name="request"><seealso cref="WebRequest"/></param>
        /// <returns></returns>
        public static string PrepareCanonicalizedHeaders(WebRequest request)
        {
            WebHeaderCollection headers = request.Headers;
            Dictionary<string, string> headersValues = new Dictionary<string, string>();
            foreach (var headername in headers.Keys)
            {
                bool isAmazParamHeader = headername.ToString().ToLower().Contains("x-amz-");
                bool isAmazDateParamHeader = headername.ToString().ToLower().Contains("x-amz-date");
                if (isAmazParamHeader && !isAmazDateParamHeader)
                {
                    if (!headersValues.ContainsKey(headername.ToString().ToLower()))
                        headersValues.Add(headername.ToString().ToLower(), headers.Get(headername.ToString()).TrimEnd().TrimStart());
                    else
                        headersValues[headername.ToString().ToLower()] += "," + headers.Get(headername.ToString()).TrimEnd().TrimStart();


                    headersValues[headername.ToString().ToLower()].Replace("\\s+", " ");
                    headersValues[headername.ToString().ToLower()] += "\n";
                }

            }


            headersValues = headersValues.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            return string.Join("", headersValues.Select(m => m.Key + ":" + m.Value).ToArray());
        }
        /// <summary>
        /// Préparer CanonicalizedHeaders
        /// </summary>
        /// <param name="request"><seealso cref="HttpRequestMessage"/></param>
        /// <returns></returns>
        public static string PrepareCanonicalizedHeaders(HttpRequestMessage request)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = request.Headers;
            Dictionary<string, string> headersValues = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                bool isAmazParamHeader = header.Key.ToString().ToLower().Contains("x-amz-");
                bool isAmazDateParamHeader = header.Key.ToString().ToLower().Contains("x-amz-date");
                if (isAmazParamHeader && !isAmazDateParamHeader)
                {
                    if (!headersValues.ContainsKey(header.Key.ToString().ToLower()))
                        headersValues.Add(header.Key.ToString().ToLower(), headers.GetValues(header.Key.ToString()).FirstOrDefault().TrimEnd().TrimStart());
                    else
                        headersValues[header.Key.ToString().ToLower()] += "," + headers.GetValues(header.Key.ToString()).FirstOrDefault().TrimEnd().TrimStart();


                    headersValues[header.Key.ToString().ToLower()].Replace("\\s+", " ");
                    headersValues[header.Key.ToString().ToLower()] += "\n";
                }

            }


            headersValues = headersValues.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            return string.Join("", headersValues.Select(m => m.Key + "=" + m.Value).ToArray());
        }

        /// <summary>
        /// Méhode qui permet de préparer la partie CanonicalizedRessources.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string PrepareCanonicalizedRessources(WebRequest request, string domain = "")
        {
            string CanonicalizedRessources = "";
            string host = ((HttpWebRequest)request).Host;
            string virtualhost = host;
            if(!string.IsNullOrEmpty(virtualhost) && host.Split(':').Length == 2)
            {
                virtualhost = host.Split(':')[0];
            }
           // string host = "s3.amazonaws.com";
            string bucketname = string.Empty;
            if (!string.IsNullOrEmpty(virtualhost) && !string.IsNullOrEmpty(domain) &&  virtualhost.Contains(domain))
            {
                bucketname = "/" + virtualhost.Substring(0, virtualhost.IndexOf(domain) - 1);
            }
            else if (!string.IsNullOrEmpty(virtualhost))
            {
                bucketname = "/" + virtualhost;
            }

            CanonicalizedRessources += bucketname;

            string sheme = request.RequestUri.GetLeftPart(UriPartial.Scheme);
            string query = request.RequestUri.GetLeftPart(UriPartial.Query);
            string path = request.RequestUri.GetLeftPart(UriPartial.Path);
            string authority = request.RequestUri.GetLeftPart(UriPartial.Authority);
            CanonicalizedRessources += path.Substring(authority.Length);

            
            // Récupération des subressources.
            CanonicalizedRessources += GetSubRessources(query.Substring(path.Length));

           
            // Récupération des overidedresponseheader
            CanonicalizedRessources += GetOverridedResponseHeaderValues(query);

            return CanonicalizedRessources;
        }
        /// <summary>
        /// Méhode qui permet de préparer la partie CanonicalizedRessources.
        /// </summary>
        /// <param name="request"><seealso cref="HttpRequestMessage"/></param>
        /// <param name="domain">domain</param>
        /// <returns></returns>
        public static string PrepareCanonicalizedRessources(HttpRequestMessage request, string domain = "")
        {
            string sheme = request.RequestUri.GetLeftPart(UriPartial.Scheme);
            string query = request.RequestUri.GetLeftPart(UriPartial.Query);
            string path = request.RequestUri.GetLeftPart(UriPartial.Path);
            string authority = request.RequestUri.GetLeftPart(UriPartial.Authority);

            string CanonicalizedRessources = "";
            string host = authority.Replace(sheme, "");
            string virtualhost = host;
            if (!string.IsNullOrEmpty(virtualhost) && host.Split(':').Length == 2)
            {
                virtualhost = host.Split(':')[0];
            }
            string bucketname = string.Empty;
            if (!string.IsNullOrEmpty(virtualhost) && !string.IsNullOrEmpty(domain) && virtualhost.Contains(domain))
            {
                bucketname = "/" + virtualhost.Substring(0, virtualhost.IndexOf(domain) - 1);
            }
            else if (!string.IsNullOrEmpty(virtualhost))
            {
                bucketname = "/" + virtualhost;
            }

            CanonicalizedRessources += bucketname;


            CanonicalizedRessources += path.Substring(authority.Length);

           
            CanonicalizedRessources += GetSubRessources(query.Substring(path.Length));

           

            CanonicalizedRessources += GetOverridedResponseHeaderValues(query);

            return CanonicalizedRessources;
        }
        /// <summary>
        /// Méthode qui permet de récupérer OverridedResponseHeaderValues
        /// </summary>
        /// <param name="paramsOverrideResponseHeader">liste des paramètres</param>
        /// <param name="query">query</param>
        /// <returns></returns>
        private static string GetOverridedResponseHeaderValues(string query)
        {
            if (string.IsNullOrEmpty(query)) return string.Empty;

            string[] paramsQuery = query.Split('&');
            string overridedResponseHeaderValues = "";
            foreach (string param in paramsQuery)
            {
                string[] keyValue = new string[2];
                if (ParamsOverrideResponseHeader.Contains(param.ToLower()))
                {
                    overridedResponseHeaderValues += param;
                }
            }

            return overridedResponseHeaderValues;
        }
        /// <summary>
        /// Méthode qui permet de récupérer les SubRessources
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        private static string GetSubRessources(string query)
        {
            if (string.IsNullOrEmpty(query)) return string.Empty;

            string[] paramsQuery = query.Split('&');

            Dictionary<string, List<string>> subRessourcesValues = new Dictionary<string, List<string>>();

            foreach (string param in paramsQuery)
            {
                string[] keyValue = param.Split('=');
                string key = keyValue[0];

                if (SubRessourceNames.Contains(key.ToLower()))
                {
                    string value = "";
                    if (keyValue.Length == 2)
                        value = keyValue[1];
                    if (subRessourcesValues.ContainsKey(value))
                        subRessourcesValues[value].Add(key);
                    else
                    {
                        List<string> values = new List<string>();
                        values.Add(key);
                        subRessourcesValues.Add(value, values);
                    }
                }
            }

            return FormateListe(subRessourcesValues, "&");
        }


        /// <summary>
        /// Formater un dictionnary vers une chaine avec un format spécifique.
        /// </summary>
        /// <param name="subRessourcesValues">dictionnary (clé, liste des chains)</param>
        /// <param name="separator">separateur de formatage</param>
        /// <returns></returns>
        private static string FormateListe(Dictionary<string, List<string>> subRessourcesValues, string separator)
        {
            string formatLisValue = "";

            foreach (string key in subRessourcesValues.Keys)
            {
                List<string> values = subRessourcesValues[key];
                values = values.OrderByDescending(s => s).ToList<string>();
                if (!string.IsNullOrEmpty(key))
                    formatLisValue += values.Aggregate((i, j) => i + separator + j) + "=" + key;
                else
                    formatLisValue += values.Aggregate((i, j) => i + separator + j);
            }

            return formatLisValue;
        }

    }
}
