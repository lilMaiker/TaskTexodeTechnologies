using System;
using System.Collections.Generic;
using System.Text;
using TrackerApplication.ViewModels.Base;

namespace TrackerApplication.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private string _Title = "TaskTexodeTechnologies";

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
    }
}
