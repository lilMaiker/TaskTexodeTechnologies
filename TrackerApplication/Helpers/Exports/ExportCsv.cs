using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using TrackerApplication.Helpers.Exports.Base;

namespace TrackerApplication.Helpers.Exports 
{
    internal class ExportCsv : Export
    {
        /// <summary>
        /// Export to CSV File
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pList">List</param>
        /// <returns>true - ok, false - bad</returns>
        public static Boolean Export<T>(List<T> pList)
        {
            try
            {
                SaveFileDialog.Filter = "CSV(*.csv)|*.csv|All files(*.*)|*.*";
                SaveFileDialog.Title = "Browse save CSV file";
                StringBuilder sb = new StringBuilder();
                if (SaveFileDialog.ShowDialog() == false)
                    return false;
                var finalPath = SaveFileDialog.FileName;
                var header = "";
                var info = typeof(T).GetProperties();
                if (!File.Exists(finalPath))
                {
                    var file = File.Create(finalPath);
                    file.Close();
                    header = typeof(T).GetProperties().Aggregate(header, (current, prop) => current + (prop.Name + "; "));
                    header = header.Substring(0, header.Length - 2);
                    sb.AppendLine(header);
                    TextWriter sw = new StreamWriter(finalPath, true);
                    sw.Write(sb.ToString());
                    sw.Close();
                }
                foreach (T obj in pList)
                {
                    sb = new StringBuilder();
                    string line = info.Aggregate("", (current, prop) => current + (prop.GetValue(obj, null) + "; "));
                    line = line.Substring(0, line.Length - 2);
                    sb.AppendLine(line);
                    TextWriter sw = new StreamWriter(finalPath, true);
                    sw.Write(sb.ToString());
                    sw.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Error export to csv. Details: {e.Message}");
                return false;
            }
        }
    }
}
