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
using RacursCore.SatelliteModel;
using RacursConfig.Models;
using RacursLib.LibMath;
using RacursConfig.Pages.SatellitePage;

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
        private Matrix3 _TI;
        public Matrix3 TI
        {
            get
            {
                return _TI;
            }
            set
            {
                _TI = value;
                OnPropertyChanged(nameof(TI));
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

        private Satellite _SatelliteEditor;
        public Satellite SatelliteEditor
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
        public RelayCommand OpenSattelliteModelEdtorCommand
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
            SatelliteEditor = new Satellite {SatelliteType= "A" , Name="Name"};
            
            EditCommand = new RelayCommand(x => Edit(x));
          
            OpenSattelliteModelEdtorCommand = new RelayCommand(x => OpenModelEditor());
            Messages = new ObservableCollection<string>();
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        }

        private void OpenModelEditor()
        {
            SatellitteComponetsEditor editorWindow = new SatellitteComponetsEditor(SatelliteEditor);
            if (editorWindow.ShowDialog() == true)
            {
             
            }
        }

       

     

      
        private bool canSave()
        {
            //DependencyObject do_ = (Application.Current.MainWindow);
            //var frame = FindVisualChildren<Frame>((do_));
            //List<NumberField> fieldsNum = FindVisualChildren<NumberField>(frame.First()).ToList();
            //var find_FalseNum = fieldsNum.Where(p => p.IsValid == false);
            //bool result = find_FalseNum.Count() == 0 ? true : false;

            //if (!result)
            //{
            //    return result;
            //}

            //List<TextField> fieldsText = FindVisualChildren<TextField>(frame.First()).ToList();
            //var find_FalseText = fieldsText.Where(p => p.IsValid == false);
            //bool resultText = find_FalseText.Count() == 0 ? true : false;

            //return resultText;
            return true;
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
                    SatelliteEditor = JsonSerializer.Deserialize<Satellite>(result.Result, options);
                    SatelliteEditor.Ssat1 = SatelliteEditor.SmallWheels[0];
                    SatelliteEditor.Ssat2 = SatelliteEditor.SmallWheels[1];
                    SatelliteEditor.Ssat3 = SatelliteEditor.SmallWheels[2];
                    SatelliteEditor.Ssat4 = SatelliteEditor.SmallWheels[3];
                    SatelliteEditor.Msat1 = SatelliteEditor.MicroWheels[0];
                    SatelliteEditor.Msat2 = SatelliteEditor.MicroWheels[1];
                    SatelliteEditor.Msat3 = SatelliteEditor.MicroWheels[2];
                    SatelliteEditor.Msat4 = SatelliteEditor.MicroWheels[3];
                    SatelliteEditor.MTM1 = SatelliteEditor.Magnetometers[0];
                    SatelliteEditor.MTM2 = SatelliteEditor.Magnetometers[1];
                    SatelliteEditor.MTM3 = SatelliteEditor.Magnetometers[2];
                    SatelliteEditor.ElMagnet1 = SatelliteEditor.ElMagnets[0];
                    SatelliteEditor.ElMagnet2 = SatelliteEditor.ElMagnets[1];
                    SatelliteEditor.ElMagnet3 = SatelliteEditor.ElMagnets[2];
                    SatelliteEditor.ARS1 = SatelliteEditor.ARS[0];
                    SatelliteEditor.ARS2 = SatelliteEditor.ARS[1];
                    SatelliteEditor.ARS3 = SatelliteEditor.ARS[2];
                    SatelliteEditor.Gyro1 = SatelliteEditor.Gyros[0];
                    SatelliteEditor.Gyro2 = SatelliteEditor.Gyros[1];
                    SatelliteEditor.Gyro3 = SatelliteEditor.Gyros[2];
                    SatelliteEditor.StarSensor1 = SatelliteEditor.StarSensors[0];
                    SatelliteEditor.StarSensor2 = SatelliteEditor.StarSensors[1];
                    SatelliteEditor.StarSensor3 = SatelliteEditor.StarSensors[2];
                    SatelliteEditor.StarSensor4 = SatelliteEditor.StarSensors[3];
                    SatelliteEditor.SunSensor1 = SatelliteEditor.SunSensors[0];
                    SatelliteEditor.SunSensor2 = SatelliteEditor.SunSensors[1];
                    SatelliteEditor.SunSensor3 = SatelliteEditor.SunSensors[2];
                    SatelliteEditor.SunSensor4 = SatelliteEditor.SunSensors[3];
                    SatelliteEditor.SunSensor5 = SatelliteEditor.SunSensors[4];
                    SatelliteEditor.SunSensor6 = SatelliteEditor.SunSensors[5];




                    TI = SatelliteEditor.TI;
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
               AddSatelliteToDataBase();
            }
            if (mode == "Edit")
            {
                EditSatellite();
            }
        }
        private void Add()
        {
            EditorVisibility = Visibility.Visible;
            mode = "Add";            
            SatelliteEditor.SatelliteType = "A";
            SatelliteEditor.N = 2;
            SatelliteEditor.M = 1;
            SatelliteEditor.Name = "Name";
            SatelliteEditor.SmallWheels = new FlywheelModel[4];
            TI = new Matrix3(0,0,0,0,0,0,0,0,0);
            

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
        private async void EditSatellite()
        {
            try
            {

                setComponents();               
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, routeSatellite);
                string content = JsonConvert.SerializeObject(SatelliteEditor);
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
        private async void AddSatelliteToDataBase()
        {
            try
            {
                setComponents();
                string content = JsonConvert.SerializeObject(SatelliteEditor);

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
        private void setComponents() {
            FlywheelModel[] SmallWheels = new FlywheelModel[4];
            SmallWheels[0] = SatelliteEditor.Ssat1;
            SmallWheels[1] = SatelliteEditor.Ssat2;
            SmallWheels[2] = SatelliteEditor.Ssat3;
            SmallWheels[3] = SatelliteEditor.Ssat4;
            SatelliteEditor.SmallWheels = SmallWheels;
            FlywheelModel[]  MicroWheels = new FlywheelModel[4];
            MicroWheels[0] = SatelliteEditor.Msat1;
            MicroWheels[1] = SatelliteEditor.Msat2;
            MicroWheels[2] = SatelliteEditor.Msat3;
            MicroWheels[3] = SatelliteEditor.Msat4;
            SatelliteEditor.MicroWheels = MicroWheels;
            MagnetometerModel[] Magnetometers= new MagnetometerModel[3];
            Magnetometers[0] = SatelliteEditor.MTM1;
            Magnetometers[1] = SatelliteEditor.MTM2;
            Magnetometers[2] = SatelliteEditor.MTM3;
            SatelliteEditor.Magnetometers = Magnetometers;
            ElMagnetModel[]   ElMagnets= new ElMagnetModel[3];
            ElMagnets[0] = SatelliteEditor.ElMagnet1;
            ElMagnets[1] = SatelliteEditor.ElMagnet2;
            ElMagnets[2] = SatelliteEditor.ElMagnet3;
            SatelliteEditor.ElMagnets = ElMagnets;
            ARSModel[] Arses = new ARSModel[3];
            Arses[0] = SatelliteEditor.ARS1;
            Arses[1] = SatelliteEditor.ARS2;
            Arses[2] = SatelliteEditor.ARS3;
            SatelliteEditor.ARS = Arses;
            GyroModel[] Gyros = new GyroModel[3];
            Gyros[0] = SatelliteEditor.Gyro1;
            Gyros[1] = SatelliteEditor.Gyro2;
            Gyros[2] = SatelliteEditor.Gyro3;
            SatelliteEditor.Gyros = Gyros;
            StarSensorModel[] StarSensors = new StarSensorModel[4];
            StarSensors[0] = SatelliteEditor.StarSensor1;
            StarSensors[1] = SatelliteEditor.StarSensor2;
            StarSensors[2] = SatelliteEditor.StarSensor3;
            StarSensors[3] = SatelliteEditor.StarSensor4;
            SatelliteEditor.StarSensors = StarSensors;
            SunSensorModel[] SunSensors = new SunSensorModel[6];
            SunSensors[0] = SatelliteEditor.SunSensor1;
            SunSensors[1] = SatelliteEditor.SunSensor2;
            SunSensors[2] = SatelliteEditor.SunSensor3;
            SunSensors[3] = SatelliteEditor.SunSensor4;
            SunSensors[4] = SatelliteEditor.SunSensor5;
            SunSensors[5] = SatelliteEditor.SunSensor6;
            SatelliteEditor.SunSensors = SunSensors;


        }
    }

}

