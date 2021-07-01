using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class CustomScrollBar {
        // 委派事件
        public delegate void EventHandler(object sender, EventArgs e);
        public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);
        public delegate void MouseEventHandler(object sender, MouseEventArgs e);
        public EventHandler ValueChanged;
        public ScrollEventHandler Scroll;
        public MouseEventHandler MouseUp;
        // 內件參數
        public string Name = "";
        public int Value = defaultValue;
        public int minValue = 0;
        public int maxValue = 100;
        public int smallChange = 10;
        public Image imgScrollBarLine;
        public Image imgThumbNormal;
        public Image imgThumbHover;
        public NumericUpDown bindingNumericUpDown;
        public TextBox bindingTextBox;

        private Form formMain;
        private Panel scrollBar;
        private PictureBox thumb;
        private PictureBox arrowLeft;
        private PictureBox arrowRight;
        private int px, py;
        private bool isDragging = false;
        private Thread threadMoving;
        private Thread threadValueChanged;
        private Thread threadUpdateNumeric;
        private Thread threadUpdateTextBox;
        private int minPosOffset = 0;
        private int maxPosOffset = 16;
        private ScrollEventType curScrollType;
        private enum FocusOn { ScrollBar, Input }
        private FocusOn curFocusOn = FocusOn.ScrollBar;
        private string placeHolder = null;
        private const int defaultValue = 0;
        //private bool showPlaceHolder = true;

        public CustomScrollBar(Form formMain, Panel scrollBar, PictureBox thumb, PictureBox arrowLeft, PictureBox arrowRight) {
            this.formMain = formMain;
            this.scrollBar = scrollBar;
            this.thumb = thumb;
            this.arrowLeft = arrowLeft;
            this.arrowRight = arrowRight;
        }

        public void Initialize() {
            // 初始化位置
            thumb.Location = new Point(
                (int)(((float)Value / (float)maxValue) * (scrollBar.Size.Width - maxPosOffset) + minPosOffset),
                thumb.Location.Y
            );

            // 改值偵測
            threadValueChanged = new Thread(CheckValueChanged);
            threadValueChanged.Start();

            // numerice改值thread
            if (bindingNumericUpDown != null) {
                threadUpdateNumeric = new Thread(UpdateNumeric);
                threadUpdateNumeric.Start();
            }
            // textBox改值thread
            if (bindingTextBox != null) {
                threadUpdateTextBox = new Thread(UpdateTextBox);
                threadUpdateTextBox.Start();
            }

            InitEvents();
        }

        private void InitEvents() {
            thumb.MouseDown += Thumb_MouseDown;
            thumb.MouseUp += Thumb_MouseUp;
            thumb.MouseMove += Thumb_MouseMove;
            if (arrowLeft != null)
                arrowLeft.MouseDown += ArrowLeft_MouseDown;
            if (arrowRight != null)
                arrowRight.MouseDown += ArrowRight_MouseDown;

            // numeric
            if (bindingNumericUpDown != null)
                bindingNumericUpDown.ValueChanged += BindingNumericUpDown_ValueChanged;
            // textbox
            if (bindingTextBox != null) {
                bindingTextBox.Enter += BindingTextBox_Enter;
                bindingTextBox.Leave += BindingTextBox_Leave;
                bindingTextBox.KeyDown += BindingTextBox_KeyDown;
            }
        }

        private void ArrowRight_MouseDown(object sender, MouseEventArgs e) {
            curFocusOn = FocusOn.ScrollBar;
            if (Value + smallChange <= maxValue)
                Value += smallChange;
        }

        private void ArrowLeft_MouseDown(object sender, MouseEventArgs e) {
            curFocusOn = FocusOn.ScrollBar;
            if (Value - smallChange >= 0)
                Value -= smallChange;
        }

        private void BindingTextBox_Leave(object sender, EventArgs e) {
            //if (string.IsNullOrEmpty(bindingTextBox.Text))
            //    showPlaceHolder = true;
            //curFocusOn = FocusOn.ScrollBar;
        }

        private void BindingTextBox_Enter(object sender, EventArgs e) {
            curFocusOn = FocusOn.Input;

            //showPlaceHolder = false;
        }

        private void BindingTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                int a = 0;
                if (Int32.TryParse(bindingTextBox.Text, out a)) {
                    Value = a;
                }
            }
        }

        private void BindingNumericUpDown_ValueChanged(object sender, EventArgs e) {
            Value = (int)bindingNumericUpDown.Value;

            // 自定義邏輯(可修正)
            if (!isDragging)
                if (MouseUp != null)
                    MouseUp(this, null);
        }

        private void Thumb_MouseMove(object sender, MouseEventArgs e) {
            if (isDragging)
                // 5為能顯示完整thumb的最大位置
                targetDistance = scrollBar.PointToClient(Cursor.Position).X - 5;
        }

        private void Thumb_MouseUp(object sender, MouseEventArgs e) {
            isDragging = false;
            curScrollType = ScrollEventType.EndScroll;
            curFocusOn = FocusOn.Input;

            threadMoving.Abort();

            // scrollBar mouse up event
            if (MouseUp != null)
                MouseUp(this, e);
        }

        private void Thumb_MouseDown(object sender, MouseEventArgs e) {
            px = e.X; // 記住滑鼠點下時相對於元件的 (x,y) 坐標。
            py = e.Y;
            isDragging = true;
            curScrollType = ScrollEventType.ThumbTrack;
            curFocusOn = FocusOn.ScrollBar;

            threadMoving = new Thread(MoveThumb);
            threadMoving.Start();
        }

        int targetDistance = 0;
        private void MoveThumb() {
            try {
                while (true) {
                    formMain.Invoke(new Action(() => {
                        if (thumb.Location.X > targetDistance) {
                            int targetX = thumb.Location.X - 1;
                            if (targetX >= minPosOffset)
                                thumb.Location = new Point(targetX, thumb.Location.Y);
                        } else if (thumb.Location.X < targetDistance) {
                            int targetX = thumb.Location.X + 1;
                            int arrowRightWidth = arrowRight != null ? arrowRight.Size.Width : 0;
                            if (targetX <= scrollBar.Size.Width - maxPosOffset + arrowRightWidth)
                                thumb.Location = new Point(targetX, thumb.Location.Y);
                        }


                        //double curPercent = (float)(thumb.Location.X) / ((float)scrollBar.Size.Width - maxPosOffset);
                        //Value = (int)(curPercent * maxValue);

                        Value = (int)((float)(thumb.Location.X - minPosOffset) / (float)(scrollBar.Size.Width - maxPosOffset) * maxValue);

                        // 滾動事件
                        if (Scroll != null) {
                            ScrollEventArgs e = new ScrollEventArgs(curScrollType, Value);
                            Scroll(this, e);
                        }
                    }));

                    Thread.Sleep(1);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                // 滾動事件
                if (Scroll != null) {
                    formMain.Invoke(new Action(() => {
                        ScrollEventArgs e = new ScrollEventArgs(curScrollType, -1);
                        Scroll(this, e);
                    }));
                }
            }
        }

        private void CheckValueChanged() {
            try {
                int newValue = Value;
                int oldValue = Value;

                while (true) {
                    // 偵測主Form是否已關閉
                    if (formMain.IsDisposed)
                        break;

                    newValue = Value;

                    if (oldValue != newValue) {
                        formMain.Invoke(new Action(() => {
                            // 初始化位置
                            thumb.Location = new Point(
                                (int)(((float)Value / (float)maxValue) * (scrollBar.Size.Width - maxPosOffset) + minPosOffset),
                                thumb.Location.Y
                            );

                            if (ValueChanged != null)
                                ValueChanged(this, null);
                        }));
                    }

                    oldValue = newValue;

                    Thread.Sleep(100);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        private void UpdateNumeric() {
            try {
                while (true) {
                    // 偵測主Form是否已關閉
                    if (formMain.IsDisposed)
                        break;

                    if (bindingNumericUpDown != null && curFocusOn == FocusOn.ScrollBar) {
                        formMain.Invoke(new Action(() => {
                            bindingNumericUpDown.Value = Value;
                        }));
                    }

                    Thread.Sleep(1);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void UpdateTextBox() {
            try {
                while (true) {
                    // 偵測主Form是否已關閉
                    if (formMain.IsDisposed)
                        break;

                    if (bindingTextBox != null && curFocusOn == FocusOn.ScrollBar) {
                        formMain.Invoke(new Action(() => {
                            bindingTextBox.Text = Value.ToString();

                            if (Int32.TryParse(bindingTextBox.Text, out int curTextBoxValue)) {
                                if (curTextBoxValue > maxValue)
                                    bindingTextBox.Text = maxValue.ToString();
                                if (curTextBoxValue < minValue)
                                    bindingTextBox.Text = minValue.ToString();
                            }
                        }));
                    }

                    Thread.Sleep(1);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
