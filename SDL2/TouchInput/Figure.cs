using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ICTest
{
    public class Figure : BaseHandler
    {
        Color _color;
        List<PointF> _points = new List<PointF>();
        List<PointF> _basePoints = new List<PointF>();
        List<PointF> _baseXY = new List<PointF>();
        List<double> _angles = new List<double>();
        PointF _center = PointF.Empty;
        TouchForm _form;

        bool _pivot;

        public Figure(TouchForm form, Color color, bool pivot, bool rails)
            : base()
        {
            _form = form;
            _color = color;
            _pivot = pivot;

            Win32.INTERACTION_CONTEXT_CONFIGURATION[] cfg = new Win32.INTERACTION_CONTEXT_CONFIGURATION[]
            {
                new Win32.INTERACTION_CONTEXT_CONFIGURATION(Win32.INTERACTION.MANIPULATION,
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION |
                    //Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_TRANSLATION_X |
                    //Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_TRANSLATION_Y |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_SCALING |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_ROTATION |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_TRANSLATION_INERTIA |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_ROTATION_INERTIA |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_SCALING_INERTIA),

                new Win32.INTERACTION_CONTEXT_CONFIGURATION(Win32.INTERACTION.TAP,
                    Win32.INTERACTION_CONFIGURATION_FLAGS.TAP |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.TAP_DOUBLE)
            };

            if (!pivot)
            {
                cfg[0].Enable |=
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_TRANSLATION_X |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_TRANSLATION_Y;
            }

            if (rails)
            {
                cfg[0].Enable |=
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_RAILS_X |
                    Win32.INTERACTION_CONFIGURATION_FLAGS.MANIPULATION_RAILS_Y;
            }

            Win32.SetInteractionConfigurationInteractionContext(Context, cfg.Length, cfg);
        }

        internal override void ProcessEvent(InteractionOutput output)
        {
            if (output.Data.Interaction == Win32.INTERACTION.TAP)
            {
                if (output.Data.Tap.Count == 2)
                {
                    ResetPoints();
                    _form.Invalidate();
                }
            }
            else if (output.Data.Interaction == Win32.INTERACTION.MANIPULATION)
            {
                if (output.IsBegin())
                {
                    _baseXY.Clear();
                    _center = PointF.Empty;
                    foreach (PointF pt in _points)
                    {
                        _baseXY.Add(new PointF(pt.X, pt.Y));
                        _center.X += pt.X;
                        _center.Y += pt.Y;
                    }
                    _center.X /= _points.Count;
                    _center.Y /= _points.Count;
                }
                else
                {
                    Win32.MANIPULATION_TRANSFORM mt = output.Data.Manipulation.Cumulative;
                    PointF ct = _center;
                    ct.X += mt.TranslationX;
                    ct.Y += mt.TranslationY;
                    for (int i = 0; i < _points.Count; i++)
                    {
                        PointF pt = _baseXY[i];
                        pt.X += mt.TranslationX;
                        pt.Y += mt.TranslationY;

                        float deltaX = pt.X - ct.X;
                        float deltaY = pt.Y - ct.Y;
                        double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                        double a = Math.Atan2(deltaY, deltaX) + mt.Rotation;
                        pt.X = Convert.ToSingle(Math.Cos(a) * r) + ct.X;
                        pt.Y = Convert.ToSingle(Math.Sin(a) * r) + ct.Y;

                        pt.X = mt.Scale * (pt.X - ct.X) + ct.X;
                        pt.Y = mt.Scale * (pt.Y - ct.Y) + ct.Y;

                        _points[i] = pt;
                    }

                    _form.Invalidate();
                }
            }
        }

        public bool Pivot
        {
            get { return _pivot; }
        }

        public List<PointF> Points
        {
            get { return _points; }
        }

        public void AddPoint(float x, float y)
        {
            _points.Add(new PointF(x, y));
            _basePoints.Add(new PointF(x, y));
        }

        public void ResetPoints()
        {
            for (int i = 0; i < _basePoints.Count; i++)
            {
                _points[i] = _basePoints[i];
            }
        }

        public void BringToFront()
        {
            int index = _form.Figures.IndexOf(this);
            if (index > 0)
            {
                _form.Figures.RemoveAt(index);
                _form.Figures.Insert(0, this);
            }
        }

        public bool HitTest(float x, float y)
        {
            if (_points.Count < 3)
            {
                return false;
            }
            _angles.Clear();
            foreach (PointF pt in _points)
            {
                _angles.Add(Math.Atan2(pt.Y - y, pt.X - x));
            }
            double min1 = _angles[0];
            double max1 = min1;
            double min2 = min1;
            if (min2 < 0.0)
            {
                min2 += Math.PI * 2;
            }
            double max2 = min2;
            for (int i = 1; i < _angles.Count; i++)
            {
                double a = _angles[i];
                if (a < min1)
                    min1 = a;
                if (a > max1)
                    max1 = a;
                if (a < 0.0)
                    a += Math.PI * 2;
                if (a < min2)
                    min2 = a;
                if (a > max2)
                    max2 = a;
            }
            return Math.Min(max1 - min1, max2 - min2) > Math.PI;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(_color, 3f);
            for (int i = _points.Count - 1; i >= 1; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    g.DrawLine(pen, _points[i], _points[j]);
                }
            }
            pen.Dispose();
        }
    }
}
