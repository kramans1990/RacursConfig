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
    class StarSensorPageVM: BaseVM
    {
        private HttpClient httpClient;
        private string mode;
        private string route = "/api/StarSensor";
        private string deleteMessage = " Запись успешно удалена";
        private string addMessage = " Запись успешно добавлена";
        private string getMessage = " Запрос списка ЗД";
        private string editMessage = " Запись успешно изменена";
        private JsonSerializerOptions options;





        private List<StarSensor> _StarSensors;
        public List<StarSensor> StarSensors
        {
            get
            {
                return _StarSensors;
            }
            set
            {
                _StarSensors = value;
                OnPropertyChanged(nameof(StarSensors));
            }
        }


        private StarSensor _StarSensorEditor;
        public StarSensor StarSensorEditor
        {
            get
            {
                return _StarSensorEditor;
            }
            set
            {
                _StarSensorEditor = value;
                OnPropertyChanged(nameof(StarSensor));
            }
        }


        public StarSensorPageVM()
        {
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            getFlyheels();

            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => Delete(x));
            SaveCommand = new RelayCommand(x => Save(), p => canSave());
            StarSensorEditor = new StarSensor();
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
            bool result = find_FalseNum.Count() == 0 ? true : false;

            if (!result)
            {
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
            StarSensorEditor = JsonSerializer.Deserialize<StarSensor>(s, options);
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
                AddStarSensorToDataBase(StarSensorEditor);
            }
            if (mode == "Edit")
            {
                EditStarSensor(StarSensorEditor);
            }
        }
        private void Add()
        {
            EditorVisibility = Visibility.Visible;
            mode = "Add";

            StarSensorEditor = new StarSensor();

        }
        private async void Delete(object ElMagnet)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, route);
                string content = JsonConvert.SerializeObject(ElMagnet);
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
        private async void EditStarSensor(StarSensor StarSensor)
        {
            try
            {


                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, route);
                string content = JsonConvert.SerializeObject(StarSensor);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    Messages.Add(GetTimeLabel() + editMessage);
                    getFlyheels();

                }
                else { Messages.Add(GetTimeLabel() + response.ReasonPhrase); }
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
                    StarSensors = JsonSerializer.Deserialize<List<StarSensor>>(result.Result, options);
                }
                else { Messages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private async void AddStarSensorToDataBase(StarSensor StarSensor)
        {
            try
            {


                string content = JsonConvert.SerializeObject(StarSensor);
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
