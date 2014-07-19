using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using RemoveEnrollments.Properties;

namespace RemoveEnrollments
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;database=identitystream;user=root;password=pa$$w0rd;"))
            {
                MySqlCommand com = new MySqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandText = "CREATE TEMPORARY TABLE new_templatebytes_uids (uid char(36) NOT NULL);" +
                                    "INSERT INTO new_templatebytes_uids(uid) " +
                                    "SELECT authentication_template_uid FROM authentication_templates_metadata " +
                                    "WHERE authentication_templates_metadata.last_change_datetime > @removeDate;" +
                                    "DELETE FROM authentication_templates_bytes WHERE authentication_templates_bytes.uid IN (SELECT uid FROM new_templatebytes_uids);" +
                                    "CREATE TEMPORARY TABLE new_template_uids (uid char(36) NOT NULL);" +
                                    "INSERT INTO new_template_uids(uid) " + 
                                    "SELECT authentication_template_uid FROM authentication_templates_metadata " +
                                    "WHERE authentication_templates_metadata.last_change_datetime > @removeDate; " +
                                    "DELETE FROM authentication_templates WHERE authentication_templates.uid IN (SELECT uid FROM new_template_uids);" +
                                    "CREATE TEMPORARY TABLE new_enrollment_uids (uid char(36) NOT NULL);" +
                                    "INSERT INTO new_enrollment_uids(uid) " +
                                    "SELECT enrollment_uid FROM enrollments_metadata " +
                                    "WHERE enrollments_metadata.last_change_datetime > @removeDate;" +
                                    "DELETE FROM enrollments WHERE enrollments.uid IN (SELECT uid FROM new_enrollment_uids);",
                    Connection = con
                };
                com.Parameters.AddWithValue("@removeDate", Settings.Default.RemoveDate);
                con.Open();
                com.ExecuteNonQuery();
            }
            Console.WriteLine("Complete");
            Console.ReadLine();
        }
    }
}
