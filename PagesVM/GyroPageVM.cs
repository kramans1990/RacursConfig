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
    class GyroPageVM :BaseVM
    {
        private HttpClient httpClient;
        private string mode;
        private string route = "/api/gyro";      
        private string deleteMessage = " Запись успешно удалена";
        private string addMessage = " Запись успешно добавлена";
        private string getMessage = " Запрос списка Гиропар";
        private string editMessage = " Запись успешно изменена";
        private JsonSerializerOptions options;


        private Page _Page;
        public Page Page
        {
            get { return _Page; }
            set { _Page = value; OnPropertyChanged(nameof(Page)); }
        }
     

        private List<Gyro> _Gyros;
        public List<Gyro> Gyros
        {
            get
            {
                return _Gyros;
            }
            set
            {
                _Gyros = value;
                OnPropertyChanged(nameof(Gyros));
            }
        }

        private Gyro _SelectedGyro;
        public Gyro SelectedGyro
        {
            get
            {
                return _SelectedGyro;
            }
            set
            {
                _SelectedGyro = value;
                OnPropertyChanged(nameof(SelectedGyro));
            }
        }

        private Gyro _GyroEditor;
        public Gyro GyroEditor
        {
            get
            {
                return _GyroEditor;
            }
            set
            {
                _GyroEditor = value;
                OnPropertyChanged(nameof(GyroEditor));
            }
        }


        private GyroVectors _GyroVectorY;
        public GyroVectors GyroVectorY
        {
            get
            {
                return _GyroVectorY;
            }
            set
            {
                _GyroVectorY = value;
                OnPropertyChanged(nameof(GyroVectorY));
            }
        }
        private GyroVectors _GyroVectorX;
        public GyroVectors GyroVectorX
        {
            get
            {
                return _GyroVectorX;
            }
            set
            {
                _GyroVectorX = value;
                OnPropertyChanged(nameof(GyroVectorX));
            }
        }
        private GyroVectors _GyroVectorZ;
        public GyroVectors GyroVectorZ
        {
            get
            {
                return _GyroVectorZ;
            }
            set
            {
                _GyroVectorZ = value;
                OnPropertyChanged(nameof(GyroVectorZ));
            }
        }


        public GyroPageVM() {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            getGyros();
            SelectedGyro = new Gyro();           
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save(),p=>canSave());
            GyroEditor = new Gyro();
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
            List<NumberField> fieldsNum = FindVisualChildren<NumberField>(frame.First()).ToList();
            var find_FalseNum = fieldsNum.Where(p => p.IsValid == false);
            bool result =  find_FalseNum.Count() == 0 ? true : false;

            if (!result) {
                return result;
            }

            List<TextField> fieldsText = FindVisualChildren<TextField>(frame.First()).ToList();
            var find_FalseText = fieldsText.Where(p => p.IsValid == false);
            bool resultText = find_FalseText.Count() == 0 ? true : false;

            return resultText;
        }

        private void Edit(object Gyro)
        {
           
            string s = JsonConvert.SerializeObject(Gyro);
            GyroEditor = JsonSerializer.Deserialize<Gyro>(s, options);
            GyroVectorX = GyroEditor.XPair;
            GyroVectorY = GyroEditor.YPair;
            GyroVectorZ = GyroEditor.ZPair;
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
                AddGyroToDataBase(GyroEditor);
            }
            if (mode == "Edit")
            {
                EditGyro(GyroEditor);
            }
        }
        private void Add()
        {   
            EditorVisibility = Visibility.Visible;
            mode = "Add";
            //Axis = new Vector(0,0,0);
            //Att = new Attitude(0, 0, 0, 0);
            GyroVectorX = new GyroVectors(new Vector(0,0,0),new Vector(0,0,0));
            GyroVectorY = new GyroVectors(new Vector(0, 0, 0), new Vector(0, 0, 0));
            GyroVectorZ = new GyroVectors(new Vector(0, 0, 0), new Vector(0, 0, 0));
            GyroEditor = new Gyro();
          
        }
        private async void Delete(object Gyro)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, route);
                string content = JsonConvert.SerializeObject(Gyro);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Messages.Add(GetTimeLabel() + deleteMessage);
                    getGyros();

                }
               else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void EditGyro(Gyro Gyro)
        {
            try
            {   
                //Gyro.Att = Att;
                //Gyro.Axis = Axis;
                Gyro.XPair = GyroVectorX;
                Gyro.YPair = GyroVectorY;
                Gyro.ZPair = GyroVectorZ;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, route);
                string content = JsonConvert.SerializeObject(Gyro);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + editMessage);
                    getGyros();

                }
               else {Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                    Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void getGyros()
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    Messages.Add(GetTimeLabel() + getMessage);                  
                    Gyros = JsonSerializer.Deserialize<List<Gyro>>(result.Result,options);                    
                }
              else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void AddGyroToDataBase(Gyro Gyro)
        {
            try
            {
                Gyro.XPair = GyroVectorX;
                Gyro.YPair = GyroVectorY;
                Gyro.ZPair = GyroVectorZ;
                //Gyro.Axis = Axis;
                //Gyro.Att = Att;
                string content = JsonConvert.SerializeObject(Gyro);
                //content = System.Text.Json.JsonSerializer.Serialize(Gyro);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, route);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + addMessage);
                    getGyros();

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
