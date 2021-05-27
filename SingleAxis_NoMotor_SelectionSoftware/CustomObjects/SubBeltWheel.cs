using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class SubBeltWheel : BeltWheel {
        /// <summary>
        /// 從動皮帶輪轉動慣量
        /// </summary>
        public double rotateInertia_subWheel;

        /// <summary>
        /// 從動輪
        /// </summary>
        /// <param name="diameter">輪徑(mm)</param>
        /// <param name="width">皮帶輪寬度(mm)</param>
        /// <param name="materialDensity">皮帶輪材質密度</param>
        public SubBeltWheel(double diameter, double width) : base(diameter, width) {
            // 計算從動皮帶輪轉動慣量
            rotateInertia_subWheel = GetSubWheelRotateInertia();
        }

        // 從動皮帶輪轉動慣量公式
        private double GetSubWheelRotateInertia() {
            return (Math.PI * 7.8f * Math.Pow(10, 3) * (38f / 1000f) * Math.Pow(diameter / 1000, 4)) / 32f;
        }
    }
}
