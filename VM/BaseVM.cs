using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace RacursConfig
{
    public abstract class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        public static void NotifyStaticPropertyChanged([CallerMemberName] string name = null)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(name));
        }
        public RelayCommand AddCommand
        {
            get;set;
        }
        public RelayCommand CancelCommand
        {
            get; set;
        }
        public RelayCommand SaveCommand
        {
            get; set;
        }
        public RelayCommand DeleteCommand
        {
            get; set;
        }
        public RelayCommand EditCommand
        {
            get; set;
        }
        private List<string> _WarningMessages;
        public List<string> WarningMessages
        {
            get
            {
                return _WarningMessages;
            }
            set
            {
                _WarningMessages = value;
                OnPropertyChanged(nameof(WarningMessages));
            }
        }

        private ObservableCollection<string> _Messages;

        public ObservableCollection<string> Messages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }


        private Visibility _EditorVisibility;
        public Visibility EditorVisibility
        {
            get
            {
                return _EditorVisibility;
            }
            set
            {
                _EditorVisibility = value;

                OnPropertyChanged(nameof(EditorVisibility));
            }
        }

        public string GetTimeLabel()
        {
            return "[" + DateTime.Now.ToString() + "] ";
        }
        /// <summary>
        /// метод для получения элементов управления определенного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
            }
        }
    }
}