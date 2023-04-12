using Newtonsoft.Json;
using RacursCore;
using RacursCore.SatilliteComponents;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace RacursConfig.Pages
{
    public class ElMagnetPageVM :BaseVM
    {
       
           
        private HttpClient httpClient;
        private string mode;
        private string route = "/api/elmagnets";       
        private string deleteMessage = " Запись успешно удалена\n";
        private string addMessage = " Запись успешно добавлена\n";
        private string getMessage = " Запрос списка электромагнитов\n";
        private string editMessage = " Запись успешно изменена\n";
        private List<ElMagnet> _ElMagnets;
        public List<ElMagnet> ElMagnets
        {
            get
            {
                return _ElMagnets;
            }
            set
            {
                _ElMagnets = value;
                OnPropertyChanged(nameof(ElMagnets));
            }
        }
      
        private ElMagnet _SelectedElMagnet;
        public ElMagnet SelectedElMagnet
        {
            get
            {
                return _SelectedElMagnet;
            }
            set
            {
                _SelectedElMagnet = value;
                OnPropertyChanged(nameof(SelectedElMagnet));
            }
        }



        private ElMagnet _ElMagnetEditor;
        public ElMagnet ElMagnetEditor
        {
            get
            {
                return _ElMagnetEditor;
            }
            set
            {
                _ElMagnetEditor = value;
                OnPropertyChanged(nameof(ElMagnetEditor));
            }
        }

        //private Visibility _EditorVisibility;
        //public Visibility EditorVisibility
        //{
        //    get
        //    {
        //        return _EditorVisibility;
        //    }
        //    set
        //    {
        //        _EditorVisibility = value;

        //        OnPropertyChanged(nameof(EditorVisibility));
        //    }
        //}



        //private string _WarningMessages;
        //public string WarningMessages
        //{
        //    get
        //    {
        //        return _WarningMessages.ToString();
        //    }
        //    set
        //    {
        //        _WarningMessages = value;
        //        OnPropertyChanged(nameof(WarningMessages));
        //    }
        //}

       
        public ElMagnetPageVM()
        {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
         
            getElMagnets();
            SelectedElMagnet = new ElMagnet();
            WarningMessages = new List<string>();
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save());
            EditCommand = new RelayCommand(x => Edit());
            ElMagnet elMagnet = new ElMagnet();
        }

        private void Edit()
        {
            ElMagnetEditor = JsonConvert.DeserializeObject<ElMagnet>(JsonConvert.SerializeObject(SelectedElMagnet));
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
                AddElMagnetToDataBase(ElMagnetEditor);
            }
            if (mode == "Edit")
            {
                EditElMagnet(ElMagnetEditor);
            }
        }
        private void Add()
        {
            EditorVisibility = Visibility.Visible;
            mode = "Add";
            ElMagnetEditor = new ElMagnet { Name = "Новый Электромагнит", Description = "Описание" };

        }
        private async void Delete(object elMagnet)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, route);
                string content = JsonConvert.SerializeObject(elMagnet);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                   // WarningMessages += GetTimeLabel() + deleteMessage;
                    getElMagnets();

                }
                //else { _WarningMessages += GetTimeLabel() + (response.ReasonPhrase  +'\n'); }
            }
            catch (Exception exception)
            {
                //WarningMessages +=(GetTimeLabel() + exception.Message);
            }
        }
        private async void EditElMagnet(ElMagnet elMagnet)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, route);
               
                string content = JsonConvert.SerializeObject(elMagnet);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                   //WarningMessages += GetTimeLabel() + editMessage;
                    getElMagnets();

                }
              //  else { _WarningMessages += (GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                //WarningMessages += (GetTimeLabel() + exception.Message);
            }
        }       
        private async void getElMagnets()
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                  //  WarningMessages += (GetTimeLabel() + getMessage);
                    ElMagnets = JsonConvert.DeserializeObject<List<ElMagnet>>(result.Result);
                }
                //else { _WarningMessages += (response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
              // WarningMessages += (GetTimeLabel() + exception.Message);
            }
        }
        private async void AddElMagnetToDataBase(ElMagnet elMagnet)
        {
            try
            {
                
                string content = JsonConvert.SerializeObject(elMagnet);               
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/elMagnets");
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                   //  WarningMessages += GetTimeLabel() + addMessage;
                    getElMagnets();

                }
                else {
                    //arningMessages += GetTimeLabel() + response.ReasonPhrase; 
                }
            }
            catch (Exception exception)
            {
               // WarningMessages +=GetTimeLabel() + exception.Message;
            }
        }
       
    }

}