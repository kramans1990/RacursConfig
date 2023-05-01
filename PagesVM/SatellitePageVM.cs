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
using RacursCore;

namespace RacursConfig.PagesVM
{
    class SatellitePageVM : BaseVM
    {
        private HttpClient httpClient;
        private string mode;
        private string routeSatellite = "/Satellite";
        private string routeFlyWheels = "/api/Flywheel";
        private string deleteMessage = " Запись успешно удалена";
        private string addMessage = " Запись успешно добавлена";
        private string getMessage = " Запрос списка спутников";
        private string editMessage = " Запись успешно изменена";
        private JsonSerializerOptions options;


        private Page _Page;
        public Page Page
        {
            get { return _Page; }
            set { _Page = value; OnPropertyChanged(nameof(Page)); }
        }


        private List<Satellite> _Satellites;
        public List<Satellite> Satellites
        {
            get
            {
                return _Satellites;
            }
            set
            {
                _Satellites = value;
                OnPropertyChanged(nameof(Satellites));
            }
        }
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

        private Satellite _SelectedSatellite;
        public Satellite SelectedSatellite
        {
            get
            {
                return _SelectedSatellite;
            }
            set
            {
                _SelectedSatellite = value;
                OnPropertyChanged(nameof(SelectedSatellite));
            }
        }

        private SatelliteModel _SatelliteEditor;
        public SatelliteModel SatelliteEditor
        {
            get
            {
                return _SatelliteEditor;
            }
            set
            {
                _SatelliteEditor = value;
                OnPropertyChanged(nameof(SatelliteEditor));
            }
        }


     


        public SatellitePageVM()
        {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            getSatellites();
            getFlyWheels();
            SelectedSatellite = new Satellite();
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save(), p => canSave());
            SelectedSatellite = new Satellite();
            EditCommand = new RelayCommand(x => Edit(x));
            Messages = new ObservableCollection<string>();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        }
        private async void getFlyWheels()
        {
            try
            {
                var response = await httpClient.GetAsync(routeFlyWheels);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    Messages.Add(GetTimeLabel() + getMessage);
                    Flywheels = JsonSerializer.Deserialize<List<Flywheel>>(result.Result, options);
                }
                else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private bool canSave()
        {
            DependencyObject do_ = (Application.Current.MainWindow);
            var frame = FindVisualChildren<Frame>((do_));
            List<NumberField> fieldsNum = FindVisualChildren<NumberField>(frame.First()).ToList();
            var find_FalseNum = fieldsNum.Where(p => p.IsValid == false);
            bool result = find_FalseNum.Count() == 0 ? true : false;

            if (!result)
            {
                return result;
            }

            List<TextField> fieldsText = FindVisualChildren<TextField>(frame.First()).ToList();
            var find_FalseText = fieldsText.Where(p => p.IsValid == false);
            bool resultText = find_FalseText.Count() == 0 ? true : false;

            return resultText;
        }

        private void Edit(object satellite)
        {    
            int satId = ((Satellite)satellite).Id;
            getSatelliteModel(satId);
            EditorVisibility = Visibility.Visible;
            mode = "Edit";


        }

        private async void getSatelliteModel(int satId)
        {
            try
            {
                var response = await httpClient.GetAsync(routeSatellite + "/" + satId);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    Messages.Add(GetTimeLabel() + getMessage);
                    SatelliteEditor = JsonSerializer.Deserialize<SatelliteModel>(result.Result, options);
                }
                else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                Messages.Add(GetTimeLabel() + exception.Message);
            }
        }

        private void Cancel()
        {
            EditorVisibility = Visibility.Hidden;
        }
        private void Save()
        {
            if (mode == "Add")
            {
               // AddSatelliteToDataBase(SatelliteEditor);
            }
            if (mode == "Edit")
            {
               // EditSatellite(SatelliteEditor);
            }
        }
        private void Add()
        {
            EditorVisibility = Visibility.Visible;
            mode = "Add";          
            SelectedSatellite = new Satellite();

        }
        private async void Delete(object satellite)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, routeSatellite);
                string content = JsonConvert.SerializeObject(satellite);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Messages.Add(GetTimeLabel() + deleteMessage);
                    getSatellites();

                }
                else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void EditSatellite(Satellite satellite)
        {
            try
            {
               
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, routeSatellite);
                string content = JsonConvert.SerializeObject(satellite);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + editMessage);
                    getSatellites();

                }
                else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void getSatellites()
        {
            try
            {
                var response = await httpClient.GetAsync(routeSatellite);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    Messages.Add(GetTimeLabel() + getMessage);
                    Satellites = JsonSerializer.Deserialize<List<Satellite>>(result.Result, options);
                }
                else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void AddSatelliteToDataBase(Satellite satellite)
        {
            try
            {

                
                string content = JsonConvert.SerializeObject(satellite);
                //content = System.Text.Json.JsonSerializer.Serialize(ARS);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, routeSatellite);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + addMessage);
                    getSatellites();

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

