using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Condition : Model {
        // 使用頻率
        public class UseFrequence { public int countPerMinute, hourPerDay, dayPerYear; }

        public enum CalcAccordingItem { None, Load, Moment, All }                    // 修正壽命的根據項目列舉
        public enum PowerSelection { Standard, SelectedPower, Custom }               // 馬達瓦數帶值
        public enum CalcVmax { Max, Custom }                                         // 線速度計算帶值

        public SetupMethod setupMethod;                                              // 安裝方式            
        public CalcAccordingItem calcCloseToStandardItem = CalcAccordingItem.None;   // 修正壽命的根據項目
        public Moment calcUpdateMoment = Moment.A;                                   // 修正壽命的力舉項目
        public PowerSelection powerSelection = PowerSelection.Standard;              // 馬達功率帶最大或自訂            
        public CalcVmax vMaxCalcMode = CalcVmax.Max;                                 // 線速度計算時帶入的值
        public int selectedPower = -1;                                               // 馬達瓦數選擇            
        public UseFrequence useFrequence = new UseFrequence();                       // 使用頻率            
        public Dictionary<string, int> reducerRatio = new Dictionary<string, int>(); // 減速比
        public (string model, double lead) curSelectModel;                           // 目前選擇的項目            
        public (string model, double lead) curCheckedModel;                          // 目前打勾的項目            
        public bool isMomentLimitByCatalog = false;                                  // 最大力舉是否被型錄鎖定
        public (string model, double lead) calcModel;                                // 要計算的型號     
        public Func<double, bool> RepeatabilityCondition;                            // 重複定位精度搜尋範圍
        public const int defaultExpectServiceLifeTime = 3;                           // 希望壽命預設要求
        public int expectServiceLifeTime = defaultExpectServiceLifeTime;             // 希望壽命(年)(-1無希望壽命)
    }
}
