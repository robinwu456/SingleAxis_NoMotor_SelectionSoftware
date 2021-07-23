using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public static class FileUtil {
        public class FileStatus {
            [DllImport("kernel32.dll")]
            private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
            [DllImport("kernel32.dll")]
            private static extern bool CloseHandle(IntPtr hObject);
            private const int OF_READWRITE = 2;
            private const int OF_SHARE_DENY_NONE = 0x40;
            private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
            public static int FileIsOpen(string fileFullName) {
                if (!File.Exists(fileFullName)) {
                    return -1;
                }
                IntPtr handle = _lopen(fileFullName, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (handle == HFILE_ERROR) {
                    return 1;
                }
                CloseHandle(handle);
                return 0;
            }
        }

        public static void CheckFileIsOpen(this Form formMain, string fileName) {
            // 檔案已開啟為1，測試總表
            while (FileUtil.FileStatus.FileIsOpen(fileName) == 1) {
                DialogResult stillContinue = MessageBox.Show(string.Format("[{0}]\n檔案已被開啟，確認關閉後按下確定。", new FileInfo(fileName).FullName), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (stillContinue == DialogResult.No)
                    formMain.Close();
            }
        }

        public static string FileRead(string fileName) {
            StringBuilder resultString = new StringBuilder();

            try {
                using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default)) {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        resultString.AppendLine(line);
                }
            } catch (Exception ex) {
                Console.WriteLine("The file could not be read [" + fileName + "] :" + ex.ToString());
            }

            if (!resultString.ToString().Contains("??"))
                return resultString.ToString();

            resultString.Clear();
            try {
                using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.UTF8)) {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        resultString.AppendLine(line);
                }
            } catch (Exception ex) {
                Console.WriteLine("The file could not be read [" + fileName + "] :" + ex.ToString());
            }

            return resultString.ToString();
        }

        public static void FileWrite(string fileName, string content) {
            try {
                if (File.Exists(fileName))
                    File.Delete(fileName);

                using (StreamWriter sw = File.AppendText(fileName)) {
                    sw.WriteLine(content);
                }
            } catch (Exception ex) {
                Console.WriteLine("The file could not be write [" + fileName + "] :" + ex.ToString());
            }
        }

        // Csv讀取
        public static DataTable ReadCsv(string fileName) {
            DataTable dt = new DataTable();
            string content = FileRead(fileName);
            string[] rows = content.Replace("\r\n", "|").Split('|');
            string[] columns = rows[0].Split(',');

            // 新增欄位
            foreach (string column in columns)
                dt.Columns.Add(column);

            // 資料匯入
            for (int i = 1; i < rows.Length - 1; i++) {
                // 空的則跳過
                if (rows[i] == string.Empty)
                    continue;

                DataRow newRow = dt.NewRow();
                foreach (DataColumn column in dt.Columns)
                    newRow[column] = rows[i].Split(',')[dt.Columns.IndexOf(column)];
                dt.Rows.Add(newRow);
            }

            return dt;
        }        

        public static void OutputDgv(DataGridView dgv) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "Output.csv";
            bool fileError = false;
            if (sfd.ShowDialog() == DialogResult.OK) {
                if (File.Exists(sfd.FileName)) {
                    try {
                        File.Delete(sfd.FileName);
                    } catch (IOException ex) {
                        fileError = true;
                        MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                    }
                }
                if (!fileError) {
                    try {
                        int columnCount = dgv.Columns.Count;
                        string columnNames = "";
                        string[] outputCsv = new string[dgv.Rows.Count + 1];
                        for (int i = 0; i < columnCount; i++) {
                            columnNames += dgv.Columns[i].HeaderText.ToString() + ",";
                        }
                        outputCsv[0] += columnNames;

                        for (int i = 1; (i - 1) < dgv.Rows.Count; i++) {
                            for (int j = 0; j < columnCount; j++) {
                                outputCsv[i] += dgv.Rows[i - 1].Cells[j].Value.ToString() + ",";
                            }
                        }

                        File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                        MessageBox.Show("Data Exported Successfully !!!", "Info");
                    } catch (Exception ex) {
                        MessageBox.Show("Error :" + ex.Message);
                    }
                }
            }
        }

        public static void LogModelInfo(Model model) {
            var result = new List<(string key, object value)>() {
                ("型號", model.name),
                ( "導程", model.lead ),
                ( "移動行程", model.stroke ),
                ( "C(N) Dyn", model.c ),
                ( "Moment(Nm)", "" ),
                ( "  MR_C", model.mr_C ),
                ( "  MP_C", model.mp_C ),
                ( "  MY_C", model.my_C ),
                ( "馬達資訊", "" ),
                ( "  功率(W)", model.usePower ),
                ( "  額定轉速(RPM)", model.rpm ),
                ( "  額定轉矩(Nm)", model.ratedTorque ),
                ( "  最大轉矩(Nm)", model.maxTorque ),
                ( "  轉動慣量(kg-m^2)", model.rotateInertia.ToString("#0.00000000") ),
                ( "Velocity", "" ),
                ( "  Vmax(mm/s)", model.vMax ),
                ( "  加減速時間(s)", model.accelTime ),
                ( "  等速時間(s)", model.constantTime ),
                ( "  加減速度(mm/s^2)", model.accelSpeed ),
                ( "  總時間(s)", model.moveTime ),
                ( "行程參數", "" ),
                ( "  加速距離(mm)", model.accelDistance ),
                ( "  等速距離(mm)", model.constantDistance ),
                ( "  減速距離(mm)", model.decelDistance ),
                ( "荷重資訊", "" ),
                ( "  質量(kg)", model.load ),
                ( "  A(mm)", model.moment_A ),
                ( "  B(mm)", model.moment_B ),
                ( "  B(mm)", model.moment_C ),
                ( "加速區", "" ),
                ( "  W", model.w ),
                ( "  Mr", model.mr ),
                ( "  Mp", model.mp_a ),
                ( "  My", model.my_a ),
                ( "  P_a", model.p_a ),
                ( "等速區", "" ),
                ( "  W", model.w ),
                ( "  Mr", model.mr ),
                ( "  Mp", model.mp_c ),
                ( "  My", model.my_c ),
                ( "  P_c", model.p_c ),
                ( "減速區", "" ),
                ( "  W", model.w ),
                ( "  Mr", model.mr ),
                ( "  Mp", model.mp_d ),
                ( "  My", model.my_d ),
                ( "  P_d", model.p_d ),
                ( "平均負載Pm", model.pmSlide ),
                ( "負荷係數Fw", model.fwSlide ),
                ( "預估壽命L(km)", model.slideTrackServiceLifeDistance ),
                ( "各階段軸向外力", "" ),
                ( "  加速區外力(N)", " " ),
                ( "    滾動摩擦", model.rollingFriction_accel ),
                ( "    配件摩擦", model.mainWheel_P1 == null ? model.accessoriesFriction_accel : model.accessoriesFriction_belt_accel ),
                ( "    其他外力", model.otherForce_accel ),
                ( "    合計", model.forceTotal_accel ),
                ( "  等速區外力(N)", " " ),
                ( "    滾動摩擦", model.rollingFriction_constant ),
                ( "    配件摩擦", model.mainWheel_P1 == null ? model.accessoriesFriction_constant : model.accessoriesFriction_belt_constant ),
                ( "    其他外力", model.otherForce_constant ),
                ( "    合計", model.forceTotal_constant ),
                ( "  減速區外力(N)", " " ),
                ( "    滾動摩擦", model.rollingFriction_decel ),
                ( "    配件摩擦", model.mainWheel_P1 == null ? model.accessoriesFriction_decel : model.accessoriesFriction_belt_decel ),
                ( "    其他外力", model.otherForce_decel ),
                ( "    合計", model.forceTotal_decel ),
                ( "  停置區外力(N)", " " ),
                ( "    滾動摩擦", model.rollingFriction_stop ),
                ( "    配件摩擦", model.mainWheel_P1 == null ? model.accessoriesFriction_stop : model.accessoriesFriction_belt_stop ),
                ( "    其他外力", model.otherForce_stop ),
                ( "    合計", model.forceTotal_stop ),
                // 各階段馬達負擔扭矩
                ( "  等速區扭矩", " " ),
                ( "    慣性扭矩", model.inertialTorque_constant ),
                ( "    外力扭矩", model.forceTorque_constant ),
                ( "    合計扭矩", model.torqueTotal_constant ),
                ( "  減速區扭矩", " " ),
                ( "    慣性扭矩", model.inertialTorque_decel ),
                ( "    外力扭矩", model.forceTorque_decel ),
                ( "    合計扭矩", model.torqueTotal_decel ),
                ( "  停置區扭矩", " " ),
                ( "    慣性扭矩", model.inertialTorque_stop ),
                ( "    外力扭矩", model.forceTorque_stop ),
                ( "    合計扭矩", model.torqueTotal_stop ),
                ( "T_max最大扭矩確認", "" ),
                ( "  T_max", model.tMax ),
                ( "  安全係數", model.tMaxSafeCoefficient ),
                ( "  確認", model.is_tMax_OK ? "OK" : "NG" ),
            };
            var screwInfo = new List<(string key, object value)>() {
                ( "螺桿資訊", "" ),
                ( "  外徑(mm)", model.outerDiameter ),
                ( "  螺桿長度(mm)", model.screwLength ),
                ( "  動額定負載(N)", model.dynamicLoadRating ),
            };
            var motorInfoScrew = new List<(string key, object value)>() {
                ( "馬達能力預估-轉動慣量", "" ),
                ( "  馬達", model.rotateInertia_motor ),
                ( "  水平移動體", model.rotateInertia_horizontalMove ),
                ( "  聯軸器", model.rotateInertia_couplingItem ),
                ( "  滾珠軸承", model.rotateInertia_ballBearing ),
                ( "  合計", model.rotateInertia_total ),
            };
            var motorTorqueInfoScrew = new List<(string key, object value)>() {
                ( "各階段馬達負擔扭矩", "" ),
                ( "  加速區扭矩", " " ),
                ( "    慣性扭矩", model.inertialTorque_accel ),
                ( "    外力扭矩", model.forceTorque_accel ),
                ( "    合計扭矩", model.torqueTotal_accel ),
            };
            var tRmsInfo = new List<(string key, object value)>() {
                ( "T_Rms扭矩確認", "" ),
                ( "  T_Rms", model.tRms ),
                ( "  安全係數", model.tRmsSafeCoefficient ),
                ( "  確認", model.is_tRms_OK ? "OK" : "NG" ),
            };
            var screwLifeInfo = new List<(string key, object value)>() {
                ( "螺桿壽命預估", "" ),
                ( "  加速區外力", " " ),
                ( "    滾動摩擦", model.rollingFriction_accel ),
                ( "    配件摩擦", model.accessoriesFriction_accel ),
                ( "    慣性負載", model.inertialLoad_accel ),
                ( "    其他外力", model.otherForce_accel ),
                ( "    等效負載", model.equivalentLoad_accel ),
                ( "  等速區外力", " " ),
                ( "    滾動摩擦", model.rollingFriction_constant ),
                ( "    配件摩擦", model.accessoriesFriction_constant ),
                ( "    慣性負載", model.inertialLoad_constant ),
                ( "    其他外力", model.otherForce_constant ),
                ( "    等效負載", model.equivalentLoad_constant ),
                ( "  減速區外力", " " ),
                ( "    滾動摩擦", model.rollingFriction_decel ),
                ( "    配件摩擦", model.accessoriesFriction_decel ),
                ( "    慣性負載", model.inertialLoad_decel ),
                ( "    其他外力", model.otherForce_decel ),
                ( "    等效負載", model.equivalentLoad_decel ),
                ( "  壽命統計", " " ),
                ( "    平均負載", model.pmScrew ),
                ( "    負荷係數", model.fwScrew ),
                ( "    預估壽命(km)", model.screwServiceLifeDistance ),
            };
            var beltInfo = new List<(string key, object value)>();
            var motorInfoBelt = new List<(string key, object value)>();
            var motorTorqueInfoBelt = new List<(string key, object value)>();
            var beltTorqueInfo = new List<(string key, object value)>();
            if (model.mainWheel_P1 != null) {
                beltInfo = new List<(string key, object value)>() {
                    ( "主動輪資訊(P1)", "" ),
                    ( "  輪徑(mm)", model.mainWheel_P1.diameter ),
                    ( "  皮帶輪寬度(mm)", model.mainWheel_P1.width ),
                    ( "  皮帶輪材質密度(kg/m^3)", model.mainWheel_P1.materialDensity ),
                    ( "  皮帶輪轉動慣量(kg‧mm^2)", model.mainWheel_P1.rotateInertia ),
                    ( "從動輪資訊(P2)", "" ),
                    ( "  輪徑(mm)", model.subWheel_P2.diameter ),
                    ( "  皮帶輪寬度(mm)", model.subWheel_P2.width ),
                    ( "  皮帶輪材質密度(kg/m^3)", model.subWheel_P2.materialDensity ),
                    ( "  皮帶輪轉動慣量(kg‧mm^2)", model.subWheel_P2.rotateInertia ),
                    ( "  從動皮帶輪轉動慣量(kg‧mm^2)", model.subWheel_P2.rotateInertia_subWheel ),
                    ( "從動輪資訊(P3)", "" ),
                    ( "  輪徑(mm)", model.subWheel_P3.diameter ),
                    ( "  皮帶輪寬度(mm)", model.subWheel_P3.width ),
                    ( "  皮帶輪材質密度(kg/m^3)", model.subWheel_P3.materialDensity ),
                    ( "  皮帶輪轉動慣量(kg‧mm^2)", model.subWheel_P3.rotateInertia ),
                    ( "  從動皮帶輪轉動慣量(kg‧mm^2)", model.subWheel_P3.rotateInertia_subWheel ),
                    ( "從動輪資訊(P4)", "" ),
                    ( "  輪徑(mm)", model.subWheel_P4.diameter ),
                    ( "  皮帶輪寬度(mm)", model.subWheel_P4.width ),
                    ( "  皮帶輪材質密度(kg/m^3)", model.subWheel_P4.materialDensity ),
                    ( "  皮帶輪轉動慣量(kg‧mm^2)", model.subWheel_P4.rotateInertia ),
                    ( "  從動皮帶輪轉動慣量(kg‧mm^2)", model.subWheel_P4.rotateInertia_subWheel ),
                    ( "皮帶輪加減速關係", "" ),
                    ( "  主動輪(馬達)轉速(rpm)", model.mainWheelRpm ),
                    ( "  從動輪(減速機)轉速(rpm)", model.subWheelRpm ),
                    ( "  減速機轉速比(N1/N2)", model.reducerRpmRatio ),
                    ( "  皮帶寬(mm)", model.beltWidth ),
                    ( "  皮帶長度(mm)", model.beltLength ),
                    ( "  皮帶單位密度(g/mm寬*m長)", model.beltUnitDensity ),
                    ( "  皮帶重量(kg)", model.beltLoad ),
                    ( "  皮帶容許拉力(N)", model.beltAllowableTension ),

                };
                motorInfoBelt = new List<(string key, object value)>() {
                    ( "馬達能力預估-轉動慣量", "" ),
                    ( "  轉子馬達轉動慣量", model.rotateInertia_motor ),
                    ( "  負載物的等效轉動慣量", model.rotateInertia_load ),
                    ( "  皮帶轉動慣量", model.rotateInertia_belt ),
                    ( "  聯軸器", model.rotateInertia_couplingItem ),
                    ( "  滾珠軸承", model.rotateInertia_ballBearing ),
                    ( "  合計", model.rotateInertia_total ),
                    ( "  負載慣量與力矩比", model.loadInertiaMomentRatio ),
                    ( "  馬達是否適用", model.isMotorOK ? "OK" : "NG" ),
                };
                motorTorqueInfoBelt = new List<(string key, object value)>() {
                    ( "各階段馬達負擔扭矩", "" ),
                    ( "  加速區扭矩", " " ),
                    ( "    負載移動時的等效慣性矩(相對於從動輪)", model.rotateInertia_loadMoving_subWheel ),
                    ( "    負載移動時的等效慣性矩(對馬達)", model.rotateInertia_loadMoving_motor ),
                    ( "    慣性扭矩", model.inertialTorque_accel ),
                    ( "    外力扭矩", model.forceTorque_accel ),
                    ( "    合計扭矩", model.torqueTotal_accel ),
                };
                beltTorqueInfo = new List<(string key, object value)>() {
                    ( "皮帶各階段最大扭矩", "" ),
                    ( "  加速扭矩", model.beltTorque_accel ),
                    ( "  等速扭矩", model.beltTorque_constant ),
                    ( "  減速扭矩", model.beltTorque_decel ),
                    ( "  停置扭矩", model.beltTorque_stop ),
                    ( "皮帶評估-Tmax最大扭矩評估", "" ),                    
                    ( "  T_max", model.belt_tMax ),
                    ( "  皮帶承受力", model.beltEndurance ),
                    ( "  安全係數", model.beltSafeCoefficient ),
                    ( "  評估(本身具有3的安全係數)", model.is_belt_tMax_OK ? "OK" : "NG" ),
                };
            }
            if (model.mainWheel_P1 == null) {
                ///
                /// 螺桿型
                ///

                // 螺桿資訊
                if (!model.modelType.ToString().Contains("皮帶"))                    
                    result.InsertRange(result.IndexOf(result.First(r => r.key == "Velocity")), screwInfo);
                // 馬達能力預估
                result.InsertRange(result.IndexOf(result.First(r => r.key == "各階段軸向外力")), motorInfoScrew);
                // 各階段馬達負擔扭矩
                result.InsertRange(result.IndexOf(result.First(r => r.key == "  等速區扭矩")), motorTorqueInfoScrew);
                // T_Rms扭矩確認
                result.AddRange(tRmsInfo);
                // 螺桿壽命預估
                if (!model.modelType.ToString().Contains("皮帶"))
                    result.AddRange(screwLifeInfo);
            } else {
                ///
                /// 皮帶型
                ///

                // 皮帶輪資訊
                result.InsertRange(result.IndexOf(result.First(r => r.key == "Velocity")), beltInfo);
                // 馬達能力預估
                result.InsertRange(result.IndexOf(result.First(r => r.key == "各階段軸向外力")), motorInfoBelt);
                // 各階段馬達負擔扭矩
                result.InsertRange(result.IndexOf(result.First(r => r.key == "  等速區扭矩")), motorTorqueInfoBelt);
                // 皮帶能力預估
                result.AddRange(beltTorqueInfo);
            }

            // 匯出總資訊
            string printInfo = "";
            foreach (var info in result) {
                // 一階
                if (info.value.ToString() == "")
                    printInfo += "\r\n" + info.key + "\r\n";
                // 二階
                else if (info.value.ToString() == " ")
                    printInfo += info.key + "\r\n";
                // 數值
                else
                    printInfo += info.key + "：" + info.value + "\r\n";
            }
            FileWrite(Config.LOG_PARAM_FILENAME, printInfo);
        }
    }
}
