using System.Collections.Generic;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Model {
        public enum SetupMethod { Horizontal, WallHang, Vertical }
        public enum Moment { A, B, C }                                    // 力舉參數
        /// <summary>
        /// 傳動方式列舉
        /// </summary>
        public enum ModelType {
            /// <summary>
            /// 螺桿
            /// </summary>
            Screw,
            /// <summary>
            /// 皮帶
            /// </summary>
            Belt
        }

        /// <summary>
        /// 型號名稱
        /// </summary>
        public string name;

        /// <summary>
        /// 總預估壽命(km)
        /// </summary>
        public long serviceLifeDistance;

        /// <summary>
        /// 總預估壽命時間
        /// </summary>
        public (int year, int month, int day) serviceLifeTime;

        /// <summary>
        /// 重複定位精度
        /// </summary>
        public double repeatability = -1;
        /// <summary>
        /// 傳動方式
        /// </summary>
        public ModelType modelType = ModelType.Screw;

        /// <summary>
        /// 滑軌預估壽命(km)
        /// </summary>
        public long slideTrackServiceLifeDistance;
        /// <summary>
        /// 平均負載
        /// </summary>
        public double pm;
        /// <summary>
        /// 負荷係數
        /// </summary>
        public double fw;
        /// <summary>
        /// 壽命是否在規範內
        /// </summary>
        //public bool isLifeWithinSpecifications;

        /// <summary>
        /// W
        /// </summary>
        public double w;
        /// <summary>
        /// 加速區 Mr
        /// </summary>
        public double mr;

        /// <summary>
        /// 加速區
        /// </summary>
        public double mp_a, my_a, p_a;
        /// <summary>
        /// 等速區 Mp
        /// </summary>
        public double mp_c, my_c, p_c;
        /// <summary>
        /// 減速區 Mp
        /// </summary>
        public double mp_d, my_d, p_d;

        /// <summary>
        /// Velocity(速度) Vmax
        /// </summary>
        public double vMax;
        /// <summary>
        /// 最高線速度
        /// </summary>
        public double vMax_max;
        /// <summary>
        /// 導程
        /// </summary>
        public double lead;
        /// <summary>
        /// 外徑
        /// </summary>
        public int outerDiameter;
        /// <summary>
        /// 螺桿長度
        /// </summary>
        public int screwLength;

        #region 馬達資訊
        /// <summary>
        ///  適用馬達瓦數
        /// </summary>
        public List<int> availablePowers;
        /// <summary>
        /// 使用馬達瓦數
        /// </summary>
        public int usePower;
        /// <summary>
        /// 額定轉速(rpm)
        /// </summary>
        public int rpm;
        /// <summary>
        /// 最大轉矩
        /// </summary>
        public double maxTorque;
        /// <summary>
        /// 轉動慣量
        /// </summary>
        public double rotateInertia;
        /// <summary>
        /// 額定轉矩
        /// </summary>
        public double ratedTorque;
        #endregion

        #region 荷重資訊
        /// <summary>
        /// 質量(kg)
        /// </summary>
        public double load;
        /// <summary>
        /// 最大荷重(-1無最大荷重)
        /// </summary>
        public double maxLoad = -1;
        /// <summary>
        /// 荷重資訊 A(mm)
        /// </summary>
        public int moment_A;
        /// <summary>
        /// 荷重資訊 B(mm)
        /// </summary>
        public int moment_B;
        /// <summary>
        /// 荷重資訊 C(mm)
        /// </summary>
        public int moment_C;
        #endregion

        /// <summary>
        /// 安裝方式
        /// </summary>
        public List<SetupMethod> supportedSetup;

        /// <summary>
        /// 加減速度
        /// </summary>
        public double accelSpeed;
        /// <summary>
        /// 加速時間
        /// </summary>
        public double accelTime;
        /// <summary>
        /// 減速時間
        /// </summary>
        public double decelTime;
        /// <summary>
        /// 等速時間
        /// </summary>
        public double constantTime;
        /// <summary>
        /// 停等時間
        /// </summary>
        public double stopTime;
        /// <summary>
        /// 運行時間
        /// </summary>
        public double moveTime;

        /// <summary>
        /// 加速距離
        /// </summary>
        public double accelDistance;
        /// <summary>
        /// 等速距離
        /// </summary>
        public double constantDistance;
        /// <summary>
        /// 減速距離
        /// </summary>
        public double decelDistance;
        /// <summary>
        /// 總行程
        /// </summary>
        public int stroke;
        /// <summary>
        /// 最大行程
        /// </summary>
        public int maxStroke;

        /// <summary>
        /// C(N) Dyn 10000km
        /// </summary>
        public double c;
        /// <summary>
        /// Moment(Nm) MR_C, MP_C, MY_C
        /// </summary>
        public double mr_C, mp_C, my_C;

        //---------------------------------------螺桿壽命計算-----------------------------------
        /// <summary>
        /// 動額定負載
        /// </summary>
        public int dynamicLoadRating;
        /// <summary>
        /// 螺桿預估壽命(km)
        /// </summary>
        public long screwServiceLifeDistance;

        #region 加速區外力        
        /// <summary>
        /// 加速區 慣性負載
        /// </summary>
        public double inertialLoad_accel;
        /// <summary>
        /// 加速區 等效負載
        /// </summary>
        public double equivalentLoad_accel;
        #endregion

        #region 等速區外力        
        /// <summary>
        /// 等速區 慣性負載
        /// </summary>
        public double inertialLoad_constant;
        /// <summary>
        /// 等速區 等效負載
        /// </summary>
        public double equivalentLoad_constant;
        #endregion

        #region 減速區外力        
        /// <summary>
        /// 減速區 慣性負載
        /// </summary>
        public double inertialLoad_decel;
        /// <summary>
        /// 減速區 等效負載
        /// </summary>
        public double equivalentLoad_decel;
        #endregion

        // -------------------------------------------扭矩計算---------------------------------------
        /// <summary>
        /// T_max
        /// </summary>
        public double tMax;
        /// <summary>
        /// T_max安全係數
        /// </summary>
        public double tMaxSafeCoefficient;
        /// <summary>
        /// T_max最大扭矩確認
        /// </summary>
        public bool is_tMax_OK;
        /// <summary>
        /// T_max安全係數標準(大於等於)
        /// </summary>
        public const float tMaxStandard = 1.3f;

        /// <summary>
        /// T_Rms
        /// </summary>
        public double tRms;
        /// <summary>
        /// T_Rms安全係數
        /// </summary>
        public double tRmsSafeCoefficient;
        /// <summary>
        /// T_Rms扭矩確認
        /// </summary>
        public bool is_tRms_OK;
        /// <summary>
        /// T_Rms安全係數標準(大於)
        /// </summary>
        public const float tRmsStandard = 4f;


        #region 馬達能力預估-轉動慣量
        /// <summary>
        /// 轉動慣量-馬達
        /// </summary>
        public double rotateInertia_motor;
        /// <summary>
        /// 轉動慣量-螺桿
        /// </summary>
        public double rotateInertia_screw;
        /// <summary>
        /// 轉動慣量-水平移動體
        /// </summary>
        public double rotateInertia_horizontalMove;
        /// <summary>
        /// 轉動慣量-聯軸器
        /// </summary>
        public double rotateInertia_couplingItem = 0;
        /// <summary>
        /// 轉動慣量-滾珠軸承
        /// </summary>
        public double rotateInertia_ballBearing = 0;
        /// <summary>
        /// 轉動慣量-合計
        /// </summary>
        public double rotateInertia_total;
        #endregion

        #region 各階段軸向外力
        /// <summary>
        /// 加速區 滾動摩擦
        /// </summary>
        public double rollingFriction_accel;
        /// <summary>
        /// 加速區 配件摩擦
        /// </summary>
        public double accessoriesFriction_accel = 12;
        /// <summary>
        /// 加速區 其他外力
        /// </summary>
        public double otherForce_accel = 0;
        /// <summary>
        /// 加速區 外力合計
        /// </summary>
        public double forceTotal_accel;

        /// <summary>
        /// 等速區 滾動摩擦
        /// </summary>
        public double rollingFriction_constant;
        /// <summary>
        /// 等速區 配件摩擦
        /// </summary>
        public double accessoriesFriction_constant = 12;
        /// <summary>
        /// 等速區 其他外力
        /// </summary>
        public double otherForce_constant = 0;
        /// <summary>
        /// 等速區 外力合計
        /// </summary>
        public double forceTotal_constant;

        /// <summary>
        /// 減速區 滾動摩擦
        /// </summary>
        public double rollingFriction_decel;
        /// <summary>
        /// 減速區 配件摩擦
        /// </summary>
        public double accessoriesFriction_decel = 12;
        /// <summary>
        /// 減速區 其他外力
        /// </summary>
        public double otherForce_decel = 0;
        /// <summary>
        /// 減速區 外力合計
        /// </summary>
        public double forceTotal_decel;

        /// <summary>
        /// 停置區 滾動摩擦
        /// </summary>
        public double rollingFriction_stop = 0;
        /// <summary>
        /// 停置區 配件摩擦
        /// </summary>
        public double accessoriesFriction_stop = 0;
        /// <summary>
        /// 停置區 其他外力
        /// </summary>
        public double otherForce_stop = 0;
        /// <summary>
        /// 停置區 外力合計
        /// </summary>
        public double forceTotal_stop;
        #endregion

        #region 各階段馬達負擔扭矩
        /// <summary>
        /// 加速區 慣性扭矩
        /// </summary>
        public double inertialTorque_accel;
        /// <summary>
        /// 加速區 外力扭矩
        /// </summary>
        public double forceTorque_accel;
        /// <summary>
        /// 加速區 合計扭矩
        /// </summary>
        public double torqueTotal_accel;

        /// <summary>
        /// 等速區 慣性扭矩
        /// </summary>
        public double inertialTorque_constant;
        /// <summary>
        /// 等速區 外力扭矩
        /// </summary>
        public double forceTorque_constant;
        /// <summary>
        /// 等速區 合計扭矩
        /// </summary>
        public double torqueTotal_constant;

        /// <summary>
        /// 減速區 慣性扭矩
        /// </summary>
        public double inertialTorque_decel;
        /// <summary>
        /// 減速區 外力扭矩
        /// </summary>
        public double forceTorque_decel;
        /// <summary>
        /// 減速區 合計扭矩
        /// </summary>
        public double torqueTotal_decel;

        /// <summary>
        /// 停置區 慣性扭矩
        /// </summary>
        public double inertialTorque_stop;
        /// <summary>
        /// 停置區 外力扭矩
        /// </summary>
        public double forceTorque_stop;
        /// <summary>
        /// 停置區 合計扭矩
        /// </summary>
        public double torqueTotal_stop;
        #endregion

    }
}
