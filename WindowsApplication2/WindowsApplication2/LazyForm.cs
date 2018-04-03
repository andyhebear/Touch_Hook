using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using MouseKeyboardLibrary;
using System.Reflection;

namespace LazyPPiigg
{
    public partial class LazyForm : Form
    {
        #region 私人變數
        /// <summary>
        /// mouser hook
        /// </summary>
        private MouseHook mMouseHook = new MouseHook();
        /// <summary>
        /// keyboard hook
        /// </summary>
        private KeyboardHook mKeyboardHook = new KeyboardHook();
        /// <summary>
        /// int reader
        /// </summary>
        private IniFile mIniFile = null;//new IniFile("lazy.ini");
        /// <summary>
        /// 放大鏡擷取的點
        /// </summary>
        private Point mMagnifierPosition = Point.Empty;
        /// <summary>
        /// 放大截圖的大小，必須小於PictureBox才有放大的效果。
        /// </summary>
        private int mMagnifierSize = 10;
        /// <summary>
        /// 固定要掃描的點
        /// </summary>
        private List<Point> mFixPoints = new List<Point>();
        /// <summary>
        /// 目標List
        /// </summary>
        private List<Point> mRemainPointList = new List<Point>();
        /// <summary>
        /// 助攻Point List
        /// </summary>
        private List<Point> mAssistPointList = new List<Point>();
        /// <summary>
        /// 遊戲判斷
        /// </summary>
        private List<GameRobot> mGameRobots = new List<GameRobot>();
        /// <summary>
        /// Attack目標Size
        /// </summary>
        private readonly Size AttackTargeSize = new Size(15, 15);
        /// <summary>
        /// 判斷是否戰鬥掃描的點
        /// </summary>
        private readonly Point SpecifyPoint = new Point(10, 47);
        /// <summary>
        /// 判斷是否戰鬥掃描的大小
        /// </summary>
        private readonly Size SpecifySize = new Size(1, 1);
        /// <summary>
        /// 判斷Mp、Hp掃描的大小
        /// </summary>
        private readonly Size FixSize = new Size(6, 6);
        /// <summary>
        /// 判斷非戰鬥Mp掃描的點
        /// </summary>
        private readonly Point MpPoint = new Point(35, 49);
             
        #endregion

        #region 公開變數
        /// <summary>
        /// 血條顏色
        /// </summary>
        public Color HpColor = Color.White;
        /// <summary>
        /// Mp顏色
        /// </summary>
        public Color MpColor = Color.White;
        /// <summary>
        /// 時間顏色
        /// </summary>
        public Color TimeColor = Color.White;
        /// <summary>
        /// 指定座標顏色
        /// </summary>
        public Color SpecifyColor = Color.White;
        /// <summary>
        /// 目標顏色
        /// </summary>
        public Color TargetColor = Color.White;
        #endregion

        #region 系統相關method
        [DllImport("User32.dll", EntryPoint = "WindowFromPoint")]
        private static extern IntPtr WindowFromPoint(Point point);

        public LazyForm()
        {
            //初始化勿更動
            InitializeComponent();
            //ini檔設定為相對路徑
            mIniFile = new IniFile(System.IO.Path.Combine(Application.StartupPath, "lazy.ini "));

            mKeyboardHook.KeyDown += KeyboardHook_KeyDown;
            mKeyboardHook.Start();

            mMouseHook.MouseMove += new MouseEventHandler(mMouserHook_MouseMove);
            
            //將顏色從ini讀出
            foreach (FieldInfo field in this.GetType().GetFields())
            {
                if (field.FieldType != typeof(Color) )
                    continue;
                field.SetValue(this, mIniFile.ReadIni<Color>("lazy", field.Name));
            }
            string text = mIniFile.ReadIni("Keys", "primaryCombo");
            if (!string.IsNullOrEmpty(text))
                primaryCombo.Text = text;
            text = mIniFile.ReadIni("Keys", "secondaryCombo");
            if (!string.IsNullOrEmpty(text))
                secondaryCombo.Text = text;
            text = mIniFile.ReadIni("Keys", "otherCombo");
            if (!string.IsNullOrEmpty(text))
                otherCombo.Text = text;
            text = mIniFile.ReadIni("Keys", "addedCombo");
            if (!string.IsNullOrEmpty(text))
                addedCombo.Text = text;

            RefreshText();
            InitFixPoints();
            InitRobots();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }

        /// <summary>
        /// 初始化掃描的點
        /// </summary>
        private void InitFixPoints()
        {
            mFixPoints.Add(new Point(60, 382));
            mFixPoints.Add(new Point(125, 336));
            mFixPoints.Add(new Point(277, 223));
            mFixPoints.Add(new Point(202, 275));
            mFixPoints.Add(new Point(348, 169));
            mFixPoints.Add(new Point(124, 443));
            mFixPoints.Add(new Point(196, 391));
            mFixPoints.Add(new Point(296, 334));
            mFixPoints.Add(new Point(340, 284));
            mFixPoints.Add(new Point(410, 229));
            mAssistPointList.Add(new Point(528, 438));
            mAssistPointList.Add(new Point(592, 498));
        }
        /// <summary>
        /// 初始化機器人
        /// </summary>
        private void InitRobots()
        {
            GameRobot robot = new GameRobot(mFixPoints);
            robot.AttackTargeSize = AttackTargeSize;
            robot.TargetColor = TargetColor;
            robot.SpecifyColor = SpecifyColor;
            robot.SpecifySize = SpecifySize;
            robot.SpecifyPoint = SpecifyPoint;
            robot.FixSize = FixSize;
            robot.MpPoint = MpPoint;
            robot.IsEnable = true;
            robot.IsAttack = attackCheckBox.Checked;
            robot.IsSiege = siegeCheckBox.Checked;
            robot.PrimaryKey = (Keys)Enum.Parse(typeof(Keys), primaryCombo.Text);
            robot.SecondaryKey = (Keys)Enum.Parse(typeof(Keys), secondaryCombo.Text);
            robot.OtherKey = (Keys)Enum.Parse(typeof(Keys), otherCombo.Text);
            robot.AddedKey = (Keys)Enum.Parse(typeof(Keys), addedCombo.Text);
            
            mGameRobots.Add(robot);
        }

        /// <summary>
        /// 重新整理顏色文字
        /// </summary>
        private void RefreshText()
        {
            hpTxt.Text = HpColor.ToArgb().ToString("X");
            mpTxt.Text = MpColor.ToArgb().ToString("X");
            timeTxt.Text = TimeColor.ToArgb().ToString("X");
            specifyTxt.Text = SpecifyColor.ToArgb().ToString("X");
            targetTxt.Text = TargetColor.ToArgb().ToString("X");
        }

        /// <summary>
        /// 應用程式關閉的callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            //將顏色存入ini檔
            foreach (FieldInfo field in this.GetType().GetFields())
            {
                if (field.FieldType != typeof(Color))
                    continue;
                mIniFile.WriteIni("lazy", field.Name, ((Color)field.GetValue(this)).ToArgb().ToString("X"));
            }                        
        }

        //private bool PostMessage(IntPtr hWnd,uint Msg, int wParam, int lParam);
        /// <summary>
        /// 鍵盤事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F10:
                    MouserTimer.Enabled = true;
                    break;
                case Keys.F12:
                    MouserTimer.Enabled = false;
                    break;
                case Keys.B:
                    {
                        //KeyboardSimulator.SendKey("{F11}");
                        //KeyboardSimulator.KeyDown(Keys.F11);
                        //Thread.Sleep(1000);
                        //KeyboardSimulator.KeyUp(Keys.F11);
                    }
                    break;
            }

            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.D1:
                        HpColor = ScreenLib.GetScreenColor(MouseSimulator.Position);
                        hpTxt.Text = HpColor.ToArgb().ToString("X");
                        break;
                    case Keys.D2:
                        MpColor = ScreenLib.GetScreenColor(MouseSimulator.Position);
                        mpTxt.Text = MpColor.ToArgb().ToString("X");
                        break;
                    case Keys.D3:
                        TimeColor = ScreenLib.GetScreenColor(MouseSimulator.Position);
                        timeTxt.Text = TimeColor.ToArgb().ToString("X");
                        break;
                    case Keys.D5:
                        SpecifyColor = ScreenLib.GetScreenColor(MouseSimulator.Position);
                        specifyTxt.Text = SpecifyColor.ToArgb().ToString("X");
                        break;
                    case Keys.D4:
                        TargetColor = ScreenLib.GetScreenColor(MouseSimulator.Position);
                        targetTxt.Text = TargetColor.ToArgb().ToString("X");
                        break;
                }
            }

            if (e.Control)
            {

            }
        }

        /// <summary>
        /// 放大鏡更新用的timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            Position.Text = Cursor.Position.ToString();
            ColorLabel.Text = ScreenLib.GetScreenColor(Cursor.Position).ToArgb().ToString("X");
            Magnifier.Image = ScreenLib.GetScreen(mMagnifierPosition, new Size(mMagnifierSize, mMagnifierSize));
        }
        /// <summary>
        /// 滑鼠動作專用timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouserTimer_Tick(object sender, EventArgs e)
        {
            OnMouseClick();
            OnRightAndLeftClick();

            foreach (GameRobot robot in mGameRobots)
            {
                if (robot.IsEnable)
                    robot.OnUpdate(0);
            }

        }

        /// <summary>
        /// 滑鼠單點延遲20ms
        /// </summary>
        private void OnMouseClick()
        {
            if (DoubleClickCheckBox.Checked)
            {
                MouseSimulator.MouseDown(MouseButton.Left);
                Thread.Sleep(20);
                MouseSimulator.MouseUp(MouseButton.Left);
            }
        }

        /// <summary>
        /// 左右鍵連點
        /// </summary>
        private void OnRightAndLeftClick()
        {
            if (LeftAndRightCheckBox.Checked)
            {
                MouseSimulator.Click(MouseButton.Right);
                MouseSimulator.Click(MouseButton.Left);
            }
        }
 
        /// <summary>
        /// 滑鼠移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMouserHook_MouseMove(object sender, MouseEventArgs e)
        {
            mMagnifierPosition = new Point(e.X - (mMagnifierSize / 2), e.Y - (mMagnifierSize / 2));
        }

        /// <summary>
        /// 抓圖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tools_CheckedChanged(object sender, EventArgs e)
        {
            Timer.Enabled = ToolsCheckBox.Checked;
            if (ToolsCheckBox.Checked)
                mMouseHook.Start();
            else
                mMouseHook.Stop();
        }

        /// <summary>
        /// form 關閉中的callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LazyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                //取消關閉
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                notifyIcon.Tag = string.Empty;
                notifyIcon.ShowBalloonTip(3000, this.Text,
                     "程式並未結束，要結束請在圖示上按右鍵，選取結束功能!",
                     ToolTipIcon.Info);
                this.ShowInTaskbar = false;
            }
        }

        /// <summary>
        /// 當tray icon 的選單按下結束時的callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitContextMenu_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            mIniFile.WriteIni("Keys", "primaryCombo", primaryCombo.Text);
            mIniFile.WriteIni("Keys", "secondaryCombo", secondaryCombo.Text);
            mIniFile.WriteIni("Keys", "otherCombo", otherCombo.Text);
            mIniFile.WriteIni("Keys", "addedCombo", addedCombo.Text);
            this.Close();
        }

        /// <summary>
        /// tray icon 連點兩下顯示視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //如果目前是縮小狀態，才要回覆成一般大小的視窗
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
            this.Focus();
        }

        private void siegeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GameRobot robot in mGameRobots)
            {
                robot.IsSiege = siegeCheckBox.Checked;
                System.Diagnostics.Debug.Write("\nrobot.IsAttack" + robot.IsAttack);
                System.Diagnostics.Debug.Write("\nrobot.IsSiege" + robot.IsSiege);
            }
        }

        private void attackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GameRobot robot in mGameRobots)
            {
                robot.IsAttack = attackCheckBox.Checked;
                System.Diagnostics.Debug.Write("\nrobot.IsAttack" + robot.IsAttack);
                System.Diagnostics.Debug.Write("\nrobot.IsSiege" + robot.IsSiege);
            }
        }

        #endregion

        private void primaryCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(GameRobot robot in mGameRobots)
                robot.PrimaryKey = (Keys)Enum.Parse(typeof(Keys), primaryCombo.Text);
        }

        private void secondaryCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GameRobot robot in mGameRobots)
                robot.SecondaryKey = (Keys)Enum.Parse(typeof(Keys), secondaryCombo.Text);
        }

        private void otherCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GameRobot robot in mGameRobots)
                robot.OtherKey = (Keys)Enum.Parse(typeof(Keys), otherCombo.Text);
        }

        private void addedCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GameRobot robot in mGameRobots)
                robot.AddedKey = (Keys)Enum.Parse(typeof(Keys), addedCombo.Text);
        }
    }
}