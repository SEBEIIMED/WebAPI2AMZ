using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestApiSecurity.AmzSecurity
{

    public class AmzSecurity : IAmzSecurity
    {

        /// <summary>
        /// Méthode qui permet de générer l'authorisation à une ressource.
        /// </summary>
        /// <param name="accessKey">accessKey</param>
        /// <param name="accessSecretKey">secretkey</param>
        /// <param name="request">Request</param>
        /// <param name="domain">domain</param>
        /// <returns></returns>
        public string GetAuthorisation(string accessKey, string accessSecretKey, WebRequest request, string domain = "")
        {
            AmzSecurityData amzSecurityData = new AmzSecurityData(request, domain);
            string valuetosign = this.PrepareValueToSign(amzSecurityData);
            string signature = this.GetSignature(accessSecretKey, valuetosign);
            string authorization = "AWS" + " " + accessKey + ":" + signature;

            return authorization;
        }
        /// <summary>
        /// Méthode qui permet de vérifier l'autorisation.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool VerifySignature(HttpRequestMessage request, string domain = "")
        {
            try
            {

                AmzSecurityData amzSecurityData = new AmzSecurityData(request, domain);
                string valuetosign = this.PrepareValueToSign(amzSecurityData);
                string authorisation = request.Headers.GetValues("Authorization").FirstOrDefault();
                if (!authorisation.Contains("AWS "))
                    return false;
                else
                {
                    string accessKeysignature = authorisation.Substring(4);
                    string[] accesskeySignatureTab = accessKeysignature.Split(':');

                    if (accesskeySignatureTab.Length != 2)
                        return false;

                    string accessKey = accesskeySignatureTab[0];
                    string signaturetoverify = accesskeySignatureTab[1];
                    string accessSecretKey = this.GetAccessSecretKey(accessKey);

                    if (string.IsNullOrEmpty(accessSecretKey)) return false;

                    string signature = this.GetSignature(accessSecretKey, valuetosign);

                    if (signature != signaturetoverify)
                        return false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }


        /// <summary>
        /// Méthode utilisée pour retourner le secret key(mock value).
        /// </summary>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        private string GetAccessSecretKey(string accessKey)
        {
            if (accessKey == "AKIAIOSFODNN7EXAMPLE")
                return "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";

            return "";
        }
        /// <summary>
        /// Méthode qui permet de créer la signature.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valuetosign"></param>
        /// <returns></returns>
        private string GetSignature(string secretkey, string valuetosign)
        {
            Byte[] secretkeyToByte = UTF8Encoding.UTF8.GetBytes(secretkey);
            HMACSHA1 hmac = new HMACSHA1(secretkeyToByte);

            Byte[] data = UTF8Encoding.UTF8.GetBytes(valuetosign);
            Byte[] hach_data = hmac.ComputeHash(data);
            String hach_value = Convert.ToBase64String(hach_data);
            return hach_value;
        }
        /// <summary>
        /// Méthode qui permet de préparer la chaine.
        /// </summary>
        /// <param name="amzSecurityData"><seealso cref="AmzSecurityData"/></param>
        /// <returns></returns>
        private string PrepareValueToSign (AmzSecurityData amzSecurityData)           
        {

            return string.Format("{0}\n{1}\n{2}\n{3}\n{4}{5}", amzSecurityData.Method, amzSecurityData.MD5_ContentType,
                                 amzSecurityData.ContentType, amzSecurityData.Date, amzSecurityData.CanonicalizedHeaders, amzSecurityData.CanonicalizedRessources);
        }
    }
}
