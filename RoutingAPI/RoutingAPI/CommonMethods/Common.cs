using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingAPI.CommonMethods
{
    public class Common
    {
        public static void ExecuteQuery(IConfiguration _configuration, List<string> qStrings, string tableName)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(_configuration.GetConnectionString(tableName));

                conn.Open();
                qStrings.ForEach(qStr => {
                    OleDbCommand cmd = new OleDbCommand(qStr, conn);
                    cmd.ExecuteNonQuery();
                });
                conn.Close();

                return;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
