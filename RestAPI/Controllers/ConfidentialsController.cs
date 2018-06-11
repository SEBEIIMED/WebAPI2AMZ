using RestApiSecurity.AmzSecurity;
using RestApiSecurity.BasicAuthentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestApi.Controllers
{

    [RoutePrefix("api/confidentials")]
    public class ConfidentialsController : ApiController
    {
        /// <summary>
        /// Permet de vérifier l'authentification (authentifié ou non)
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>return true si authentfié et authorisation vérifiée</returns>
        [HttpGet]
        [Route("{email}")]
        public IHttpActionResult Get(string email)
        {
            try
            {
                IBasicAuthentification basicAuhenification = new BasicAuthentification();
                // valider l'authentification.
                bool isAuthentifier = basicAuhenification.IsAuthentifier(email);
                IAmzSecurity amzSecurity = new AmzSecurity();
                // vérification de la signature.
                bool isVerifierSignature = amzSecurity.VerifySignature(this.Request);

                
                return new SuccessResponse(this.Request, isVerifierSignature && isAuthentifier);
            }
            catch(Exception ex)
            {
                return new ExceptionResponse(ex.Message);
            }
        }
    }
}
