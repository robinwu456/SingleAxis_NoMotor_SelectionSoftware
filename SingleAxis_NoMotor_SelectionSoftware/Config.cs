using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public static class Config {
        public static string DATABASE_FILENAME = "./PRX/";
        public static string MODEL_INFO_FILENAME = "./PRX/ModelInfo.csv";
        public static string MOMENT_FILENAME = "./PRX/Moment.csv";
        public static string STROKE_RPM_FILENAME = "./PRX/StrokeRPM.csv";
        public static string MOTOR_INFO_FILENAME = "./PRX/MotorInfo.csv";
        public static string REDUCER_INFO_FILENAME = "./PRX/ReducerInfo.csv";
        public static string BELT_INFO_FILENAME = "./PRX/BeltInfo.csv";
        public static string MODEL_TYPE_INFO_FILENAME = "./PRX/ModelTypeInfo.csv";
        public static string LOG_FILENAME = "./Model.log";
    }
}
