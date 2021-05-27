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
    }
}
