namespace SingleAxis_NoMotor_SelectionSoftware {
    partial class FormWaiting {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lbLoadingPercent = new System.Windows.Forms.Label();
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lbLoadingPercent
            // 
            this.lbLoadingPercent.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbLoadingPercent.Location = new System.Drawing.Point(20, 0);
            this.lbLoadingPercent.Name = "lbLoadingPercent";
            this.lbLoadingPercent.Size = new System.Drawing.Size(181, 29);
            this.lbLoadingPercent.TabIndex = 0;
            this.lbLoadingPercent.Text = "43%";
            this.lbLoadingPercent.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarLoading.Location = new System.Drawing.Point(20, 32);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(181, 26);
            this.progressBarLoading.TabIndex = 1;
            // 
            // FormWaiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 78);
            this.ControlBox = false;
            this.Controls.Add(this.progressBarLoading);
            this.Controls.Add(this.lbLoadingPercent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormWaiting";
            this.Padding = new System.Windows.Forms.Padding(20, 0, 20, 20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading...";
            this.Load += new System.EventHandler(this.FormWaiting_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbLoadingPercent;
        private System.Windows.Forms.ProgressBar progressBarLoading;
    }
}