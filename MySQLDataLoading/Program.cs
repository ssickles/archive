using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Data.Common;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using Mindscape.LightSpeed;
using System.ComponentModel;
using System.Runtime.Serialization;
using MySQLDataLoading.ServiceReference1;

namespace MySQLDataLoading
{
    class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private static List<int> identities = new List<int>();

        static void Main(string[] args)
        {
            ManualObjectLoad();
            LightSpeedLoad();
            Console.ReadLine();
        }

        private static void LightSpeedLoad()
        {
            long start, end, freq;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            using (IdsUnitOfWork uow = getContext().CreateUnitOfWork())
            {
                uow.Identities.ToList<Identity>();
            }
            Console.WriteLine("Loaded Identities");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        private static void LightSpeedLoadLogins()
        {
            IEnumerable<Login> logins;
            long start, end, freq;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            using (IdsUnitOfWork uow = getContext().CreateUnitOfWork())
            {
                logins = uow.Logins;
                foreach (Login login in logins)
                {
                    //Console.WriteLine(login.Username);
                }
            }
            Console.WriteLine("Loaded Identities");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        private static LightSpeedContext<IdsUnitOfWork> getContext()
        {
            LightSpeedContext<IdsUnitOfWork> context = new LightSpeedContext<IdsUnitOfWork>();
            context.DataProvider = DataProvider.MySql5;
            context.ConnectionString = LightSpeedContext.Default.ConnectionString;
            context.PluralizeTableNames = LightSpeedContext.Default.PluralizeTableNames;
            context.IdentityMethod = LightSpeedContext.Default.IdentityMethod;
            return context;
        }

        private static MySqlParameter getParameter(string Name, MySqlDbType Type, object Value)
        {
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = Name;
            param.MySqlDbType = Type;
            param.Value = Value;
            return param;
        }

        private static void AddLargeIdentitySums()
        {
            long start, end, freq;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=pa$$w0rd;database=identitystream;"))
            {

                conn.Open();
                string getCommand = "sp_addIdentity";
                MySqlCommand com = new MySqlCommand(getCommand, conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(getParameter("FirstName", MySqlDbType.VarChar, "Scott"));
                com.Parameters.Add(getParameter("LastName", MySqlDbType.VarChar, "Sickles"));
                com.Parameters.Add(getParameter("CountryCode", MySqlDbType.VarChar, "US"));
                com.Parameters.Add(getParameter("Active", MySqlDbType.Int32, 1));
                com.Parameters.Add(getParameter("BioEnabled", MySqlDbType.Int32, 0));
                com.Parameters.Add(getParameter("IdentityCode", MySqlDbType.VarChar, "E"));

                for (int i = 0; i < 10000; i++)
                {
                    com.Parameters["FirstName"].Value = string.Format("Scott{0}", i);
                    com.Parameters["LastName"].Value = string.Format("Sickles{0}", i);
                    try
                    {
                        identities.Add(int.Parse(com.ExecuteScalar().ToString()));
                    }
                    catch (DbException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                conn.Close();
            }
            Console.WriteLine("Loaded Identities");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        private static void AddLargeLoginSums()
        {
            long start, end, freq;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=pa$$w0rd;database=identitystream;"))
            {

                conn.Open();
                string getCommand = "sp_addLogin";
                MySqlCommand com = new MySqlCommand(getCommand, conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(getParameter("Login", MySqlDbType.VarChar, "ssickles"));
                com.Parameters.Add(getParameter("Password", MySqlDbType.VarChar, "1"));
                com.Parameters.Add(getParameter("ApplicationCode", MySqlDbType.VarChar, "T24"));
                com.Parameters.Add(getParameter("IdentityId", MySqlDbType.Int32, 1));
                com.Parameters.Add(getParameter("SystemLoginId", MySqlDbType.VarChar, "R08"));
                com.Parameters.Add(getParameter("SystemLoginPassword", MySqlDbType.VarChar, "password"));
                com.Parameters.Add(getParameter("RoleCode", MySqlDbType.VarChar, "T24USER"));
                com.Parameters.Add(getParameter("FirstLogon", MySqlDbType.Int32, 1));
                com.Parameters.Add(getParameter("UseGeneratedPass", MySqlDbType.Int32, 1));
                com.Parameters.Add(getParameter("OrigPassword", MySqlDbType.VarChar, "1"));

                foreach (int id in identities)
                {
                    com.Parameters["IdentityId"].Value = id;
                    try
                    {
                        com.ExecuteNonQuery();
                        com.ExecuteNonQuery();
                    }
                    catch (DbException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                conn.Close();
            }
            Console.WriteLine("Loaded Logins");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        private static void DataTableLoad()
        {
            long start, end, freq;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            DataTable table = new DataTable("Identities");
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=pa$$w0rd;database=identitystream;"))
            {

                conn.Open();
                string getCommand = "SELECT * FROM identities";
                MySqlCommand com = new MySqlCommand(getCommand, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(com);

                try
                {
                    da.Fill(table);
                }
                catch (DbException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                conn.Close();
            }
            Console.WriteLine("Loaded Identities");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        private static void DataTableLoadLogins()
        {
            long start, end, freq;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            DataTable table = new DataTable("Logins");
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=pa$$w0rd;database=identitystream;"))
            {

                conn.Open();
                string getCommand = "SELECT * FROM logins";
                MySqlCommand com = new MySqlCommand(getCommand, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(com);

                try
                {
                    da.Fill(table);
                }
                catch (DbException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                conn.Close();
            }
            foreach (DataRow row in table.Rows)
            {
                //
            }
            Console.WriteLine("Loaded Logins");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        private static void ManualObjectLoad()
        {
            long start, end, freq;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            List<CustomIdentity> identities = new List<CustomIdentity>();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=pa$$w0rd;database=identitystream;"))
            {

                conn.Open();
                string getCommand = "SELECT * FROM identities";
                MySqlCommand com = new MySqlCommand(getCommand, conn);

                try
                {
                    MySqlDataReader reader = com.ExecuteReader();

                    while (reader.Read())
                    {
                        CustomIdentity iden = new CustomIdentity();
                        iden.Id = reader.GetInt32("id");
                        iden.FirstName = reader.GetString("first_name");
                        iden.LastName = reader.GetString("last_name");
                        iden.CountryCode = reader.GetString("country_code");
                        iden.Active = reader.GetInt32("active");
                        iden.BioEnabled = reader.GetInt32("bio_enabled");
                        iden.IdentityCode = reader.GetString("identity_code");
                        //iden.T24Id = reader.GetString("t24_id");
                        identities.Add(iden);
                    }
                }
                catch (DbException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                conn.Close();
            }
            Console.WriteLine("Loaded Identities");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        private static void ManualObjectLoadLogins(int page, int pagesize)
        {
            long start, end, freq;
            int numRows = -10;
            QueryPerformanceFrequency(out freq);
            QueryPerformanceCounter(out start);
            List<CustomLogin> logins = new List<CustomLogin>();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=pa$$w0rd;database=identitystream;"))
            {

                conn.Open();
                string getCommand = string.Format("SELECT SQL_CALC_FOUND_ROWS * FROM logins WHERE ID > 50000 LIMIT {0}, {1}", page, pagesize);
                MySqlCommand com = new MySqlCommand(getCommand, conn);

                try
                {
                    MySqlDataReader reader = com.ExecuteReader();

                    while (reader.Read())
                    {
                        CustomLogin login = new CustomLogin();
                        login.Id = reader.GetInt32("id");
                        login.Username = reader.GetString("login");
                        login.Password = reader.GetString("password");
                        login.ApplicationCode = reader.GetString("application_code");
                        login.IdentityId = reader.GetInt32("identity_id");
                        //login.SystemLoginId = reader.GetString("systemlogin_id");
                        //login.SystemLoginPassword = reader.GetString("systemlogin_password");
                        login.RoleCode = reader.GetString("role_code");
                        login.UseGeneratedPassword = reader.GetInt32("use_generated_pass");
                        //login.OriginalPassword = reader.GetString("orig_password");
                        login.FirstLogon = reader.GetInt32("first_logon");
                        logins.Add(login);
                    }
                    reader.Close();
                }
                catch (DbException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                com.CommandText = "SELECT FOUND_ROWS();";

                try
                {
                    int.TryParse(com.ExecuteScalar().ToString(), out numRows);
                }
                catch (DbException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                conn.Close();
            }
            Console.WriteLine("Loaded Logins");
            QueryPerformanceCounter(out end);
            Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
            Console.WriteLine(string.Format("Total rows: {0}", numRows));

            //start = end;
            //int i = 0;
            //foreach (CustomLogin login in logins)
            //{
            //    int result;
            //    Math.DivRem(i, 3, out result);
            //    if (result == 0)
            //    {
            //        login.FirstLogon = 0;
            //    }
            //    i++;
            //}
            //Console.WriteLine("Manipulated Logins");
            //QueryPerformanceCounter(out end);
            //Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));

            //start = end;
            //IEnumerable<CustomLogin> updated = logins.Where(e => e.FirstLogon == 0);
            //Console.WriteLine(updated.Count().ToString());
            //Console.WriteLine("Selected Logins");
            //QueryPerformanceCounter(out end);
            //Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
        }

        [DataContract]
        public class CustomIdentity: INotifyPropertyChanged
        {
            [DataMember]
            public int Id { get; set; }
            [DataMember]
            public string FirstName { get; set; }
            [DataMember]
            public string LastName { get; set; }
            [DataMember]
            public string CountryCode { get; set; }
            [DataMember]
            public int Active { get; set; }
            [DataMember]
            public int BioEnabled { get; set; }
            [DataMember]
            public string IdentityCode { get; set; }
            [DataMember]
            public string T24Id { get; set; }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }

        [DataContract]
        public class CustomLogin: INotifyPropertyChanged
        {
            [DataMember]
            public int Id { get; set; }
            [DataMember]
            public string Username { get; set; }
            [DataMember]
            public string Password { get; set; }
            [DataMember]
            public string ApplicationCode { get; set; }
            [DataMember]
            public int IdentityId { get; set; }
            [DataMember]
            public string SystemLoginId { get; set; }
            [DataMember]
            public string SystemLoginPassword { get; set; }
            [DataMember]
            public string RoleCode { get; set; }
            [DataMember]
            public int UseGeneratedPassword { get; set; }
            [DataMember]
            public string OriginalPassword { get; set; }
            [DataMember]
            public int FirstLogon { get; set; }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }
    }
}
