using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.IO;
using System.Collections;
using System.Data;
using System.Reflection;

namespace LoadT24Data
{
    public class t24Parsing
    {
        private static List<MapField> LoadMapping(string Type)
        {
            XmlDocument mapping = new XmlDocument();
            List<MapField> fields = new List<MapField>();
            mapping.Load(Assembly.GetExecutingAssembly().Location + "../../../../mapping.xml");

            XmlNode mapRoot = mapping.SelectSingleNode(string.Format("/t24Mapping/{0}", Type));
            if (mapRoot == null)
            {
                throw new Exception(string.Format("Section was not found in the mapping xml file. {0}", Type));
            }

            XmlNodeList mapFields = mapRoot.SelectNodes("field");
            if (mapFields.Count == 0)
            {
                throw new Exception(string.Format("No fields were found in the mapping for {0}.", Type)); 
            }

            foreach (XmlNode fieldNode in mapFields)
            {
                XmlAttribute att = fieldNode.Attributes["name"];
                if (att == null)
                {
                    throw new Exception(string.Format("The name attribute was not found for a field in {0}.", Type));
                }

                XmlAttribute att2 = fieldNode.Attributes["sourceName"];
                if (att2 == null)
                {
                    throw new Exception(string.Format("The sourceName attribute was not found for a field in {0}.", Type));
                }

                fields.Add(new MapField(att.Value, att2.Value));
            }
            return fields;
        }

        public static DataSet ParseCSV(string Filename, string MapName)
        {
            List<MapField> fields = LoadMapping(MapName);
            DataSet data = new DataSet();
            DataTable table = new DataTable(MapName);
            data.Tables.Add(table);
            DataTable errors = new DataTable("Errors");
            errors.Columns.Add("Description");
            errors.Columns.Add("RawData");
            data.Tables.Add(errors);
            StreamReader reader = new StreamReader(Filename);
            int i = 0;
            while (!reader.EndOfStream)
            {
                string temp = string.Empty;
                try
                {
                    temp = reader.ReadLine();
                }
                catch (Exception ex)
                {
                    errors.Rows.Add("Unable to read line from csv file.", string.Empty);
                    break;
                }
                i++;
                
                //the example files seem to have some empty lines now and again
                //the check for line length will allow us to just ignore these
                if (temp.Length > 0)
                {
                    string[] parseFields = temp.Split('~');
                    //check to make sure the correct number of fields are parsed per line
                    if (parseFields.Length != fields.Count)
                    {
                        if (i == 1)
                        {
                            throw new Exception(string.Format("{0} field headers expected on the first line of the file. {1} were found.", fields.Count, parseFields.Length));
                        }
                        //invalid line, how should this be handled?
                        //add this line to an list of invalid lines (can show the user the number of invalid lines and then display them)
                        errors.Rows.Add(string.Format("The number of values for this line ({0}) do not match the number of mapped fields ({1}).", parseFields.Length, fields.Count), temp);
                        
                    }
                    else
                    {
                        //the first line should always be the column headers
                        if (i == 1)
                        {
                            //check to make sure the header names match the source mapping
                            MapFieldPredicate pred = new MapFieldPredicate();
                            foreach (string f in parseFields)
                            {
                                //make sure this field is in our mapping
                                pred.Name = f;
                                MapField foundField = fields.Find(pred.FindField);
                                if (foundField != null)
                                {
                                    table.Columns.Add(foundField.Name);
                                }
                                else
                                {
                                    //field from mapping not found in the csv file
                                    throw new Exception(string.Format("A field header ({0}) in the file was not found in the mapping.", f));
                                }
                            }
                        }
                        else
                        {
                            DataRow newRow = table.NewRow();
                            int j = 0;
                            foreach (MapField field in fields)
                            {
                                newRow[field.Name] = parseFields[j];
                                j++;
                            }
                            table.Rows.Add(newRow);
                        }
                    }
                }
            }
            return data;
        }
    }

    public class MapField
    {
        public MapField(string Name, string SourceName)
        {
            this.Name = Name;
            this.SourceName = SourceName;
        }

        public string Name { get; set; }
        public string SourceName { get; set; }
    }

    internal class MapFieldPredicate
    {
        public string Name { get; set; }

        public bool FindField(MapField Field)
        {
            return (Field.SourceName.ToLower() == this.Name.ToLower());
        }
    }
}
