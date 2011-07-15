using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using MySql.Data.MySqlClient;

namespace TransactionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=pa$$w0rd;persist security info=True;database=identitystream;pooling=False"))
                    {
                        MySqlCommand com = new MySqlCommand();
                        com.CommandText = "INSERT INTO transaction_types(description, code) values ('Test', 'test')";
                        com.Connection = conn;

                        conn.Open();
                        com.ExecuteNonQuery();

                        MySqlCommand com2 = new MySqlCommand();
                        com2.CommandText = "INSERT INTO transaction_types(blah, blah) values ('Test', 'test')";
                        com2.Connection = conn;

                        com2.ExecuteNonQuery();
                    }
                    scope.Complete();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.TargetSite.Name.ToLower() == "open")
                    Console.WriteLine("Unable to connect to the database");
                else
                    Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
