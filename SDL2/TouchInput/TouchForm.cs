using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace ICTest
{
    public partial class TouchForm : Form
    {
        #region ** fields

        List<Figure> _figures = new List<Figure>();
        Background _background;

        #endregion

        #region ** initialization

        public TouchForm()
        {
            InitializeComponent();

            Win32.EnableMouseInPointer(true);

            _background = new Background(this);

            Figure f;
            f = new Figure(this, Color.LightCoral, false, false);

            f.AddPoint(200, 100);
            f.AddPoint(320, 250);
            f.AddPoint(200, 400);
            f.AddPoint(80, 250);

            _figures.Add(f);

            f = new Figure(this, Color.LightSeaGreen, false, true);

            f.AddPoint(500, 60);
            f.AddPoint(390, 220);
            f.AddPoint(610, 220);
            f.AddPoint(500, 163);

            _figures.Add(f);

            f = new Figure(this, Color.LightSteelBlue, true, false);

            f.AddPoint(500, 250);
            f.AddPoint(700, 250);
            f.AddPoint(500, 400);
            f.AddPoint(700, 400);

            _figures.Add(f);
        }

        #endregion

        #region ** finalization

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Figure f in _figures)
                {
                    f.Dispose();
                }
                _figures.Clear();

                _background.Dispose();
                _background = null;

                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        public List<Figure> Figures
        {
            get { return _figures; }
        }

        [SecurityPermission(SecurityAction.LinkDemand)]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_POINTERDOWN:
                case Win32.WM_POINTERUP:
                case Win32.WM_POINTERUPDATE:
                case Win32.WM_POINTERCAPTURECHANGED:
                    break;

                default:
                    base.WndProc(ref m);
                    return;
            }
            int pointerID = Win32.GET_POINTER_ID(m.WParam);
            Win32.POINTER_INFO pi = new Win32.POINTER_INFO();
            if (!Win32.GetPointerInfo(pointerID, ref pi))
            {
                Win32.CheckLastError();
            }
            bool processed = false;
            switch (m.Msg)
            {
                case Win32.WM_POINTERDOWN:
                    {
                        if ((pi.PointerFlags & Win32.POINTER_FLAGS.PRIMARY) != 0)
                        {
                            this.Capture = true;
                        }
                        Point pt = PointToClient(pi.PtPixelLocation.ToPoint());
                        foreach (Figure f in _figures)
                        {
                            if (f.HitTest(pt.X, pt.Y))
                            {
                                f.BringToFront();
                                f.AddPointer(pointerID);
                                f.ProcessPointerFrames(pointerID, pi.FrameID);
                                if (f.Pivot)
                                {
                                    Point pt1 = Point.Ceiling(f.Points[0]);
                                    Point pt0 = Point.Ceiling(f.Points[3]);
                                    pt0.X = (pt0.X + pt1.X) / 2;
                                    pt0.Y = (pt0.Y + pt1.Y) / 2;
                                    pt0 = PointToScreen(pt0);
                                    Win32.SetPivotInteractionContext(f.Context, pt0.X, pt0.Y, 0f);
                                }
                                processed = true;
                                break;
                            }
                        }
                        if (!processed)
                        {
                            _background.AddPointer(pointerID);
                            _background.ProcessPointerFrames(pointerID, pi.FrameID);
                        }
                    }
                    break;

                case Win32.WM_POINTERUP:

                    if ((pi.PointerFlags & Win32.POINTER_FLAGS.PRIMARY) != 0)
                    {
                        this.Capture = false;
                    }
                    foreach (Figure f in _figures)
                    {
                        if (f.ActivePointers.Contains(pointerID))
                        {
                            f.ProcessPointerFrames(pointerID, pi.FrameID);
                            f.RemovePointer(pointerID);
                            processed = true;
                            break;
                        }
                    }
                    if (!processed && _background.ActivePointers.Contains(pointerID))
                    {
                        _background.ProcessPointerFrames(pointerID, pi.FrameID);
                        _background.RemovePointer(pointerID);
                    }
                    break;

                case Win32.WM_POINTERUPDATE:

                    foreach (Figure f in _figures)
                    {
                        if (f.ActivePointers.Contains(pointerID))
                        {
                            f.ProcessPointerFrames(pointerID, pi.FrameID);
                            processed = true;
                            break;
                        }
                    }
                    if (!processed && _background.ActivePointers.Contains(pointerID))
                    {
                        _background.ProcessPointerFrames(pointerID, pi.FrameID);
                    }
                    break;

                case Win32.WM_POINTERCAPTURECHANGED:

                    this.Capture = false;
                    foreach (Figure f in _figures)
                    {
                        if (f.ActivePointers.Contains(pointerID))
                        {
                            f.StopProcessing();
                            processed = true;
                            break;
                        }
                    }
                    if (!processed && _background.ActivePointers.Contains(pointerID))
                    {
                        _background.StopProcessing();
                    }
                    break;
            }
            m.Result = IntPtr.Zero;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            for (int i = _figures.Count - 1; i >= 0; i--)
            {
                _figures[i].Draw(g);
            }
        }
    }
}
