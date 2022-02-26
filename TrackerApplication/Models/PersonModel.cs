using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TrackerApplication.Annotations;

namespace TrackerApplication.Models
{
    internal class PersonModel : INotifyPropertyChanged
    {
        public string Fio { get; set; }
        public int AvgSteps { get; set; }
        public int MaxSteps { get; set; }
        public int MinSteps { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
