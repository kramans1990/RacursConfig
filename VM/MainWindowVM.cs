
using RacursConfig.Pages;
using RacursConfig.PagesVM;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RacursConfig
{
    internal class MainWindowVM : BaseVM
    {
        /// <summary>
        /// Строка для отображения версии программы
        /// </summary>
        public string Version { get; private set; }

        private Page _CurrentPage;
        /// <summary>
        /// Текущая страница для отображения
        /// </summary>
        public Page CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
               
                _CurrentPage = value;              
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public List<Page> pages { get; set; }



        

       

        public RelayCommand StationsPageCommand => new RelayCommand(x => { CurrentPage = new StationsPage(); });
        public RelayCommand ElMagnetsPageCommand => new RelayCommand(x => { CurrentPage = new ElMagnetPage(); });

        public RelayCommand MagnetometersPageCommand => new RelayCommand(x => { CurrentPage = new MagnetometersPage(); });
        public RelayCommand ARSPageCommand => new RelayCommand(x => { CurrentPage = new ARSPage(); });
        public RelayCommand FlyWheelPageCommand => new RelayCommand(x => { CurrentPage = new FlyWheelPage(); });

        public RelayCommand GyroPageCommand => new RelayCommand(x => { CurrentPage = new GyroPage(); });
        public MainWindowVM()
        {
           
            CurrentPage = new StationsPage();
        }

    
    }
}
