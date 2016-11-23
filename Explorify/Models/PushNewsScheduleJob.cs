using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Quartz;

namespace Explorify.Web.Models
{
    public class PushNewsScheduleJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var category = dataMap.GetString("category");
            var skills = dataMap.GetString("skills");

            var pnm = new PushNewsModel()
            {

                id = dataMap.GetString("idd"),
                lat = dataMap.GetString("lat"),
                lng = dataMap.GetString("lng"),
                title = dataMap.GetString("title"),
                description = dataMap.GetString("description"),
                expirationdate = dataMap.GetString("expirationdate"),
                radius = dataMap.GetString("radius"),
                millisecond = dataMap.GetString("millisecond")
            };

            var regcountlist = GetTableRows(GetRegIdbyCategoryCount(category));
            if (regcountlist == null || regcountlist.Count <= 0) return;
            var i = int.Parse(regcountlist[0]["rgcount"].ToString());
            var totalPage = (int)Math.Ceiling((double)i / 1000);
            for (var j = 1; j <= totalPage; j++)
            {
                var treglist = GetTableRowsList(GetRegIdbyCategory(category, j));
                SendNotification(treglist, pnm);
            }
        }

        private static DataTable GetRegIdbyCategoryCount(string category)
        {
            var dt = new DataTable();
            var parameter1 = new SqlParameter("@Category", category);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SpGetRegIdbyCategoryCount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(parameter1);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        private static DataTable GetRegIdbyCategory(string category, int pageno)
        {
            var dt = new DataTable();
            var parameter1 = new SqlParameter("@Category", category);
            var parameter2 = new SqlParameter("@PageNo", pageno);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SpGetRegIdbyCategory";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                }
            }
            return dt;
        }

        private static List<Dictionary<string, object>> GetTableRows(DataTable dtData)
        {
            return (from DataRow dr in dtData.Rows select dtData.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col])).ToList();
        }

        private static ArrayList GetTableRowsList(DataTable dtData)
        {
            var lstRows = new ArrayList();

            foreach (DataRow dr in dtData.Rows)
            {
                foreach (DataColumn col in dtData.Columns)
                {
                    lstRows.Add(dr[col]);
                }
            }
            return lstRows;
        }

        private static void SendNotification(ArrayList deviceId, PushNewsModel message)
        {
            try
            {
                //var theString = string.Join("\",\"", deviceId.ToArray());
                //var theString = "[" + "'" + theString1.Replace(",", "','") + "'" + "]";
                const string googleAppId = "AIzaSyArRNIoFN07_MSfPD4nAFZKqD-F4wKttbc";
                const string senderId = "916801941777";
                //PushNewsModel value = message;
                HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
                //tRequest.ProtocolVersion = HttpVersion.Version11;
                tRequest.Method = "POST";
                tRequest.KeepAlive = false;
                tRequest.ContentType = " application/json";
                //tRequest.Headers.Add(string.Format("Authorization: key={0}", googleAppId));
                tRequest.Headers.Add(HttpRequestHeader.Authorization, string.Format("Key={0}", googleAppId));

                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                var messages = new Dictionary<string, object>();
                var data = new Dictionary<string, object>
                {
                    {"id", message.id},
                    {"lat", message.lat},
                    {"lng", message.lng},
                    {"millisecond", message.millisecond},
                    {"radius", message.radius},
                    {"title", message.title},
                    {"description", message.description},
                    {"expirationdate", message.expirationdate}
                };

                messages.Add("data", data);

                messages.Add("registration_ids", deviceId.ToArray().ToList());
                messages.Add("collapse_key", message.id);
                messages.Add("time_to_live", 108);
                //messages.Add("collapse_key", message.id);

                var postData = JsonConvert.SerializeObject(messages);

                //var postData = "{ \"collapse_key\": \"" + message.id + "\", \"time_to_live\": 108, \"data\": { \"id\": \"" + message.id + "\", \"lat\": \"" + message.lat + "\", \"lng\": \"" + message.lng + "\", \"radius\": \"" + message.radius + "\", \"millisecond\": \"" + message.millisecond + "\", \"time\": \"" +
                //DateTime.Now.ToString(CultureInfo.InvariantCulture) + "\", \"title\": \"" + message.title + "\", \"description\": \"" + message.description + "\", \"expirationdate\": \"" + message.expirationdate + "\" }, \"registration_id\": [ \"" + theString + "\" ] }";
                //Console.WriteLine(postData);
                var byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                var dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                var tResponse = (HttpWebResponse)tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                if (dataStream == null) return;
                var tReader = new StreamReader(dataStream);

                string res = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}