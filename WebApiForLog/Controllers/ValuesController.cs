using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EntityLogDataBase;
using System.Web.Hosting;
using System.IO;

namespace WebApiForLog.Controllers
{
    public class ValuesController : ApiController
    {
        LogDataBase logDataBase;
        ValuesController()
        {
            //string directory = Directory.;
            string directory = HostingEnvironment.ApplicationPhysicalPath + "StringConnection";
            string connectionString = "";

            using (var file = File.OpenText(directory))
            {
                connectionString = file.ReadLine();
            }
            this.logDataBase = new LogDataBase(connectionString);
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]Query value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
