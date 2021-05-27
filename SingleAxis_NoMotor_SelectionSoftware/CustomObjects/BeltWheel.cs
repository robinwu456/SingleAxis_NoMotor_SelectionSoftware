using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class BeltWheel {
        /// <summary>
        /// 輪徑(mm)
        /// </summary>
        public double diameter;
        /// <summary>
        /// 皮帶輪寬度(mm)
        /// </summary>
        public double width;
        /// <summary>
        /// 皮帶輪材質密度
        /// </summary>
        public double materialDensity = 7800;
        /// <summary>
        /// 皮帶輪轉動慣量
        /// </summary>
        public double rotateInertia;

        /// <summary>
        /// 皮帶輪
        /// </summary>
        /// <param name="diameter">輪徑(mm)</param>
        /// <param name="width">皮帶輪寬度(mm)</param>
        public BeltWheel(double diameter, double width) {
            this.diameter = diameter;
            this.width = width;

            // 計算皮帶輪轉動慣量
            rotateInertia = GetRotateInertia();
        }

        // 皮帶輪轉動慣量公式
        private double GetRotateInertia() {
            return (Math.PI * Math.Pow(diameter, 4) * width * materialDensity * Math.Pow(10, -9)) / 32f;
        }
    }
}
