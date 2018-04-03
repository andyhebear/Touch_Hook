using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MouseKeyboardLibrary;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace LazyPPiigg
{
    /// <summary>
    /// 用來做遊戲判斷，如果以後有後台程式可以針對單一視窗做事
    /// </summary>
    public class GameRobot
    {

        /* API 定義*/
        [DllImport("User32.dll",EntryPoint="FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName,string lpWindowName);
        [DllImport("User32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, int hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern int GetDlgCtrlID(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        private static extern IntPtr SetActiveWindow(IntPtr hwnd);
        
        public enum Type
        {
            none = 0,
            siege = 1 << 0,
            attack = 1 << 1,
        }

        private List<Point> mFixPoints;
        private List<Point> mAssists;
        private List<Point> mSurvivalPoints = new List<Point>();
        private Type mRobotType = Type.none;
        /// <summary>
        /// 是否練功
        /// </summary>
        public bool IsAttack
        {
            get { return (mRobotType & Type.attack) == Type.attack; }
            set 
            {
                if (IsAttack == value)
                    return;
                mRobotType = value ? mRobotType | Type.attack : mRobotType ^ Type.attack;
            }
        }
        /// <summary>
        /// 是否攻城
        /// </summary>
        public bool IsSiege
        {
            get { return (mRobotType & Type.siege) == Type.siege; }
            set 
            {
                if (IsSiege == value)
                    return;
                mRobotType = value ? mRobotType | Type.siege : mRobotType ^ Type.siege; 
            }
        }

        public Size AttackTargeSize { get; set; }
        public Color TargetColor { get; set; }
        public Point SpecifyPoint { get; set; }
        public Size SpecifySize { get; set; }
        public Color SpecifyColor { get; set; }
        public bool IsEnable { get; set; }
        public Size FixSize { get; set; }
        public Point MpPoint { get; set; }
        public Color MpColor { get; set; }
        public Keys PrimaryKey { get; set; }
        public Keys SecondaryKey { get; set; }
        public Keys OtherKey { get; set; }
        public Keys AddedKey { get; set; }
        public GameRobot(List<Point> fixPoints)
        {
            mFixPoints = fixPoints;
            CurrentBehavior = NormalBehavior;
        }
        private delegate void CurrentFSMFunPtr();
        private CurrentFSMFunPtr CurrentBehavior;
        virtual public void OnUpdate(float dt)
        {

            //KeyboardSimulator.SendKey("{F6}");
            if (CurrentBehavior != null)
                CurrentBehavior();
        }

        

        #region 遊戲判斷 method

        public bool IsInSiege()
        {
            return IsFighting() && IsSiege;
        }

        public bool IsInAttack()
        {
            return IsFighting() && IsAttack;
        }

        /// <summary>
        /// 無戰鬥狀態下的Update
        /// </summary>
        private void NormalBehavior()
        {
            
            if (!IsEnoughMp())
            {
                mSurvivalPoints.Clear();
                //喝水
                KeyboardSimulator.KeyPress(AddedKey);
            }

            if (IsInSiege())
            {
                AttackPoint();
                CurrentBehavior = SiegeBehavior;
            }
            else if (IsInAttack())
            {
                AttackPoint();
                CurrentBehavior = AttactBehavior;
            }

            
        }

        /// <summary>
        /// 練功狀態下的Update
        /// </summary>
        private void AttactBehavior()
        {
            if (IsFighting() && mSurvivalPoints.Count > 0)
            {
                foreach (Point point in mSurvivalPoints)
                {
                    Bitmap bmp = ScreenLib.GetScreen(point, AttackTargeSize);
                    if (!ScreenLib.Scan(bmp, TargetColor))
                        mSurvivalPoints.Remove(point);
                }
                
                MouseSimulator.Position = mSurvivalPoints[0];
                Thread.Sleep(100);
                KeyboardSimulator.KeyPress(PrimaryKey);
                Thread.Sleep(100);
                MouseSimulator.Click(MouseButton.Left);
            }
            else
            {
                mSurvivalPoints.Clear();
                CurrentBehavior = NormalBehavior;
            }
            
        }

        /// <summary>
        /// 助攻
        /// </summary>
        private void AssistBehavior()
        {
            if (IsFighting())
            {

            }
        }

        /// <summary>
        /// 攻城的update
        /// </summary>
        private void SiegeBehavior()
        {
            if (IsFighting())
            {

            }
            else
            {
                CurrentBehavior = NormalBehavior;
            }
        }

        /// <summary>
        /// 目標座標
        /// </summary>
        /// <returns></returns>
        private void AttackPoint()
        {
            if (mSurvivalPoints.Count > 0)
                return;

            foreach (Point point in mFixPoints)
            {
                if (ScreenLib.Scan(ScreenLib.GetScreen(point, AttackTargeSize), TargetColor))
                    mSurvivalPoints.Add(point);
            }
        }

        /// <summary>
        /// 是否戰鬥中
        /// </summary>
        /// <returns></returns>
        private bool IsFighting()
        {
            Bitmap fightbmp = ScreenLib.GetScreen(SpecifyPoint, SpecifySize);
            return ScreenLib.Scan(fightbmp, SpecifyColor);
        }

        /// <summary>
        /// 非戰鬥Mp是否足夠
        /// </summary>
        /// <returns></returns>
        private bool IsEnoughMp()
        {
            Bitmap mpBmp = ScreenLib.GetScreen(MpPoint, FixSize);
            return ScreenLib.Scan(mpBmp, MpColor);
        }
        #endregion
    }
}
