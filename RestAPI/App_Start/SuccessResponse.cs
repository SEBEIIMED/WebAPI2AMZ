using System;
using System.CodeDom;
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
    #region Classe qui permet de gérer le retour de service rest-------------
    public class SuccessResponse : IHttpActionResult
    {

        #region attributs----------------------------------------------------
        /// <summary>
        /// Request.
        /// </summary>
        public HttpRequestMessage Request { get; set; }
        /// <summary>
        /// Objet de retour.
        /// </summary>
        public object Poco { get; set; }
        #endregion
        #region constructors-------------------------------------------------
        public SuccessResponse(HttpRequestMessage request, object poco)
        {
            this.Poco = poco;
            this.Request = request;
        }
        #endregion


        #region methods------------------------------------------------------

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var mediatypeformater = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            mediatypeformater.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            mediatypeformater.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            response.Content = new ObjectContent(typeof(object), Poco, mediatypeformater);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
        #endregion
    }
    #endregion
}