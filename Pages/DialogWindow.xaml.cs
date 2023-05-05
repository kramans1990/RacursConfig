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
namespace RacursConfig.Pages
{
    /// <summary>
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {   
        public SatelliteComponent satelliteComponent { get; set; }
        private HttpClient httpClient;
        private string mode;
        private string routeGyro = "/api/gyro";
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
            ComponentList.SelectionChanged += ComponentList_SelectionChanged;
            AddButton.Click += AddButton_Click;
            CancelButton.Click += CancelButton_Click;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            getComponents(routeGyro);
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

        private async void getComponents(string route)
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    var components___ = JsonSerializer.Deserialize<List<Gyro>>(result.Result, options);
                    components.AddRange(components___);
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
