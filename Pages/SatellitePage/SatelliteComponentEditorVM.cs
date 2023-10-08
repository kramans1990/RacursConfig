
using RacursCore.SatilliteComponents;
using System;
using System.Windows;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text.Json;
using RacursCore;
using RacursCore.SatelliteModel;
using RacursConfig.Pages.SatellitePage.PositionEditors;
using System.Linq;

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
        #region Commands     
        public RelayCommand AddComponentCommand { get; set; }
        public RelayCommand DeleteComponentCommand { get; set; }
        public RelayCommand EditSmallSatPositionCommand { get; set; }
        public RelayCommand EditMicroSatPositionCommand {get;set;}
        public RelayCommand EditMagnetometerPositionCommand {get; set;}
        public RelayCommand EditElMagnetPositionCommand {get; set;}
        public RelayCommand EditARSPositionCommand {get; set;}
        public RelayCommand EditGyroPositionCommand {get; set;}
        public RelayCommand EditStarSensorPositionCommand {get; set;}
        public RelayCommand EditSunSensorPositionCommand {get; set;}
        public RelayCommand EditEngineCommand {get; set;}
        public RelayCommand EditLoadCommand {get; set;}
        public RelayCommand EnableDeviceCommand { get; set; }
        public RelayCommand OKCommand {get; set;}
        #endregion
        public SatelliteComponentEditorVM(Satellite satellite)
        {     
            SatelliteEditor = satellite;
            AddComponentCommand = new RelayCommand(x => AddComponent(x));
            DeleteComponentCommand = new RelayCommand(x => DeleteComponent(x));
            EditSmallSatPositionCommand = new RelayCommand(x => EditSmallSatPosition(x));
            EditMicroSatPositionCommand = new RelayCommand(x => EditMiroSatPosition(x));
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
            EnableDeviceCommand = new RelayCommand(x=> EnableDevice(x));
        }      

        private void DeleteComponent(object x)
        {
            string componentName = x.ToString().Split('-')[0];
            int slot = Convert.ToInt32(x.ToString().Split('-')[1]);
            IDeviceInstalationModel model;
            switch (componentName)
            {
                case "Gyro":
                    {
                        var devices = SatelliteEditor.Gyros;
                        devices[slot - 1] = null;
                        SatelliteEditor.Gyros = devices;
                    }
                    break;
                case "ElMagnet":                  
                    {
                        var devices = SatelliteEditor.ElMagnets;
                        devices[slot - 1] =null;
                        SatelliteEditor.ElMagnets = devices;
                    }
                    break;
                case "Magnetometer":
                     {
                        var devices = SatelliteEditor.Magnetometers;
                        devices[slot - 1] = null;
                        SatelliteEditor.Magnetometers = devices;
                    }
                    break;
                case "Ssat":
                  
                    {
                        var devices = SatelliteEditor.SmallWheels;
                        devices[slot - 1] = null;
                        SatelliteEditor.SmallWheels = devices;
                    }
                    break;
                case "Msat": 
                    {
                        var devices = SatelliteEditor.MicroWheels;
                        devices[slot - 1] = null;
                        SatelliteEditor.MicroWheels = devices;
                    }

                    break;
                     case "ARS":                   
                    {
                        var devices = SatelliteEditor.ARS;
                        devices[slot - 1] = null;
                        SatelliteEditor.ARS = devices;
                    }
                    break;
                case "SunSensor":                   
                    {
                        var devices = SatelliteEditor.SunSensors;
                        devices[slot - 1] = null;
                        SatelliteEditor.SunSensors = devices;
                    }
                    break;
                case "StarSensor":                   
                    {
                        var devices = SatelliteEditor.StarSensors;
                        devices[slot - 1] =null;
                        SatelliteEditor.StarSensors = devices;
                    }
                    break;
            }
        }

        private void EnableDevice(object x)
        {
            string componentName = x.ToString().Split('-')[0];
            int slot = Convert.ToInt32(x.ToString().Split('-')[1]);
            IDeviceInstalationModel model;
            switch (componentName)
                    {
                        case "Gyro":
                    model = SatelliteEditor.Gyros.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.Gyros;
                        devices[slot - 1] = model as GyroInstallationModel;
                        SatelliteEditor.Gyros = devices;
                    }
                    break;
                         case "ElMagnet":
                    model = SatelliteEditor.ElMagnets.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.ElMagnets;
                        devices[slot - 1] = model as ElMagnetInstallationModel;
                        SatelliteEditor.ElMagnets = devices;
                    }
                    break;
                        case "Magnetometer":
                    model = SatelliteEditor.Magnetometers.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.Magnetometers;
                        devices[slot - 1] = model as MagnetometerInstallationModel;
                        SatelliteEditor.Magnetometers = devices;
                    }
                    break;
                        case "Ssat":
                    model = SatelliteEditor.SmallWheels.Where(p=> p!=null).FirstOrDefault(p=>p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.SmallWheels;
                        devices[slot - 1] = model as FlywheelInstallationModel;
                        SatelliteEditor.SmallWheels = devices;
                    }
                    break;
                        case "Msat":
                    model = SatelliteEditor.MicroWheels.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.MicroWheels;
                        devices[slot - 1] = model as FlywheelInstallationModel;
                        SatelliteEditor.MicroWheels = devices;
                    }
                    
                    break;
                        case "ARS":
                    model = SatelliteEditor.ARS.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.ARS;
                        devices[slot - 1] = model as ARSInstallationModel;
                        SatelliteEditor.ARS = devices;
                    }
                    break;
                        case "SunSensor":
                    model = SatelliteEditor.SunSensors.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.SunSensors;
                        devices[slot - 1] = model as SunSensorInstallationModel;
                        SatelliteEditor.SunSensors = devices;
                    }
                    break;
                        case "StarSensor":
                    model = SatelliteEditor.StarSensors.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
                    model.IsEnable = !model.IsEnable;
                    {
                        var devices = SatelliteEditor.StarSensors;
                        devices[slot - 1] = model as StarSensorInstallationModel;
                        SatelliteEditor.StarSensors = devices;
                    }
                    break;
                    }

                
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
        #region EditDevicesMethods
        private void EditSunSensorPosition(object x)
        {
            int slot = Convert.ToInt32(x);
            SunSensorInstallationModel model = SatelliteEditor.SunSensors.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            SunSensorPositionEditor editorWindow = new SunSensorPositionEditor(model.Att);
            if (editorWindow.ShowDialog() == true)
            {

                model.Att = editorWindow.Att;
                var devices = SatelliteEditor.SunSensors;
                devices[slot - 1] = model;
                SatelliteEditor.SunSensors = devices;
            }
        }
        private void EditStarSensorPosition(object x)
        {
            int slot = Convert.ToInt32(x);
            StarSensorInstallationModel model = SatelliteEditor.StarSensors.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            StarSensorPositionEditor editorWindow = new StarSensorPositionEditor(model.Att);
            if (editorWindow.ShowDialog() == true)
            {

                model.Att = editorWindow.Att;
                var devices = SatelliteEditor.StarSensors;
                devices[slot - 1] = model;
                SatelliteEditor.StarSensors = devices;
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
            EngineEditor editorWindow = new EngineEditor(SatelliteEditor.EnginePulse, SatelliteEditor.EngineTime,SatelliteEditor.EnginePull,SatelliteEditor.EnginePullUnit);
            if (editorWindow.ShowDialog() == true)
            {
                SatelliteEditor.EnginePulse = editorWindow.Pulse;
                SatelliteEditor.EngineTime = editorWindow.Time;
                SatelliteEditor.EnginePull = editorWindow.Pull;
                SatelliteEditor.EnginePullUnit = editorWindow.PullUnit;
            }
        }
        private void EditGyroPosition(object x)
        {
            int slot = Convert.ToInt32(x);
            GyroInstallationModel model = SatelliteEditor.Gyros.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            GyroPositionEditor editorWindow = new GyroPositionEditor(model.Axis_g, model.Axis_r, model.Theta, model.Lambda);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis_g = editorWindow.AxisG;
                model.Axis_r = editorWindow.AxisR;
                model.Theta = editorWindow.Theta;
                model.Lambda = editorWindow.Lambda;
                var devices = SatelliteEditor.Gyros;
                devices[slot - 1] = model;
                SatelliteEditor.Gyros = devices;
            }
        }
        private void EditARSPosition(object x)
        {


            int slot = Convert.ToInt32(x);
            ARSInstallationModel model = SatelliteEditor.ARS.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            ARSPositionEditor editorWindow = new ARSPositionEditor(model.Axis);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis = editorWindow.Axis;
                var devices = SatelliteEditor.ARS;
                devices[slot - 1] = model;
                SatelliteEditor.ARS = devices;
            }
        }
        private void EditMagnetometerPosition(object x)
        {
            int slot = Convert.ToInt32(x);
            MagnetometerInstallationModel model = SatelliteEditor.Magnetometers.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            MagnetometerPositionEditor editorWindow = new MagnetometerPositionEditor(model.Att);
            if (editorWindow.ShowDialog() == true)
            {
                model.Att = editorWindow.Att;
                var devices = SatelliteEditor.Magnetometers;
                devices[slot - 1] = model;
                SatelliteEditor.Magnetometers = devices;
            }
        }
        private void EditSmallSatPosition(object x)
        {
            int slot = Convert.ToInt32(x);
            FlywheelInstallationModel model = SatelliteEditor.SmallWheels.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            FlywheelPositionEditor editorWindow = new FlywheelPositionEditor(model.Axis, model.Eta, model.Gamma);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis = editorWindow.Axis;
                model.Eta = editorWindow.Eta;
                model.Gamma = editorWindow.Gamma;
                var devices = SatelliteEditor.SmallWheels;
                devices[slot - 1] = model;
                SatelliteEditor.SmallWheels = devices;
            }
        }
        private void EditMiroSatPosition(object x)
        {
            int slot = Convert.ToInt32(x);
            FlywheelInstallationModel model = SatelliteEditor.MicroWheels.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            FlywheelPositionEditor editorWindow = new FlywheelPositionEditor(model.Axis, model.Eta, model.Gamma);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis = editorWindow.Axis;
                model.Eta = editorWindow.Eta;
                model.Gamma = editorWindow.Gamma;
                var devices = SatelliteEditor.MicroWheels;
                devices[slot - 1] = model;
                SatelliteEditor.MicroWheels = devices;
            }
        }
        private void EditElMagnetPosition(object x)
        {
            int slot = Convert.ToInt32(x);
            ElMagnetInstallationModel model = SatelliteEditor.ElMagnets.Where(p => p != null).FirstOrDefault(p => p.Slot == slot);
            ElMagnetPositionEditor editorWindow = new ElMagnetPositionEditor(model.Axis);
            if (editorWindow.ShowDialog() == true)
            {
                model.Axis = editorWindow.Axis;
                var devices = SatelliteEditor.ElMagnets;
                devices[slot - 1] = model;
                SatelliteEditor.ElMagnets = devices;
            }
        }
        #endregion


        #region add-device-methods
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
                            addGyro(component, slot);
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
            StarSensorInstallationModel model = new StarSensorInstallationModel();
            model.StarSensorId = (component as StarSensor).Id;
            model.Slot = slot;
            model.Name = (component as StarSensor).Name;
            model.IsEnable = true;
            var devices = SatelliteEditor.StarSensors;
            devices[slot - 1] = model;
            SatelliteEditor.StarSensors = devices;
        }
        private void addSunSensor(SatelliteComponent component, int slot)
        {
            SunSensorInstallationModel model = new SunSensorInstallationModel();
            model.SunSensorId = (component as SunSensor).Id;
            model.Slot = slot;
            model.Name = (component as SunSensor).Name;
            model.IsEnable = true;
            var devices = SatelliteEditor.SunSensors;
            devices[slot - 1] = model;
            SatelliteEditor.SunSensors = devices;
        }
        private void addArs(SatelliteComponent component, int slot)
        {
            ARSInstallationModel model = new ARSInstallationModel();
            model.ArsId = (component as ARS).Id;
            model.Slot = slot;
            model.Name = (component as ARS).Name;
            model.IsEnable = true;
            var devices = SatelliteEditor.ARS;
            devices[slot - 1] = model;
            SatelliteEditor.ARS = devices;            
        }
        private void addMicroSat(SatelliteComponent component, int slot)
        {
            FlywheelInstallationModel model = new FlywheelInstallationModel();
            model.FlywheelId = (component as Flywheel).Id;
            model.Slot = slot;
            model.Name = (component as Flywheel).Name;
            model.IsEnable = true;
            var devices = SatelliteEditor.MicroWheels;
            devices[slot - 1] = model;
            SatelliteEditor.MicroWheels = devices;           
        }
        private void addSmallSat(SatelliteComponent component, int slot)
        {   
            FlywheelInstallationModel model = new FlywheelInstallationModel();
            model.IsEnable = true;
            model.Axis = new RacursCore.types.Vector();           
            model.FlywheelId = (component as Flywheel).Id;
            model.Slot = slot;
            model.Name = (component as Flywheel).Name;
            
            var devices = SatelliteEditor.SmallWheels;
            devices[slot-1]=model;           
            SatelliteEditor.SmallWheels = devices;  
        }
        private void addMagnetometer(SatelliteComponent component, int slot)
        {
            MagnetometerInstallationModel model = new MagnetometerInstallationModel();
            model.MagnetometerId = (component as Magnetometer).Id;
            model.Slot = slot;
            model.Name = (component as Magnetometer).Name;
            model.IsEnable = true;
            var devices = SatelliteEditor.Magnetometers;
            devices[slot - 1] = model;
            SatelliteEditor.Magnetometers = devices;            
        }
        private void addElectroMagnet(SatelliteComponent component, int slot)
        {
            ElMagnetInstallationModel model = new ElMagnetInstallationModel();
            model.ElectromagnetId = (component as ElMagnet).Id;
            model.Slot = slot;
            model.Name = (component as ElMagnet).Name;
            model.IsEnable = true;
            var devices = SatelliteEditor.ElMagnets;
            devices[slot - 1] = model;
            SatelliteEditor.ElMagnets = devices;
        }
        private void addGyro(SatelliteComponent component, int slot)
        {
            GyroInstallationModel model = new GyroInstallationModel();
            model.GyroId = (component as Gyro).Id;
            model.Slot = slot;
            model.Name = (component as Gyro).Name;
            model.IsEnable = true;
            var devices = SatelliteEditor.Gyros;
            devices[slot - 1] = model;
            SatelliteEditor.Gyros = devices;            
        }
        #endregion
    }

}

