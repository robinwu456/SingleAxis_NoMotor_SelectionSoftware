using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
    }
}
