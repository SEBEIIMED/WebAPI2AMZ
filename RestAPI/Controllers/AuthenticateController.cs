using RestApiSecurity.BasicAuthentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Results;

namespace RestApi.Controllers
{
    [RoutePrefix("api/authenticate")]
    public class AuthenticateController : ApiController
    {
       /// <summary>
       /// Authentification basique
       /// </summary>
       /// <param name="email">email</param>
       /// <param name="password">mot de passe</param>
       /// <returns></returns>
        [HttpGet]
        [Route("{email}/{password}")]
        public IHttpActionResult Get(string email, string password)
        {
            try
            {
                IBasicAuthentification basicAuhenification = new BasicAuthentification();
                bool isvalid = basicAuhenification.Valider(email, password);
                return new SuccessResponse(this.Request, isvalid);

            }
            catch(Exception  ex)
            {
                return new ExceptionResponse(ex.Message);
            }
        }
    }
}
