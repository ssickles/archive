using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using LoadingCustomDataObjects.Properties;
using System.Runtime.InteropServices;

namespace LoadingCustomDataObjects
{
    class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        static void Main(string[] args)
        {
            using (MySqlConnection conn = new MySqlConnection(Settings.Default.MySqlIdsConnectionString))
            {
                long start, end, freq;
                QueryPerformanceFrequency(out freq);
                QueryPerformanceCounter(out start);

                MySqlObjectAdapter<AuthenticationLevelData> oa = new MySqlObjectAdapter<AuthenticationLevelData>(conn);
                List<AuthenticationLevelData> data = oa.GetObjects();
                MySqlObjectAdapter<AuthenticationUnitData> oau = new MySqlObjectAdapter<AuthenticationUnitData>(conn);
                List<AuthenticationUnitData> data1 = oau.GetObjects();
                MySqlObjectAdapter<AuthenticationTemplateData> oat = new MySqlObjectAdapter<AuthenticationTemplateData>(conn);
                List<AuthenticationTemplateData> data2 = oat.GetObjects();
                MySqlObjectAdapter<EnrollmentData> oae = new MySqlObjectAdapter<EnrollmentData>(conn);
                List<EnrollmentData> data3 = oae.GetObjects();
                MySqlObjectAdapter<IdentityData> oai = new MySqlObjectAdapter<IdentityData>(conn);
                IdentityData d = oai.GetNextObject();
                while (d != null)
                {
                    d = oai.GetNextObject();
                }
                //List<IdentityData> data4 = oai.GetObjects();

                QueryPerformanceCounter(out end);
                Console.WriteLine(string.Format("Execution time: {0}", (double)((end - start) / (double)freq) * 1000));
            }
            Console.ReadLine();
        }
    }
}
