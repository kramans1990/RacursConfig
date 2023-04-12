using RacursCore.types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacursConfig.types
{

        public class Magnetometer
        {
           
            public int Id { get; set; }
            /// <summary>
            /// ориентация магнитометра относительно корпуса КА (кватернион- вектор4(x,y,z,w))
            /// </summary>
          
            private Attitude Att { get; set; }
            

         
            private Matrix3 Attm { get; set; }
            


            public Matrix3 SkewM { get; set; }
           
            /// <summary>
            /// Чувствительность (порог срабатывания)
            /// </summary>
            public double Sens { get; set; }
            /// <summary>
            /// Максимальное значение магнитной индукции (Тл)
            /// </summary>
            public double B_max { get; set; }
            /// <summary>
            /// разрядность АЦП
            /// </summary>
            public int ADC_bits { get; set; }
            /// <summary>
            ///  масштабный коэффициент
            /// </summary>
            public double Scale { get; set; }
            /// <summary>
            ///  стабильность масштабного коэффициента
            /// </summary>
            public double Scale_drift { get; set; }
            /// <summary>
            /// погрешность измерения (относительная)
            /// </summary>
            public double Error_rel { get; set; }
            /// <summary>
            /// погрешность измерения (абсолютная) (Тл)
            /// </summary>
            public double Error_abs { get; set; }
            /// <summary>
            /// смещение нуля (собственное магнитное поле)
            /// </summary>
            public double Bias { get; set; }
            /// <summary>
            ///  стандартное отклонение смещения нуля
            /// </summary>
            public double Bias_sd { get; set; }
            /// <summary>
            /// нестабильность смещения нуля (Тл/\sqrt{c})
            /// </summary>
            public double Bias_drift { get; set; }
            /// <summary>
            /// шум датчика в полосе
            /// </summary>
            public double Noise { get; set; }
            /// <summary>
            /// Название
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Описание
            /// </summary>
            public string Description { get; set; }


        }
    }

