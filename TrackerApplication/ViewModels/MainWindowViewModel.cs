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
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using TrackerApplication.Data;
using TrackerApplication.Helpers.Exports;
using TrackerApplication.Infrastructure.Commands;
using TrackerApplication.Models;
using TrackerApplication.ViewModels.Base;

namespace TrackerApplication.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Lists
        public ObservableCollection<PersonModel> People { get; set; }
        public List<PersonInfoDaysModel> PersonInfoList { get; set; }
        public ObservableCollection<FitnesInfoModel> FitnesInfo { get; set; }//onproperty add

        //For Chart
        List<int> listSteps = new List<int>();
        List<int> listDays = new List<int>();

        #endregion

        //Other
        private readonly DataAccessJson _da = new DataAccessJson();


        #region SeriesCollection - Collection for Chart

        /// <summary>
        /// Collection
        /// </summary>
        private SeriesCollection _SeriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _SeriesCollection;
            set => Set(ref _SeriesCollection, value);
        }

        #endregion

        #region Fucntions
        private void GetPersonal()
        {
            try
            {
                PersonInfoList.Clear();
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

        private void MsgBoxSuccessExport(bool isTrue)
        {
            if (isTrue)
                MessageBox.Show("Export complete", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Export error", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearList()
        {
            listSteps.Clear();
            listDays.Clear();
        }

        private void BindChart()
        {
            try
            {
                ClearList();
                foreach (var t in PersonInfo)
                {
                    listSteps.Add(t.Steps);
                    listDays.Add(t.Day);
                }

                int min = 0;
                int max = listSteps.Prepend(0).Max();
                min = listSteps.Prepend(max).Min();

                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<int>(listSteps),
                        Title = "Шагов",
                        Configuration = Mappers.Xy<int>()
                            .X((value, index) => index)
                            .Y((value, index) => value)
                            .Stroke((value, index) =>  value == max || value == min ? Brushes.Red : null)
                            .Fill((value, index) => value == max || value == min ? Brushes.Red : null)
                    },
                    new ColumnSeries
                    {
                        Values = new ChartValues<int>(listDays),
                        Title = "День"
                    }
                };
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        #endregion


        private ObservableCollection<PersonInfoDaysModel> _PersonInfo;
        public ObservableCollection<PersonInfoDaysModel> PersonInfo
        {
            get => _PersonInfo;
            set => Set(ref _PersonInfo, value);
        }
        
        private PersonModel _SelectedItemPerson;
        public PersonModel SelectedItemPerson
        {
            get
            {
                //Debug.Print(_SelectedItemPerson.Fio);
                GetPersonal();
                BindChart();
                return _SelectedItemPerson;
            }
            set => Set(ref _SelectedItemPerson, value);
        }

        private PersonModel _GetSteps;
        public int GetSteps
        {
            get
            {
                if (_GetSteps.AvgSteps == null || _GetSteps.AvgSteps == 0)
                    _GetSteps.AvgSteps = 0;
                return _GetSteps.AvgSteps;
            }
            set
            {
                var avgSteps = _SelectedItemPerson.AvgSteps;
                Set(ref avgSteps, value);
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        private string _Title = "TaskTexodeTechnologies";
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
            PersonInfoList.AddRange(from t in FitnesInfo
                where _SelectedItemPerson.Fio == t.User
                select new PersonInfoDaysModel() { Day = t.Day, Rank = t.Rank, Status = t.Status, Steps = t.Steps });
            PersonInfo = new ObservableCollection<PersonInfoDaysModel>(PersonInfoList);
        }

        public ICommand ReadJsonFileCommand { get; }
        private bool CanReadJsonFileExecute(object p) => true;
        private void OnReadJsonFileExecuted(object p)
        {
            FitnesInfo = new ObservableCollection<FitnesInfoModel>(_da.ReadJsonFile());
        }

        public ICommand ReadJsonFilesCommand { get; }
        private bool CanReadJsonFilesExecute(object p) => true;
        private void OnReadJsonFilesExecuted(object p)
        {
            FitnesInfo = new ObservableCollection<FitnesInfoModel>(_da.ReadJsonFiles());
        }


        /// <summary>
        /// Check Correct working command
        /// </summary>
        public ICommand MessageBoxCommand { get; }
        private bool CanMessageBoxCommandExecuted(object p) => true;
        private void OnMessageBoxCommandExecuted(object p)
        {
            MessageBox.Show("MsgBoxCommand");
        }

        public ICommand ExportToCsvCommand { get; }
        private bool CanExportToCsvCommandExecuted(object p) => true;
        private void OnExportToCsvCommandExecuted(object p)
        {
            MsgBoxSuccessExport(ExportCsv.Export(_da.GetPersonsAndInfo()));
        }

        public ICommand ExportToJsonCommand { get; }
        private bool CanExportToJsonCommandExecuted(object p) => true;
        private void OnExportToJsonCommandExecuted(object p)
        {
            MsgBoxSuccessExport(ExportJson.Export(_da.GetPersonsAndInfo()));
        }

        public ICommand ExportToXmlCommand { get; }
        private bool CanExportToXmlCommandExecuted(object p) => true;
        private void OnExportToXmlCommandExecuted(object p)
        {
            MsgBoxSuccessExport(ExportXml.Export(_da.GetPersonsAndInfo()));
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
            ExportToCsvCommand = new LambdaCommand(OnExportToCsvCommandExecuted, CanExportToCsvCommandExecuted);
            ExportToJsonCommand = new LambdaCommand(OnExportToJsonCommandExecuted, CanExportToJsonCommandExecuted);
            ExportToXmlCommand = new LambdaCommand(OnExportToXmlCommandExecuted, CanExportToXmlCommandExecuted);
            ReadJsonFileCommand = new LambdaCommand(OnReadJsonFileExecuted, CanReadJsonFileExecute);
            ReadJsonFilesCommand = new LambdaCommand(OnReadJsonFilesExecuted, CanReadJsonFilesExecute);
        }
    }
}
