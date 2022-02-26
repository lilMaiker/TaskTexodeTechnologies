using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TrackerApplication.Data;
using TrackerApplication.Infrastructure.Commands;
using TrackerApplication.Models;
using TrackerApplication.ViewModels.Base;

namespace TrackerApplication.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ObservableCollection<PersonModel> People { get; set; }
        private readonly DataAccessJson _da = new DataAccessJson();
        private string _Title = "TaskTexodeTechnologies";

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #region Commands

        public ICommand MessageBoxCommand { get; }

        private bool CanMessageBoxCommandExecuted(object p) => true;

        private void OnMessageBoxCommandExecuted(object p)
        {
            MessageBox.Show("MsgBoxCommand");
        }

        #endregion

       


        public MainWindowViewModel()
        {
            People = new ObservableCollection<PersonModel>(_da.GetPersonsAndInfo());
            MessageBoxCommand = new LambdaCommand(OnMessageBoxCommandExecuted, CanMessageBoxCommandExecuted);
        }
    }
}
