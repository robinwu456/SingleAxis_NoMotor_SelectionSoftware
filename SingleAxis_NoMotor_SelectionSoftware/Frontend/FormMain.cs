using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Binarymission.WinForms.Controls.NavigationControls;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Globalization;
using System.IO;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public partial class FormMain : Form {
        public SideTable sideTable;
        public Page1 page1;
        public Page2 page2;
        public Page3 page3;

        private string version;
        //private int trailVersionUseDays = 0;    // 試用版使用期限(天數)
        private DateTime trailVersionDateTime = new DateTime(2021, 12, 31);

        public FormMain() {
            InitializeComponent();            

            // 標題列
            ToyoBorder toyoBorder = new ToyoBorder(this);

            // 測邊欄
            sideTable = new SideTable(this);

            // Step
            page1 = new Page1(this);
            page2 = new Page2(this);
            page3 = new Page3(this);
        }        

        private void FormMain_Load(object sender, EventArgs e) {
            // 判斷DB被開啟
            foreach (string fileName in Directory.GetFiles(Config.DATABASE_FILENAME)) {
                // 檔案已開啟為1，測試總表
                while (FileUtil.FileStatus.FileIsOpen(fileName) == 1) {
                    DialogResult stillContinue = MessageBox.Show(string.Format("[{0}]\n檔案已被開啟，確認關閉後按下確定。", new FileInfo(fileName).FullName), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (stillContinue == DialogResult.No)
                        Close();
                    else {
                        // 重Load DB
                        page2.calc.modelInfo = FileUtil.ReadCsv(Config.MODEL_INFO_FILENAME);
                        page2.calc.strokeRpm = FileUtil.ReadCsv(Config.STROKE_RPM_FILENAME);
                        page2.calc.momentData = FileUtil.ReadCsv(Config.MOMENT_FILENAME);
                        page2.calc.motorInfo = FileUtil.ReadCsv(Config.MOTOR_INFO_FILENAME);
                        page2.calc.reducerInfo = FileUtil.ReadCsv(Config.REDUCER_INFO_FILENAME);
                        page2.calc.beltInfo = FileUtil.ReadCsv(Config.BELT_INFO_FILENAME);
                        page2.calc.modelTypeInfo = FileUtil.ReadCsv(Config.MODEL_TYPE_INFO_FILENAME);
                    }
                }
            }

            // 取得當前版本
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            version = fvi.FileVersion;
            lbTitle.Text += " v" + version;

            // 到期日判斷
            //UseDaysLimit();
            VerifyDateTimeLimit();
        }

        private void FormMain_Resize(object sender, EventArgs e) {
            sideTable.ResizeSideTable();
        }

        private void UseDaysLimit() {
            ////RegistryKey retKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("software", true).CreateSubKey("mrwxk").CreateSubKey("mrwxk.ini");
            //RegistryKey retKey = Registry.LocalMachine.OpenSubKey("software", true);

            //string subKeyName = "NoMotorSelectionSoftware";
            //string valueName = "EndTime";

            //Int32 tLong;
            //try {
            //    tLong = (Int32)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\" + subKeyName, valueName, 0);
            //    tLong = (Int32)retKey.GetValue(subKeyName, valueName);
            //    MessageBox.Show("感谢您已使用了" + tLong + "次", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //} catch {
            //    //首次使用软件
            //    Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\" + subKeyName, valueName, 0, RegistryValueKind.DWord);
            //    MessageBox.Show("欢迎新用户使用本软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            ////获取软件已经使用的次数
            //tLong = (Int32)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\" + subKeyName, valueName, 0);
            //if (tLong < 10) {
            //    int Times = tLong + 1; //计算软件本次是第几次使用
            //    Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\" + subKeyName, valueName, Times); //将软件使用次数写入注册表
            //} else {
            //    //Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\tryTimes", "UseTimes", 0); //将软件使用次数写入注册表 
            //    //MessageBox.Show("试用次数已到", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    Application.Exit(); //退出应用程序
            //}

            // -------------------------------------------------以下為正式Code----------------------------------------------------------------
            //// 到期日讀取
            //RegistryKey retKey = Registry.LocalMachine.OpenSubKey("software", true).OpenSubKey("SingleAxis_NoMotor_SelectionSoftware", true);
            //string setupDateTimeValueName = "SetupDateTime";
            //string userLevelValueName = "UserLevel";
            //string value = "";
            //try {
            //    value = (string)retKey.GetValue(setupDateTimeValueName, 0);                
            //} catch {
            //    MessageBox.Show("未正常安裝軟體，請再安裝一次", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Application.Exit();
            //}

            //// userLevel=1時永久使用
            //try {
            //    string userLevel = (string)retKey.GetValue(userLevelValueName, 0);
            //    bool isAdmin = userLevel == "1";
            //    if (isAdmin)
            //        return;
            //} catch { }

            //// 到期日判斷
            //DateTime setupTime = DateTime.ParseExact(value, "yyMMddHHmm", CultureInfo.InvariantCulture);
            //DateTime endDate = setupTime.AddDays(trailVersionUseDays);
            ////MessageBox.Show("到期日為：" + endDate, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //if (DateTime.Now > endDate) {
            //    MessageBox.Show("已過使用期限", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Application.Exit();
            //}
        }

        private int openAdvanceModeCount = 0;       // 目前點擊次數
        private int openAdvanceModeNeedCount = 5;   // 需要點擊次數
        private DateTime advanceModeLastClickTime;  // 最後一次觸發進階模式密碼時間
        private void pictureBoxToyo_DoubleClick(object sender, EventArgs e) {
            // 每次點擊間格超過5秒就重製記數
            if (DateTime.Now.Second - advanceModeLastClickTime.Second > 5)
                openAdvanceModeCount = 0;
            advanceModeLastClickTime = DateTime.Now;

            openAdvanceModeCount++;
            if (openAdvanceModeCount >= openAdvanceModeNeedCount) {
                // 開啟永久使用
                RegistryKey retKey = Registry.LocalMachine.OpenSubKey("software", true).OpenSubKey("SingleAxis_NoMotor_SelectionSoftware", true);
                retKey.SetValue("UserLevel", "1", RegistryValueKind.String);
                MessageBox.Show("已開啟永久使用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                openAdvanceModeCount = 0;
            }
        }

        private void VerifyDateTimeLimit() {
            if (DateTime.Now > trailVersionDateTime.AddDays(1)) {
                MessageBox.Show("已過使用期限", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
