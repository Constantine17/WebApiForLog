using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EntityLogDataBase
{
    class LogDataBase
    {
        string stringConnection = @"Data Source=CONSTANTINE-PC\SQLEXPRESS;Initial Catalog=Logs;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        const string formatDate = "dd-MM-yyyy HH:mm:ss";

        public LogDataBase(string stringConnection)
        {
            this.stringConnection = stringConnection;
        }

        /// <summary>
        /// Конвертация даты для базы данных
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private string ConvertDate(DateTime dateTime)
        {
            return dateTime.ToString(formatDate);
        }

        /// <summary>
        /// Добавить новый лог
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddRequestData(RequestData data)
        {
            try
            {
                string insert = @"insert into Request";
                string tableName = "";
                string values = "";

                if (data.RequestDate != DateTime.MinValue) { tableName += "RequestDate,"; values += "'" + ConvertDate(data.RequestDate) + "',"; }
                if (data.UtcCode != null) { tableName += "UtcCode,"; values += "'" + data.UtcCode + "',"; }
                if (data.IPorHost != null) { tableName += "IPorHost,"; values += "'" + data.IPorHost + "',"; }
                if (data.TypeRequest != null) { tableName += "TypeRequest,"; values += "'" + data.TypeRequest + "',"; }
                if (data.Route != null) { tableName += "Route,"; values += "'" + data.Route + "',"; }
                if (data.Parameters != null) { tableName += "Parameters,"; values += "'" + data.Parameters + "',"; }
                if (data.Geolocation != null) { tableName += "Geolocation,"; values += "'" + data.Geolocation + "',"; }
                if (data.Code != 0) { tableName += "Code,"; values += data.Code + ","; }
                tableName += "SizeByte"; values += data.SizeByte;

                if (tableName != "")
                    insert += "(" + tableName + ") values (" + values + ")";
                else return false;

                using (var Connection = new SqlConnection(stringConnection))
                {
                    SqlCommand command = new SqlCommand(insert, Connection);
                    Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                }

                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// получить топ-N хостов, отсортированных по убыванию за указаный промежуток времени
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<string> GetHosts(Query query)
        {
            if (query.n <= 0) query.n = 10;
            string select = @"Select Top " + query.n + " IPorHost From Request ";
            string where = "";
            string starDate = "";
            string endDate = "";

            if (query.start != DateTime.MinValue) { where = "where "; starDate = "RequestDate >" + "'" + ConvertDate(query.start) + "'"; }
            if (query.end != DateTime.MinValue) { where = "where "; endDate = "RequestDate <" + "'" + ConvertDate(query.end)+ "'"; }
            if (starDate != "" && endDate != "") where += starDate + " and " + endDate;
            else where += starDate + endDate;

            select += where + " order by RequestDate desc";

            List<string> output = new List<string>();

            using (var Connection = new SqlConnection(stringConnection))
            {
                SqlCommand command = new SqlCommand(select, Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        output.Add((string)reader["IPorHost"]);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// получить топ-N роутов, отсортированных по убыванию за указаный промежуток времени
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<string> GetRoutes(Query query)
        {
            if (query.n <= 0) query.n = 10;
            string select = @"Select Top " + query.n + " Route From Request ";
            string where = "";
            string starDate = "";
            string endDate = "";

            if (query.start != DateTime.MinValue) { where = "where "; starDate = "RequestDate >" + "'" + ConvertDate(query.start) + "'"; }
            if (query.end != DateTime.MinValue) { where = "where "; endDate = "RequestDate <" + "'" + ConvertDate(query.end) + "'"; }
            if (starDate != "" && endDate != "") where += starDate + " and " + endDate;
            else where += starDate + endDate;

            select += where + " order by RequestDate desc";

            List<string> output = new List<string>();

            using (var Connection = new SqlConnection(stringConnection))
            {
                SqlCommand command = new SqlCommand(select, Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        output.Add((string)reader["Route"]);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// получить полный лог запросов, отсортированный по увеличению времени за указанный интервал
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<RequestData> GetAll(QueryAll query)
        {
            if (query.limit <= 0) query.limit = 10;
            string select = @"Select Top " + query.limit + " * From Request ";
            string where = "";
            string starDate = "";
            string endDate = "";

            if (query.start != DateTime.MinValue) { where = "where "; starDate = "RequestDate >" + "'" + ConvertDate(query.start) + "'"; }
            if (query.end != DateTime.MinValue) { where = "where "; endDate = "RequestDate <" + "'" + ConvertDate(query.end) + "'"; }
            if (starDate != "" && endDate != "") where += starDate + " and " + endDate;
            else where += starDate + endDate;
            if (where == "") where = "where";
            else where += " and";

            select += where + " Id>='"+ query.offset+ "' order by RequestDate desc";

            List<RequestData> output = new List<RequestData>();

            using (var Connection = new SqlConnection(stringConnection))
            {
                SqlCommand command = new SqlCommand(select, Connection);
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RequestData requestData = new RequestData();

                        try { requestData.IPorHost = (string)reader["IPorHost"]; } catch { }
                        try { requestData.RequestDate = (DateTime)reader["RequestDate"]; } catch { }
                        try { requestData.UtcCode = (string)reader["UtcCode"]; } catch { }
                        try { requestData.TypeRequest = (string)reader["TypeRequest"]; } catch { }
                        try { requestData.Route = (string)reader["Route"]; } catch { }
                        try { requestData.Parameters = (string)reader["Parameters"]; } catch { }
                        try { requestData.Code = (int)reader["Code"]; } catch { }
                        try { requestData.SizeByte = (long)reader["SizeByte"]; } catch { }
                        try { requestData.Geolocation = (string)reader["Geolocation"]; } catch { }

                        output.Add(requestData);
                    }
                }
            }

            return output;
        }
    }
}
