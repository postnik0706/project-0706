using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace indicator
{
    public enum Direction { CLOCKWISE, ANTICLOCKWISE };
    
    public partial class CircularIndicator : UserControl
    {
        private PointF m_centerPoint;
        private Pen m_Pen;
        private int m_spokesCount;
        private const float MINIMUM_PEN_WIDTH = 1;
        private const int MINIMUM_INNER_RADIUS = 5;
        private const int MINIMUM_OUTER_RADIUS = 8;
        private int m_startAngle;
        private int m_alphaStartValue;
        private Color m_tickColor;
        private Direction m_Rotation;
        private int m_angleIncrement;
        private int m_alphaDecrement;
        private bool m_Terminated;
        
        public CircularIndicator()
        {
            this.DoubleBuffered = true;
            InitializeComponent();

            this.BackColor = Color.Transparent;
            m_spokesCount = 12;
            m_startAngle = 0;
            m_tickColor = Color.DarkGreen;
            m_Pen = new Pen(new SolidBrush(m_tickColor));
            m_Pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            m_Pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            m_alphaStartValue = 255;
            m_Rotation = Direction.CLOCKWISE;
            m_angleIncrement = (int)(360 / m_spokesCount);
            m_alphaDecrement = (int)( (m_alphaStartValue - 15) / m_spokesCount);
            m_Terminated = false;
        }

        private double DegreeToRadian(double Angle)
        {
            return Math.PI * Angle / 180;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ThreadPool.QueueUserWorkItem((t) =>
            {
                while (!m_Terminated)
                {
                    if (m_Rotation == Direction.CLOCKWISE)
                    {
                        m_startAngle += m_angleIncrement;
                        if (m_startAngle >= 360)
                            m_startAngle = 0;
                    }
                    else
                    {
                        m_startAngle -= m_angleIncrement;
                        if (m_startAngle <= -360)
                            m_startAngle = 0;
                    }

                    if (this.Created)
                        BeginInvoke((ThreadStart)(() => { Invalidate(); }));
                    Thread.Sleep(40);
                }
            });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            int width = Math.Min(this.Width, this.Height);
            m_centerPoint = new PointF(Width / 2, Height / 2);
            m_Pen.Width = (int)width / 15;
            if (m_Pen.Width < MINIMUM_PEN_WIDTH)
                m_Pen.Width = MINIMUM_PEN_WIDTH;

            int innerRadius = (int)(width * (140 / (float)800));
            if (innerRadius < MINIMUM_INNER_RADIUS)
                innerRadius = MINIMUM_INNER_RADIUS;
            int outerRadius = (int)(width * (250 / (float)800));
            if (outerRadius < MINIMUM_OUTER_RADIUS)
                outerRadius = MINIMUM_OUTER_RADIUS;


            int alpha = m_alphaStartValue;
            int angle = m_startAngle;
            for (int i = 0; i < m_spokesCount; i++)
            {
                PointF pt1 = new PointF(innerRadius * (float)Math.Cos(DegreeToRadian(angle)),
                    innerRadius * (float)Math.Sin(DegreeToRadian(angle)));
                PointF pt2 = new PointF(outerRadius * (float)Math.Cos(DegreeToRadian(angle)),
                    outerRadius * (float)Math.Sin(DegreeToRadian(angle)));

                pt1.X += m_centerPoint.X;
                pt1.Y += m_centerPoint.Y;
                pt2.X += m_centerPoint.X;
                pt2.Y += m_centerPoint.Y;

                m_Pen.Color = Color.FromArgb(alpha, m_tickColor);
                e.Graphics.DrawLine(m_Pen, pt1, pt2);

                if (m_Rotation == Direction.CLOCKWISE)
                    angle -= m_angleIncrement;
                else
                    angle += m_angleIncrement;

                alpha -= m_alphaDecrement;
                if (alpha < 0)
                    alpha = m_alphaStartValue;
            }
        }

        protected override void DestroyHandle()
        {
            m_Terminated = true;
            base.DestroyHandle();
        }
    }
}
