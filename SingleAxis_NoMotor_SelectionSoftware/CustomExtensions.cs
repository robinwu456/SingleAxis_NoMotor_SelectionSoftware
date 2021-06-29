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

        public static bool IsBeltType(this Model.ModelType modelType) {
            Model.ModelType[] beltType = {
                Model.ModelType.標準皮帶滑台,
                Model.ModelType.歐規皮帶滑台,
                Model.ModelType.軌道內嵌皮帶滑台,
                Model.ModelType.軌道內嵌皮帶滑台
            };

            return beltType.Contains(modelType);
        }
    }
}
