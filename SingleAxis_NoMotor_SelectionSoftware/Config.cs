using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public static class Config {
        public static string DATABASE_FILENAME = "./PRX/";
        public static string MODEL_INFO_FILENAME = "./PRX/ModelInfo.csv";
        public static string MOMENT_FILENAME = "./PRX/Moment.csv";
        public static string STROKE_RPM_MAX_3000_FILENAME = "./PRX/StrokeRPM_Max3000.csv";
        public static string STROKE_RPM_MAX_3600_FILENAME = "./PRX/StrokeRPM_Max3600.csv";
        public static string MOTOR_INFO_FILENAME = "./PRX/MotorInfo.csv";
        public static string REDUCER_INFO_FILENAME = "./PRX/ReducerInfo.csv";

        public static string[] SETUP_ENV_TXT = { "標準", "無塵" };
        public enum SetupEnv {
            /// <summary>
            /// 標準
            /// </summary>
            Standard,
            /// <summary>
            /// 無塵
            /// </summary>
            DustFree
        }
        public static string[] MODEL_TYPE_TXT = {
            "軌道內嵌式螺桿滑台",
            "軌道內嵌推桿式螺桿滑台",
            "標準螺桿滑台",
            "推桿式螺桿滑台",
            "標準皮帶滑台",
            "歐規皮帶滑台",
        };
        public enum ModelType {
            /// <summary>
            /// 軌道內嵌式螺桿滑台
            /// </summary>
            Build_in_Linear_Motion_Guide_Ball_Screw_Actuator,
            /// <summary>
            /// 軌道內嵌推桿式螺桿滑台
            /// </summary>
            Rod_Type_Build_in_Linear_Motion_Guide_Ball_Screw_Actuator,
            /// <summary>
            /// 標準螺桿滑台
            /// </summary>
            Standard_Ball_Screw_Actuator,
            /// <summary>
            /// 推桿式螺桿滑台
            /// </summary>
            Rod_Type_Actuator,
            /// <summary>
            /// 標準皮帶滑台
            /// </summary>
            Standard_Belt_Actuator,
            /// <summary>
            /// 歐規皮帶滑台
            /// </summary>
            Europe_Type_Belt_Actuator,
        }
    }
}
