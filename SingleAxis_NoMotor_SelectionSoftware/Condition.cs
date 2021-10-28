using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Condition : Model {
        // 使用頻率
        public class UseFrequence {
            public double countPerMinute;
            public int hourPerDay;
            public int dayPerYear; 
        }

        public enum PowerSelection { Standard, SelectedPower, Custom }               // 馬達瓦數帶值
        public enum CalcVmax { Max, Custom }                                         // 線速度計算帶值
        public enum CalcMaxItem { Vmax, AccelSpeed, AccelTime }                      // 最大值計算固定項目
        public enum MoveSpeedUnit { Vmax, RPM }                                      // 運行速度單位
        public enum CalcMode { 
            /// <summary>
            /// 一般計算
            /// </summary>
            Normal, 
            /// <summary>
            /// 測試
            /// </summary>
            Test, 
            /// <summary>
            /// 最高速計算
            /// </summary>
            CalcMax 
        }
        public enum CalcMaxUnit {
            mms,
            RPM,
            mms2,
            G,
            s
        }

        public SetupMethod setupMethod;                                              // 安裝方式            
        public PowerSelection powerSelection = PowerSelection.Standard;              // 馬達功率帶最大或自訂            
        public CalcVmax vMaxCalcMode = CalcVmax.Max;                                 // 線速度計算時帶入的值
        public int selectedPower = -1;                                               // 馬達瓦數選擇            
        public UseFrequence useFrequence = new UseFrequence();                       // 使用頻率            
        public (string model, double lead) curSelectModel;                           // 目前選擇的項目            
        public (string model, double lead) curCheckedModel;                          // 目前打勾的項目            
        public (string model, double lead) calcModel;                                // 要計算的型號     
        public const int defaultExpectServiceLifeTime = 3;                           // 希望壽命預設要求
        public int expectServiceLifeTime = defaultExpectServiceLifeTime;             // 希望壽命(年)(-1無希望壽命)
        public MoveSpeedUnit moveSpeedUnit = MoveSpeedUnit.Vmax;
        public CalcMode calcMode = CalcMode.Normal;
        public CalcMaxItem calcMaxItem = CalcMaxItem.Vmax;
        public CalcMaxUnit calcMaxUnit = CalcMaxUnit.mms;
        public bool isRpmLimitByStroke = true;
        public bool isCalcByMaxLoad = true;
    }
}
