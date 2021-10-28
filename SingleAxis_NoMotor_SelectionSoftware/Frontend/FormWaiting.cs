using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public partial class FormWaiting : Form {

        public Func<(int percent, bool isError)> GetPercent;
        public Action InterruptCalc;

        public FormWaiting(Func<(int percent, bool isError)> GetPercentEvent, Action InterruptCalc) {
            GetPercent = GetPercentEvent;
            this.InterruptCalc = InterruptCalc;

            InitializeComponent();
        }

        private void FormWaiting_Load(object sender, EventArgs e) {
            new Thread(() => {
                Thread.Sleep(1000);

                while (GetPercent().percent < 100) {
                    if (GetPercent().isError)
                        break;
                    int percent = GetPercent().percent;

                    //// 中斷計算
                    //if (this.IsDisposed) {
                    //    InterruptCalc();
                    //    break;
                    //}

                    try {
                        this.Invoke(new Action(() => {
                            try {
                                lbLoadingPercent.Text = percent + "%";
                                progressBarLoading.Value = percent;
                            } catch (Exception ex) {
                                Console.WriteLine(ex);
                            }
                        }));
                    } catch (Exception ex) {
                        InterruptCalc();
                        return;
                    }
                }

                this.Invoke(new Action(() => {
                    this.Close();
                }));
            }).Start();            
        }
    }
}
