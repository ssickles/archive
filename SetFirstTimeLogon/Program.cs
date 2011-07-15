using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using SetFirstTimeLogon.Properties;

namespace SetFirstTimeLogon
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MySqlConnection conn = new MySqlConnection(Settings.Default.MySqlIdsConnectionString))
            {
                MySqlCommand com = new MySqlCommand("update logins set first_logon = 1", conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
