using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using StoreDatabase;

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : System.Windows.Application
    {
        private static StoreDB storeDB = new StoreDB();
        public static StoreDB StoreDB
        {
            get { return storeDB; }
        }

        private static StoreDB2 storeDB2 = new StoreDB2();
        public static StoreDB2 StoreDB2
        {
            get { return storeDB2; }
        }
    }
}