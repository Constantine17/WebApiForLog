using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EntityLogDataBase;
using System.Web.Hosting;
using WebApiForLog.App_Start;

namespace WebApiForLog.Controllers
{
    [RoutePrefix("select")]
    public class GetHostController : ApiController
    {
        LogDataBase logDataBase = ConnectionToDataBase.Get();
        // GET select/
        [Route("host")]
        public List<string> Get()
        {
            return logDataBase.GetHosts(new Query());
        }

        // POST host/
        [Route("host")]
        public List<string> Post([FromBody]Query query)
        {
            return logDataBase.GetHosts(query);
        }
    }
}