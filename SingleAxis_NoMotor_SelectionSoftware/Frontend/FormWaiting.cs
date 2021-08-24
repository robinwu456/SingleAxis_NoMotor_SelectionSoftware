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

        public FormWaiting(Func<(int percent, bool isError)> GetPercentEvent) {
            GetPercent = GetPercentEvent;

            InitializeComponent();
        }

        private void FormWaiting_Load(object sender, EventArgs e) {
            new Thread(() => {
                Thread.Sleep(1000);

                while (GetPercent().percent < 100) {
                    if (GetPercent().isError)
                        break;
                    int percent = GetPercent().percent;
                    this.Invoke(new Action(() => {
                        try {
                            lbLoadingPercent.Text = percent + "%";
                            progressBarLoading.Value = percent;
                        } catch (Exception ex) {
                            Console.WriteLine(ex);
                        }
                    }));
                }

                this.Invoke(new Action(() => {
                    this.Close();
                }));
            }).Start();            
        }
    }
}
