﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Page1 {
        public enum ModelSelectionMode {  
            /// <summary>
            /// 型號選型
            /// </summary>
            ModelSelection, 
            /// <summary>
            /// 條件選型
            /// </summary>
            ConditionSelection 
        }
        public ModelSelectionMode modelSelectionMode = ModelSelectionMode.ModelSelection;

        private FormMain formMain;

        public Page1(FormMain formMain) {
            this.formMain = formMain;                                   

            InitEvents();
        }        

        private void InitEvents() {
            formMain.cmdShapeSelection.Click += CmdSelectionMode_Click;
            formMain.cmdModelSelection.Click += CmdSelectionMode_Click;
        }

        private void CmdSelectionMode_Click(object sender, EventArgs e) {
            PictureBox btn = sender as PictureBox;

            // 選行方式
            if (btn == formMain.cmdModelSelection)
                modelSelectionMode = ModelSelectionMode.ModelSelection;
            else if (btn == formMain.cmdShapeSelection)
                modelSelectionMode = ModelSelectionMode.ConditionSelection;

            formMain.tabMain.SelectTab("tabContent");
            formMain.page2.Load();
        }
    }
}
