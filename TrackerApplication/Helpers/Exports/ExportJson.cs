using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using TrackerApplication.Helpers.Exports.Base;
using TrackerApplication.Models;

namespace TrackerApplication.Helpers.Exports
{
    internal class ExportJson : Export
    {
        /// <summary>
        /// Export to JSON File
        /// </summary>
        /// <param name="pList">List PersonModel</param>
        /// <returns></returns>
        public static Boolean Export(List<PersonModel> pList)
        {
            try
            {
                SaveFileDialog.Filter = "JSON(*.json)|*.json|All files(*.*)|*.*";
                SaveFileDialog.Title = "Browse save JSON file";
                XmlSerializer ser = new XmlSerializer(typeof(List<PersonModel>));
                if (SaveFileDialog.ShowDialog() == false)
                    return false;
                JsonSerializerOptions opt = new JsonSerializerOptions() { WriteIndented = true };
                string strJson = JsonSerializer.Serialize<List<PersonModel>>(pList, opt);
                File.WriteAllText(SaveFileDialog.FileName, strJson);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Error export to json. Details: {e.Message}");
                return false;
            }
        }
    }
}
