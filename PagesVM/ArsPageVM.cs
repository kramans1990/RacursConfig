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
    class ArsPageVM :BaseVM
    {
        private HttpClient httpClient;
        private string mode;
        private string route = "/api/ARS";      
        private string deleteMessage = " Запись успешно удалена";
        private string addMessage = " Запись успешно добавлена";
        private string getMessage = " Запрос списка ДУС";
        private string editMessage = " Запись успешно изменена";
        private JsonSerializerOptions options;


        private Page _Page;
        public Page Page
        {
            get { return _Page; }
            set { _Page = value; OnPropertyChanged(nameof(Page)); }
        }
     

        private List<ARS> _ARSes;
        public List<ARS> ARSes
        {
            get
            {
                return _ARSes;
            }
            set
            {
                _ARSes = value;
                OnPropertyChanged(nameof(ARSes));
            }
        }

        private ARS _SelectedARS;
        public ARS SelectedARS
        {
            get
            {
                return _SelectedARS;
            }
            set
            {
                _SelectedARS = value;
                OnPropertyChanged(nameof(SelectedARS));
            }
        }

        private ARS _ARSEditor;
        public ARS ARSEditor
        {
            get
            {
                return _ARSEditor;
            }
            set
            {
                _ARSEditor = value;
                OnPropertyChanged(nameof(ARSEditor));
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


        public ArsPageVM() {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            getARSs();
            SelectedARS = new ARS();           
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save(),p=>canSave());
            ARSEditor = new ARS();
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

        private void Edit(object ARS)
        {
           
            string s = JsonConvert.SerializeObject(ARS);
            ARSEditor = JsonSerializer.Deserialize<ARS>(s, options);
            Att = ARSEditor.Att;
            Axis = ARSEditor.Axis;
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
                AddARSToDataBase(ARSEditor);
            }
            if (mode == "Edit")
            {
                EditARS(ARSEditor);
            }
        }
        private void Add()
        {   
            EditorVisibility = Visibility.Visible;
            mode = "Add";
            Axis = new Vector(0,0,0);
            Att = new Attitude(0, 0, 0, 0);
            ARSEditor = new ARS();
          
        }
        private async void Delete(object ARS)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, route);
                string content = JsonConvert.SerializeObject(ARS);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Messages.Add(GetTimeLabel() + deleteMessage);
                    getARSs();

                }
               else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void EditARS(ARS ARS)
        {
            try
            {   
                ARS.Att = Att;
                ARS.Axis = Axis;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, route);
                string content = JsonConvert.SerializeObject(ARS);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + editMessage);
                    getARSs();

                }
               else {Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                    Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void getARSs()
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    Messages.Add(GetTimeLabel() + getMessage);                  
                    ARSes = JsonSerializer.Deserialize<List<ARS>>(result.Result,options);                    
                }
              else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
               Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void AddARSToDataBase(ARS ARS)
        {
            try
            {
               
                ARS.Axis = Axis;
                ARS.Att = Att;
                string content = JsonConvert.SerializeObject(ARS);
                //content = System.Text.Json.JsonSerializer.Serialize(ARS);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, route);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + addMessage);
                    getARSs();

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
