namespace SingleAxis_NoMotor_SelectionSoftware {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelBase = new System.Windows.Forms.Panel();
            this.splitContainerBase = new System.Windows.Forms.SplitContainer();
            this.lbTitle = new System.Windows.Forms.Label();
            this.pictureBoxToyo = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmdExplorer = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.cmdNarrow = new System.Windows.Forms.PictureBox();
            this.cmdZoom = new System.Windows.Forms.PictureBox();
            this.cmdClose = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabMain = new SingleAxis_NoMotor_SelectionSoftware.CustomTabControl();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label47 = new System.Windows.Forms.Label();
            this.cmdShapeSelection = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label48 = new System.Windows.Forms.Label();
            this.cmdModelSelection = new System.Windows.Forms.PictureBox();
            this.tabContent = new System.Windows.Forms.TabPage();
            this.explorerBar = new System.Windows.Forms.Panel();
            this.panelNextPage = new System.Windows.Forms.Panel();
            this.panelConfirmBtnsStep2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmdConfirmStep2 = new CustomButton.CustomButton();
            this.lbPrePage = new System.Windows.Forms.Label();
            this.panelCalcResult = new System.Windows.Forms.Panel();
            this.dgvRecommandList = new System.Windows.Forms.DataGridView();
            this.鎖定 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.項次 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.重複定位精度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.導程 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.荷重 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.最高轉速 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.運行速度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.加速度 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.最大行程 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.運行時間 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.力矩A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.力矩B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.力矩C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.力矩警示 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.馬達瓦數 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.皮帶馬達安全係數 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.T_max安全係數 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.皮帶T_max安全係數 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.T_Rms安全係數 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.運行距離 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.運行壽命 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否推薦 = new System.Windows.Forms.DataGridViewImageColumn();
            this.更詳細資訊 = new System.Windows.Forms.DataGridViewImageColumn();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panelChart = new System.Windows.Forms.Panel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lbAccelSpeed = new System.Windows.Forms.Label();
            this.lbAccelTime = new System.Windows.Forms.Label();
            this.lbCycleTime = new System.Windows.Forms.Label();
            this.lbConstantTime = new System.Windows.Forms.Label();
            this.lbMaxSpeed = new System.Windows.Forms.Label();
            this.lbRunTime = new System.Windows.Forms.Label();
            this.panelCalc = new System.Windows.Forms.Panel();
            this.scrollBarPanelLoad = new System.Windows.Forms.Panel();
            this.scrollBarThumbLoad = new System.Windows.Forms.PictureBox();
            this.scrollBarPanelStroke = new System.Windows.Forms.Panel();
            this.scrollBarThumbStroke = new System.Windows.Forms.PictureBox();
            this.lbTitleCalc = new System.Windows.Forms.Label();
            this.cmdCalc = new CustomButton.CustomButton();
            this.panelReducer = new System.Windows.Forms.Panel();
            this.label55 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.dgvReducerInfo = new System.Windows.Forms.DataGridView();
            this.columnModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnReducerRatio = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panelAdvanceParams = new System.Windows.Forms.Panel();
            this.lbMaxSpeedAlarm = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.txtAccelSpeed = new System.Windows.Forms.TextBox();
            this.optMaxSpeedType_rpm = new System.Windows.Forms.RadioButton();
            this.optMaxSpeedType_mms = new System.Windows.Forms.RadioButton();
            this.label79 = new System.Windows.Forms.Label();
            this.txtMaxSpeed = new System.Windows.Forms.TextBox();
            this.lbRpm = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panelExpectServiceLifeTime = new System.Windows.Forms.Panel();
            this.label72 = new System.Windows.Forms.Label();
            this.lbExpectServiceLifeAlarm = new System.Windows.Forms.Label();
            this.txtExpectServiceLifeTime = new System.Windows.Forms.TextBox();
            this.optExpectServiceLife = new System.Windows.Forms.RadioButton();
            this.optNoExpectServiceLife = new System.Windows.Forms.RadioButton();
            this.panelAdvanceMode = new System.Windows.Forms.Panel();
            this.chkAdvanceMode = new CustomToggle.CustomToggle();
            this.labelAdvanceOption = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.panelPowerModifyMode = new System.Windows.Forms.Panel();
            this.optMotorParamsModifySimple = new System.Windows.Forms.RadioButton();
            this.optMotorParamsModifyAdvance = new System.Windows.Forms.RadioButton();
            this.label34 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.lbDaysPerYearAlarm = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.lbTimesPerMinuteAlarm = new System.Windows.Forms.Label();
            this.panelMotorParams = new System.Windows.Forms.Panel();
            this.label35 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.txtRotateInertia = new System.Windows.Forms.TextBox();
            this.txtMaxTorque = new System.Windows.Forms.TextBox();
            this.txtRatedTorque = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.lbHoursPerDayAlarm = new System.Windows.Forms.Label();
            this.txtTimesPerMinute = new System.Windows.Forms.TextBox();
            this.txtDayPerYear = new System.Windows.Forms.TextBox();
            this.panelPowerSelection = new System.Windows.Forms.Panel();
            this.label60 = new System.Windows.Forms.Label();
            this.cboMotorParamsMotorPowerSelection = new System.Windows.Forms.ComboBox();
            this.label59 = new System.Windows.Forms.Label();
            this.txtHourPerDay = new System.Windows.Forms.TextBox();
            this.txtLoad = new System.Windows.Forms.TextBox();
            this.cboPower = new System.Windows.Forms.ComboBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.txtStroke = new System.Windows.Forms.TextBox();
            this.labelStopTimeAlarm = new System.Windows.Forms.Label();
            this.labelStrokeAlarm = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtRunTime = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.labelLoadAlarm = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panelMoment = new System.Windows.Forms.Panel();
            this.pictureBox19 = new System.Windows.Forms.PictureBox();
            this.lbTitleMoment = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtMomentB = new System.Windows.Forms.TextBox();
            this.txtMomentC = new System.Windows.Forms.TextBox();
            this.txtMomentA = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.panelSetup = new System.Windows.Forms.Panel();
            this.panelSetupMode = new System.Windows.Forms.Panel();
            this.optVerticalUse = new System.Windows.Forms.RadioButton();
            this.optHorizontalUse = new System.Windows.Forms.RadioButton();
            this.optWallHangingUse = new System.Windows.Forms.RadioButton();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.lbTitleSetup = new System.Windows.Forms.Label();
            this.panelModelSelection = new System.Windows.Forms.Panel();
            this.lbTitleModelSelection = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.cboSeries = new System.Windows.Forms.ComboBox();
            this.label74 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.cboModel = new System.Windows.Forms.ComboBox();
            this.cboLead = new System.Windows.Forms.ComboBox();
            this.panelModelType = new System.Windows.Forms.Panel();
            this.lbTitleModelType = new System.Windows.Forms.Label();
            this.optBuildInSupportTrackActuator = new System.Windows.Forms.RadioButton();
            this.picBuildInSupportTrackActuator = new System.Windows.Forms.PictureBox();
            this.optBuildOutRodTypeActuator = new System.Windows.Forms.RadioButton();
            this.picBuildOutRodTypeActuator = new System.Windows.Forms.PictureBox();
            this.optBuildInBeltActuator = new System.Windows.Forms.RadioButton();
            this.picBuildInBeltActuator = new System.Windows.Forms.PictureBox();
            this.optEuropeBeltActuator = new System.Windows.Forms.RadioButton();
            this.picEuropeBeltActuator = new System.Windows.Forms.PictureBox();
            this.optStandardBeltActuator = new System.Windows.Forms.RadioButton();
            this.picStandardBeltActuator = new System.Windows.Forms.PictureBox();
            this.optSupportTrackRodTypeActuator = new System.Windows.Forms.RadioButton();
            this.picSupportTrackRodTypeActuator = new System.Windows.Forms.PictureBox();
            this.optNoTrackRodTypeActuator = new System.Windows.Forms.RadioButton();
            this.picNoTrackRodTypeActuator = new System.Windows.Forms.PictureBox();
            this.optBuildInRodTypeScrewActuator = new System.Windows.Forms.RadioButton();
            this.picBuildInRodTypeScrewActuator = new System.Windows.Forms.PictureBox();
            this.optBuildInScrewActuator = new System.Windows.Forms.RadioButton();
            this.picBuildInScrewActuator = new System.Windows.Forms.PictureBox();
            this.optStandardScrewActuator = new System.Windows.Forms.RadioButton();
            this.picStandardScrewActuator = new System.Windows.Forms.PictureBox();
            this.panelUseEnv = new System.Windows.Forms.Panel();
            this.picDustFree = new System.Windows.Forms.PictureBox();
            this.optDustFreeEnv = new System.Windows.Forms.RadioButton();
            this.optStandardEnv = new System.Windows.Forms.RadioButton();
            this.picStandardEnv = new System.Windows.Forms.PictureBox();
            this.lbTitleUseEnv = new System.Windows.Forms.Label();
            this.panelSideTable = new SingleAxis_NoMotor_SelectionSoftware.CustomPanel();
            this.panelSideTableIcon = new System.Windows.Forms.Panel();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelSideTableSelections = new SingleAxis_NoMotor_SelectionSoftware.CustomPanel();
            this.tableSelections = new System.Windows.Forms.TableLayoutPanel();
            this.customPanel4 = new SingleAxis_NoMotor_SelectionSoftware.CustomPanel();
            this.lbSideTableModelInfo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.customPanel3 = new SingleAxis_NoMotor_SelectionSoftware.CustomPanel();
            this.lbSideTableMsg = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.customPanel2 = new SingleAxis_NoMotor_SelectionSoftware.CustomPanel();
            this.picModelImg = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.explorerBar_step5 = new System.Windows.Forms.Panel();
            this.panelResult = new System.Windows.Forms.Panel();
            this.label51 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.Label();
            this.picBoxResultImg = new System.Windows.Forms.PictureBox();
            this.panelConfirmBtnsStep5 = new System.Windows.Forms.TableLayoutPanel();
            this.cmdConfirmStep5 = new CustomButton.CustomButton();
            this.cmdResetStep5 = new CustomButton.CustomButton();
            this.label70 = new System.Windows.Forms.Label();
            this.panelBase.SuspendLayout();
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
            this.panel4.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmdShapeSelection)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmdModelSelection)).BeginInit();
            this.tabContent.SuspendLayout();
            this.explorerBar.SuspendLayout();
            this.panelNextPage.SuspendLayout();
            this.panelConfirmBtnsStep2.SuspendLayout();
            this.panelCalcResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecommandList)).BeginInit();
            this.panelChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.panelCalc.SuspendLayout();
            this.scrollBarPanelLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scrollBarThumbLoad)).BeginInit();
            this.scrollBarPanelStroke.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scrollBarThumbStroke)).BeginInit();
            this.panelReducer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReducerInfo)).BeginInit();
            this.panelAdvanceParams.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panelExpectServiceLifeTime.SuspendLayout();
            this.panelAdvanceMode.SuspendLayout();
            this.panelPowerModifyMode.SuspendLayout();
            this.panelMotorParams.SuspendLayout();
            this.panelPowerSelection.SuspendLayout();
            this.panelMoment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).BeginInit();
            this.panelSetup.SuspendLayout();
            this.panelSetupMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            this.panelModelSelection.SuspendLayout();
            this.panelModelType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInSupportTrackActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildOutRodTypeActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInBeltActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEuropeBeltActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStandardBeltActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSupportTrackRodTypeActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNoTrackRodTypeActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInRodTypeScrewActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInScrewActuator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStandardScrewActuator)).BeginInit();
            this.panelUseEnv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDustFree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStandardEnv)).BeginInit();
            this.panelSideTable.SuspendLayout();
            this.panelSideTableIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelSideTableSelections.SuspendLayout();
            this.customPanel4.SuspendLayout();
            this.customPanel3.SuspendLayout();
            this.customPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picModelImg)).BeginInit();
            this.tabResult.SuspendLayout();
            this.explorerBar_step5.SuspendLayout();
            this.panelResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxResultImg)).BeginInit();
            this.panelConfirmBtnsStep5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBase
            // 
            this.panelBase.Controls.Add(this.splitContainerBase);
            resources.ApplyResources(this.panelBase, "panelBase");
            this.panelBase.Name = "panelBase";
            // 
            // splitContainerBase
            // 
            this.splitContainerBase.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.splitContainerBase, "splitContainerBase");
            this.splitContainerBase.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerBase.Name = "splitContainerBase";
            // 
            // splitContainerBase.Panel1
            // 
            this.splitContainerBase.Panel1.BackColor = System.Drawing.Color.Black;
            this.splitContainerBase.Panel1.Controls.Add(this.lbTitle);
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
            this.splitContainerBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainerBase.Panel2.Controls.Add(this.panel4);
            // 
            // lbTitle
            // 
            this.lbTitle.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbTitle, "lbTitle");
            this.lbTitle.ForeColor = System.Drawing.Color.Silver;
            this.lbTitle.Name = "lbTitle";
            // 
            // pictureBoxToyo
            // 
            resources.ApplyResources(this.pictureBoxToyo, "pictureBoxToyo");
            this.pictureBoxToyo.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.component;
            this.pictureBoxToyo.Name = "pictureBoxToyo";
            this.pictureBoxToyo.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // cmdExplorer
            // 
            resources.ApplyResources(this.cmdExplorer, "cmdExplorer");
            this.cmdExplorer.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_web2;
            this.cmdExplorer.Name = "cmdExplorer";
            this.cmdExplorer.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // cmdNarrow
            // 
            resources.ApplyResources(this.cmdNarrow, "cmdNarrow");
            this.cmdNarrow.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_Narrow;
            this.cmdNarrow.Name = "cmdNarrow";
            this.cmdNarrow.TabStop = false;
            // 
            // cmdZoom
            // 
            resources.ApplyResources(this.cmdZoom, "cmdZoom");
            this.cmdZoom.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_Zoom;
            this.cmdZoom.Name = "cmdZoom";
            this.cmdZoom.TabStop = false;
            // 
            // cmdClose
            // 
            resources.ApplyResources(this.cmdClose, "cmdClose");
            this.cmdClose.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Top_Close;
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.tabMain);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabStart);
            this.tabMain.Controls.Add(this.tabContent);
            this.tabMain.Controls.Add(this.tabResult);
            resources.ApplyResources(this.tabMain, "tabMain");
            this.tabMain.IsBorderShow = true;
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            // 
            // tabStart
            // 
            this.tabStart.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.tabStart, "tabStart");
            this.tabStart.Name = "tabStart";
            this.tabStart.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label47);
            this.panel1.Controls.Add(this.cmdShapeSelection);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label47
            // 
            resources.ApplyResources(this.label47, "label47");
            this.label47.Name = "label47";
            // 
            // cmdShapeSelection
            // 
            this.cmdShapeSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.cmdShapeSelection, "cmdShapeSelection");
            this.cmdShapeSelection.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.型號選型;
            this.cmdShapeSelection.Name = "cmdShapeSelection";
            this.cmdShapeSelection.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label48);
            this.panel2.Controls.Add(this.cmdModelSelection);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // label48
            // 
            resources.ApplyResources(this.label48, "label48");
            this.label48.Name = "label48";
            // 
            // cmdModelSelection
            // 
            this.cmdModelSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.cmdModelSelection, "cmdModelSelection");
            this.cmdModelSelection.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.動作選型;
            this.cmdModelSelection.Name = "cmdModelSelection";
            this.cmdModelSelection.TabStop = false;
            // 
            // tabContent
            // 
            this.tabContent.BackColor = System.Drawing.Color.White;
            this.tabContent.Controls.Add(this.explorerBar);
            resources.ApplyResources(this.tabContent, "tabContent");
            this.tabContent.Name = "tabContent";
            // 
            // explorerBar
            // 
            resources.ApplyResources(this.explorerBar, "explorerBar");
            this.explorerBar.Controls.Add(this.panelNextPage);
            this.explorerBar.Controls.Add(this.lbPrePage);
            this.explorerBar.Controls.Add(this.panelCalcResult);
            this.explorerBar.Controls.Add(this.panelChart);
            this.explorerBar.Controls.Add(this.panelCalc);
            this.explorerBar.Controls.Add(this.panelMoment);
            this.explorerBar.Controls.Add(this.panelSetup);
            this.explorerBar.Controls.Add(this.panelModelSelection);
            this.explorerBar.Controls.Add(this.panelModelType);
            this.explorerBar.Controls.Add(this.panelUseEnv);
            this.explorerBar.Controls.Add(this.panelSideTable);
            this.explorerBar.Name = "explorerBar";
            // 
            // panelNextPage
            // 
            this.panelNextPage.Controls.Add(this.panelConfirmBtnsStep2);
            resources.ApplyResources(this.panelNextPage, "panelNextPage");
            this.panelNextPage.Name = "panelNextPage";
            // 
            // panelConfirmBtnsStep2
            // 
            resources.ApplyResources(this.panelConfirmBtnsStep2, "panelConfirmBtnsStep2");
            this.panelConfirmBtnsStep2.Controls.Add(this.cmdConfirmStep2, 1, 0);
            this.panelConfirmBtnsStep2.Name = "panelConfirmBtnsStep2";
            // 
            // cmdConfirmStep2
            // 
            this.cmdConfirmStep2.BackColor = System.Drawing.Color.Transparent;
            this.cmdConfirmStep2.BackColor_Hover = System.Drawing.Color.DarkRed;
            this.cmdConfirmStep2.BackColor_Normal = System.Drawing.Color.Red;
            this.cmdConfirmStep2.BackColor_Press = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdConfirmStep2.BackColor2_Hover = System.Drawing.Color.DarkRed;
            this.cmdConfirmStep2.BackColor2_Normal = System.Drawing.Color.Red;
            this.cmdConfirmStep2.BackColor2_Press = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdConfirmStep2.ButtonEnabled = true;
            resources.ApplyResources(this.cmdConfirmStep2, "cmdConfirmStep2");
            this.cmdConfirmStep2.Curvature = 15;
            this.cmdConfirmStep2.GradientMode = CustomButton.LinearGradientMode.Horizontal;
            this.cmdConfirmStep2.Name = "cmdConfirmStep2";
            this.cmdConfirmStep2.TextFont = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            // 
            // lbPrePage
            // 
            resources.ApplyResources(this.lbPrePage, "lbPrePage");
            this.lbPrePage.BackColor = System.Drawing.Color.White;
            this.lbPrePage.ForeColor = System.Drawing.Color.Red;
            this.lbPrePage.Name = "lbPrePage";
            // 
            // panelCalcResult
            // 
            this.panelCalcResult.Controls.Add(this.dgvRecommandList);
            this.panelCalcResult.Controls.Add(this.label12);
            this.panelCalcResult.Controls.Add(this.label13);
            resources.ApplyResources(this.panelCalcResult, "panelCalcResult");
            this.panelCalcResult.Name = "panelCalcResult";
            // 
            // dgvRecommandList
            // 
            this.dgvRecommandList.AllowUserToAddRows = false;
            this.dgvRecommandList.AllowUserToDeleteRows = false;
            this.dgvRecommandList.AllowUserToResizeColumns = false;
            this.dgvRecommandList.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvRecommandList, "dgvRecommandList");
            this.dgvRecommandList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecommandList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(58)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecommandList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecommandList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecommandList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.鎖定,
            this.項次,
            this.重複定位精度,
            this.導程,
            this.荷重,
            this.最高轉速,
            this.運行速度,
            this.加速度,
            this.最大行程,
            this.運行時間,
            this.力矩A,
            this.力矩B,
            this.力矩C,
            this.力矩警示,
            this.馬達瓦數,
            this.皮帶馬達安全係數,
            this.T_max安全係數,
            this.皮帶T_max安全係數,
            this.T_Rms安全係數,
            this.運行距離,
            this.運行壽命,
            this.是否推薦,
            this.更詳細資訊});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecommandList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecommandList.EnableHeadersVisualStyles = false;
            this.dgvRecommandList.MultiSelect = false;
            this.dgvRecommandList.Name = "dgvRecommandList";
            this.dgvRecommandList.ReadOnly = true;
            this.dgvRecommandList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecommandList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRecommandList.RowHeadersVisible = false;
            this.dgvRecommandList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.dgvRecommandList.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRecommandList.RowTemplate.Height = 24;
            this.dgvRecommandList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // 鎖定
            // 
            this.鎖定.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.鎖定.FillWeight = 56.40312F;
            this.鎖定.Frozen = true;
            resources.ApplyResources(this.鎖定, "鎖定");
            this.鎖定.Name = "鎖定";
            this.鎖定.ReadOnly = true;
            this.鎖定.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 項次
            // 
            this.項次.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.項次.FillWeight = 68.39409F;
            this.項次.Frozen = true;
            resources.ApplyResources(this.項次, "項次");
            this.項次.Name = "項次";
            this.項次.ReadOnly = true;
            this.項次.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 重複定位精度
            // 
            this.重複定位精度.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.重複定位精度, "重複定位精度");
            this.重複定位精度.Name = "重複定位精度";
            this.重複定位精度.ReadOnly = true;
            this.重複定位精度.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 導程
            // 
            this.導程.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.導程, "導程");
            this.導程.Name = "導程";
            this.導程.ReadOnly = true;
            this.導程.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 荷重
            // 
            this.荷重.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.荷重, "荷重");
            this.荷重.Name = "荷重";
            this.荷重.ReadOnly = true;
            this.荷重.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 最高轉速
            // 
            this.最高轉速.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.最高轉速, "最高轉速");
            this.最高轉速.Name = "最高轉速";
            this.最高轉速.ReadOnly = true;
            this.最高轉速.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 運行速度
            // 
            this.運行速度.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.運行速度, "運行速度");
            this.運行速度.Name = "運行速度";
            this.運行速度.ReadOnly = true;
            this.運行速度.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 加速度
            // 
            this.加速度.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.加速度, "加速度");
            this.加速度.Name = "加速度";
            this.加速度.ReadOnly = true;
            this.加速度.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 最大行程
            // 
            this.最大行程.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.最大行程, "最大行程");
            this.最大行程.Name = "最大行程";
            this.最大行程.ReadOnly = true;
            this.最大行程.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 運行時間
            // 
            this.運行時間.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.運行時間, "運行時間");
            this.運行時間.Name = "運行時間";
            this.運行時間.ReadOnly = true;
            this.運行時間.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 力矩A
            // 
            this.力矩A.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.力矩A, "力矩A");
            this.力矩A.Name = "力矩A";
            this.力矩A.ReadOnly = true;
            this.力矩A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 力矩B
            // 
            this.力矩B.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.力矩B, "力矩B");
            this.力矩B.Name = "力矩B";
            this.力矩B.ReadOnly = true;
            this.力矩B.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 力矩C
            // 
            this.力矩C.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.力矩C, "力矩C");
            this.力矩C.Name = "力矩C";
            this.力矩C.ReadOnly = true;
            this.力矩C.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 力矩警示
            // 
            this.力矩警示.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.力矩警示.FillWeight = 68.39409F;
            resources.ApplyResources(this.力矩警示, "力矩警示");
            this.力矩警示.Name = "力矩警示";
            this.力矩警示.ReadOnly = true;
            this.力矩警示.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 馬達瓦數
            // 
            this.馬達瓦數.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.馬達瓦數, "馬達瓦數");
            this.馬達瓦數.Name = "馬達瓦數";
            this.馬達瓦數.ReadOnly = true;
            this.馬達瓦數.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 皮帶馬達安全係數
            // 
            this.皮帶馬達安全係數.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.皮帶馬達安全係數, "皮帶馬達安全係數");
            this.皮帶馬達安全係數.Name = "皮帶馬達安全係數";
            this.皮帶馬達安全係數.ReadOnly = true;
            this.皮帶馬達安全係數.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // T_max安全係數
            // 
            this.T_max安全係數.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.T_max安全係數.FillWeight = 158.5393F;
            resources.ApplyResources(this.T_max安全係數, "T_max安全係數");
            this.T_max安全係數.Name = "T_max安全係數";
            this.T_max安全係數.ReadOnly = true;
            this.T_max安全係數.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 皮帶T_max安全係數
            // 
            this.皮帶T_max安全係數.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            resources.ApplyResources(this.皮帶T_max安全係數, "皮帶T_max安全係數");
            this.皮帶T_max安全係數.Name = "皮帶T_max安全係數";
            this.皮帶T_max安全係數.ReadOnly = true;
            this.皮帶T_max安全係數.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // T_Rms安全係數
            // 
            this.T_Rms安全係數.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.T_Rms安全係數.FillWeight = 167.8455F;
            resources.ApplyResources(this.T_Rms安全係數, "T_Rms安全係數");
            this.T_Rms安全係數.Name = "T_Rms安全係數";
            this.T_Rms安全係數.ReadOnly = true;
            this.T_Rms安全係數.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 運行距離
            // 
            this.運行距離.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.運行距離, "運行距離");
            this.運行距離.Name = "運行距離";
            this.運行距離.ReadOnly = true;
            this.運行距離.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 運行壽命
            // 
            this.運行壽命.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.運行壽命, "運行壽命");
            this.運行壽命.Name = "運行壽命";
            this.運行壽命.ReadOnly = true;
            this.運行壽命.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 是否推薦
            // 
            this.是否推薦.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.是否推薦, "是否推薦");
            this.是否推薦.Name = "是否推薦";
            this.是否推薦.ReadOnly = true;
            this.是否推薦.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 更詳細資訊
            // 
            this.更詳細資訊.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.更詳細資訊.FillWeight = 80F;
            resources.ApplyResources(this.更詳細資訊, "更詳細資訊");
            this.更詳細資訊.Name = "更詳細資訊";
            this.更詳細資訊.ReadOnly = true;
            this.更詳細資訊.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label13.Name = "label13";
            // 
            // panelChart
            // 
            this.panelChart.Controls.Add(this.chart);
            this.panelChart.Controls.Add(this.lbAccelSpeed);
            this.panelChart.Controls.Add(this.lbAccelTime);
            this.panelChart.Controls.Add(this.lbCycleTime);
            this.panelChart.Controls.Add(this.lbConstantTime);
            this.panelChart.Controls.Add(this.lbMaxSpeed);
            this.panelChart.Controls.Add(this.lbRunTime);
            resources.ApplyResources(this.panelChart, "panelChart");
            this.panelChart.Name = "panelChart";
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Title = "Time (s)";
            chartArea1.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated270;
            chartArea1.AxisY.Title = "V (mm/s)";
            chartArea1.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea";
            this.chart.ChartAreas.Add(chartArea1);
            resources.ApplyResources(this.chart, "chart");
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            this.chart.Series.Add(series1);
            // 
            // lbAccelSpeed
            // 
            resources.ApplyResources(this.lbAccelSpeed, "lbAccelSpeed");
            this.lbAccelSpeed.Name = "lbAccelSpeed";
            // 
            // lbAccelTime
            // 
            resources.ApplyResources(this.lbAccelTime, "lbAccelTime");
            this.lbAccelTime.Name = "lbAccelTime";
            // 
            // lbCycleTime
            // 
            resources.ApplyResources(this.lbCycleTime, "lbCycleTime");
            this.lbCycleTime.Name = "lbCycleTime";
            // 
            // lbConstantTime
            // 
            resources.ApplyResources(this.lbConstantTime, "lbConstantTime");
            this.lbConstantTime.Name = "lbConstantTime";
            // 
            // lbMaxSpeed
            // 
            resources.ApplyResources(this.lbMaxSpeed, "lbMaxSpeed");
            this.lbMaxSpeed.Name = "lbMaxSpeed";
            // 
            // lbRunTime
            // 
            resources.ApplyResources(this.lbRunTime, "lbRunTime");
            this.lbRunTime.Name = "lbRunTime";
            // 
            // panelCalc
            // 
            this.panelCalc.Controls.Add(this.scrollBarPanelLoad);
            this.panelCalc.Controls.Add(this.scrollBarPanelStroke);
            this.panelCalc.Controls.Add(this.lbTitleCalc);
            this.panelCalc.Controls.Add(this.cmdCalc);
            this.panelCalc.Controls.Add(this.panelReducer);
            this.panelCalc.Controls.Add(this.panelAdvanceParams);
            this.panelCalc.Controls.Add(this.panel6);
            this.panelCalc.Controls.Add(this.panelAdvanceMode);
            this.panelCalc.Controls.Add(this.label71);
            this.panelCalc.Controls.Add(this.label44);
            this.panelCalc.Controls.Add(this.panelPowerModifyMode);
            this.panelCalc.Controls.Add(this.label53);
            this.panelCalc.Controls.Add(this.label45);
            this.panelCalc.Controls.Add(this.lbDaysPerYearAlarm);
            this.panelCalc.Controls.Add(this.label52);
            this.panelCalc.Controls.Add(this.lbTimesPerMinuteAlarm);
            this.panelCalc.Controls.Add(this.panelMotorParams);
            this.panelCalc.Controls.Add(this.lbHoursPerDayAlarm);
            this.panelCalc.Controls.Add(this.txtTimesPerMinute);
            this.panelCalc.Controls.Add(this.txtDayPerYear);
            this.panelCalc.Controls.Add(this.panelPowerSelection);
            this.panelCalc.Controls.Add(this.txtHourPerDay);
            this.panelCalc.Controls.Add(this.txtLoad);
            this.panelCalc.Controls.Add(this.cboPower);
            this.panelCalc.Controls.Add(this.label46);
            this.panelCalc.Controls.Add(this.label6);
            this.panelCalc.Controls.Add(this.label7);
            this.panelCalc.Controls.Add(this.label50);
            this.panelCalc.Controls.Add(this.txtStroke);
            this.panelCalc.Controls.Add(this.labelStopTimeAlarm);
            this.panelCalc.Controls.Add(this.labelStrokeAlarm);
            this.panelCalc.Controls.Add(this.label22);
            this.panelCalc.Controls.Add(this.txtRunTime);
            this.panelCalc.Controls.Add(this.label23);
            this.panelCalc.Controls.Add(this.labelLoadAlarm);
            this.panelCalc.Controls.Add(this.label15);
            this.panelCalc.Controls.Add(this.label10);
            resources.ApplyResources(this.panelCalc, "panelCalc");
            this.panelCalc.Name = "panelCalc";
            // 
            // scrollBarPanelLoad
            // 
            this.scrollBarPanelLoad.BackgroundImage = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.scrollBar;
            resources.ApplyResources(this.scrollBarPanelLoad, "scrollBarPanelLoad");
            this.scrollBarPanelLoad.Controls.Add(this.scrollBarThumbLoad);
            this.scrollBarPanelLoad.Name = "scrollBarPanelLoad";
            // 
            // scrollBarThumbLoad
            // 
            this.scrollBarThumbLoad.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.scrollBarThumb;
            resources.ApplyResources(this.scrollBarThumbLoad, "scrollBarThumbLoad");
            this.scrollBarThumbLoad.Name = "scrollBarThumbLoad";
            this.scrollBarThumbLoad.TabStop = false;
            // 
            // scrollBarPanelStroke
            // 
            this.scrollBarPanelStroke.BackgroundImage = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.scrollBar;
            resources.ApplyResources(this.scrollBarPanelStroke, "scrollBarPanelStroke");
            this.scrollBarPanelStroke.Controls.Add(this.scrollBarThumbStroke);
            this.scrollBarPanelStroke.Name = "scrollBarPanelStroke";
            // 
            // scrollBarThumbStroke
            // 
            this.scrollBarThumbStroke.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.scrollBarThumb;
            resources.ApplyResources(this.scrollBarThumbStroke, "scrollBarThumbStroke");
            this.scrollBarThumbStroke.Name = "scrollBarThumbStroke";
            this.scrollBarThumbStroke.TabStop = false;
            // 
            // lbTitleCalc
            // 
            resources.ApplyResources(this.lbTitleCalc, "lbTitleCalc");
            this.lbTitleCalc.BackColor = System.Drawing.Color.White;
            this.lbTitleCalc.ForeColor = System.Drawing.Color.DimGray;
            this.lbTitleCalc.Name = "lbTitleCalc";
            // 
            // cmdCalc
            // 
            this.cmdCalc.BackColor = System.Drawing.Color.Transparent;
            this.cmdCalc.BackColor_Hover = System.Drawing.Color.DarkRed;
            this.cmdCalc.BackColor_Normal = System.Drawing.Color.Red;
            this.cmdCalc.BackColor_Press = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdCalc.BackColor2_Hover = System.Drawing.Color.DarkRed;
            this.cmdCalc.BackColor2_Normal = System.Drawing.Color.Red;
            this.cmdCalc.BackColor2_Press = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdCalc.ButtonEnabled = true;
            resources.ApplyResources(this.cmdCalc, "cmdCalc");
            this.cmdCalc.Curvature = 15;
            this.cmdCalc.GradientMode = CustomButton.LinearGradientMode.Horizontal;
            this.cmdCalc.Name = "cmdCalc";
            this.cmdCalc.TextFont = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            // 
            // panelReducer
            // 
            this.panelReducer.Controls.Add(this.label55);
            this.panelReducer.Controls.Add(this.label58);
            this.panelReducer.Controls.Add(this.dgvReducerInfo);
            resources.ApplyResources(this.panelReducer, "panelReducer");
            this.panelReducer.Name = "panelReducer";
            // 
            // label55
            // 
            resources.ApplyResources(this.label55, "label55");
            this.label55.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label55.Name = "label55";
            // 
            // label58
            // 
            this.label58.BackColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.label58, "label58");
            this.label58.Name = "label58";
            // 
            // dgvReducerInfo
            // 
            this.dgvReducerInfo.AllowUserToAddRows = false;
            this.dgvReducerInfo.AllowUserToDeleteRows = false;
            this.dgvReducerInfo.AllowUserToResizeColumns = false;
            this.dgvReducerInfo.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvReducerInfo, "dgvReducerInfo");
            this.dgvReducerInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReducerInfo.BackgroundColor = System.Drawing.Color.White;
            this.dgvReducerInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(58)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReducerInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvReducerInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReducerInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnModel,
            this.columnReducerRatio});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReducerInfo.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvReducerInfo.EnableHeadersVisualStyles = false;
            this.dgvReducerInfo.Name = "dgvReducerInfo";
            this.dgvReducerInfo.RowHeadersVisible = false;
            this.dgvReducerInfo.RowTemplate.Height = 24;
            // 
            // columnModel
            // 
            resources.ApplyResources(this.columnModel, "columnModel");
            this.columnModel.Name = "columnModel";
            this.columnModel.ReadOnly = true;
            this.columnModel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnReducerRatio
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(58)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(58)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.columnReducerRatio.DefaultCellStyle = dataGridViewCellStyle6;
            this.columnReducerRatio.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            resources.ApplyResources(this.columnReducerRatio, "columnReducerRatio");
            this.columnReducerRatio.Name = "columnReducerRatio";
            // 
            // panelAdvanceParams
            // 
            this.panelAdvanceParams.Controls.Add(this.lbMaxSpeedAlarm);
            this.panelAdvanceParams.Controls.Add(this.label78);
            this.panelAdvanceParams.Controls.Add(this.label81);
            this.panelAdvanceParams.Controls.Add(this.label82);
            this.panelAdvanceParams.Controls.Add(this.txtAccelSpeed);
            this.panelAdvanceParams.Controls.Add(this.optMaxSpeedType_rpm);
            this.panelAdvanceParams.Controls.Add(this.optMaxSpeedType_mms);
            this.panelAdvanceParams.Controls.Add(this.label79);
            this.panelAdvanceParams.Controls.Add(this.txtMaxSpeed);
            this.panelAdvanceParams.Controls.Add(this.lbRpm);
            resources.ApplyResources(this.panelAdvanceParams, "panelAdvanceParams");
            this.panelAdvanceParams.Name = "panelAdvanceParams";
            // 
            // lbMaxSpeedAlarm
            // 
            resources.ApplyResources(this.lbMaxSpeedAlarm, "lbMaxSpeedAlarm");
            this.lbMaxSpeedAlarm.ForeColor = System.Drawing.Color.Red;
            this.lbMaxSpeedAlarm.Name = "lbMaxSpeedAlarm";
            this.lbMaxSpeedAlarm.Tag = "txtMaxSpeed";
            // 
            // label78
            // 
            resources.ApplyResources(this.label78, "label78");
            this.label78.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label78.Name = "label78";
            // 
            // label81
            // 
            resources.ApplyResources(this.label81, "label81");
            this.label81.Name = "label81";
            // 
            // label82
            // 
            resources.ApplyResources(this.label82, "label82");
            this.label82.ForeColor = System.Drawing.Color.Red;
            this.label82.Name = "label82";
            this.label82.Tag = "txtAccelSpeed";
            // 
            // txtAccelSpeed
            // 
            resources.ApplyResources(this.txtAccelSpeed, "txtAccelSpeed");
            this.txtAccelSpeed.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtAccelSpeed.Name = "txtAccelSpeed";
            // 
            // optMaxSpeedType_rpm
            // 
            resources.ApplyResources(this.optMaxSpeedType_rpm, "optMaxSpeedType_rpm");
            this.optMaxSpeedType_rpm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optMaxSpeedType_rpm.Name = "optMaxSpeedType_rpm";
            this.optMaxSpeedType_rpm.UseVisualStyleBackColor = true;
            // 
            // optMaxSpeedType_mms
            // 
            resources.ApplyResources(this.optMaxSpeedType_mms, "optMaxSpeedType_mms");
            this.optMaxSpeedType_mms.Checked = true;
            this.optMaxSpeedType_mms.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optMaxSpeedType_mms.Name = "optMaxSpeedType_mms";
            this.optMaxSpeedType_mms.TabStop = true;
            this.optMaxSpeedType_mms.UseVisualStyleBackColor = true;
            // 
            // label79
            // 
            resources.ApplyResources(this.label79, "label79");
            this.label79.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label79.Name = "label79";
            // 
            // txtMaxSpeed
            // 
            resources.ApplyResources(this.txtMaxSpeed, "txtMaxSpeed");
            this.txtMaxSpeed.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtMaxSpeed.Name = "txtMaxSpeed";
            // 
            // lbRpm
            // 
            resources.ApplyResources(this.lbRpm, "lbRpm");
            this.lbRpm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.lbRpm.Name = "lbRpm";
            this.lbRpm.Tag = "txtMaxSpeed";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panelExpectServiceLifeTime);
            this.panel6.Controls.Add(this.optExpectServiceLife);
            this.panel6.Controls.Add(this.optNoExpectServiceLife);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // panelExpectServiceLifeTime
            // 
            this.panelExpectServiceLifeTime.Controls.Add(this.label72);
            this.panelExpectServiceLifeTime.Controls.Add(this.lbExpectServiceLifeAlarm);
            this.panelExpectServiceLifeTime.Controls.Add(this.txtExpectServiceLifeTime);
            resources.ApplyResources(this.panelExpectServiceLifeTime, "panelExpectServiceLifeTime");
            this.panelExpectServiceLifeTime.Name = "panelExpectServiceLifeTime";
            // 
            // label72
            // 
            resources.ApplyResources(this.label72, "label72");
            this.label72.Name = "label72";
            // 
            // lbExpectServiceLifeAlarm
            // 
            resources.ApplyResources(this.lbExpectServiceLifeAlarm, "lbExpectServiceLifeAlarm");
            this.lbExpectServiceLifeAlarm.ForeColor = System.Drawing.Color.Red;
            this.lbExpectServiceLifeAlarm.Name = "lbExpectServiceLifeAlarm";
            this.lbExpectServiceLifeAlarm.Tag = "txtExpectServiceLifeTime";
            // 
            // txtExpectServiceLifeTime
            // 
            resources.ApplyResources(this.txtExpectServiceLifeTime, "txtExpectServiceLifeTime");
            this.txtExpectServiceLifeTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtExpectServiceLifeTime.Name = "txtExpectServiceLifeTime";
            // 
            // optExpectServiceLife
            // 
            resources.ApplyResources(this.optExpectServiceLife, "optExpectServiceLife");
            this.optExpectServiceLife.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.optExpectServiceLife.Name = "optExpectServiceLife";
            this.optExpectServiceLife.UseVisualStyleBackColor = true;
            // 
            // optNoExpectServiceLife
            // 
            resources.ApplyResources(this.optNoExpectServiceLife, "optNoExpectServiceLife");
            this.optNoExpectServiceLife.Checked = true;
            this.optNoExpectServiceLife.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.optNoExpectServiceLife.Name = "optNoExpectServiceLife";
            this.optNoExpectServiceLife.TabStop = true;
            this.optNoExpectServiceLife.UseVisualStyleBackColor = true;
            // 
            // panelAdvanceMode
            // 
            this.panelAdvanceMode.Controls.Add(this.chkAdvanceMode);
            this.panelAdvanceMode.Controls.Add(this.labelAdvanceOption);
            resources.ApplyResources(this.panelAdvanceMode, "panelAdvanceMode");
            this.panelAdvanceMode.Name = "panelAdvanceMode";
            // 
            // chkAdvanceMode
            // 
            this.chkAdvanceMode.BackImg_ToggleOff_Disabled = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.toggleOff_disable;
            this.chkAdvanceMode.BackImg_ToggleOff_Hover = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.toggleOff_hover;
            this.chkAdvanceMode.BackImg_ToggleOff_Normal = ((System.Drawing.Image)(resources.GetObject("chkAdvanceMode.BackImg_ToggleOff_Normal")));
            this.chkAdvanceMode.BackImg_ToggleOn_Disabled = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.toggleOn_disable;
            this.chkAdvanceMode.BackImg_ToggleOn_Hover = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.toggleOn_hover;
            this.chkAdvanceMode.BackImg_ToggleOn_Normal = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.toggleOn;
            this.chkAdvanceMode.Checked = false;
            resources.ApplyResources(this.chkAdvanceMode, "chkAdvanceMode");
            this.chkAdvanceMode.Name = "chkAdvanceMode";
            // 
            // labelAdvanceOption
            // 
            resources.ApplyResources(this.labelAdvanceOption, "labelAdvanceOption");
            this.labelAdvanceOption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.labelAdvanceOption.Name = "labelAdvanceOption";
            // 
            // label71
            // 
            resources.ApplyResources(this.label71, "label71");
            this.label71.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label71.Name = "label71";
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.Name = "label44";
            // 
            // panelPowerModifyMode
            // 
            this.panelPowerModifyMode.Controls.Add(this.optMotorParamsModifySimple);
            this.panelPowerModifyMode.Controls.Add(this.optMotorParamsModifyAdvance);
            this.panelPowerModifyMode.Controls.Add(this.label34);
            resources.ApplyResources(this.panelPowerModifyMode, "panelPowerModifyMode");
            this.panelPowerModifyMode.Name = "panelPowerModifyMode";
            // 
            // optMotorParamsModifySimple
            // 
            resources.ApplyResources(this.optMotorParamsModifySimple, "optMotorParamsModifySimple");
            this.optMotorParamsModifySimple.Checked = true;
            this.optMotorParamsModifySimple.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optMotorParamsModifySimple.Name = "optMotorParamsModifySimple";
            this.optMotorParamsModifySimple.TabStop = true;
            this.optMotorParamsModifySimple.UseVisualStyleBackColor = true;
            // 
            // optMotorParamsModifyAdvance
            // 
            resources.ApplyResources(this.optMotorParamsModifyAdvance, "optMotorParamsModifyAdvance");
            this.optMotorParamsModifyAdvance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optMotorParamsModifyAdvance.Name = "optMotorParamsModifyAdvance";
            this.optMotorParamsModifyAdvance.UseVisualStyleBackColor = true;
            // 
            // label34
            // 
            resources.ApplyResources(this.label34, "label34");
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label34.Name = "label34";
            // 
            // label53
            // 
            resources.ApplyResources(this.label53, "label53");
            this.label53.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label53.Name = "label53";
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.Name = "label45";
            // 
            // lbDaysPerYearAlarm
            // 
            resources.ApplyResources(this.lbDaysPerYearAlarm, "lbDaysPerYearAlarm");
            this.lbDaysPerYearAlarm.ForeColor = System.Drawing.Color.Red;
            this.lbDaysPerYearAlarm.Name = "lbDaysPerYearAlarm";
            this.lbDaysPerYearAlarm.Tag = "txtDayPerYear";
            // 
            // label52
            // 
            resources.ApplyResources(this.label52, "label52");
            this.label52.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label52.Name = "label52";
            // 
            // lbTimesPerMinuteAlarm
            // 
            resources.ApplyResources(this.lbTimesPerMinuteAlarm, "lbTimesPerMinuteAlarm");
            this.lbTimesPerMinuteAlarm.ForeColor = System.Drawing.Color.Red;
            this.lbTimesPerMinuteAlarm.Name = "lbTimesPerMinuteAlarm";
            this.lbTimesPerMinuteAlarm.Tag = "txtTimesPerMinute";
            // 
            // panelMotorParams
            // 
            this.panelMotorParams.Controls.Add(this.label35);
            this.panelMotorParams.Controls.Add(this.label43);
            this.panelMotorParams.Controls.Add(this.label42);
            this.panelMotorParams.Controls.Add(this.label41);
            this.panelMotorParams.Controls.Add(this.label40);
            this.panelMotorParams.Controls.Add(this.label39);
            this.panelMotorParams.Controls.Add(this.label38);
            this.panelMotorParams.Controls.Add(this.txtRotateInertia);
            this.panelMotorParams.Controls.Add(this.txtMaxTorque);
            this.panelMotorParams.Controls.Add(this.txtRatedTorque);
            this.panelMotorParams.Controls.Add(this.label37);
            this.panelMotorParams.Controls.Add(this.label36);
            resources.ApplyResources(this.panelMotorParams, "panelMotorParams");
            this.panelMotorParams.Name = "panelMotorParams";
            // 
            // label35
            // 
            resources.ApplyResources(this.label35, "label35");
            this.label35.Name = "label35";
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Name = "label43";
            this.label43.Tag = "txtMaxTorque";
            // 
            // label42
            // 
            resources.ApplyResources(this.label42, "label42");
            this.label42.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label42.Name = "label42";
            // 
            // label41
            // 
            resources.ApplyResources(this.label41, "label41");
            this.label41.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label41.Name = "label41";
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // label39
            // 
            resources.ApplyResources(this.label39, "label39");
            this.label39.ForeColor = System.Drawing.Color.Red;
            this.label39.Name = "label39";
            this.label39.Tag = "txtRatedTorque";
            // 
            // label38
            // 
            resources.ApplyResources(this.label38, "label38");
            this.label38.ForeColor = System.Drawing.Color.Red;
            this.label38.Name = "label38";
            this.label38.Tag = "txtRotateInertia";
            // 
            // txtRotateInertia
            // 
            resources.ApplyResources(this.txtRotateInertia, "txtRotateInertia");
            this.txtRotateInertia.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtRotateInertia.Name = "txtRotateInertia";
            // 
            // txtMaxTorque
            // 
            resources.ApplyResources(this.txtMaxTorque, "txtMaxTorque");
            this.txtMaxTorque.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtMaxTorque.Name = "txtMaxTorque";
            // 
            // txtRatedTorque
            // 
            resources.ApplyResources(this.txtRatedTorque, "txtRatedTorque");
            this.txtRatedTorque.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtRatedTorque.Name = "txtRatedTorque";
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label37.Name = "label37";
            // 
            // label36
            // 
            resources.ApplyResources(this.label36, "label36");
            this.label36.Name = "label36";
            // 
            // lbHoursPerDayAlarm
            // 
            resources.ApplyResources(this.lbHoursPerDayAlarm, "lbHoursPerDayAlarm");
            this.lbHoursPerDayAlarm.ForeColor = System.Drawing.Color.Red;
            this.lbHoursPerDayAlarm.Name = "lbHoursPerDayAlarm";
            this.lbHoursPerDayAlarm.Tag = "txtHourPerDay";
            // 
            // txtTimesPerMinute
            // 
            resources.ApplyResources(this.txtTimesPerMinute, "txtTimesPerMinute");
            this.txtTimesPerMinute.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTimesPerMinute.Name = "txtTimesPerMinute";
            // 
            // txtDayPerYear
            // 
            resources.ApplyResources(this.txtDayPerYear, "txtDayPerYear");
            this.txtDayPerYear.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtDayPerYear.Name = "txtDayPerYear";
            // 
            // panelPowerSelection
            // 
            this.panelPowerSelection.Controls.Add(this.label60);
            this.panelPowerSelection.Controls.Add(this.cboMotorParamsMotorPowerSelection);
            this.panelPowerSelection.Controls.Add(this.label59);
            resources.ApplyResources(this.panelPowerSelection, "panelPowerSelection");
            this.panelPowerSelection.Name = "panelPowerSelection";
            // 
            // label60
            // 
            resources.ApplyResources(this.label60, "label60");
            this.label60.Name = "label60";
            // 
            // cboMotorParamsMotorPowerSelection
            // 
            resources.ApplyResources(this.cboMotorParamsMotorPowerSelection, "cboMotorParamsMotorPowerSelection");
            this.cboMotorParamsMotorPowerSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cboMotorParamsMotorPowerSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMotorParamsMotorPowerSelection.DropDownWidth = 180;
            this.cboMotorParamsMotorPowerSelection.ForeColor = System.Drawing.Color.White;
            this.cboMotorParamsMotorPowerSelection.FormattingEnabled = true;
            this.cboMotorParamsMotorPowerSelection.Name = "cboMotorParamsMotorPowerSelection";
            // 
            // label59
            // 
            resources.ApplyResources(this.label59, "label59");
            this.label59.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label59.Name = "label59";
            // 
            // txtHourPerDay
            // 
            resources.ApplyResources(this.txtHourPerDay, "txtHourPerDay");
            this.txtHourPerDay.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtHourPerDay.Name = "txtHourPerDay";
            // 
            // txtLoad
            // 
            resources.ApplyResources(this.txtLoad, "txtLoad");
            this.txtLoad.Name = "txtLoad";
            // 
            // cboPower
            // 
            this.cboPower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cboPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPower.DropDownWidth = 180;
            resources.ApplyResources(this.cboPower, "cboPower");
            this.cboPower.ForeColor = System.Drawing.Color.White;
            this.cboPower.FormattingEnabled = true;
            this.cboPower.Name = "cboPower";
            // 
            // label46
            // 
            resources.ApplyResources(this.label46, "label46");
            this.label46.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label46.Name = "label46";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label50
            // 
            resources.ApplyResources(this.label50, "label50");
            this.label50.Name = "label50";
            // 
            // txtStroke
            // 
            resources.ApplyResources(this.txtStroke, "txtStroke");
            this.txtStroke.Name = "txtStroke";
            // 
            // labelStopTimeAlarm
            // 
            resources.ApplyResources(this.labelStopTimeAlarm, "labelStopTimeAlarm");
            this.labelStopTimeAlarm.ForeColor = System.Drawing.Color.Red;
            this.labelStopTimeAlarm.Name = "labelStopTimeAlarm";
            this.labelStopTimeAlarm.Tag = "txtRunTime";
            // 
            // labelStrokeAlarm
            // 
            resources.ApplyResources(this.labelStrokeAlarm, "labelStrokeAlarm");
            this.labelStrokeAlarm.ForeColor = System.Drawing.Color.Red;
            this.labelStrokeAlarm.Name = "labelStrokeAlarm";
            this.labelStrokeAlarm.Tag = "txtStroke";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label22.Name = "label22";
            // 
            // txtRunTime
            // 
            resources.ApplyResources(this.txtRunTime, "txtRunTime");
            this.txtRunTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtRunTime.Name = "txtRunTime";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // labelLoadAlarm
            // 
            resources.ApplyResources(this.labelLoadAlarm, "labelLoadAlarm");
            this.labelLoadAlarm.ForeColor = System.Drawing.Color.Red;
            this.labelLoadAlarm.Name = "labelLoadAlarm";
            this.labelLoadAlarm.Tag = "txtLoad";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label15.Name = "label15";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // panelMoment
            // 
            this.panelMoment.Controls.Add(this.pictureBox19);
            this.panelMoment.Controls.Add(this.lbTitleMoment);
            this.panelMoment.Controls.Add(this.label17);
            this.panelMoment.Controls.Add(this.label24);
            this.panelMoment.Controls.Add(this.label31);
            this.panelMoment.Controls.Add(this.label30);
            this.panelMoment.Controls.Add(this.label28);
            this.panelMoment.Controls.Add(this.label27);
            this.panelMoment.Controls.Add(this.txtMomentB);
            this.panelMoment.Controls.Add(this.txtMomentC);
            this.panelMoment.Controls.Add(this.txtMomentA);
            this.panelMoment.Controls.Add(this.label25);
            this.panelMoment.Controls.Add(this.label26);
            this.panelMoment.Controls.Add(this.label29);
            resources.ApplyResources(this.panelMoment, "panelMoment");
            this.panelMoment.Name = "panelMoment";
            // 
            // pictureBox19
            // 
            this.pictureBox19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox19.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.moment;
            resources.ApplyResources(this.pictureBox19, "pictureBox19");
            this.pictureBox19.Name = "pictureBox19";
            this.pictureBox19.TabStop = false;
            // 
            // lbTitleMoment
            // 
            resources.ApplyResources(this.lbTitleMoment, "lbTitleMoment");
            this.lbTitleMoment.BackColor = System.Drawing.Color.White;
            this.lbTitleMoment.ForeColor = System.Drawing.Color.DimGray;
            this.lbTitleMoment.Name = "lbTitleMoment";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.ForeColor = System.Drawing.Color.Red;
            this.label24.Name = "label24";
            this.label24.Tag = "txtMomentA";
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label31.Name = "label31";
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.ForeColor = System.Drawing.Color.Red;
            this.label30.Name = "label30";
            this.label30.Tag = "txtMomentB";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label28.Name = "label28";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.ForeColor = System.Drawing.Color.Red;
            this.label27.Name = "label27";
            this.label27.Tag = "txtMomentC";
            // 
            // txtMomentB
            // 
            resources.ApplyResources(this.txtMomentB, "txtMomentB");
            this.txtMomentB.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtMomentB.Name = "txtMomentB";
            // 
            // txtMomentC
            // 
            resources.ApplyResources(this.txtMomentC, "txtMomentC");
            this.txtMomentC.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtMomentC.Name = "txtMomentC";
            // 
            // txtMomentA
            // 
            resources.ApplyResources(this.txtMomentA, "txtMomentA");
            this.txtMomentA.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtMomentA.Name = "txtMomentA";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label25.Name = "label25";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // panelSetup
            // 
            this.panelSetup.Controls.Add(this.panelSetupMode);
            this.panelSetup.Controls.Add(this.lbTitleSetup);
            resources.ApplyResources(this.panelSetup, "panelSetup");
            this.panelSetup.Name = "panelSetup";
            // 
            // panelSetupMode
            // 
            this.panelSetupMode.Controls.Add(this.optVerticalUse);
            this.panelSetupMode.Controls.Add(this.optHorizontalUse);
            this.panelSetupMode.Controls.Add(this.optWallHangingUse);
            this.panelSetupMode.Controls.Add(this.pictureBox13);
            resources.ApplyResources(this.panelSetupMode, "panelSetupMode");
            this.panelSetupMode.Name = "panelSetupMode";
            // 
            // optVerticalUse
            // 
            resources.ApplyResources(this.optVerticalUse, "optVerticalUse");
            this.optVerticalUse.BackColor = System.Drawing.Color.Transparent;
            this.optVerticalUse.ForeColor = System.Drawing.Color.Black;
            this.optVerticalUse.Name = "optVerticalUse";
            this.optVerticalUse.UseVisualStyleBackColor = false;
            // 
            // optHorizontalUse
            // 
            resources.ApplyResources(this.optHorizontalUse, "optHorizontalUse");
            this.optHorizontalUse.Checked = true;
            this.optHorizontalUse.ForeColor = System.Drawing.Color.Black;
            this.optHorizontalUse.Name = "optHorizontalUse";
            this.optHorizontalUse.TabStop = true;
            this.optHorizontalUse.UseVisualStyleBackColor = true;
            // 
            // optWallHangingUse
            // 
            resources.ApplyResources(this.optWallHangingUse, "optWallHangingUse");
            this.optWallHangingUse.ForeColor = System.Drawing.Color.Black;
            this.optWallHangingUse.Name = "optWallHangingUse";
            this.optWallHangingUse.UseVisualStyleBackColor = true;
            // 
            // pictureBox13
            // 
            this.pictureBox13.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.setupMode;
            resources.ApplyResources(this.pictureBox13, "pictureBox13");
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.TabStop = false;
            // 
            // lbTitleSetup
            // 
            resources.ApplyResources(this.lbTitleSetup, "lbTitleSetup");
            this.lbTitleSetup.BackColor = System.Drawing.Color.White;
            this.lbTitleSetup.ForeColor = System.Drawing.Color.DimGray;
            this.lbTitleSetup.Name = "lbTitleSetup";
            // 
            // panelModelSelection
            // 
            this.panelModelSelection.Controls.Add(this.lbTitleModelSelection);
            this.panelModelSelection.Controls.Add(this.label61);
            this.panelModelSelection.Controls.Add(this.label54);
            this.panelModelSelection.Controls.Add(this.label73);
            this.panelModelSelection.Controls.Add(this.cboSeries);
            this.panelModelSelection.Controls.Add(this.label74);
            this.panelModelSelection.Controls.Add(this.label75);
            this.panelModelSelection.Controls.Add(this.cboModel);
            this.panelModelSelection.Controls.Add(this.cboLead);
            resources.ApplyResources(this.panelModelSelection, "panelModelSelection");
            this.panelModelSelection.Name = "panelModelSelection";
            // 
            // lbTitleModelSelection
            // 
            resources.ApplyResources(this.lbTitleModelSelection, "lbTitleModelSelection");
            this.lbTitleModelSelection.BackColor = System.Drawing.Color.White;
            this.lbTitleModelSelection.ForeColor = System.Drawing.Color.DimGray;
            this.lbTitleModelSelection.Name = "lbTitleModelSelection";
            // 
            // label61
            // 
            resources.ApplyResources(this.label61, "label61");
            this.label61.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label61.Name = "label61";
            // 
            // label54
            // 
            resources.ApplyResources(this.label54, "label54");
            this.label54.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label54.Name = "label54";
            // 
            // label73
            // 
            resources.ApplyResources(this.label73, "label73");
            this.label73.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label73.Name = "label73";
            // 
            // cboSeries
            // 
            this.cboSeries.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cboSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSeries.DropDownWidth = 180;
            resources.ApplyResources(this.cboSeries, "cboSeries");
            this.cboSeries.ForeColor = System.Drawing.Color.White;
            this.cboSeries.FormattingEnabled = true;
            this.cboSeries.Name = "cboSeries";
            // 
            // label74
            // 
            resources.ApplyResources(this.label74, "label74");
            this.label74.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label74.Name = "label74";
            // 
            // label75
            // 
            resources.ApplyResources(this.label75, "label75");
            this.label75.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.label75.Name = "label75";
            // 
            // cboModel
            // 
            this.cboModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModel.DropDownWidth = 180;
            resources.ApplyResources(this.cboModel, "cboModel");
            this.cboModel.ForeColor = System.Drawing.Color.White;
            this.cboModel.FormattingEnabled = true;
            this.cboModel.Name = "cboModel";
            // 
            // cboLead
            // 
            this.cboLead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cboLead.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLead.DropDownWidth = 180;
            resources.ApplyResources(this.cboLead, "cboLead");
            this.cboLead.ForeColor = System.Drawing.Color.White;
            this.cboLead.FormattingEnabled = true;
            this.cboLead.Name = "cboLead";
            // 
            // panelModelType
            // 
            this.panelModelType.Controls.Add(this.lbTitleModelType);
            this.panelModelType.Controls.Add(this.optBuildInSupportTrackActuator);
            this.panelModelType.Controls.Add(this.picBuildInSupportTrackActuator);
            this.panelModelType.Controls.Add(this.optBuildOutRodTypeActuator);
            this.panelModelType.Controls.Add(this.picBuildOutRodTypeActuator);
            this.panelModelType.Controls.Add(this.optBuildInBeltActuator);
            this.panelModelType.Controls.Add(this.picBuildInBeltActuator);
            this.panelModelType.Controls.Add(this.optEuropeBeltActuator);
            this.panelModelType.Controls.Add(this.picEuropeBeltActuator);
            this.panelModelType.Controls.Add(this.optStandardBeltActuator);
            this.panelModelType.Controls.Add(this.picStandardBeltActuator);
            this.panelModelType.Controls.Add(this.optSupportTrackRodTypeActuator);
            this.panelModelType.Controls.Add(this.picSupportTrackRodTypeActuator);
            this.panelModelType.Controls.Add(this.optNoTrackRodTypeActuator);
            this.panelModelType.Controls.Add(this.picNoTrackRodTypeActuator);
            this.panelModelType.Controls.Add(this.optBuildInRodTypeScrewActuator);
            this.panelModelType.Controls.Add(this.picBuildInRodTypeScrewActuator);
            this.panelModelType.Controls.Add(this.optBuildInScrewActuator);
            this.panelModelType.Controls.Add(this.picBuildInScrewActuator);
            this.panelModelType.Controls.Add(this.optStandardScrewActuator);
            this.panelModelType.Controls.Add(this.picStandardScrewActuator);
            resources.ApplyResources(this.panelModelType, "panelModelType");
            this.panelModelType.Name = "panelModelType";
            // 
            // lbTitleModelType
            // 
            resources.ApplyResources(this.lbTitleModelType, "lbTitleModelType");
            this.lbTitleModelType.BackColor = System.Drawing.Color.White;
            this.lbTitleModelType.ForeColor = System.Drawing.Color.DimGray;
            this.lbTitleModelType.Name = "lbTitleModelType";
            // 
            // optBuildInSupportTrackActuator
            // 
            resources.ApplyResources(this.optBuildInSupportTrackActuator, "optBuildInSupportTrackActuator");
            this.optBuildInSupportTrackActuator.ForeColor = System.Drawing.Color.Black;
            this.optBuildInSupportTrackActuator.Name = "optBuildInSupportTrackActuator";
            this.optBuildInSupportTrackActuator.UseVisualStyleBackColor = true;
            // 
            // picBuildInSupportTrackActuator
            // 
            this.picBuildInSupportTrackActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBuildInSupportTrackActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.GTYD;
            resources.ApplyResources(this.picBuildInSupportTrackActuator, "picBuildInSupportTrackActuator");
            this.picBuildInSupportTrackActuator.Name = "picBuildInSupportTrackActuator";
            this.picBuildInSupportTrackActuator.TabStop = false;
            // 
            // optBuildOutRodTypeActuator
            // 
            resources.ApplyResources(this.optBuildOutRodTypeActuator, "optBuildOutRodTypeActuator");
            this.optBuildOutRodTypeActuator.ForeColor = System.Drawing.Color.Black;
            this.optBuildOutRodTypeActuator.Name = "optBuildOutRodTypeActuator";
            this.optBuildOutRodTypeActuator.UseVisualStyleBackColor = true;
            // 
            // picBuildOutRodTypeActuator
            // 
            this.picBuildOutRodTypeActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBuildOutRodTypeActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Y65L;
            resources.ApplyResources(this.picBuildOutRodTypeActuator, "picBuildOutRodTypeActuator");
            this.picBuildOutRodTypeActuator.Name = "picBuildOutRodTypeActuator";
            this.picBuildOutRodTypeActuator.TabStop = false;
            // 
            // optBuildInBeltActuator
            // 
            resources.ApplyResources(this.optBuildInBeltActuator, "optBuildInBeltActuator");
            this.optBuildInBeltActuator.ForeColor = System.Drawing.Color.Black;
            this.optBuildInBeltActuator.Name = "optBuildInBeltActuator";
            this.optBuildInBeltActuator.UseVisualStyleBackColor = true;
            // 
            // picBuildInBeltActuator
            // 
            this.picBuildInBeltActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.picBuildInBeltActuator, "picBuildInBeltActuator");
            this.picBuildInBeltActuator.Name = "picBuildInBeltActuator";
            this.picBuildInBeltActuator.TabStop = false;
            // 
            // optEuropeBeltActuator
            // 
            resources.ApplyResources(this.optEuropeBeltActuator, "optEuropeBeltActuator");
            this.optEuropeBeltActuator.ForeColor = System.Drawing.Color.Black;
            this.optEuropeBeltActuator.Name = "optEuropeBeltActuator";
            this.optEuropeBeltActuator.UseVisualStyleBackColor = true;
            // 
            // picEuropeBeltActuator
            // 
            this.picEuropeBeltActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picEuropeBeltActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.M系列;
            resources.ApplyResources(this.picEuropeBeltActuator, "picEuropeBeltActuator");
            this.picEuropeBeltActuator.Name = "picEuropeBeltActuator";
            this.picEuropeBeltActuator.TabStop = false;
            // 
            // optStandardBeltActuator
            // 
            resources.ApplyResources(this.optStandardBeltActuator, "optStandardBeltActuator");
            this.optStandardBeltActuator.ForeColor = System.Drawing.Color.Black;
            this.optStandardBeltActuator.Name = "optStandardBeltActuator";
            this.optStandardBeltActuator.UseVisualStyleBackColor = true;
            // 
            // picStandardBeltActuator
            // 
            this.picStandardBeltActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picStandardBeltActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.螺桿滑台;
            resources.ApplyResources(this.picStandardBeltActuator, "picStandardBeltActuator");
            this.picStandardBeltActuator.Name = "picStandardBeltActuator";
            this.picStandardBeltActuator.TabStop = false;
            // 
            // optSupportTrackRodTypeActuator
            // 
            resources.ApplyResources(this.optSupportTrackRodTypeActuator, "optSupportTrackRodTypeActuator");
            this.optSupportTrackRodTypeActuator.ForeColor = System.Drawing.Color.Black;
            this.optSupportTrackRodTypeActuator.Name = "optSupportTrackRodTypeActuator";
            this.optSupportTrackRodTypeActuator.UseVisualStyleBackColor = true;
            // 
            // picSupportTrackRodTypeActuator
            // 
            this.picSupportTrackRodTypeActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSupportTrackRodTypeActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Y65D;
            resources.ApplyResources(this.picSupportTrackRodTypeActuator, "picSupportTrackRodTypeActuator");
            this.picSupportTrackRodTypeActuator.Name = "picSupportTrackRodTypeActuator";
            this.picSupportTrackRodTypeActuator.TabStop = false;
            // 
            // optNoTrackRodTypeActuator
            // 
            resources.ApplyResources(this.optNoTrackRodTypeActuator, "optNoTrackRodTypeActuator");
            this.optNoTrackRodTypeActuator.ForeColor = System.Drawing.Color.Black;
            this.optNoTrackRodTypeActuator.Name = "optNoTrackRodTypeActuator";
            this.optNoTrackRodTypeActuator.UseVisualStyleBackColor = true;
            // 
            // picNoTrackRodTypeActuator
            // 
            this.picNoTrackRodTypeActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNoTrackRodTypeActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.Y65;
            resources.ApplyResources(this.picNoTrackRodTypeActuator, "picNoTrackRodTypeActuator");
            this.picNoTrackRodTypeActuator.Name = "picNoTrackRodTypeActuator";
            this.picNoTrackRodTypeActuator.TabStop = false;
            // 
            // optBuildInRodTypeScrewActuator
            // 
            resources.ApplyResources(this.optBuildInRodTypeScrewActuator, "optBuildInRodTypeScrewActuator");
            this.optBuildInRodTypeScrewActuator.ForeColor = System.Drawing.Color.Black;
            this.optBuildInRodTypeScrewActuator.Name = "optBuildInRodTypeScrewActuator";
            this.optBuildInRodTypeScrewActuator.UseVisualStyleBackColor = true;
            // 
            // picBuildInRodTypeScrewActuator
            // 
            this.picBuildInRodTypeScrewActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBuildInRodTypeScrewActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.推桿式;
            resources.ApplyResources(this.picBuildInRodTypeScrewActuator, "picBuildInRodTypeScrewActuator");
            this.picBuildInRodTypeScrewActuator.Name = "picBuildInRodTypeScrewActuator";
            this.picBuildInRodTypeScrewActuator.TabStop = false;
            // 
            // optBuildInScrewActuator
            // 
            resources.ApplyResources(this.optBuildInScrewActuator, "optBuildInScrewActuator");
            this.optBuildInScrewActuator.Checked = true;
            this.optBuildInScrewActuator.ForeColor = System.Drawing.Color.Black;
            this.optBuildInScrewActuator.Name = "optBuildInScrewActuator";
            this.optBuildInScrewActuator.TabStop = true;
            this.optBuildInScrewActuator.UseVisualStyleBackColor = true;
            // 
            // picBuildInScrewActuator
            // 
            this.picBuildInScrewActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBuildInScrewActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.一般環境;
            resources.ApplyResources(this.picBuildInScrewActuator, "picBuildInScrewActuator");
            this.picBuildInScrewActuator.Name = "picBuildInScrewActuator";
            this.picBuildInScrewActuator.TabStop = false;
            // 
            // optStandardScrewActuator
            // 
            resources.ApplyResources(this.optStandardScrewActuator, "optStandardScrewActuator");
            this.optStandardScrewActuator.ForeColor = System.Drawing.Color.Black;
            this.optStandardScrewActuator.Name = "optStandardScrewActuator";
            this.optStandardScrewActuator.UseVisualStyleBackColor = true;
            // 
            // picStandardScrewActuator
            // 
            this.picStandardScrewActuator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picStandardScrewActuator.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.皮帶滑台;
            resources.ApplyResources(this.picStandardScrewActuator, "picStandardScrewActuator");
            this.picStandardScrewActuator.Name = "picStandardScrewActuator";
            this.picStandardScrewActuator.TabStop = false;
            // 
            // panelUseEnv
            // 
            this.panelUseEnv.Controls.Add(this.picDustFree);
            this.panelUseEnv.Controls.Add(this.optDustFreeEnv);
            this.panelUseEnv.Controls.Add(this.optStandardEnv);
            this.panelUseEnv.Controls.Add(this.picStandardEnv);
            this.panelUseEnv.Controls.Add(this.lbTitleUseEnv);
            resources.ApplyResources(this.panelUseEnv, "panelUseEnv");
            this.panelUseEnv.Name = "panelUseEnv";
            // 
            // picDustFree
            // 
            this.picDustFree.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.dustFreeEnviroment;
            resources.ApplyResources(this.picDustFree, "picDustFree");
            this.picDustFree.Name = "picDustFree";
            this.picDustFree.TabStop = false;
            // 
            // optDustFreeEnv
            // 
            resources.ApplyResources(this.optDustFreeEnv, "optDustFreeEnv");
            this.optDustFreeEnv.ForeColor = System.Drawing.Color.Black;
            this.optDustFreeEnv.Name = "optDustFreeEnv";
            this.optDustFreeEnv.UseVisualStyleBackColor = true;
            // 
            // optStandardEnv
            // 
            resources.ApplyResources(this.optStandardEnv, "optStandardEnv");
            this.optStandardEnv.Checked = true;
            this.optStandardEnv.ForeColor = System.Drawing.Color.Black;
            this.optStandardEnv.Name = "optStandardEnv";
            this.optStandardEnv.TabStop = true;
            this.optStandardEnv.UseVisualStyleBackColor = true;
            // 
            // picStandardEnv
            // 
            this.picStandardEnv.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.standardEnviroment;
            resources.ApplyResources(this.picStandardEnv, "picStandardEnv");
            this.picStandardEnv.Name = "picStandardEnv";
            this.picStandardEnv.TabStop = false;
            // 
            // lbTitleUseEnv
            // 
            resources.ApplyResources(this.lbTitleUseEnv, "lbTitleUseEnv");
            this.lbTitleUseEnv.BackColor = System.Drawing.Color.White;
            this.lbTitleUseEnv.ForeColor = System.Drawing.Color.DimGray;
            this.lbTitleUseEnv.Name = "lbTitleUseEnv";
            // 
            // panelSideTable
            // 
            resources.ApplyResources(this.panelSideTable, "panelSideTable");
            this.panelSideTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSideTable.Controls.Add(this.panelSideTableIcon);
            this.panelSideTable.Controls.Add(this.panelSideTableSelections);
            this.panelSideTable.Controls.Add(this.customPanel4);
            this.panelSideTable.Controls.Add(this.label4);
            this.panelSideTable.Controls.Add(this.customPanel3);
            this.panelSideTable.Controls.Add(this.label3);
            this.panelSideTable.Controls.Add(this.customPanel2);
            this.panelSideTable.Controls.Add(this.label2);
            this.panelSideTable.Curvature = 8;
            this.panelSideTable.Name = "panelSideTable";
            this.panelSideTable.Tag = "967, 100";
            // 
            // panelSideTableIcon
            // 
            this.panelSideTableIcon.Controls.Add(this.pictureBox5);
            this.panelSideTableIcon.Controls.Add(this.pictureBox4);
            this.panelSideTableIcon.Controls.Add(this.pictureBox2);
            resources.ApplyResources(this.panelSideTableIcon, "panelSideTableIcon");
            this.panelSideTableIcon.Name = "panelSideTableIcon";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources._2D_3D;
            resources.ApplyResources(this.pictureBox5, "pictureBox5");
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.order;
            resources.ApplyResources(this.pictureBox4, "pictureBox4");
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.pdf;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // panelSideTableSelections
            // 
            resources.ApplyResources(this.panelSideTableSelections, "panelSideTableSelections");
            this.panelSideTableSelections.BorderColor = System.Drawing.Color.Silver;
            this.panelSideTableSelections.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSideTableSelections.Controls.Add(this.tableSelections);
            this.panelSideTableSelections.Curvature = 8;
            this.panelSideTableSelections.Name = "panelSideTableSelections";
            // 
            // tableSelections
            // 
            resources.ApplyResources(this.tableSelections, "tableSelections");
            this.tableSelections.Name = "tableSelections";
            // 
            // customPanel4
            // 
            this.customPanel4.BorderColor = System.Drawing.Color.Silver;
            this.customPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customPanel4.Controls.Add(this.lbSideTableModelInfo);
            this.customPanel4.Curvature = 8;
            resources.ApplyResources(this.customPanel4, "customPanel4");
            this.customPanel4.Name = "customPanel4";
            // 
            // lbSideTableModelInfo
            // 
            resources.ApplyResources(this.lbSideTableModelInfo, "lbSideTableModelInfo");
            this.lbSideTableModelInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(88)))), ((int)(((byte)(111)))));
            this.lbSideTableModelInfo.Name = "lbSideTableModelInfo";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Name = "label4";
            // 
            // customPanel3
            // 
            this.customPanel3.BorderColor = System.Drawing.Color.Silver;
            this.customPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customPanel3.Controls.Add(this.lbSideTableMsg);
            this.customPanel3.Curvature = 8;
            resources.ApplyResources(this.customPanel3, "customPanel3");
            this.customPanel3.Name = "customPanel3";
            // 
            // lbSideTableMsg
            // 
            resources.ApplyResources(this.lbSideTableMsg, "lbSideTableMsg");
            this.lbSideTableMsg.ForeColor = System.Drawing.Color.Red;
            this.lbSideTableMsg.Name = "lbSideTableMsg";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Name = "label3";
            // 
            // customPanel2
            // 
            this.customPanel2.BorderColor = System.Drawing.Color.Silver;
            this.customPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customPanel2.Controls.Add(this.picModelImg);
            this.customPanel2.Curvature = 8;
            resources.ApplyResources(this.customPanel2, "customPanel2");
            this.customPanel2.Name = "customPanel2";
            // 
            // picModelImg
            // 
            resources.ApplyResources(this.picModelImg, "picModelImg");
            this.picModelImg.Image = global::SingleAxis_NoMotor_SelectionSoftware.Properties.Resources.MK85;
            this.picModelImg.Name = "picModelImg";
            this.picModelImg.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tabResult
            // 
            this.tabResult.BackColor = System.Drawing.Color.White;
            this.tabResult.Controls.Add(this.explorerBar_step5);
            resources.ApplyResources(this.tabResult, "tabResult");
            this.tabResult.Name = "tabResult";
            // 
            // explorerBar_step5
            // 
            resources.ApplyResources(this.explorerBar_step5, "explorerBar_step5");
            this.explorerBar_step5.Controls.Add(this.panelResult);
            this.explorerBar_step5.Name = "explorerBar_step5";
            // 
            // panelResult
            // 
            this.panelResult.Controls.Add(this.label51);
            this.panelResult.Controls.Add(this.label56);
            this.panelResult.Controls.Add(this.lbResult);
            this.panelResult.Controls.Add(this.picBoxResultImg);
            this.panelResult.Controls.Add(this.panelConfirmBtnsStep5);
            this.panelResult.Controls.Add(this.label70);
            resources.ApplyResources(this.panelResult, "panelResult");
            this.panelResult.Name = "panelResult";
            // 
            // label51
            // 
            resources.ApplyResources(this.label51, "label51");
            this.label51.ForeColor = System.Drawing.Color.Red;
            this.label51.Name = "label51";
            // 
            // label56
            // 
            resources.ApplyResources(this.label56, "label56");
            this.label56.BackColor = System.Drawing.Color.LightGray;
            this.label56.Name = "label56";
            // 
            // lbResult
            // 
            resources.ApplyResources(this.lbResult, "lbResult");
            this.lbResult.ForeColor = System.Drawing.Color.Red;
            this.lbResult.Name = "lbResult";
            // 
            // picBoxResultImg
            // 
            resources.ApplyResources(this.picBoxResultImg, "picBoxResultImg");
            this.picBoxResultImg.Name = "picBoxResultImg";
            this.picBoxResultImg.TabStop = false;
            // 
            // panelConfirmBtnsStep5
            // 
            resources.ApplyResources(this.panelConfirmBtnsStep5, "panelConfirmBtnsStep5");
            this.panelConfirmBtnsStep5.Controls.Add(this.cmdConfirmStep5, 1, 0);
            this.panelConfirmBtnsStep5.Controls.Add(this.cmdResetStep5, 2, 0);
            this.panelConfirmBtnsStep5.Name = "panelConfirmBtnsStep5";
            // 
            // cmdConfirmStep5
            // 
            this.cmdConfirmStep5.BackColor = System.Drawing.Color.Transparent;
            this.cmdConfirmStep5.BackColor_Hover = System.Drawing.Color.DarkRed;
            this.cmdConfirmStep5.BackColor_Normal = System.Drawing.Color.Red;
            this.cmdConfirmStep5.BackColor_Press = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdConfirmStep5.BackColor2_Hover = System.Drawing.Color.DarkRed;
            this.cmdConfirmStep5.BackColor2_Normal = System.Drawing.Color.Red;
            this.cmdConfirmStep5.BackColor2_Press = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdConfirmStep5.ButtonEnabled = true;
            resources.ApplyResources(this.cmdConfirmStep5, "cmdConfirmStep5");
            this.cmdConfirmStep5.Curvature = 15;
            this.cmdConfirmStep5.GradientMode = CustomButton.LinearGradientMode.Horizontal;
            this.cmdConfirmStep5.Name = "cmdConfirmStep5";
            this.cmdConfirmStep5.TextFont = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            // 
            // cmdResetStep5
            // 
            this.cmdResetStep5.BackColor = System.Drawing.Color.Transparent;
            this.cmdResetStep5.BackColor_Hover = System.Drawing.Color.DimGray;
            this.cmdResetStep5.BackColor_Normal = System.Drawing.Color.Gray;
            this.cmdResetStep5.BackColor_Press = System.Drawing.Color.Black;
            this.cmdResetStep5.BackColor2_Hover = System.Drawing.Color.DimGray;
            this.cmdResetStep5.BackColor2_Normal = System.Drawing.Color.Gray;
            this.cmdResetStep5.BackColor2_Press = System.Drawing.Color.Black;
            this.cmdResetStep5.ButtonEnabled = true;
            resources.ApplyResources(this.cmdResetStep5, "cmdResetStep5");
            this.cmdResetStep5.Curvature = 15;
            this.cmdResetStep5.GradientMode = CustomButton.LinearGradientMode.Horizontal;
            this.cmdResetStep5.Name = "cmdResetStep5";
            this.cmdResetStep5.TextFont = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            // 
            // label70
            // 
            this.label70.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.label70, "label70");
            this.label70.ForeColor = System.Drawing.Color.DimGray;
            this.label70.Name = "label70";
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.panelBase.ResumeLayout(false);
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
            this.panel4.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabStart.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmdShapeSelection)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmdModelSelection)).EndInit();
            this.tabContent.ResumeLayout(false);
            this.tabContent.PerformLayout();
            this.explorerBar.ResumeLayout(false);
            this.explorerBar.PerformLayout();
            this.panelNextPage.ResumeLayout(false);
            this.panelConfirmBtnsStep2.ResumeLayout(false);
            this.panelCalcResult.ResumeLayout(false);
            this.panelCalcResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecommandList)).EndInit();
            this.panelChart.ResumeLayout(false);
            this.panelChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.panelCalc.ResumeLayout(false);
            this.panelCalc.PerformLayout();
            this.scrollBarPanelLoad.ResumeLayout(false);
            this.scrollBarPanelLoad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scrollBarThumbLoad)).EndInit();
            this.scrollBarPanelStroke.ResumeLayout(false);
            this.scrollBarPanelStroke.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scrollBarThumbStroke)).EndInit();
            this.panelReducer.ResumeLayout(false);
            this.panelReducer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReducerInfo)).EndInit();
            this.panelAdvanceParams.ResumeLayout(false);
            this.panelAdvanceParams.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panelExpectServiceLifeTime.ResumeLayout(false);
            this.panelExpectServiceLifeTime.PerformLayout();
            this.panelAdvanceMode.ResumeLayout(false);
            this.panelAdvanceMode.PerformLayout();
            this.panelPowerModifyMode.ResumeLayout(false);
            this.panelPowerModifyMode.PerformLayout();
            this.panelMotorParams.ResumeLayout(false);
            this.panelMotorParams.PerformLayout();
            this.panelPowerSelection.ResumeLayout(false);
            this.panelPowerSelection.PerformLayout();
            this.panelMoment.ResumeLayout(false);
            this.panelMoment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).EndInit();
            this.panelSetup.ResumeLayout(false);
            this.panelSetup.PerformLayout();
            this.panelSetupMode.ResumeLayout(false);
            this.panelSetupMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            this.panelModelSelection.ResumeLayout(false);
            this.panelModelSelection.PerformLayout();
            this.panelModelType.ResumeLayout(false);
            this.panelModelType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInSupportTrackActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildOutRodTypeActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInBeltActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEuropeBeltActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStandardBeltActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSupportTrackRodTypeActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNoTrackRodTypeActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInRodTypeScrewActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBuildInScrewActuator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStandardScrewActuator)).EndInit();
            this.panelUseEnv.ResumeLayout(false);
            this.panelUseEnv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDustFree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStandardEnv)).EndInit();
            this.panelSideTable.ResumeLayout(false);
            this.panelSideTableIcon.ResumeLayout(false);
            this.panelSideTableIcon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelSideTableSelections.ResumeLayout(false);
            this.customPanel4.ResumeLayout(false);
            this.customPanel3.ResumeLayout(false);
            this.customPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picModelImg)).EndInit();
            this.tabResult.ResumeLayout(false);
            this.tabResult.PerformLayout();
            this.explorerBar_step5.ResumeLayout(false);
            this.panelResult.ResumeLayout(false);
            this.panelResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxResultImg)).EndInit();
            this.panelConfirmBtnsStep5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBoxToyo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        public System.Windows.Forms.SplitContainer splitContainerBase;
        public System.Windows.Forms.PictureBox cmdExplorer;
        public System.Windows.Forms.PictureBox cmdNarrow;
        public System.Windows.Forms.PictureBox cmdClose;
        public System.Windows.Forms.PictureBox cmdZoom;
        private System.Windows.Forms.TabPage tabContent;
        private System.Windows.Forms.TabPage tabResult;
        public System.Windows.Forms.Panel explorerBar;
        public CustomPanel panelSideTable;
        public CustomPanel panelSideTableSelections;
        public System.Windows.Forms.TableLayoutPanel tableSelections;
        private CustomPanel customPanel4;
        private System.Windows.Forms.Label label4;
        private CustomPanel customPanel3;
        private System.Windows.Forms.Label label3;
        private CustomPanel customPanel2;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Panel panelModelSelection;
        public System.Windows.Forms.Panel panelModelType;
        public System.Windows.Forms.Panel panelUseEnv;
        public System.Windows.Forms.Panel explorerBar_step5;
        public CustomTabControl tabMain;
        public System.Windows.Forms.Label lbSideTableModelInfo;
        public System.Windows.Forms.Label lbSideTableMsg;
        public System.Windows.Forms.PictureBox picModelImg;
        public System.Windows.Forms.Label lbTitle;
        public System.Windows.Forms.Panel panelSideTableIcon;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.TabPage tabStart;
        public System.Windows.Forms.Panel panelCalc;
        public System.Windows.Forms.Label lbAccelSpeed;
        public System.Windows.Forms.Label lbCycleTime;
        public System.Windows.Forms.Label lbMaxSpeed;
        public System.Windows.Forms.Label lbRunTime;
        public System.Windows.Forms.Label lbConstantTime;
        public System.Windows.Forms.Label lbAccelTime;
        public System.Windows.Forms.DataVisualization.Charting.Chart chart;
        public System.Windows.Forms.Panel panelAdvanceMode;
        public CustomToggle.CustomToggle chkAdvanceMode;
        public System.Windows.Forms.Label labelAdvanceOption;
        public System.Windows.Forms.Panel panelPowerModifyMode;
        public System.Windows.Forms.RadioButton optMotorParamsModifySimple;
        public System.Windows.Forms.RadioButton optMotorParamsModifyAdvance;
        private System.Windows.Forms.Label label34;
        public System.Windows.Forms.Panel panelAdvanceParams;
        public System.Windows.Forms.Label lbMaxSpeedAlarm;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label81;
        public System.Windows.Forms.Label label82;
        public System.Windows.Forms.TextBox txtAccelSpeed;
        public System.Windows.Forms.RadioButton optMaxSpeedType_rpm;
        public System.Windows.Forms.RadioButton optMaxSpeedType_mms;
        private System.Windows.Forms.Label label79;
        public System.Windows.Forms.TextBox txtMaxSpeed;
        public System.Windows.Forms.Label lbRpm;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.Panel panelExpectServiceLifeTime;
        private System.Windows.Forms.Label label72;
        public System.Windows.Forms.Label lbExpectServiceLifeAlarm;
        public System.Windows.Forms.TextBox txtExpectServiceLifeTime;
        public System.Windows.Forms.RadioButton optExpectServiceLife;
        public System.Windows.Forms.RadioButton optNoExpectServiceLife;
        private System.Windows.Forms.Label label71;
        public System.Windows.Forms.Panel panelMotorParams;
        private System.Windows.Forms.Label label35;
        public System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label40;
        public System.Windows.Forms.Label label39;
        public System.Windows.Forms.Label label38;
        public System.Windows.Forms.TextBox txtRotateInertia;
        public System.Windows.Forms.TextBox txtMaxTorque;
        public System.Windows.Forms.TextBox txtRatedTorque;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        public System.Windows.Forms.Panel panelPowerSelection;
        private System.Windows.Forms.Label label60;
        public System.Windows.Forms.ComboBox cboMotorParamsMotorPowerSelection;
        private System.Windows.Forms.Label label59;
        public System.Windows.Forms.ComboBox cboPower;
        public System.Windows.Forms.TextBox txtStroke;
        public System.Windows.Forms.Label labelStrokeAlarm;
        public System.Windows.Forms.Label labelLoadAlarm;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        public System.Windows.Forms.Label labelStopTimeAlarm;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label45;
        public System.Windows.Forms.Label lbDaysPerYearAlarm;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label lbTimesPerMinuteAlarm;
        public System.Windows.Forms.Label lbHoursPerDayAlarm;
        public System.Windows.Forms.TextBox txtDayPerYear;
        private System.Windows.Forms.Label label46;
        public System.Windows.Forms.TextBox txtLoad;
        public System.Windows.Forms.TextBox txtTimesPerMinute;
        public System.Windows.Forms.TextBox txtHourPerDay;
        public System.Windows.Forms.DataGridView dgvReducerInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModel;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnReducerRatio;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label58;
        public System.Windows.Forms.Panel panelReducer;
        public CustomButton.CustomButton cmdCalc;
        public System.Windows.Forms.PictureBox cmdModelSelection;
        public System.Windows.Forms.PictureBox cmdShapeSelection;
        public System.Windows.Forms.PictureBox picDustFree;
        public System.Windows.Forms.RadioButton optDustFreeEnv;
        public System.Windows.Forms.RadioButton optStandardEnv;
        public System.Windows.Forms.PictureBox picStandardEnv;
        private System.Windows.Forms.Label lbTitleUseEnv;
        private System.Windows.Forms.Label lbTitleModelType;
        public System.Windows.Forms.RadioButton optBuildInSupportTrackActuator;
        private System.Windows.Forms.PictureBox picBuildInSupportTrackActuator;
        public System.Windows.Forms.RadioButton optBuildOutRodTypeActuator;
        private System.Windows.Forms.PictureBox picBuildOutRodTypeActuator;
        public System.Windows.Forms.RadioButton optBuildInBeltActuator;
        private System.Windows.Forms.PictureBox picBuildInBeltActuator;
        public System.Windows.Forms.RadioButton optEuropeBeltActuator;
        private System.Windows.Forms.PictureBox picEuropeBeltActuator;
        public System.Windows.Forms.RadioButton optStandardBeltActuator;
        private System.Windows.Forms.PictureBox picStandardBeltActuator;
        public System.Windows.Forms.RadioButton optSupportTrackRodTypeActuator;
        private System.Windows.Forms.PictureBox picSupportTrackRodTypeActuator;
        public System.Windows.Forms.RadioButton optNoTrackRodTypeActuator;
        private System.Windows.Forms.PictureBox picNoTrackRodTypeActuator;
        public System.Windows.Forms.RadioButton optBuildInRodTypeScrewActuator;
        private System.Windows.Forms.PictureBox picBuildInRodTypeScrewActuator;
        public System.Windows.Forms.RadioButton optBuildInScrewActuator;
        private System.Windows.Forms.PictureBox picBuildInScrewActuator;
        public System.Windows.Forms.RadioButton optStandardScrewActuator;
        private System.Windows.Forms.PictureBox picStandardScrewActuator;
        private System.Windows.Forms.Label lbTitleModelSelection;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label75;
        public System.Windows.Forms.ComboBox cboModel;
        public System.Windows.Forms.ComboBox cboLead;
        public System.Windows.Forms.Panel panelCalcResult;
        public System.Windows.Forms.TableLayoutPanel panelConfirmBtnsStep2;
        public CustomButton.CustomButton cmdConfirmStep2;
        public System.Windows.Forms.DataGridView dgvRecommandList;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.Panel panelChart;
        public System.Windows.Forms.Panel panelMoment;
        private System.Windows.Forms.PictureBox pictureBox19;
        private System.Windows.Forms.Label lbTitleMoment;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label31;
        public System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label28;
        public System.Windows.Forms.Label label27;
        public System.Windows.Forms.TextBox txtMomentB;
        public System.Windows.Forms.TextBox txtMomentC;
        public System.Windows.Forms.TextBox txtMomentA;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label29;
        public System.Windows.Forms.Panel panelSetup;
        public System.Windows.Forms.Panel panelSetupMode;
        public System.Windows.Forms.RadioButton optVerticalUse;
        public System.Windows.Forms.RadioButton optHorizontalUse;
        public System.Windows.Forms.RadioButton optWallHangingUse;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.Label lbTitleSetup;
        public System.Windows.Forms.Label lbPrePage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Panel panelNextPage;
        private System.Windows.Forms.Label lbTitleCalc;
        private System.Windows.Forms.Panel panelBase;
        public System.Windows.Forms.Panel scrollBarPanelStroke;
        public System.Windows.Forms.PictureBox scrollBarThumbStroke;
        public System.Windows.Forms.Panel scrollBarPanelLoad;
        public System.Windows.Forms.PictureBox scrollBarThumbLoad;
        public System.Windows.Forms.TextBox txtRunTime;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label73;
        public System.Windows.Forms.ComboBox cboSeries;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 鎖定;
        private System.Windows.Forms.DataGridViewTextBoxColumn 項次;
        private System.Windows.Forms.DataGridViewTextBoxColumn 重複定位精度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 導程;
        private System.Windows.Forms.DataGridViewTextBoxColumn 荷重;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最高轉速;
        private System.Windows.Forms.DataGridViewTextBoxColumn 運行速度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 加速度;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最大行程;
        private System.Windows.Forms.DataGridViewTextBoxColumn 運行時間;
        private System.Windows.Forms.DataGridViewTextBoxColumn 力矩A;
        private System.Windows.Forms.DataGridViewTextBoxColumn 力矩B;
        private System.Windows.Forms.DataGridViewTextBoxColumn 力矩C;
        private System.Windows.Forms.DataGridViewTextBoxColumn 力矩警示;
        private System.Windows.Forms.DataGridViewTextBoxColumn 馬達瓦數;
        private System.Windows.Forms.DataGridViewTextBoxColumn 皮帶馬達安全係數;
        private System.Windows.Forms.DataGridViewTextBoxColumn T_max安全係數;
        private System.Windows.Forms.DataGridViewTextBoxColumn 皮帶T_max安全係數;
        private System.Windows.Forms.DataGridViewTextBoxColumn T_Rms安全係數;
        private System.Windows.Forms.DataGridViewTextBoxColumn 運行距離;
        private System.Windows.Forms.DataGridViewTextBoxColumn 運行壽命;
        private System.Windows.Forms.DataGridViewImageColumn 是否推薦;
        private System.Windows.Forms.DataGridViewImageColumn 更詳細資訊;
        public System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.Label label70;
        public System.Windows.Forms.TableLayoutPanel panelConfirmBtnsStep5;
        public CustomButton.CustomButton cmdConfirmStep5;
        public CustomButton.CustomButton cmdResetStep5;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label56;
        public System.Windows.Forms.Label lbResult;
        public System.Windows.Forms.PictureBox picBoxResultImg;
    }
}

