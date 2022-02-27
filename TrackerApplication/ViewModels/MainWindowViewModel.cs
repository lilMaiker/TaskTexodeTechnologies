using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using TrackerApplication.Data;
using TrackerApplication.Infrastructure.Commands;
using TrackerApplication.Models;
using TrackerApplication.ViewModels.Base;

namespace TrackerApplication.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private SeriesCollection _SeriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _SeriesCollection;
            set => Set(ref _SeriesCollection, value);
        }
        public ObservableCollection<PersonModel> People { get; set; }

        private ObservableCollection<PersonInfoDaysModel> _PersonInfo;
        public List<PersonInfoDaysModel> PersonInfoList { get; set; }

        public ObservableCollection<PersonInfoDaysModel> PersonInfo
        {
            get => _PersonInfo;
            set => Set(ref _PersonInfo, value);
        }
        public ObservableCollection<FitnesInfoModel> FitnesInfo { get; }


        private readonly DataAccessJson _da = new DataAccessJson();
        private string _Title = "TaskTexodeTechnologies";
        private PersonModel _SelectedItemPerson;

        public PersonModel SelectedItemPerson
        {
            get
            {
                //Debug.Print(_SelectedItemPerson.Fio);
                GetPersonal();
                try
                {
                    var list = new List<int>();
                    var listDay = new List<int>();
                    for (int i = 0; i < PersonInfo.Count; i++)
                    {
                        list.Add(PersonInfo[i].Steps);
                        listDay.Add(PersonInfo[i].Day);
                    }

                    SeriesCollection = new SeriesCollection
                    {
                        new LineSeries
                        {
                            Values = new ChartValues<int>(list),
                            Title = "Шаги"
                        },
                        new ColumnSeries
                        {
                            Values = new ChartValues<int>(listDay),
                            Title = "День"
                        }
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return _SelectedItemPerson;
            }
            set => Set(ref _SelectedItemPerson, value);
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #region Commands

        public ICommand GetPeronalInfoCommand { get; }
        private bool CanGetPeronalInfoCommandExecuted(object p) => true;
        private void OnGetPeronalInfoCommandExecuted(object p)
        {
            PersonInfoList.Clear();
            //Console.WriteLine(_SelectedItemPerson.Fio);
            PersonInfoList.AddRange(from t in FitnesInfo
                where _SelectedItemPerson.Fio == t.User
                select new PersonInfoDaysModel() { Day = t.Day, Rank = t.Rank, Status = t.Status, Steps = t.Steps });
            PersonInfo = new ObservableCollection<PersonInfoDaysModel>(PersonInfoList);
        }

        private void GetPersonal()
        {
            try
            {
                PersonInfoList.Clear();
                //Console.WriteLine(_SelectedItemPerson.Fio);
                PersonInfoList.AddRange(from t in FitnesInfo
                    where _SelectedItemPerson.Fio == t.User
                    select new PersonInfoDaysModel() { Day = t.Day, Rank = t.Rank, Status = t.Status, Steps = t.Steps });
                PersonInfo = new ObservableCollection<PersonInfoDaysModel>(PersonInfoList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public ICommand MessageBoxCommand { get; }
        private bool CanMessageBoxCommandExecuted(object p) => true;
        private void OnMessageBoxCommandExecuted(object p)
        {
            MessageBox.Show("MsgBoxCommand");
        }

        #endregion

        public MainWindowViewModel()
        {
            PersonInfoList = new List<PersonInfoDaysModel>();
            PersonInfo = new ObservableCollection<PersonInfoDaysModel>();
            FitnesInfo = new ObservableCollection<FitnesInfoModel>(_da.GetFitnesInfos());
            People = new ObservableCollection<PersonModel>(_da.GetPersonsAndInfo());
            MessageBoxCommand = new LambdaCommand(OnMessageBoxCommandExecuted, CanMessageBoxCommandExecuted);
            GetPeronalInfoCommand = new LambdaCommand(OnGetPeronalInfoCommandExecuted, CanGetPeronalInfoCommandExecuted);
            var list = new List<int>();
            for (int i = 0; i < People.Count; i++)
            {
                list.Add(People[i].AvgSteps);
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<int>(list)
                }
            };
        }
    }
}
