using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public static class Config {
        public const string DATABASE_FILENAME = "./PRX/";
        public const string MODEL_INFO_FILENAME = "./PRX/型號參數.csv";
        public const string MOMENT_FILENAME = "./PRX/荷重表.csv";
        public const string STROKE_RPM_FILENAME = "./PRX/行程對照轉速.csv";
        public const string MOTOR_INFO_FILENAME = "./PRX/馬達參數.csv";
        public const string BELT_INFO_FILENAME = "./PRX/皮帶參數.csv";
        public const string MODEL_TYPE_INFO_FILENAME = "./PRX/型號類別.csv";
        public const string ReducerRotateInertia = "./PRX/減速機轉動慣量表.csv";

        // log
        public static string LOG_FILENAME = "./Log/";  // 所有參數Log
        public static string LOG_FAIL_MODELS_FILENAME = "./Log/FailModels/";  // 所有參數Log
        public static string LOG_PARAM_FILENAME = "./Log/Model.log";  // 所有參數Log
        public static string LOG_FILTER_FILENAME = "./Log/ModelFilter.log";  // 所有篩選Log
    }
}
