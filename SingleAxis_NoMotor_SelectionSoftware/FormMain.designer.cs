﻿namespace SingleAxis_NoMotor_SelectionSoftware {
    partial class FormMain {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.splitContainerBase = new System.Windows.Forms.SplitContainer();
            this.pictureBoxToyo = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmdExplorer = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.cmdNarrow = new System.Windows.Forms.PictureBox();
            this.cmdZoom = new System.Windows.Forms.PictureBox();
            this.cmdClose = new System.Windows.Forms.PictureBox();
            this.binaryExplorerBar = new Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBar();
            this.panelStep1 = new Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel();
            this.panelStep2 = new Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel();
            this.panelStep3 = new Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel();
            this.panelStep4 = new Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel();
            this.panelStep5 = new Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBase)).BeginInit();
            this.splitContainerBase.Panel1.SuspendLayout();
            this.splitContainerBase.Panel2.SuspendLayout();
            this.splitContainerBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToyo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdExplorer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdNarrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdClose)).BeginInit();
            this.binaryExplorerBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerBase
            // 
            this.splitContainerBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBase.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerBase.IsSplitterFixed = true;
            this.splitContainerBase.Location = new System.Drawing.Point(0, 0);
            this.splitContainerBase.Name = "splitContainerBase";
            this.splitContainerBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerBase.Panel1
            // 
            this.splitContainerBase.Panel1.BackColor = System.Drawing.Color.Black;
            this.splitContainerBase.Panel1.Controls.Add(this.pictureBoxToyo);
            this.splitContainerBase.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainerBase.Panel1.Controls.Add(this.cmdExplorer);
            this.splitContainerBase.Panel1.Controls.Add(this.pictureBox3);
            this.splitContainerBase.Panel1.Controls.Add(this.cmdNarrow);
            this.splitContainerBase.Panel1.Controls.Add(this.cmdZoom);
            this.splitContainerBase.Panel1.Controls.Add(this.cmdClose);
            // 
            // splitContainerBase.Panel2
            // 
            this.splitContainerBase.Panel2.AutoScroll = true;
            this.splitContainerBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainerBase.Panel2.Controls.Add(this.binaryExplorerBar);
            this.splitContainerBase.Size = new System.Drawing.Size(1103, 635);
            this.splitContainerBase.SplitterDistance = 35;
            this.splitContainerBase.SplitterWidth = 1;
            this.splitContainerBase.TabIndex = 0;
            // 
            // pictureBoxToyo
            // 
            this.pictureBoxToyo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxToyo.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.component;
            this.pictureBoxToyo.Location = new System.Drawing.Point(12, 0);
            this.pictureBoxToyo.Name = "pictureBoxToyo";
            this.pictureBoxToyo.Size = new System.Drawing.Size(67, 35);
            this.pictureBoxToyo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxToyo.TabIndex = 7;
            this.pictureBoxToyo.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(12, 35);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // cmdExplorer
            // 
            this.cmdExplorer.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdExplorer.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_web2;
            this.cmdExplorer.Location = new System.Drawing.Point(960, 0);
            this.cmdExplorer.Name = "cmdExplorer";
            this.cmdExplorer.Size = new System.Drawing.Size(19, 35);
            this.cmdExplorer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cmdExplorer.TabIndex = 5;
            this.cmdExplorer.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Black;
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox3.Location = new System.Drawing.Point(979, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(7, 35);
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // cmdNarrow
            // 
            this.cmdNarrow.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdNarrow.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_Narrow;
            this.cmdNarrow.Location = new System.Drawing.Point(986, 0);
            this.cmdNarrow.Name = "cmdNarrow";
            this.cmdNarrow.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.cmdNarrow.Size = new System.Drawing.Size(34, 35);
            this.cmdNarrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.cmdNarrow.TabIndex = 3;
            this.cmdNarrow.TabStop = false;
            // 
            // cmdZoom
            // 
            this.cmdZoom.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdZoom.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_Zoom;
            this.cmdZoom.Location = new System.Drawing.Point(1020, 0);
            this.cmdZoom.Name = "cmdZoom";
            this.cmdZoom.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.cmdZoom.Size = new System.Drawing.Size(34, 35);
            this.cmdZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.cmdZoom.TabIndex = 2;
            this.cmdZoom.TabStop = false;
            // 
            // cmdClose
            // 
            this.cmdClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdClose.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_Close;
            this.cmdClose.Location = new System.Drawing.Point(1054, 0);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.cmdClose.Size = new System.Drawing.Size(49, 35);
            this.cmdClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.cmdClose.TabIndex = 1;
            this.cmdClose.TabStop = false;
            // 
            // binaryExplorerBar
            // 
            this.binaryExplorerBar.AutoScroll = true;
            this.binaryExplorerBar.BackColor = System.Drawing.Color.White;
            this.binaryExplorerBar.BackgroundStyle = Binarymission.WinForms.Controls.NavigationControls.ControlBackgroundPaintingStyle.GradientColorStyle;
            this.binaryExplorerBar.Controls.Add(this.panelStep1);
            this.binaryExplorerBar.Controls.Add(this.panelStep2);
            this.binaryExplorerBar.Controls.Add(this.panelStep3);
            this.binaryExplorerBar.Controls.Add(this.panelStep4);
            this.binaryExplorerBar.Controls.Add(this.panelStep5);
            this.binaryExplorerBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.binaryExplorerBar.DefaultPanelTitleBackColor = System.Drawing.Color.RoyalBlue;
            this.binaryExplorerBar.DefaultPanelTitleBackgroundImageTransparentColor = System.Drawing.Color.Empty;
            this.binaryExplorerBar.DefaultPanelTitleFont = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.binaryExplorerBar.DefaultPanelTitleForeColor = System.Drawing.Color.Green;
            this.binaryExplorerBar.DefaultPanelTitleGradientBeginColor = System.Drawing.Color.White;
            this.binaryExplorerBar.DefaultPanelTitleGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(210)))), ((int)(((byte)(134)))));
            this.binaryExplorerBar.DefaultPanelTitleHoverColor = System.Drawing.Color.Empty;
            this.binaryExplorerBar.DefaultPanelTopMargin = 10;
            this.binaryExplorerBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binaryExplorerBar.GradientBeginColor = System.Drawing.Color.White;
            this.binaryExplorerBar.GradientEndColor = System.Drawing.Color.White;
            this.binaryExplorerBar.Location = new System.Drawing.Point(0, 0);
            this.binaryExplorerBar.Name = "binaryExplorerBar";
            this.binaryExplorerBar.OnHoverUpDownArrowColor = System.Drawing.Color.White;
            this.binaryExplorerBar.OnHoverUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.binaryExplorerBar.OnNormalUpDownArrowColor = System.Drawing.Color.White;
            this.binaryExplorerBar.OnNormalUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.binaryExplorerBar.Panels.AddRange(new Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel[] {
            this.panelStep1,
            this.panelStep2,
            this.panelStep3,
            this.panelStep4,
            this.panelStep5});
            this.binaryExplorerBar.ReflectCurrentXPTheme = false;
            this.binaryExplorerBar.Size = new System.Drawing.Size(1103, 599);
            this.binaryExplorerBar.TabIndex = 0;
            this.binaryExplorerBar.ThemeOrCustomColors = Binarymission.WinForms.Controls.NavigationControls.ThemeOrCustomColors.CustomColors;
            this.binaryExplorerBar.XPTheme = Binarymission.WinForms.Controls.NavigationControls.XPTheme.None;
            // 
            // panelStep1
            // 
            this.panelStep1.AutoScroll = true;
            this.panelStep1.BackColor = System.Drawing.Color.White;
            this.panelStep1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep1.CurrentExplorerBarPanelState = Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanelState.Collapsed;
            this.panelStep1.CurrentHeight = 200;
            this.panelStep1.DrawPanelTitleBackgroundImageWithTheDefaultHeight = false;
            this.panelStep1.DrawPanelTitleText = true;
            this.panelStep1.Location = new System.Drawing.Point(12, 51);
            this.panelStep1.Name = "panelStep1";
            this.panelStep1.OnHoverUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep1.OnHoverUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep1.OnNormalUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep1.OnNormalUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep1.PanelTitleBackColor = System.Drawing.Color.White;
            this.panelStep1.PanelTitleBackgroundImage = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.title_bg01;
            this.panelStep1.PanelTitleBackgroundImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep1.PanelTitleBackStyle = Binarymission.WinForms.Controls.NavigationControls.ControlBackgroundPaintingStyle.ImageStyle;
            this.panelStep1.PanelTitleFont = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panelStep1.PanelTitleForeColor = System.Drawing.Color.White;
            this.panelStep1.PanelTitleGradientBeginColor = System.Drawing.Color.White;
            this.panelStep1.PanelTitleGradientEndColor = System.Drawing.Color.White;
            this.panelStep1.PanelTitleHoverColor = System.Drawing.Color.Empty;
            this.panelStep1.PanelTitleImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep1.PanelTitleText = "    Step.1";
            this.panelStep1.ReflectCurrentXPTheme = false;
            this.panelStep1.Size = new System.Drawing.Size(1078, 0);
            this.panelStep1.TabIndex = 0;
            // 
            // panelStep2
            // 
            this.panelStep2.AutoScroll = true;
            this.panelStep2.BackColor = System.Drawing.Color.White;
            this.panelStep2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep2.CurrentExplorerBarPanelState = Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanelState.Collapsed;
            this.panelStep2.DrawPanelTitleBackgroundImageWithTheDefaultHeight = false;
            this.panelStep2.DrawPanelTitleText = true;
            this.panelStep2.Location = new System.Drawing.Point(12, 104);
            this.panelStep2.Name = "panelStep2";
            this.panelStep2.OnHoverUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep2.OnHoverUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep2.OnNormalUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep2.OnNormalUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep2.PanelTitleBackColor = System.Drawing.Color.White;
            this.panelStep2.PanelTitleBackgroundImage = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.title_bg01;
            this.panelStep2.PanelTitleBackgroundImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep2.PanelTitleBackStyle = Binarymission.WinForms.Controls.NavigationControls.ControlBackgroundPaintingStyle.ImageStyle;
            this.panelStep2.PanelTitleFont = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panelStep2.PanelTitleForeColor = System.Drawing.Color.White;
            this.panelStep2.PanelTitleGradientBeginColor = System.Drawing.Color.White;
            this.panelStep2.PanelTitleGradientEndColor = System.Drawing.Color.White;
            this.panelStep2.PanelTitleHoverColor = System.Drawing.Color.Empty;
            this.panelStep2.PanelTitleImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep2.PanelTitleText = "    Step.2";
            this.panelStep2.ReflectCurrentXPTheme = false;
            this.panelStep2.Size = new System.Drawing.Size(1078, 0);
            this.panelStep2.TabIndex = 1;
            // 
            // panelStep3
            // 
            this.panelStep3.AutoScroll = true;
            this.panelStep3.BackColor = System.Drawing.Color.White;
            this.panelStep3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep3.CurrentExplorerBarPanelState = Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanelState.Collapsed;
            this.panelStep3.DrawPanelTitleBackgroundImageWithTheDefaultHeight = false;
            this.panelStep3.DrawPanelTitleText = true;
            this.panelStep3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelStep3.Location = new System.Drawing.Point(12, 157);
            this.panelStep3.Name = "panelStep3";
            this.panelStep3.OnHoverUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep3.OnHoverUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep3.OnNormalUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep3.OnNormalUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep3.PanelTitleBackColor = System.Drawing.Color.White;
            this.panelStep3.PanelTitleBackgroundImage = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.title_bg01;
            this.panelStep3.PanelTitleBackgroundImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep3.PanelTitleBackStyle = Binarymission.WinForms.Controls.NavigationControls.ControlBackgroundPaintingStyle.ImageStyle;
            this.panelStep3.PanelTitleFont = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panelStep3.PanelTitleForeColor = System.Drawing.Color.White;
            this.panelStep3.PanelTitleGradientBeginColor = System.Drawing.Color.White;
            this.panelStep3.PanelTitleGradientEndColor = System.Drawing.Color.White;
            this.panelStep3.PanelTitleHoverColor = System.Drawing.Color.Empty;
            this.panelStep3.PanelTitleImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep3.PanelTitleText = "    Step3";
            this.panelStep3.ReflectCurrentXPTheme = false;
            this.panelStep3.Size = new System.Drawing.Size(1078, 0);
            this.panelStep3.TabIndex = 3;
            // 
            // panelStep4
            // 
            this.panelStep4.AutoScroll = true;
            this.panelStep4.BackColor = System.Drawing.Color.White;
            this.panelStep4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep4.CurrentExplorerBarPanelState = Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanelState.Collapsed;
            this.panelStep4.DrawPanelTitleBackgroundImageWithTheDefaultHeight = false;
            this.panelStep4.DrawPanelTitleText = true;
            this.panelStep4.Location = new System.Drawing.Point(12, 210);
            this.panelStep4.Name = "panelStep4";
            this.panelStep4.OnHoverUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep4.OnHoverUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep4.OnNormalUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep4.OnNormalUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep4.PanelTitleBackColor = System.Drawing.Color.White;
            this.panelStep4.PanelTitleBackgroundImage = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.title_bg01;
            this.panelStep4.PanelTitleBackgroundImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep4.PanelTitleBackStyle = Binarymission.WinForms.Controls.NavigationControls.ControlBackgroundPaintingStyle.ImageStyle;
            this.panelStep4.PanelTitleFont = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panelStep4.PanelTitleForeColor = System.Drawing.Color.White;
            this.panelStep4.PanelTitleGradientBeginColor = System.Drawing.Color.White;
            this.panelStep4.PanelTitleGradientEndColor = System.Drawing.Color.White;
            this.panelStep4.PanelTitleHoverColor = System.Drawing.Color.Empty;
            this.panelStep4.PanelTitleImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep4.PanelTitleText = "    Step4";
            this.panelStep4.ReflectCurrentXPTheme = false;
            this.panelStep4.Size = new System.Drawing.Size(1078, 0);
            this.panelStep4.TabIndex = 5;
            // 
            // panelStep5
            // 
            this.panelStep5.AutoScroll = true;
            this.panelStep5.BackColor = System.Drawing.Color.White;
            this.panelStep5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep5.CurrentExplorerBarPanelState = Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanelState.Collapsed;
            this.panelStep5.DrawPanelTitleBackgroundImageWithTheDefaultHeight = false;
            this.panelStep5.DrawPanelTitleText = true;
            this.panelStep5.Location = new System.Drawing.Point(12, 263);
            this.panelStep5.Name = "panelStep5";
            this.panelStep5.OnHoverUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep5.OnHoverUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep5.OnNormalUpDownArrowColor = System.Drawing.Color.White;
            this.panelStep5.OnNormalUpDownArrowEllipseFillColor = System.Drawing.Color.White;
            this.panelStep5.PanelTitleBackColor = System.Drawing.Color.White;
            this.panelStep5.PanelTitleBackgroundImage = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.title_bg01;
            this.panelStep5.PanelTitleBackgroundImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep5.PanelTitleBackStyle = Binarymission.WinForms.Controls.NavigationControls.ControlBackgroundPaintingStyle.ImageStyle;
            this.panelStep5.PanelTitleFont = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panelStep5.PanelTitleForeColor = System.Drawing.Color.White;
            this.panelStep5.PanelTitleGradientBeginColor = System.Drawing.Color.White;
            this.panelStep5.PanelTitleGradientEndColor = System.Drawing.Color.White;
            this.panelStep5.PanelTitleHoverColor = System.Drawing.Color.Empty;
            this.panelStep5.PanelTitleImageTransparentColor = System.Drawing.Color.Empty;
            this.panelStep5.PanelTitleText = "    Step5";
            this.panelStep5.ReflectCurrentXPTheme = false;
            this.panelStep5.Size = new System.Drawing.Size(1078, 0);
            this.panelStep5.TabIndex = 7;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 635);
            this.Controls.Add(this.splitContainerBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.splitContainerBase.Panel1.ResumeLayout(false);
            this.splitContainerBase.Panel1.PerformLayout();
            this.splitContainerBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBase)).EndInit();
            this.splitContainerBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToyo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdExplorer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdNarrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdClose)).EndInit();
            this.binaryExplorerBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerBase;
        private System.Windows.Forms.PictureBox pictureBoxToyo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox cmdExplorer;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox cmdNarrow;
        private System.Windows.Forms.PictureBox cmdZoom;
        private System.Windows.Forms.PictureBox cmdClose;
        private Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBar binaryExplorerBar;
        private Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel panelStep1;
        private Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel panelStep2;
        private Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel panelStep3;
        private Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel panelStep4;
        private Binarymission.WinForms.Controls.NavigationControls.BinaryExplorerBarPanel panelStep5;
        private System.Windows.Forms.Label label1;
    }
}

