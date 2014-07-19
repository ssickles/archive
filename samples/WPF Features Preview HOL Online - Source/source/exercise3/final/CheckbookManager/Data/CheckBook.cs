namespace CheckbookManager.Data
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// This class represents our global data which is shown and
    /// manipulated in the UI.  Consider this the "model" for the UI.
    /// </summary>
    public static class CheckBook
    {
        private static readonly CheckRegisterCollection _register = new CheckRegisterCollection();

        /// <summary>
        /// The check book register
        /// </summary>
        public static CheckRegisterCollection Register
        {
            get { return _register; }
        }

        /// <summary>
        /// All the categories we have seen so far
        /// </summary>
        public static IEnumerable<string> Descriptions
        {
            get
            {
                return _register.Select(rt => rt.Memo).Distinct().OrderBy(s => s);
            }
        }

        /// <summary>
        /// All the categories we have seen so far
        /// </summary>
        public static IEnumerable<string> Categories
        {
            get
            {
                return _register.Select(rt => rt.Category).Distinct().OrderBy(s => s);
            }
        }

        /// <summary>
        /// Next check number to use
        /// </summary>
        public static int NextCheckNumber
        {
            get { return Register.Max(rt => (rt.CheckNumber.HasValue) ? rt.CheckNumber.Value+1 : 101); }
        }

        /// <summary>
        /// Loads the checkbook register from a file.  If it fails
        /// then no items are loaded.
        /// </summary>
        /// <param name="filename">XML file to load data from</param>
        /// <returns>true/false success</returns>
        public static bool Load(string filename)
        {
            try
            {
                XDocument doc = XDocument.Load(filename);
                if (doc.Root == null)
                    return false;

                var items = from di in doc.Root.Elements("entry")
                            select new RegisterTransaction
                                       {
                                           CheckNumber = (string.IsNullOrEmpty(di.Attribute("number").Value) ? null : (int?) Int32.Parse(di.Attribute("number").Value)),
                                           Date = DateTime.Parse(di.Attribute("date").Value),
                                           Amount = double.Parse(di.Attribute("amount").Value),
                                           Cleared = bool.Parse(di.Attribute("cleared").Value),
                                           Category = di.Attribute("category").Value,
                                           Memo = di.Attribute("comment").Value,
                                           Recipient = di.Attribute("to").Value,
                                       };
                items.ForEach(_register.Add);
            }
            catch /*(Exception ex)*/
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// This persists the in-memory register back to a disk file.
        /// </summary>
        /// <param name="filename">Filename to save to</param>
        public static void Save(string filename)
        {
            var data = new XElement("entries",
                     from entry in _register
                     select new XElement("entry",
                             new XAttribute("number", (entry.CheckNumber.HasValue) ? entry.CheckNumber.ToString() : ""),
                             new XAttribute("date", entry.Date),
                             new XAttribute("amount", entry.Amount),
                             new XAttribute("cleared", entry.Cleared),
                             new XAttribute("category", entry.Category),
                             new XAttribute("comment", entry.Memo),
                             new XAttribute("to", entry.Recipient)));

            data.Save(filename);
        }
    }
}
