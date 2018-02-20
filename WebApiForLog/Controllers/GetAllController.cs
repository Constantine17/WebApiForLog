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
    public class GetAllController : ApiController
    {
        LogDataBase logDataBase = ConnectionToDataBase.Get();
        // GET api/<controller>
        [Route("all")]
        public List<RequestData> Get()
        {
            return logDataBase.GetAll(new QueryAll());
        }

        // POST api/<controller>
        [Route("all")]
        public List<RequestData> Post([FromBody]QueryAll query)
        {
            return logDataBase.GetAll(query);
        }

    }
}