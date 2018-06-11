using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RestApi
{
    #region Classe qui permet de gérer les exeptions de service rest---------
    public class ExceptionResponse : IHttpActionResult
    {
        #region attributs-------------------------------------------------
        /// <summary>
        /// Request.
        /// </summary>
        public HttpRequestMessage Request { get; set; }
        /// <summary>
        /// L'erreur
        /// </summary>
        public string Error { get; set; }
        #endregion
        #region constructors-------------------------------------------------
        public ExceptionResponse(string error)
        {
            this.Error = error;
            
        }
        #endregion
        #region methods-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Content = new ObjectContent<string>(Error, GlobalConfiguration.Configuration.Formatters.JsonFormatter);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
        #endregion
    }
    #endregion
}