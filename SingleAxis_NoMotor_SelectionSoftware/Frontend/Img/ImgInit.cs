using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ImgInit {
        // 按鈕狀態
        public enum ButtonStatus {
            Normal,
            Hover,
            Press,
            Enable,
            Disable
        }
        // 按鈕圖片宣告
        public Dictionary<PictureBox, Dictionary<ButtonStatus, Image>> img = new Dictionary<PictureBox, Dictionary<ButtonStatus, Image>>();

        // 圖片按鈕宣告
        public Dictionary<PictureBox, RadioButton> cmdOptMap;         // 使用環境

        public ImgInit(FormMain formMain) {

            img = new Dictionary<PictureBox, Dictionary<ButtonStatus, Image>>() {
                // 選型方式
                { formMain.cmdShapeSelection, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.selectionType_shape_normal },
                    { ButtonStatus.Hover, Properties.Resources.selectionType_shape_hover },
                    { ButtonStatus.Press, Properties.Resources.selectionType_shape_normal },
                } },
                { formMain.cmdModelSelection, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.selectionType_model_normal },
                    { ButtonStatus.Hover, Properties.Resources.selectionType_model_hover },
                    { ButtonStatus.Press, Properties.Resources.selectionType_model_normal },
                } },
                // 使用環境
                { formMain.picStandardEnv, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.useEnv_standard_normal },
                    { ButtonStatus.Hover, Properties.Resources.useEnv_standard_hover },
                    { ButtonStatus.Press, Properties.Resources.useEnv_standard_hover },
                    { ButtonStatus.Enable, Properties.Resources.useEnv_standard_enabled },
                } },
                { formMain.picDustFree, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.useEnv_dustfree_normal },
                    { ButtonStatus.Hover, Properties.Resources.useEnv_dustfree_hover },
                    { ButtonStatus.Press, Properties.Resources.useEnv_dustfree_hover },
                    { ButtonStatus.Enable, Properties.Resources.useEnv_dustfree_enabled },
                } },
                // 傳動方式
                { formMain.picGTH, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.GTH_normal },
                    { ButtonStatus.Hover, Properties.Resources.GTH_hover },
                    { ButtonStatus.Press, Properties.Resources.GTH_hover },
                    { ButtonStatus.Enable, Properties.Resources.GTH_enabled },
                } },
                { formMain.picGTY, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.GTY_normal },
                    { ButtonStatus.Hover, Properties.Resources.GTY_hover },
                    { ButtonStatus.Press, Properties.Resources.GTY_hover },
                    { ButtonStatus.Enable, Properties.Resources.GTY_enabled },
                } },
                { formMain.picETH, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.ETH_normal },
                    { ButtonStatus.Hover, Properties.Resources.ETH_hover },
                    { ButtonStatus.Press, Properties.Resources.ETH_hover },
                    { ButtonStatus.Enable, Properties.Resources.ETH_enabled },
                } },
                { formMain.picY, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.Y_normal },
                    { ButtonStatus.Hover, Properties.Resources.Y_hover },
                    { ButtonStatus.Press, Properties.Resources.Y_hover },
                    { ButtonStatus.Enable, Properties.Resources.Y_enabled },
                } },
                { formMain.picYD, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.YD_normal },
                    { ButtonStatus.Hover, Properties.Resources.YD_hover },
                    { ButtonStatus.Press, Properties.Resources.YD_hover },
                    { ButtonStatus.Enable, Properties.Resources.YD_enabled },
                } },
                { formMain.picYL, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.YL_normal },
                    { ButtonStatus.Hover, Properties.Resources.YL_hover },
                    { ButtonStatus.Press, Properties.Resources.YL_hover },
                    { ButtonStatus.Enable, Properties.Resources.YL_enabled },
                } },
                { formMain.picMG, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.MG_normal },
                    { ButtonStatus.Hover, Properties.Resources.MG_hover },
                    { ButtonStatus.Press, Properties.Resources.MG_hover },
                    { ButtonStatus.Enable, Properties.Resources.MG_enabled },
                } },
                { formMain.picM, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.M_normal },
                    { ButtonStatus.Hover, Properties.Resources.M_hover },
                    { ButtonStatus.Press, Properties.Resources.M_hover },
                    { ButtonStatus.Enable, Properties.Resources.M_enabled },
                } },
                { formMain.picETB, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.ETB_normal },
                    { ButtonStatus.Hover, Properties.Resources.ETB_hover },
                    { ButtonStatus.Press, Properties.Resources.ETB_hover },
                    { ButtonStatus.Enable, Properties.Resources.ETB_enabled },
                } },
                { formMain.picGCH, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.GCH_normal },
                    { ButtonStatus.Hover, Properties.Resources.GCH_hover },
                    { ButtonStatus.Press, Properties.Resources.GCH_hover },
                    { ButtonStatus.Enable, Properties.Resources.GCH_enabled },
                } },
                { formMain.picECH, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.ECH_normal },
                    { ButtonStatus.Hover, Properties.Resources.ECH_hover },
                    { ButtonStatus.Press, Properties.Resources.ECH_hover },
                    { ButtonStatus.Enable, Properties.Resources.ECH_enabled },
                } },
                { formMain.picECB, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.ECB_normal },
                    { ButtonStatus.Hover, Properties.Resources.ECB_hover },
                    { ButtonStatus.Press, Properties.Resources.ECB_hover },
                    { ButtonStatus.Enable, Properties.Resources.ECB_enabled },
                } },
                // 安裝方式
                { formMain.picHorizontalUse, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.setup_horizontal_normal },
                    { ButtonStatus.Hover, Properties.Resources.setup_horizontal_hover },
                    { ButtonStatus.Press, Properties.Resources.setup_horizontal_hover },
                    { ButtonStatus.Enable, Properties.Resources.setup_horizontal_enabled },
                } },
                { formMain.picVerticalUse, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.setup_vertical_normal },
                    { ButtonStatus.Hover, Properties.Resources.setup_vertical_hover },
                    { ButtonStatus.Press, Properties.Resources.setup_vertical_hover },
                    { ButtonStatus.Enable, Properties.Resources.setup_vertical_enabled },
                } },
                { formMain.picWallHangingUse, new Dictionary<ButtonStatus, Image>(){
                    { ButtonStatus.Normal, Properties.Resources.setup_wallHang_normal },
                    { ButtonStatus.Hover, Properties.Resources.setup_wallHang_hover },
                    { ButtonStatus.Press, Properties.Resources.setup_wallHang_hover },
                    { ButtonStatus.Enable, Properties.Resources.setup_wallHang_enabled },
                } },
            };

            cmdOptMap = new Dictionary<PictureBox, RadioButton>() {
                // 使用環境
                { formMain.picStandardEnv, formMain.optStandardEnv },
                { formMain.picDustFree, formMain.optDustFreeEnv},
                // 傳動方式
                { formMain.picGTH, formMain.optGTH },
                { formMain.picGTY, formMain.optGTY },
                { formMain.picETH, formMain.optETH },
                { formMain.picM, formMain.optM },
                { formMain.picMG, formMain.optMG },
                { formMain.picETB, formMain.optETB },
                { formMain.picY, formMain.optY },
                { formMain.picYD, formMain.optYD },
                { formMain.picYL, formMain.optYL },
                { formMain.picGCH, formMain.optGCH },
                { formMain.picECH, formMain.optECH },
                { formMain.picECB, formMain.optECB },
                // 安裝方式
                { formMain.picHorizontalUse, formMain.optHorizontalUse },
                { formMain.picVerticalUse, formMain.optVerticalUse},
                { formMain.picWallHangingUse, formMain.optWallHangingUse},
            };
        }
    }
}
