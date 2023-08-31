
using RacursCore.SatilliteComponents;
using System;
using System.Windows;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text.Json;
using RacursCore;
using RacursCore.SatelliteModel;
using RacursConfig.Pages.SatellitePage.PositionEditors;


namespace RacursConfig.Pages.SatellitePage
{
    public class SatelliteComponentEditorVM : BaseVM
    {

        private JsonSerializerOptions options;

        public Satellite model { get; set; }


       

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
        public RelayCommand DeleteSmallSatCommand
        {
            get; set;
        }
        public RelayCommand DeleteMicroSatCommand
        {
            get; set;
        }
        public RelayCommand DeleteMagnetometerCommand
        {
            get; set;
        }
        public RelayCommand DeleteElMagnetCommand
        {
            get; set;
        }
        public RelayCommand DeleteARSCommand
        {
            get; set;
        }
        public RelayCommand DeleteGyroCommand
        {
            get; set;
        }
        public RelayCommand DeleteStarSensorCommand
        {
            get; set;
        }
        public RelayCommand DeleteSunSensorCommand
        {
            get; set;
        }

        public RelayCommand AddComponentCommand
        {
            get; set;
        }
        //public RelayCommand OpenSattelliteModelEdtorCommand
        //{
        //    get; set;
        //}
        public RelayCommand EditSmallSatPositionCommand
        {
            get; set;
        }
        public RelayCommand EditMagnetometerPositionCommand
        {
            get; set;
        }
        public RelayCommand EditElMagnetPositionCommand
        {
            get; set;
        }
        public RelayCommand EditARSPositionCommand
        {
            get; set;
        }
        public RelayCommand EditGyroPositionCommand
        {
            get; set;
        }
        public RelayCommand EditStarSensorPositionCommand
        {
            get; set;
        }
        public RelayCommand EditSunSensorPositionCommand
        {
            get; set;
        }
        public RelayCommand EditEngineCommand
        {
            get; set;
        }
        public RelayCommand EditLoadCommand
        {
            get; set;
        }
        public RelayCommand OKCommand
        {
            get; set;
        }
       
        public SatelliteComponentEditorVM(Satellite satellite)
        {
     
            SatelliteEditor = satellite;
            DeleteSmallSatCommand = new RelayCommand(x => DeleteSmallSat(x));
            DeleteMicroSatCommand = new RelayCommand(x => DeleteMicroSat(x));
            DeleteMagnetometerCommand = new RelayCommand(x => DeleteMagnetometer(x));
            DeleteElMagnetCommand = new RelayCommand(x => DeleteElectroMagnet(x));
            DeleteARSCommand = new RelayCommand(x => DeleteArs(x));
            DeleteGyroCommand = new RelayCommand(x => DeleteGyro(x));
            DeleteSunSensorCommand = new RelayCommand(x => DeleteSunSensor(x));
            DeleteStarSensorCommand = new RelayCommand(x => DeleteStarSensor(x));
            AddComponentCommand = new RelayCommand(x => AddComponent(x));
            EditSmallSatPositionCommand = new RelayCommand(x => EditSmallSatPosition(x));
            EditMagnetometerPositionCommand = new RelayCommand(x => EditMagnetometerPosition(x));
            EditElMagnetPositionCommand = new RelayCommand(x => EditElMagnetPosition(x));
            EditARSPositionCommand = new RelayCommand(x => EditARSPosition(x));
            EditGyroPositionCommand = new RelayCommand(x => EditGyroPosition(x));
            EditStarSensorPositionCommand = new RelayCommand(x => EditStarSensorPosition(x));
            EditSunSensorPositionCommand = new RelayCommand(x => EditSunSensorPosition(x));
            EditEngineCommand = new RelayCommand(x=>EditEngine());
            EditLoadCommand = new RelayCommand(x => EditLoad());
            OKCommand = new RelayCommand(x => SaveChanges(x));
            CancelCommand = new RelayCommand(x => Exit(x));


            
          
            //SatelliteEditor.Ssat1 = null;
        }

        private void Exit(object x)
        {
            Window window = x as Window; if (window != null)
            {
                SatelliteEditor = model;
                var t = 0;
                window.Close();
            }         
           
        }

        private void SaveChanges(object x)
        {
            Window window = x as Window; if (window != null)
            {
                window.DialogResult = true;
            }
        }

        private void EditSunSensorPosition(object x)
        {
            SunSensorModel model = (SunSensorModel)(x);
            SunSensorPositionEditor editorWindow = new SunSensorPositionEditor(model.Att);
            if (editorWindow.ShowDialog() == true)
            {
                model.Att = editorWindow.Att;
            }
        }

        private void EditStarSensorPosition(object x)
        {
            StarSensorModel model = (StarSensorModel)(x);
           StarSensorPositionEditor editorWindow = new StarSensorPositionEditor(model.Att);
            if (editorWindow.ShowDialog() == true)
            {
                model.Att = editorWindow.Att;
            }
        }

        private void DeleteStarSensor(object x)
        {
            StarSensorModel model = (StarSensorModel)(x);
            int slot = model.Slot;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.StarSensor1 = null;
                    break;
                case 2:
                    SatelliteEditor.StarSensor2 = null;
                    break;
                case 3:
                    SatelliteEditor.StarSensor3 = null;
                    break;
                case 4:
                    SatelliteEditor.StarSensor4 = null;
                    break;
            }
        }

        private void DeleteSunSensor(object x)
        {
            SunSensorModel model = (SunSensorModel)(x);
            int slot = model.Slot;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.SunSensor1 = null;
                    break;
                case 2:
                    SatelliteEditor.SunSensor2 = null;
                    break;
                case 3:
                    SatelliteEditor.SunSensor3 = null;
                    break;
                case 4:
                    SatelliteEditor.SunSensor4 = null;
                    break;
                case 5:
                    SatelliteEditor.SunSensor4 = null;
                    break;
                case 6:
                    SatelliteEditor.SunSensor4 = null;
                    break;
            }
        }

        private void EditLoad()
        {
            LoadEditor editorWindow = new LoadEditor(SatelliteEditor.LoadM1, SatelliteEditor.LoadM2);
            if (editorWindow.ShowDialog() == true)
            {
                SatelliteEditor.LoadM1 = editorWindow.LoadM1;
                SatelliteEditor.LoadM2 = editorWindow.LoadM2;
            }
        }

        private void EditEngine()
        {
            EngineEditor editorWindow = new EngineEditor(SatelliteEditor.EnginePulse, SatelliteEditor.EngineTime);
            if (editorWindow.ShowDialog() == true)
            {
                SatelliteEditor.EnginePulse = editorWindow.Pulse;
                SatelliteEditor.EngineTime = editorWindow.Time;
            }
        }

        private void EditGyroPosition(object x)
        {
            GyroModel model = (GyroModel)(x);
            GyroPositionEditor editorWindow = new GyroPositionEditor(model.Axis_g,model.Axis_r,model.Theta, model.Lambda);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis_g = editorWindow.AxisG;
                model.Axis_r = editorWindow.AxisR;
                model.Theta = editorWindow.Theta;
                model.Lambda = editorWindow.Lambda;
            }
        }

        private void EditARSPosition(object x)
        {
            ARSModel model = (ARSModel)(x);
             ARSPositionEditor editorWindow = new ARSPositionEditor(model.Axis);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis = editorWindow.Axis;                
            }
        }

        private void EditMagnetometerPosition(object x)
        {
            MagnetometerModel model = (MagnetometerModel)(x);
            MagnetometerPositionEditor editorWindow = new MagnetometerPositionEditor(model.Att);
            if (editorWindow.ShowDialog() == true)
            {
                model.Att = editorWindow.Att;
            }
        }

        private void EditSmallSatPosition(object x)
        {
            FlywheelModel model = (FlywheelModel)(x);
            FlywheelPositionEditor editorWindow = new FlywheelPositionEditor(model.Axis,model.Eta,model.Gamma);           
            if (editorWindow.ShowDialog() == true)
            {
               model.Axis = editorWindow.Axis;
               model.Eta = editorWindow.Eta;
               model.Gamma = editorWindow.Gamma;
            }
        }
        private void EditElMagnetPosition(object x)
        {
            ElMagnetModel model = (ElMagnetModel)(x);
            ElMagnetPositionEditor editorWindow = new ElMagnetPositionEditor(model.Axis);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis = editorWindow.Axis;
            }
        }

        private void AddComponent(object x)
        {   
            int slot = Convert.ToInt32(x.ToString().Split('-')[1]);
            string componentName = x.ToString().Split('-')[0];
            DialogWindow dialogWindow = new DialogWindow(componentName);
            if (dialogWindow.ShowDialog() == true)
            {
                SatelliteComponent component = dialogWindow.SelectedConponent;
                if (component != null)
                {

                    switch (componentName)
                    {
                        case "Gyro":
                            addGyro(component,slot);
                            break;
                        case "ElMagnet":
                            addElectroMagnet(component, slot);
                            break;
                        case "Magnetometer":
                            addMagnetometer(component, slot);
                            break;
                        case "Ssat":
                            addSmallSat(component, slot);
                            break;
                        case "Msat":
                            addMicroSat(component, slot);
                            break;
                        case "ARS":
                            addArs(component, slot);
                            break;
                        case "SunSensor":
                            addSunSensor(component, slot);
                            break;
                        case "StarSensor":
                            addStarSensor(component, slot);
                            break;
                    }

                }
            }
        }
        private void addStarSensor(SatelliteComponent component, int slot)
        {
            StarSensorModel model = new StarSensorModel();
            model.StarSensorId = (component as StarSensor).Id;
            model.Slot = slot;
            model.Name = (component as StarSensor).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.StarSensor1 = model;
                    break;
                case 2:
                    SatelliteEditor.StarSensor2 = model;
                    break;
                case 3:
                    SatelliteEditor.StarSensor3 = model;
                    break;
                case 4:
                    SatelliteEditor.StarSensor4 = model;
                    break;
            }
        }

        private void addSunSensor(SatelliteComponent component, int slot)
        {
            SunSensorModel model = new SunSensorModel();
            model.SunSensorId = (component as SunSensor).Id;
            model.Slot = slot;
            model.Name = (component as SunSensor).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.SunSensor1 = model;
                    break;
                case 2:
                    SatelliteEditor.SunSensor2 = model;
                    break;
                case 3:
                    SatelliteEditor.SunSensor3 = model;
                    break;
                case 4:
                    SatelliteEditor.SunSensor4 = model;
                    break;
                case 5:
                    SatelliteEditor.SunSensor5 = model;
                    break;
                case 6:
                    SatelliteEditor.SunSensor6 = model;
                    break;
            }
        }

        private void addArs(SatelliteComponent component, int slot)
        {
            ARSModel model = new ARSModel();
            model.ArsId = (component as ARS).Id;
            model.Slot = slot;
            model.Name = (component as ARS).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.ARS1 = model;
                    break;
                case 2:
                    SatelliteEditor.ARS2 = model;
                    break;
                case 3:
                    SatelliteEditor.ARS3 = model;
                    break;
            }
        }

        private void addMicroSat(SatelliteComponent component, int slot)
        {
            FlywheelModel model = new FlywheelModel();
            model.FlywheelId = (component as Flywheel).Id;
            model.Slot = slot;
            model.Name = (component as Flywheel).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.Msat1 = model;
                    break;
                case 2:
                    SatelliteEditor.Msat2 = model;
                    break;
                case 3:
                    SatelliteEditor.Msat3 = model;
                    break;
                case 4:
                    SatelliteEditor.Msat4 = model;
                    break;
            }
        }

        private void addSmallSat(SatelliteComponent component, int slot)
        {
            FlywheelModel model = new FlywheelModel();
            model.FlywheelId = (component as Flywheel).Id;
            model.Slot = slot;
            model.Name = (component as Flywheel).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.Ssat1 = model;
                    break;
                case 2:
                    SatelliteEditor.Ssat2 = model;
                    break;
                case 3:
                    SatelliteEditor.Ssat3 = model;
                    break;
                case 4:
                    SatelliteEditor.Ssat4 = model;
                    break;
            }
        }

        private void addMagnetometer(SatelliteComponent component, int slot)
        {
            MagnetometerModel model = new MagnetometerModel();
            model.MagnetometerId = (component as Magnetometer).Id;
            model.Slot = slot;
            model.Name = (component as Magnetometer).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.MTM1 = model;
                    break;
                case 2:
                    SatelliteEditor.MTM2 = model;
                    break;
                case 3:
                    SatelliteEditor.MTM3 = model;
                    break;
               
            }
        }

        private void addElectroMagnet(SatelliteComponent component, int slot)
        {
            ElMagnetModel model = new ElMagnetModel();
            model.ElectromagnetId = (component as ElMagnet).Id;
            model.Slot = slot;
            model.Name = (component as ElMagnet).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.ElMagnet1 = model;
                    break;
                case 2:
                    SatelliteEditor.ElMagnet2 = model;
                    break;
                case 3:
                    SatelliteEditor.ElMagnet3 = model;
                    break;

            }
        }

        private void addGyro(SatelliteComponent component, int slot)
        {
            GyroModel model = new GyroModel();
            model.GyroId = (component as Gyro).Id;
            model.Slot = slot;
            model.Name = (component as Gyro).Name;
            model.IsEnable = true;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.Gyro1 = model;
                    break;
                case 2:
                    SatelliteEditor.Gyro2 = model;
                    break;
                case 3:
                    SatelliteEditor.Gyro3 = model;
                    break;

            }
        }

        private void DeleteSmallSat(object x)
        {   
            FlywheelModel model = (FlywheelModel)(x);
            int slot = model.Slot;
            switch (slot) {
                    case 1:
                    SatelliteEditor.Ssat1 = null;
                    break;
                    case 2:
                    SatelliteEditor.Ssat2 = null;
                    break;
                    case 3:
                    SatelliteEditor.Ssat3 = null;
                    break;
                    case 4:
                    SatelliteEditor.Ssat4 = null;
                    break;
            }
         
          //  SatelliteEditor = JsonSerializer.Deserialize<SatelliteModel>(JsonConvert.SerializeObject(SatelliteEditor), options);

        }

        private void DeleteMicroSat(object x)
        {
            FlywheelModel model = (FlywheelModel)(x);
            int slot = model.Slot;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.Msat1 = null;
                    break;
                case 2:
                    SatelliteEditor.Msat2 = null;
                    break;
                case 3:
                    SatelliteEditor.Msat3 = null;
                    break;
                case 4:
                    SatelliteEditor.Msat4 = null;
                    break;
            }

            //  SatelliteEditor = JsonSerializer.Deserialize<SatelliteModel>(JsonConvert.SerializeObject(SatelliteEditor), options);

        }

        private void DeleteMagnetometer(object x)
        {
            MagnetometerModel model = (MagnetometerModel)(x);
            int slot = model.Slot;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.MTM1 = null;
                    break;
                case 2:
                    SatelliteEditor.MTM2 = null;
                    break;
                case 3:
                    SatelliteEditor.MTM3 = null;
                    break;                
            }

        }
        private void DeleteElectroMagnet(object x)
        {
            ElMagnetModel model = (ElMagnetModel)(x);
            int slot = model.Slot;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.ElMagnet1 = null;
                    break;
                case 2:
                    SatelliteEditor.ElMagnet2 = null;
                    break;
                case 3:
                    SatelliteEditor.ElMagnet3 = null;
                    break;
            }
        }
        private void DeleteArs(object x)
        {
            ARSModel model = (ARSModel)(x);
            int slot = model.Slot;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.ARS1 = null;
                    break;
                case 2:
                    SatelliteEditor.ARS2 = null;
                    break;
                case 3:
                    SatelliteEditor.ARS3 = null;
                    break;
            }
        }
        private void DeleteGyro(object x)
        {
            GyroModel model = (GyroModel)(x);
            int slot = model.Slot;
            switch (slot)
            {
                case 1:
                    SatelliteEditor.Gyro1 = null;
                    break;
                case 2:
                    SatelliteEditor.Gyro2 = null;
                    break;
                case 3:
                    SatelliteEditor.Gyro3 = null;
                    break;
            }
        }
     

    }

}

