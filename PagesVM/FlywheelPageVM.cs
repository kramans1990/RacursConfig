using Newtonsoft.Json;
using RacursCore.SatilliteComponents;
using RacursCore.types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text.Json;
using System.Windows.Media;
using System.Windows.Controls;
using RacursConfig.Controls;
using System.Collections.ObjectModel;
using Vector = RacursCore.types.Vector;

namespace RacursConfig.PagesVM
{
    class FlywheelPageVM :BaseVM
    {
        private HttpClient httpClient;
        private string mode;
        private string route = "/api/Flywheel";      
        private string deleteMessage = " Запись успешно удалена";
        private string addMessage = " Запись успешно добавлена";
        private string getMessage = " Запрос списка Маховиков";
        private string editMessage = " Запись успешно изменена";
        private JsonSerializerOptions options;


     
     

        private List<Flywheel> _Flywheels;
        public List<Flywheel> Flywheels
        {
            get
            {
                return _Flywheels;
            }
            set
            {
                _Flywheels = value;
                OnPropertyChanged(nameof(Flywheels));
            }
        }

        private Flywheel _SelectedFlywheel;
        public Flywheel SelectedFlywheel
        {
            get
            {
                return _SelectedFlywheel;
            }
            set
            {
                _SelectedFlywheel = value;
                OnPropertyChanged(nameof(SelectedFlywheel));
            }
        }

        private Flywheel _FlywheelEditor;
        public Flywheel FlywheelEditor
        {
            get
            {
                return _FlywheelEditor;
            }
            set
            {
                _FlywheelEditor = value;
                OnPropertyChanged(nameof(FlywheelEditor));
            }
        }

        private Vector _Axis;
        public Vector Axis
        {
            get
            {
                return _Axis;
            }
            set
            {
                _Axis = value;
                OnPropertyChanged(nameof(Axis));
            }
        }


        public FlywheelPageVM() {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            getFlyheels();
            SelectedFlywheel = new Flywheel();           
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save(),p=>canSave());
            FlywheelEditor = new Flywheel();
            EditCommand = new RelayCommand(x => Edit(x));
            Messages = new ObservableCollection<string>();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        }

        private bool canSave()
        {
            DependencyObject do_ = (Application.Current.MainWindow);
            var frame = FindVisualChildren<Frame>((do_));
            List<NumberField> fieldsNum = FindVisualChildren<NumberField>(frame.First()).ToList<NumberField>();
            var find_FalseNum = fieldsNum.Where(p => p.IsValid == false);
            bool result =  find_FalseNum.Count() == 0 ? true : false;

            if (!result) {
                return result;
            }

            List<TextField> fieldsText = FindVisualChildren<TextField>(frame.First()).ToList<TextField>();
            var find_FalseText = fieldsText.Where(p => p.IsValid == false);
            bool resultText = find_FalseText.Count() == 0 ? true : false;

            return resultText;
        }

        private void Edit(object wheel)
        {
           
            string s = JsonConvert.SerializeObject(wheel);
            FlywheelEditor = JsonSerializer.Deserialize<Flywheel>(s, options);         
            Axis = FlywheelEditor.Axis;
            EditorVisibility = Visibility.Visible;
            mode = "Edit";          


        }
        private void Cancel()
        {
            EditorVisibility = Visibility.Hidden;
        }
        private void Save()
        {
            if (mode == "Add")
            {
                AddWheelToDataBase(FlywheelEditor);
            }
            if (mode == "Edit")
            {
                EditFlywheel(FlywheelEditor);
            }
        }
        private void Add()
        {   
            EditorVisibility = Visibility.Visible;
            mode = "Add";
            Axis = new Vector(0,0,0);
            FlywheelEditor = new Flywheel();
          
        }
        private async void Delete(object flywheel)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, route);
                string content = JsonConvert.SerializeObject(flywheel);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Messages.Add(GetTimeLabel() + deleteMessage);
                    getFlyheels();

                }
               else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void EditFlywheel(Flywheel flywheel)
        {
            try
            {

                flywheel.Axis = Axis;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, route);
                string content = JsonConvert.SerializeObject(flywheel);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + editMessage);
                    getFlyheels();

                }
               else {Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                    Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void getFlyheels()
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    Messages.Add(GetTimeLabel() + getMessage);                  
                    Flywheels = JsonSerializer.Deserialize<List<Flywheel>>(result.Result,options);                    
                }
              else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void AddWheelToDataBase(Flywheel flywheel)
        {
            try
            {

                flywheel.Axis = Axis;
                string content = JsonConvert.SerializeObject(flywheel);
                //content = System.Text.Json.JsonSerializer.Serialize(ARS);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, route);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + addMessage);
                    getFlyheels();

                }
                else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                 Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
       
    }
   
}
