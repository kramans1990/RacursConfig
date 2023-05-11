using RacursCore;
using RacursCore.SatilliteComponents;
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
using System.CodeDom;

namespace RacursConfig.Pages
{
    /// <summary>
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {   
        public SatelliteComponent satelliteComponent { get; set; }
        private HttpClient httpClient;
        private Type Type { get; set; }
        private string mode;
        private string routeGyro = "/api/gyro";
        private string routeElMagnet = "/api/ElMagnet";
        private string routeARS = "/api/ARS";
        private string routeFlyWheel = "/api/FlyWheel";
        private string routeMagnetometer = "/api/Magnetometer";
        private string deleteMessage = " Запись успешно удалена";
        private string addMessage = " Запись успешно добавлена";
        private string getMessage = " Запрос списка ДУС";
        private string editMessage = " Запись успешно изменена";
        private JsonSerializerOptions options;
        private List<SatelliteComponent> components = new List<SatelliteComponent>(); 
        public SatelliteComponent SelectedConponent { get; set; }
        public DialogWindow(string type)
        {   
            InitializeComponent();
            string route = "";
            switch (type) {
                 case "Gyro":
                        route = routeGyro;                    
                    break;
                case "ElMagnet":
                    route = routeElMagnet;
                    break;
                case "Magnetometer":
                    route = routeMagnetometer;
                    break;
                case "Flywheel":
                    route = routeFlyWheel;
                    break;
                case "ARS":
                    route = routeARS;
                    break;
            }
            ComponentList.SelectionChanged += ComponentList_SelectionChanged;
            AddButton.Click += AddButton_Click;
            CancelButton.Click += CancelButton_Click;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            getComponents(route, type);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedConponent = null;
            this.DialogResult = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedConponent = ComponentList.SelectedItem as SatelliteComponent;
            DialogResult = true;
        }

        private void ComponentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddButton.IsEnabled = true;
        }

        private async void getComponents(string route, string type)
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                  
                    switch (type)
                    {
                        case "Gyro":
                            var componentsGyro = JsonSerializer.Deserialize <List<Gyro>> (result.Result, options);
                            components.AddRange(componentsGyro);
                            break;
                        case "ElMagnet":
                            var componentsElMagnet = JsonSerializer.Deserialize<List<ElMagnet>>(result.Result, options);
                            components.AddRange(componentsElMagnet);
                            break;
                        case "Magnetometer":
                            var componentsMagnetometer = JsonSerializer.Deserialize<List<Magnetometer>>(result.Result, options);
                            components.AddRange(componentsMagnetometer);
                            break;
                        case "Flywheel":
                            var componentsFlywheel = JsonSerializer.Deserialize<List<Flywheel>>(result.Result, options);
                            components.AddRange(componentsFlywheel);
                            break;
                          
                        case "ARS":
                            var componentsARS = JsonSerializer.Deserialize<List<ARS>>(result.Result, options);
                            components.AddRange(componentsARS);
                            break;
                    }

                   
                    ComponentList.ItemsSource = components;
                //ComponentList.SelectedIndex = ComponentList.Items.Count > 0 ? 0 : -1;
                //AddButton.IsEnabled = ComponentList.SelectedIndex == -1 ? false : true;
                    //else { Messages.Add(response.ReasonPhrase); }
                }
            }
            catch (Exception exception)
            {
               
                //Messages.Add(GetTimeLabel() + exception.Message);
            }
        }
    }
}
