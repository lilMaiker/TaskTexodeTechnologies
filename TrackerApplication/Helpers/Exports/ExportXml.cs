using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Win32;
using TrackerApplication.Helpers.Exports.Base;
using TrackerApplication.Models;

namespace TrackerApplication.Helpers.Exports
{
    internal class ExportXml : Export
    {
        /// <summary>
        /// Export to XML File
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pList">List</param>
        /// <returns>true - ok, false - bad</returns>
        public static Boolean Export<T>(List<T> pList)
        {
            try
            {
                SaveFileDialog.Filter = "XML(*.xml)|*.xml|All files(*.*)|*.*";
                SaveFileDialog.Title = "Browse save XML file";
                if (SaveFileDialog.ShowDialog() == false)
                    return false;
                XmlSerializer ser = new XmlSerializer(typeof(List<PersonModel>));
                using (FileStream fs = new FileStream(SaveFileDialog.FileName, FileMode.OpenOrCreate))
                    ser.Serialize(fs, pList);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Error export to xml. Details: {e.Message}");
                return false;
            }
        }
    }
}
