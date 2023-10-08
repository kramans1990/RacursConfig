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
using RacursLib.LibMath;
using Matrix3 = RacursCore.types.Matrix3;

namespace RacursConfig.PagesVM
{
    class MagnetometersPageVM :BaseVM
    {
        private HttpClient httpClient;
        private string mode;
        private string route = "/api/magnetometer";      
        private string deleteMessage = " Запись успешно удалена";
        private string addMessage = " Запись успешно добавлена";
        private string getMessage = " Запрос списка магнетометров";
        private string editMessage = " Запись успешно изменена";
        private JsonSerializerOptions options;


        private Page _Page;
        public Page Page
        {
            get { return _Page; }
            set { _Page = value; OnPropertyChanged(nameof(Page)); }
        }
     

        private List<Magnetometer> _Magnetometers;
        public List<Magnetometer> Magnetometers
        {
            get
            {
                return _Magnetometers;
            }
            set
            {
                _Magnetometers = value;
                OnPropertyChanged(nameof(Magnetometers));
            }
        }

        private Magnetometer _SelectedMagnetometer;
        public Magnetometer SelectedMagnetometer
        {
            get
            {
                return _SelectedMagnetometer;
            }
            set
            {
                _SelectedMagnetometer = value;
                OnPropertyChanged(nameof(SelectedMagnetometer));
            }
        }

        private Magnetometer _MagnetometerEditor;
        public Magnetometer MagnetometerEditor
        {
            get
            {
                return _MagnetometerEditor;
            }
            set
            {
                _MagnetometerEditor = value;
                OnPropertyChanged(nameof(MagnetometerEditor));
            }
        }

        private Matrix3 _Attm;
        public Matrix3 Attm
        {
            get
            {
                return _Attm;
            }
            set
            {
                _Attm = value;
                OnPropertyChanged(nameof(Attm));
            }
        }

        private Matrix3 _Skew;
        public Matrix3 Skew
        {
            get
            {
                return _Skew;
            }
            set
            {
                _Skew = value;
                OnPropertyChanged(nameof(Skew));
            }
        }

        private Attitude _Att;
        public Attitude Att
        {
            get
            {
                return _Att;
            }
            set
            {
                _Att = value;
                OnPropertyChanged(nameof(Att));
            }
        }

       

        public MagnetometersPageVM() {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            getMagnetometers();
            SelectedMagnetometer = new Magnetometer();           
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save(),p=>canSave());
            MagnetometerEditor = new Magnetometer();
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

        private void Edit(object magnetometer)
        {
            
            // Magnetometers = JsonSerializer.Deserialize<Magnetometer>(result.Result, options);
            string s = JsonConvert.SerializeObject(magnetometer);
            MagnetometerEditor = JsonSerializer.Deserialize<Magnetometer>(s, options);
            //Att = MagnetometerEditor.Att;
            //Attm = MagnetometerEditor.Attm;
            //Skew = MagnetometerEditor.Skew;
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
                AddMagnetometerToDataBase(MagnetometerEditor);
            }
            if (mode == "Edit")
            {
                EditMagnetometer(MagnetometerEditor);
            }
        }
        private void Add()
        {   
            EditorVisibility = Visibility.Visible;
            mode = "Add";
            Attm = new Matrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);
            Skew = new Matrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);
            Att = new Attitude(0, 0, 0, 0);
            MagnetometerEditor = new Magnetometer();
          
        }
        private async void Delete(object magnetometer)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, route);
                string content = JsonConvert.SerializeObject(magnetometer);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Messages.Add(GetTimeLabel() + deleteMessage);
                    getMagnetometers();

                }
               else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void EditMagnetometer(Magnetometer magnetometer)
        {
            try
            {   
                //magnetometer.Att = Att;
                //magnetometer.Attm=Attm;
                //magnetometer.Skew = Skew;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, route);
                string content = JsonConvert.SerializeObject(magnetometer);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + editMessage);
                    getMagnetometers();

                }
               else {Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                    Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void getMagnetometers()
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    Messages.Add(GetTimeLabel() + getMessage);                  
                    Magnetometers = JsonSerializer.Deserialize<List<Magnetometer>>(result.Result,options);                    
                }
              else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void AddMagnetometerToDataBase(Magnetometer magnetometer)
        {
            try
            {
                //magnetometer.Skew = Skew;
                //magnetometer.Attm = Attm;
                magnetometer.Att = Att;
                string content = JsonConvert.SerializeObject(magnetometer);
                //content = System.Text.Json.JsonSerializer.Serialize(magnetometer);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, route);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + addMessage);
                    getMagnetometers();

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
