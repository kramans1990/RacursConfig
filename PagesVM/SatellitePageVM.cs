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
using RacursConfig.Pages;

namespace RacursConfig.PagesVM
{
    class SatellitePageVM : BaseVM
    {
        private HttpClient httpClient;
        private string mode;
        private string routeSatellite = "api/Satellite";
        //private string routeFlyWheels = "/api/Flywheel";
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



        public RelayCommand DeleteItemCommand
        {
            get; set;
        }
        public RelayCommand AddItemCommand
        {
            get; set;
        }

        public SatellitePageVM()
        {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            getSatellites();         
            SelectedSatellite = new Satellite();
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save(), p => canSave());
            SelectedSatellite = new Satellite();
            EditCommand = new RelayCommand(x => Edit(x));
            DeleteItemCommand = new RelayCommand(x => DeleteSatelliteItem(x));
            AddItemCommand = new RelayCommand(x => AddSatelliteItem(x));
            Messages = new ObservableCollection<string>();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        }

        private void AddSatelliteItem(object type)
        {
            DialogWindow dialogWindow = new DialogWindow(type.ToString());
            if (dialogWindow.ShowDialog() == true) { 
                 SatelliteComponent component = dialogWindow.SelectedConponent;
                if (component != null) { 
                    switch (type) {
                        case "Gyro":
                            SatelliteEditor.Gyros.Add((component as Gyro));
                            break;
                        case "ElMagnet":
                            SatelliteEditor.ElMagnets.Add((component as ElMagnet));
                            break;
                        case "Magnetometer":
                            SatelliteEditor.Magnetometers.Add((component as Magnetometer));
                            break;
                        case "Flywheel":
                            SatelliteEditor.Flywheels.Add((component as Flywheel));
                            break;
                        case "ARS":
                            SatelliteEditor.Ars.Add((component as ARS));
                            break;
                    }
                    SatelliteEditor = JsonSerializer.Deserialize<SatelliteModel>(JsonConvert.SerializeObject(SatelliteEditor), options);
                }
            }
        }

        private void DeleteSatelliteItem(object x)
        {
            var type = x.GetType();
            switch (type.Name) {
                case  "Gyro": SatelliteEditor.Gyros.Remove((x as Gyro));                   
                    break;
                case "ElMagnet":
                    SatelliteEditor.ElMagnets.Remove((x as ElMagnet));
                    break;
                case "Magnetometer":
                    SatelliteEditor.Magnetometers.Remove((x as Magnetometer));
                    break;
                case "ARS":
                    SatelliteEditor.Ars.Remove((x as ARS));
                    break;
                case "Flywheel":
                    SatelliteEditor.Flywheels.Remove((x as Flywheel));
                    break;

            }
            SatelliteEditor = JsonSerializer.Deserialize<SatelliteModel>(JsonConvert.SerializeObject(SatelliteEditor), options);

        }

        //private async void getFlyWheels()
        //{
        //    try
        //    {
        //        var response = await httpClient.GetAsync(routeFlyWheels);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = response.Content.ReadAsStringAsync();
        //            Messages.Add(GetTimeLabel() + getMessage);
        //            Flywheels = JsonSerializer.Deserialize<List<Flywheel>>(result.Result, options);
        //        }
        //        else { Messages.Add(response.ReasonPhrase); }
        //    }
        //    catch (Exception exception)
        //    {
        //        Messages.Add(GetTimeLabel() + exception.Message);
        //    }
        //}
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
               AddSatelliteToDataBase(SatelliteEditor);
            }
            if (mode == "Edit")
            {
                EditSatellite(SatelliteEditor);
            }
        }
        private void Add()
        {
            EditorVisibility = Visibility.Visible;
            mode = "Add";          
            SatelliteEditor = new SatelliteModel(new Satellite());

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
        private async void EditSatellite(SatelliteModel satelliteModel)
        {
            try
            {

                int[] gyros = satelliteModel.Gyros.Select(x => x.Id).ToArray();
                int[] elMagnets = satelliteModel.ElMagnets.Select(x => x.Id).ToArray();
                int[] arses = satelliteModel.Ars.Select(x => x.Id).ToArray();
                int[] magnetometers = satelliteModel.Magnetometers.Select(x => x.Id).ToArray();
                int[] Flywheels = satelliteModel.Flywheels.Select(x => x.Id).ToArray();

                Satellite satellite = SatelliteEditor.Satellite;
                satellite.Gyros = gyros;
                satellite.Magnetometers = magnetometers;
                satellite.ARS = arses;
                satellite.ElMagnets = elMagnets;
                satellite.Wheels = Flywheels;




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
        private async void AddSatelliteToDataBase(SatelliteModel satelliteModel)
        {
            try
            {


                int[] gyros = satelliteModel.Gyros.Select(x => x.Id).ToArray();
                int[] elMagnets = satelliteModel.ElMagnets.Select(x => x.Id).ToArray();
                int[] arses = satelliteModel.Ars.Select(x => x.Id).ToArray();
                int[] magnetometers = satelliteModel.Magnetometers.Select(x => x.Id).ToArray();
                int[] Flywheels = satelliteModel.Flywheels.Select(x => x.Id).ToArray();

                Satellite satellite = SatelliteEditor.Satellite;
                satellite.Gyros = gyros;
                satellite.Magnetometers = magnetometers;
                satellite.ARS = arses;
                satellite.ElMagnets = elMagnets;
                satellite.Wheels = Flywheels;

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

