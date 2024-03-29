﻿
using System;
using System.Collections.Generic;
using System.Text;
using RacursConfig;
using RacursCore;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows;
using RacursLib;
using System.Windows.Controls;

namespace Client.PagesVM
{   
    //VM для страницы добавления Станций
    public class StationsPageVM : BaseVM
    {   
        private HttpClient httpClient;
        private string mode;
        private string route = "/api/stations";
        private List<Station> _Stations;
        public List<Station> Stations
        {
            get
            {
                return _Stations;
            }
            set
            {
                _Stations = value;
                OnPropertyChanged(nameof(Stations));
            }
        }
        private List<BaseClass> _Values;
        public List<BaseClass> Values
        {
            get
            {
                return _Values;
            }
            set
            {
                _Values = value;
                OnPropertyChanged(nameof(Values));
            }
        }
        private Station _SelectedStation;
        public Station SelectedStation
        {
            get
            {
                return _SelectedStation;
            }
            set
            {
                _SelectedStation = value;
                OnPropertyChanged(nameof(SelectedStation));
            }
        }

        private Station _StationEditor;
        public Station StationEditor
        {
            get
            {
                return _StationEditor;
            }
            set
            {
                _StationEditor = value;
                OnPropertyChanged(nameof(StationEditor));
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

        private RacursLib.LibMath.Matrix3 m;
        public RacursLib.LibMath.Matrix3 M
        {
            get { return m; }
            set
            {
                m = value;
                OnPropertyChanged("M");
            }
        }

        private double testText;
        public double TestText
        {
            get { return testText; }
            set
            {
                testText = value;
                OnPropertyChanged("TestText");
            }
        }


        private List<string> _WarningMessages;
        public List<string> WarningMessages
        {
            get
            {
                return _WarningMessages;
            }
            set
            {
                _WarningMessages = value;
                OnPropertyChanged(nameof(WarningMessages));
            }
        }

        //public RelayCommand AddCommand
        //{
        //    get;
        //}
        //public RelayCommand CancelCommand
        //{
        //    get;
        //}
        //public RelayCommand SaveCommand
        //{
        //    get;
        //}
        //public RelayCommand DeleteCommand
        //{
        //    get;
        //}
        //public RelayCommand EditCommand
        //{
        //    get;
        //}

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Angle":
                        if ((StationEditor.Angle < 0) || (StationEditor.Angle > 100))
                        {
                            error = "Возраст должен быть больше 0 и меньше 100";
                        }
                        break;
                    case "Name":
                        //Обработка ошибок для свойства Name
                        break;
                    case "Position":
                        //Обработка ошибок для свойства Position
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        public StationsPageVM()
        {

            //54.436997 ? 27.828610306 ?? 237,287
            //RacursLib.LibMath.Vector vector = new RacursLib.LibMath.Vector(54.436997*Math.PI/180,27.8286103 * Math.PI / 180, 237.287);
            //RacursLib.LibMath.Vector gsk = CoordTransform.Geo_To_Gsk(vector);
            
            EditorVisibility = Visibility.Hidden;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(App.baseUrl);
            StationEditor = new Station { Name = "Новая Станция", Description = "Описание", Angle = 20};
            //StationEditor.XGsk = gsk.X;
            //StationEditor.YGsk = gsk.Y;
            //StationEditor.ZGsk = gsk.Z;
            ////{( 3409,833096517315         | 1799,977612730626         | 5358,21089490913          )
            //StationEditor.Name = "5001";
            //StationEditor.Description = "КИП Плещеницы";
            //AddStationToDataBase(StationEditor);
            getStations();
            SelectedStation = new Station();
            WarningMessages = new List<string>(); 
            AddCommand = new RelayCommand(x => Add());
            CancelCommand = new RelayCommand(x => Cancel());
            DeleteCommand = new RelayCommand(x => DeleteStation(x));
            SaveCommand = new RelayCommand(x => Save());
            EditCommand = new RelayCommand(x => Edit());
            Satellite satellite = new Satellite();
            M = new RacursLib.LibMath.Matrix3(1, 1, 1, 1, 1, 1, 1, 1, 1);
            M.N22 = 500;
            //TestText = "123";
            //List<Itnp> Itnsp = new List<Itnp>();
            //Itnp itnp1 = new Itnp {Date = DateTime.Now, SatelliteId=1,X=10,Y=20,Z=20,RadialSpeed=30,Range=1000 };
            //Itnp itnp2 = new Itnp { Date = DateTime.Now.AddSeconds(1), SatelliteId = 1, X = 20, Y = 20, Z = 220, RadialSpeed = 34, Range = 5 };
            //Itnsp.Add(itnp1);
            //Itnsp.Add(itnp2);
            // HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/api/itnps");
            // getItnp();


        }

        private void Edit()
        {
            StationEditor = JsonConvert.DeserializeObject<Station>(JsonConvert.SerializeObject(SelectedStation));
            EditorVisibility = Visibility.Visible;
            mode = "Edit";
        }

        private void Cancel()
        {
            EditorVisibility = Visibility.Hidden;
        }

       
        private void Save()
        {
            if (mode == "Add") {
                AddStationToDataBase(StationEditor);
            }
            if (mode == "Edit") {
                EditStation(StationEditor);
            }
        }

        private void Add()
        {
            //TestText = "qwe";
            var t = M;
            t.N22 = 500;
            EditorVisibility = Visibility.Visible;
            mode = "Add";
            StationEditor = new Station { Name = "Новая Станция", Description = "Описание" };
            
        }

        private async void DeleteStation(object station)
        {
            EditorVisibility = Visibility.Hidden;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, route);
                string content = JsonConvert.SerializeObject(station);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    WarningMessages.Add(GetTimeLabel() + "Станция Удалена из БД");
                    getStations();
                    
                }
                else { WarningMessages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception){
                WarningMessages.Add( GetTimeLabel() +  exception.Message);
            }
        }
        private async void EditStation(Station station)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put,route);
                RacursLib.LibMath.Vector geo = CoordTransform.Greenvich_To_Geo(new RacursLib.LibMath.Vector(station.XGsk, station.YGsk, station.ZGsk));
                station.Lattitude = Math.Round(geo.X * 180 / Math.PI, 4);
                station.Longitude = Math.Round(geo.Y * 180 / Math.PI, 4);
                station.Altitude = Math.Round(geo.Z * 1e-3,4);
                string content = JsonConvert.SerializeObject(station);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    WarningMessages.Add(GetTimeLabel() + "Изменения успешно сохранены в БД");
                    getStations();
                    
                }
                else { WarningMessages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                WarningMessages.Add(GetTimeLabel() + exception.Message);
            }
        }

        //private  async void getItnp() {
        //    try
        //    {   
        //        string paramss = "?dateTimeFrom=" + DateTime.Now.AddDays(-10).ToString("yyyy-MM-ddTHH:mm:ss") + "&" +
        //                          "dateTimeTo=" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "&satelliteId=1";



        //        var response = await httpClient.GetAsync("/api/Itnps" + paramss);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = response.Content.ReadAsStringAsync();
        //            var t  = JsonConvert.DeserializeObject<List<Itnp>>(result.Result);
        //        }
        //        else { WarningMessages.Add(response.ReasonPhrase); }
        //    }
        //    catch (Exception exception)
        //    {
        //        WarningMessages.Add(GetTimeLabel() + exception.Message);
        //    }
        //}
        private async void getStations()
        {
            try
            {
                var response = await httpClient.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    WarningMessages.Add(GetTimeLabel() + "Запрос списка станций");
                    Stations = JsonConvert.DeserializeObject<List<Station>>(result.Result);                   
                }
                else { WarningMessages.Add(response.ReasonPhrase); }
            }
            catch (Exception exception) {
                WarningMessages.Add(GetTimeLabel() +  exception.Message);
            }
        }
        private async void AddStationToDataBase(Station station)
        {
            try
            {
                RacursLib.LibMath.Vector geo = CoordTransform.Greenvich_To_Geo(new RacursLib.LibMath.Vector(station.XGsk, station.YGsk, station.ZGsk));
                station.Lattitude = Math.Round(geo.X * 180 / Math.PI, 4);
                station.Longitude = Math.Round(geo.Y * 180 / Math.PI, 4);
                station.Altitude = geo.Z * 1e-3;
                string content = JsonConvert.SerializeObject(station);
                //httpClient.DefaultRequestHeaders = Hea
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/stations");
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    EditorVisibility = Visibility.Hidden;
                    WarningMessages.Add(GetTimeLabel() + "Станция сохранена в БД");
                    getStations();
                   
                }
                else { WarningMessages.Add(GetTimeLabel() + response.ReasonPhrase); }
            }
            catch (Exception exception)
            {
                WarningMessages.Add(GetTimeLabel() + exception.Message);
            }
        }
        private string GetTimeLabel() {
            return "[" + DateTime.Now.ToString() + "] ";
        }
    }
    
}