using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using EntityLogDataBase;
using WebApiForLog.App_Start;
using System.Web.Http;

namespace WebApiForLog.Controllers
{
    [RoutePrefix("select")]
    public class GetRoutController : ApiController
    {
        LogDataBase logDataBase = ConnectionToDataBase.Get();
        // GET select/rout
        [Route("rout")]
        public List<string> Get()
        {
            return logDataBase.GetRoutes(new Query());
        }

        // POST select/rout
        [Route("rout")]
        public List<string> Post([FromBody]Query query)
        {
            return logDataBase.GetRoutes(query);
        }
    }
}