using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using TrackerApplication.Models;

namespace TrackerApplication.Data
{
    internal class DataAccessJson
    {
        /// <summary>
        /// DefaultParams
        /// </summary>
        private static string _json = string.Empty;
        private int _countSteps = 0;
        private int _countDaysFitnes = 0;
        private int _maxSteps = 0;
        private int _minSteps = 0;
        private static Int32 _countDays = 0;
        private List<FitnesInfoModel> _finesInfos = new List<FitnesInfoModel>();

        public DataAccessJson()
        {
            _countDays = GetCountDays();
        }

        /// <summary>
        /// Return count days in folder TestData
        /// </summary>
        /// <returns>Int32</returns>
        private static Int32 GetCountDays()
        {
            try
            {
                return new DirectoryInfo(@"TestData\").GetFiles()
                    .Length;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                Console.WriteLine("Not found folder");
                //Show($"Not found folder... {ex.Message}", String.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
            catch (Exception ex)
            {
                //Show($"Uniknow error...Details: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return 0;
            }
        }

        /// <summary>
        /// Selected FitnesInfo from json files
        /// </summary>
        /// <returns>FitnesInfoModel</returns>
        public static List<FitnesInfoModel> GetFitnesInfos()
        {
            List<FitnesInfoModel> output = new List<FitnesInfoModel>();
            //Int32 countDays = GetCountDays();
            for (int i = 1; i < _countDays + 1; i++)
                using (StreamReader streamReader =
                    new StreamReader($"C:\\Users\\Lenovo\\OneDrive\\Рабочий стол\\ТЗ Jun C#\\TestData\\day{i}.json"))
                {
                    _json = streamReader.ReadToEnd();
                    List<FitnesInfoModel> items = JsonConvert.DeserializeObject<List<FitnesInfoModel>>(_json);
                    if (items == null) continue;
                    Console.Out.WriteLine($"Get pos: {items.Count}");
                    Console.Out.WriteLine($"Get day: {i}");
                    output.AddRange(items.Select(t => new FitnesInfoModel()
                    {
                        Day = i,
                        Rank = t.Rank,
                        User = t.User,
                        Status = t.Status,
                        Steps = t.Steps
                    }));
                }

            Console.Out.WriteLine($"Records: {output.Count}");
            return output;
        }

        /// <summary>
        /// Selected All users in json files
        /// </summary>
        /// <returns>PersonModel</returns>
        public static List<PersonModel> GetPersons()
        {
            List<PersonModel> allUsers = new List<PersonModel>();
            List<FitnesInfoModel> finesInfos = GetFitnesInfos();
            foreach (var t in finesInfos)
            {
                bool isCheck = false;
                foreach (var t1 in allUsers.Where(t1 => t.User == t1.Fio))
                    isCheck = true;

                if (!isCheck)
                    allUsers.Add(new PersonModel() { Fio = t.User });
            }
            Console.Out.WriteLine("All users: " + allUsers.Count);
            return allUsers;
        }

        /// <summary>
        /// Selected info for users
        /// </summary>
        /// <returns>PersonModel</returns>
        public List<PersonModel> GetPersonsAndInfo()
        {
            List<PersonModel> allUsers = GetPersons();
            List<FitnesInfoModel> finesInfos = GetFitnesInfos();

            foreach (var t in allUsers)
            {
                _countSteps = 0;
                _countDaysFitnes = 0;
                _maxSteps = 0;
                foreach (var t1 in finesInfos.Where(t1 => t.Fio == t1.User))
                {
                    _countSteps += t1.Steps;
                    _countDaysFitnes++;
                    if (t1.Steps > _maxSteps)
                        _maxSteps = t1.Steps;
                }
                _minSteps = _maxSteps;
                foreach (var t1 in finesInfos.Where(t1 => t.Fio == t1.User).Where(t1 => t1.Steps < _minSteps))
                    _minSteps = t1.Steps;

                t.AvgSteps = _countSteps / _countDaysFitnes;
                t.MaxSteps = _maxSteps;
                t.MinSteps = _minSteps;
            }

            return allUsers;
        }
    }
}
