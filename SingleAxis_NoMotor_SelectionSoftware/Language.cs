using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Language {
        public enum LanguageType {
            Chinese,
            English,
            Japan
        }
        public static LanguageType curLanguage = LanguageType.Chinese;

        public static void Load(Form form) {
            if (curLanguage == LanguageType.Chinese)
                LanguageSwitch.SetDefaultLanguage("zh-TW");
            else if (curLanguage == LanguageType.English)
                LanguageSwitch.SetDefaultLanguage("en-US");
            else if (curLanguage == LanguageType.Japan)
                LanguageSwitch.SetDefaultLanguage("ja-JP");
            LanguageSwitch.LoadLanguage(form, form.GetType());

            //LoadPictureButton(form);
            LoadDgv(form);
            //LoadParam();
        }

        //private static void LoadPictureButton(Form form) {
        //    // 切換picture button
        //    foreach (Control control in form.Controls.All()) {
        //        if (control.GetType().Equals(typeof(CustomPicButton.CustomPicButton))) {
        //            CustomPicButton.CustomPicButton picCmd = control as CustomPicButton.CustomPicButton;
        //            switch (Language.curLanguage) {
        //                case Language.LanguageType.Chinese:
        //                    picCmd.Culture = CustomPicButton.CustomPicButton.Language.Chinese;
        //                    break;
        //                case Language.LanguageType.English:
        //                    picCmd.Culture = CustomPicButton.CustomPicButton.Language.English;
        //                    break;
        //                case Language.LanguageType.Japan:
        //                    picCmd.Culture = CustomPicButton.CustomPicButton.Language.Japan;
        //                    break;
        //            }
        //        }
        //    }
        //}

        private static void LoadDgv(Form form) {
            // dgv column 語言切換
            foreach (Control control in form.Controls.All()) {
                if (control is DataGridView) {
                    DataGridView dgv = control as DataGridView;
                    foreach (DataGridViewColumn col in dgv.Columns) {
                        //col.HeaderText = Lang.GetText(col.Tag.ToString());
                        col.HeaderText = CustomExtensions.GetLang(col.Tag.ToString());
                    }
                }
            }
        }

        //private static void LoadParam() {
        //    // 參數說明語係切換
        //    foreach (Parameter param in Params.table) {
        //        param.content = Params.paramContent[param.address]["Content"][curLanguage];
        //        if (Params.paramContent[param.address].Keys.Contains("Description"))
        //            param.description = Params.paramContent[param.address]["Description"][curLanguage];
        //    }
        //}
    }

    public static class LanguageSwitch {
        /// <summary>
        /// 修改語言
        /// </summary>
        /// <param name="lang">語言</param>
        public static void SetDefaultLanguage(string lang) {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            //Properties.Settings.Default.DefaultLanguage = lang;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="form">加载语言的窗口</param>
        /// <param name="formType">窗口的类型</param>
        public static void LoadLanguage(Form form, Type formType) {
            if (form != null) {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(formType);
                resources.ApplyResources(form, "$this");
                Loading(form, resources);
            }
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(Control control, System.ComponentModel.ComponentResourceManager resources) {
            if (control is MenuStrip) {
                //将资源与控件对应
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0) {
                    foreach (ToolStripMenuItem c in ms.Items) {
                        //遍历菜单
                        Loading(c, resources);
                    }
                }
            }

            foreach (Control c in control.Controls) {
                resources.ApplyResources(c, c.Name);
                Loading(c, resources);
            }
        }


        /// <summary>
        /// 遍历菜单
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources) {
            if (item is ToolStripMenuItem) {
                resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                    foreach (ToolStripMenuItem c in tsmi.DropDownItems)
                        Loading(c, resources);
            }
        }
    }
}
