using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityLogDataBase;
using System.Web.Hosting;
using System.IO;

namespace WebApiForLog.App_Start
{
    static class ConnectionToDataBase
    {
        static LogDataBase LogDataBase;
        public static LogDataBase Get()
        {
            return LogDataBase;
        }

        static ConnectionToDataBase()
        {
            //string directory = Directory.;
            string directory = HostingEnvironment.ApplicationPhysicalPath + "StringConnection";
            string connectionString = "";

            using (var file = File.OpenText(directory))
            {
                connectionString = file.ReadLine();
            }
            LogDataBase = new LogDataBase(connectionString);
        }
    }
}