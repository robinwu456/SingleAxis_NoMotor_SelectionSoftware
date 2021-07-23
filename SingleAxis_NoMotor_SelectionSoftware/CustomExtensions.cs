using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public static class CustomExtensions {

        public static IEnumerable<Control> All(this Control.ControlCollection controls) {
            foreach (Control control in controls) {
                foreach (Control grandChild in control.Controls.All())
                    yield return grandChild;

                yield return control;
            }
        }

        public static string GetLang(string key) {
            ResourceManager rm = new ResourceManager("SingleAxis_NoMotor_SelectionSoftware." + CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, Assembly.GetExecutingAssembly());
            return rm.GetString(key);
        }

        /// <summary>
        /// 是否為皮帶型號
        /// </summary>
        /// <param name="modelType">機構類別</param>
        public static bool IsBeltType(this Model.ModelType modelType) {
            Model.ModelType[] beltType = {
                Model.ModelType.皮帶系列,
                Model.ModelType.歐規皮帶系列,
                Model.ModelType.軌道內崁歐規皮帶系列,
                Model.ModelType.軌道內崁歐規皮帶系列
            };

            return beltType.Contains(modelType);
        }

        /// <summary>
        /// 是否為推桿式
        /// </summary>
        /// <param name="modelType">機構類別</param>
        /// <returns></returns>
        public static bool IsRodType(this Model.ModelType modelType) {
            Model.ModelType[] beltType = {
                Model.ModelType.推桿系列,
                Model.ModelType.軌道內崁推桿系列,
                Model.ModelType.軌道外掛推桿系列,
                Model.ModelType.輔助導桿推桿系列,
            };

            return beltType.Contains(modelType);
        }

        /// <summary>
        /// 是否為包含減速比的型號
        /// </summary>
        /// <param name="modelType">機構類別</param>
        /// <returns></returns>
        public static bool IsContainsReducerRatioType(this Model.ModelType modelType) {
            Model.ModelType[] beltType = {
                Model.ModelType.歐規皮帶系列,
                Model.ModelType.軌道內崁歐規皮帶系列,
            };

            return beltType.Contains(modelType);
        }

        /// <summary>
        /// 是否為Y系列
        /// </summary>
        /// <param name="modelType">機構類別</param>
        /// <returns></returns>
        public static bool IsSeriesY(this Model.ModelType modelType) {
            Model.ModelType[] beltType = {
                Model.ModelType.推桿系列,
                Model.ModelType.輔助導桿推桿系列,
                Model.ModelType.軌道外掛推桿系列,
            };

            return beltType.Contains(modelType);
        }
    }
}
