using mvvm_ftpclient.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm_ftpclient.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public MainMenuViewModel TestViewModel { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; NotifyPropertyChanged(); }
        }
        public MainViewModel()
        {
            TestViewModel = new MainMenuViewModel();
            CurrentView = TestViewModel;
        }
    }
}
