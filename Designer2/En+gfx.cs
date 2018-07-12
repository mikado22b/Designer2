using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Designer2
{
    #region Enum

    public enum eImage
    {
        iStd = 0, iSelected, iWarning, iFlash, iPoint, imLine, iCircle, iEllipse,
        iTriangle, iRectangle, iRoundRect, iPolygon, iArc, iColor, iICO, iList, iAddress,
        iArcUr, iArcDl, iArcDr, iArcUlFill, iArcUrFill, iArcDlFill, iArcDrFill,
        iCircleFill, iEllipseFill, iTriangleFill, iRectangleFill, iRoundRectFill,
        iPolygonFill, imLineClose, iPosition,
        iXpos, iYpos, iWidth, iHeight, iRadiusCircle, iEllipseRxRadius, iEllipseRyRadius,
        iEmpty, iRadiusArc, iUL, iUR, iDL, iDR, iSkin, iPositionWar, iXposWar, iYposWar,
        iICOwar, iPointWar, imLineWar, imLineCloseWar, iCircleWar, iCircleFillWar,
        iEllipseWar, iEllipseFillWar, iTriangleWar, iTriangleFillWar, iRectangleWar,
        iRectangleFillWar, iArcULwar, iArcURwar, iArcDLwar, iArcDRwar, iArcULfillWar,
        iArcURfillWar, iArcDLfillWar, iArcDRfillWar, iRadiusArcWar, iRadiusCircleWar,
        iEllipseRxRadiusWar, iEllipseRyRadiusWar, iWidthWar, iHeightWar, iListWar,
        iStdWar, iColorWar, iICOlist
    }

    //---B
    public enum eTest
    {
        errNoDef = 0, errCirRedToPoint, errTrRedToPoint, errTrDupPoint, errTrRedToLine,
        errRecRedToPoint, errRecWidth, errRecHeight, errArcRedToPoint,
        errmlDupPoint, errmlInline, errmlNoEfect, errOriXUnder0, errOriYUnder0,
        errEllipRXY
    }

    //---B
    public enum eColor
    {
        BackGround = 0, Workspace, Button, Bar, Border, Frame, Line, Text, SpecialText, Number,
        Highlight, Inactive, Warning, Alarm, High, Low, Hot, Mid, Could, Indicator,
        Black, White, LightGray, Gray, DarkGray, LightBlue, Blue, DarkBlue, LightRed, Red,
        DarkRed, LightGreen, Green, DarkGreen, LightYellow, Yellow, Orange, Brown, Purple, Pink,
        Def, Selected, SelectedPoint, HotPoint, None
    }

    //---B
    public enum eSkin
    { Light = 0, Dark, Gray, HiContrast, Rainbow, Lea, Fire, Lavender, GoldenBrown, Sea };

    //---B
    public enum eCommand
    {
        last = -100, end,
        dPoint = -50, dmLine, dclmLine, dCircle, fCircle,
        dTriangle, fTriangle, dRectangle, fRectangle,
        dArc, fArc, cColor, dEllipse, fEllipse, dRounRect,
        fRoundRect, dPolygon, fPolygon
    }

    //---B
    public enum eQuarter
    {
        qUL = 1, qUR, qDL, qDR
    }

    //---B
    public enum eDirection
    {
        iNone, iLeft, iUp, iRight, iDown
    }

    //---B
    [Flags]
    public enum eKey { kNone = 0, kLeft = 1, kUp = 2, kRight = 4, kDown = 8, kCenter = 16 }

    //---B
    public enum wVelocity { Slow, Medium, Fast }

    #endregion Enum

    //---R
    public class errClass
    {
        protected eTest eID;
        protected int[] eNo;
        protected string eText;

        //---
        public errClass(eTest ei, int[] en, string et)
        {
            eID = ei;
            eNo = en;
            eText = et;
        }

        //---
        public errClass()
        {
            eID = eTest.errNoDef;
            eNo = new int[0];
            eText = "Err no defined.";
        }

        //---
        public eTest ID
        {
            get
            {
                return eID;
            }
        }

        //---
        public int[] No
        {
            get
            {
                return eNo;
            }
        }

        //---
        public int count
        {
            get
            {
                return eNo.Length;
            }
        }

        //---
        public string Text
        {
            get
            {
                return eText;
            }
        }
    }

    //---Y
    public class Pix
    {
        //---
        protected List<int> it;

        public eColor color;
        public bool toDraw;

        //---
        public Pix()
        {
            it = new List<int>();
            clear();
        }

        //---
        public Pix(eColor cl)
        {
            it = new List<int>();
            color = cl;
            toDraw = true;
        }

        //---
        public void clear()
        {
            color = eColor.None;
            it.Clear();
            toDraw = true;
        }

        //---
        public void add(int nr)
        {
            if (!it.Contains(nr)) it.Add(nr);
        }

        //---
        public List<int> getItemList()
        {
            return it;
        }

        //---
        public static bool operator ==(Pix obj1, Pix obj2)
        {
            if (obj1.color == obj2.color &&
                obj1.it.Count == obj2.it.Count)
            {
                for (int i = 0; i < obj1.it.Count; i++)
                {
                    if (obj1.it[i] != obj2.it[i]) return false;
                }
                return true;
            }
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return false;
        }

        //---
        public static bool operator !=(Pix obj1, Pix obj2)
        {
            return !(obj1 == obj2);
        }

        //---
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Pix)obj);
        }

        //---
        public override int GetHashCode()
        {
            string hash = "";
            for (int i = 0; i < it.Count; i++)
            {
                hash += it[i].ToString();
            }
            unchecked
            {
                return (color.ToString() + hash).GetHashCode();
            }
        }
    }

    //---Y
    public class Basis
    {
        protected Pix[,] pixels;
        public const int dim = 383;

        //---
        public Basis()
        {
            pixels = new Pix[dim, dim];
            for (int x = 0; x < dim; x++)
                for (int y = 0; y < dim; y++)
                    pixels[x, y] = new Pix();
        }

        //---
        public Pix this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= dim | y >= dim)
                    return new Pix(eColor.None);
                return pixels[x, y];
            }
        }

        //---
        public void clear()
        {
            for (int i = 0; i < dim; i++)
            {
                for (int z = 0; z < dim; z++)
                {
                    pixels[i, z].clear();
                }
            }
        }

        //---
        public void painted()
        {
            for (int i = 0; i < dim; i++)
            {
                for (int z = 0; z < dim; z++)
                {
                    pixels[i, z].toDraw = false;
                }
            }
        }

        //---
        public static bool operator ==(Basis obj1, Basis obj2)
        {
            if (obj1 == obj2) return true;

            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return false;
        }

        //---
        public static bool operator !=(Basis obj1, Basis obj2)
        {
            return !(obj1 == obj2);
        }

        //---
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Basis)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (pixels.ToString()).GetHashCode();
            }
        }
    }

    //---Y
    public class ICOdraw
    {
        public const int step = 50;
        protected int[] vTab = { 1, 6, 36 };
        protected float pSize = 1;
        protected float offsetX = 0;
        protected float offsetY = 0;
        protected bool nodraw = false;
        protected int velocity;
        protected Color bGround;
        protected Color sel;
        protected Color selPoint;
        protected Color hPoint;

        protected Panel rulerX;
        protected Panel rulerY;
        protected Panel canvas;
        protected HScrollBar hbar;
        protected VScrollBar vbar;

        protected Basis fu;

        public delegate void pixHandler();

        public delegate void mouseMoveHandler(int x, int y);

        public event pixHandler OnPixSizeChange;

        public event mouseMoveHandler onMouseMove;

        //---
        public ICOdraw()
        {
            rulerX = new Panel();
            rulerY = new Panel();
            canvas = new Panel();
            hbar = new HScrollBar();
            vbar = new VScrollBar();
            bGround = Color.Black;
            sel = Color.Cyan;
            selPoint = Color.Yellow;
            hPoint = Color.Red;
            fu = new Basis();
            wheelVelocity = wVelocity.Medium;
        }

        //---
        public List<Control> loadItem(ContextMenuStrip cm)
        {
            int top = 56;
            int left = 2;
            int width = 205;
            int height = 312;
            int strip = 27;
            List<Control> tab = new List<Control>();
            rulerX.Left = left + strip;
            rulerX.Top = top + 1;
            rulerX.Height = strip;
            rulerX.Width = width - 2 * strip; ;
            rulerX.BorderStyle = BorderStyle.FixedSingle;
            rulerX.BackColor = SystemColors.ControlLight;
            rulerX.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tab.Add(rulerX);

            rulerY.Left = left + 1;
            rulerY.Top = top + strip; ;
            rulerY.Height = height - 2 * strip; ;
            rulerY.Width = strip;
            rulerY.BorderStyle = BorderStyle.FixedSingle;
            rulerY.BackColor = SystemColors.ControlLight;
            rulerY.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            tab.Add(rulerY);

            canvas.Left = left + strip;
            canvas.Top = top + strip;
            canvas.Height = height - 2 * strip;
            canvas.Width = width - 2 * strip;
            canvas.BorderStyle = BorderStyle.FixedSingle;
            canvas.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            canvas.BackColor = SystemColors.ControlLight;
            canvas.ContextMenuStrip = cm;
            canvas.SizeChanged += new EventHandler(panelChangeSize);
            //canvas.Invalidated += new InvalidateEventHandler(setBars);
            canvas.MouseWheel += new MouseEventHandler(wheel);
            canvas.MouseMove += new MouseEventHandler(MouseMove);
            canvas.MouseLeave += new EventHandler(mouseLeave);

            tab.Add(canvas);

            hbar.Left = left + strip + 1;
            hbar.Top = top + strip + canvas.Height;
            hbar.Height = strip;
            hbar.Width = canvas.Width - 2;
            hbar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            hbar.BackColor = SystemColors.ControlLight;
            hbar.Maximum = Basis.dim;
            hbar.Scroll += new ScrollEventHandler(scrollCanvasH);
            //hbar.SizeChanged += new EventHandler(setBars);
            tab.Add(hbar);

            vbar.Left = left + strip + canvas.Width;
            vbar.Top = top + strip + 1;
            vbar.Height = height - 2 * strip - 2;
            vbar.Width = strip;
            vbar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            vbar.BackColor = SystemColors.ControlLight;
            vbar.Maximum = Basis.dim;
            vbar.Scroll += new ScrollEventHandler(scrollCanvasV);
            //vbar.SizeChanged += new EventHandler(setBars);
            tab.Add(vbar);

            panelChangeSize(this, new EventArgs());
            //scrollCanvas(this, new ScrollEventArgs(ScrollEventType.EndScroll, 0));
            return tab;
        }

        //---
        public wVelocity wheelVelocity
        {
            set
            {
                velocity = vTab[(int)value];
            }
            get
            {
                if (velocity == vTab[(int)wVelocity.Fast]) return wVelocity.Fast;
                if (velocity == vTab[(int)wVelocity.Medium]) return wVelocity.Medium;
                velocity = vTab[(int)wVelocity.Slow];
                return wVelocity.Slow;
            }
        }

        //---
        public float pixSize
        {
            set
            {
                if (pSize != value)
                {
                    pSize = value;
                    hbar.Maximum = (int)(Basis.dim * pSize);
                    vbar.Maximum = (int)(Basis.dim * pSize);
                    OnPixSizeChange.Invoke();
                }
            }
            get
            {
                return pSize;
            }
        }

        //---
        public float shiftX
        {
            set
            {
                if (offsetX != value)
                {
                    offsetX = value;
                    forceDraw();
                }
            }
            get
            {
                return offsetX;
            }
        }

        //---
        public float shiftY
        {
            set
            {
                if (offsetY != value)
                {
                    offsetY = value;
                    forceDraw();
                }
            }
            get
            {
                return offsetY;
            }
        }

        //---
        public Color backGround
        {
            set
            {
                bool fl = false;
                if (bGround != value) fl = true;
                bGround = value;
                if (fl) forceDraw();
            }
            get
            {
                return bGround;
            }
        }

        //---
        public Color selected
        {
            set
            {
                bool fl = false;
                if (sel != value) fl = true;
                sel = value;
                if (fl) forceDraw();
            }
            get
            {
                return sel;
            }
        }

        //---
        public Color selectedPoint
        {
            set
            {
                bool fl = false;
                if (selPoint != value) fl = true;
                selPoint = value;
                if (fl) forceDraw();
            }
            get
            {
                return selPoint;
            }
        }

        //---
        public Color hotPoint
        {
            set
            {
                bool fl = false;
                if (hPoint != value) fl = true;
                hPoint = value;
                if (fl) forceDraw();
            }
            get
            {
                return hPoint;
            }
        }

        //---
        protected void forceDraw()
        {
            if (nodraw) return;
            draw(fu, true);
        }

        //---
        public void draw(Basis b, bool allDraw = false)
        {
            Graphics gfx = canvas.CreateGraphics();
            gfx.ScaleTransform(pSize, pSize);
            gfx.TranslateTransform(offsetX, offsetY);

            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            if (allDraw)
            {
                gfx.Clear(canvas.BackColor);
                gfx.FillRectangle(new SolidBrush(RGB32(eColor.None)), 0, 0, Basis.dim, Basis.dim);
            }

            for (int x = 0; x < Basis.dim; x++)
            {
                for (int y = 0; y < Basis.dim; y++)
                {
                    if (b[x, y].toDraw || (allDraw && b[x, y].color != eColor.None))
                    {
                        gfx.FillRectangle(new SolidBrush(RGB32(b[x, y].color)), x, y, 1, 1);
                    }
                }
            }
            fu = b;
        }

        //---
        public Color RGB32(eColor cl)
        {
            if (cl == eColor.Def) cl = color.Default;
            if (cl < eColor.Def)
            {
                UInt16 col = color.indColorTab[(int)cl];

                int r = (col >> 11) << 3;
                int g = ((col >> 5) & 63) << 2;
                int b = (col & 31) << 3;
                return Color.FromArgb(r, g, b);
            }
            if (cl == eColor.Selected) return selected;
            if (cl == eColor.SelectedPoint) return selectedPoint;
            if (cl == eColor.HotPoint) return hotPoint;

            return backGround;
        }

        //---
        private void panelChangeSize(object obj, EventArgs e)
        {
            nodraw = true;
            int x = canvas.Width;
            int y = canvas.Height;
            int picX = (int)(Basis.dim * pSize);
            int picY = (int)(Basis.dim * pSize);
            hbar.LargeChange = x;
            vbar.LargeChange = y;
            if (x > picX) scrollCanvasH(this, new ScrollEventArgs(ScrollEventType.EndScroll, 0));
            if (y > picY) scrollCanvasV(this, new ScrollEventArgs(ScrollEventType.EndScroll, 0));

            nodraw = false;
            forceDraw();
        }

        //---
        private void scrollCanvasH(object obj, ScrollEventArgs e)
        {
            int x = canvas.Width;

            int picX = (int)(Basis.dim * pSize);

            if (x >= picX)
            {
                shiftX = ((x - picX) / 2) / pSize;
            }
            else
            {
                shiftX = -hbar.Value / pSize;
            }
        }

        //---
        private void scrollCanvasV(object obj, ScrollEventArgs e)
        {
            int y = canvas.Height;
            int picY = (int)(Basis.dim * pSize);
            if (y >= picY)
            {
                shiftY = ((y - picY) / 2) / pSize;
            }
            else
            {
                shiftY = -vbar.Value / pSize;
            }
        }

        //---
        protected void wheel(object obj, MouseEventArgs e)
        {
            float siz = (pixSize - 1) * step;

            int w = SystemInformation.MouseWheelScrollDelta;
            w /= velocity;
            int d = e.Delta / w;

            siz += d;
            if (siz < 0) siz = 0;
            if (siz > 1000) siz = 1000;

            //pixSize = siz / step + 1;
            zoom(e.X, e.Y, siz / step + 1);
        }

        //---
        protected void MouseMove(object obj, MouseEventArgs e)
        {
            if (false)
            {
                if (canvas.Tag == null)
                {
                    canvas.Tag = new Point(e.X, e.Y);
                    return;
                }

                int dx = ((Point)canvas.Tag).X - e.X;
                int dy = ((Point)canvas.Tag).Y - e.Y;
                canvas.Tag = new Point(e.X, e.Y);
                int xb = hbar.Value;
                int yb = vbar.Value;
                xb += dx;
                yb += dy;
                if (xb < 0) xb = 0;
                if (yb < 0) yb = 0;
                if (xb > hbar.Maximum - hbar.LargeChange) xb = hbar.Maximum - hbar.LargeChange;
                if (yb > vbar.Maximum - vbar.LargeChange) yb = vbar.Maximum - vbar.LargeChange;
                hbar.Value = xb;
                vbar.Value = yb;
                scrollCanvasH(this, new ScrollEventArgs(ScrollEventType.EndScroll, 0));
                scrollCanvasV(this, new ScrollEventArgs(ScrollEventType.EndScroll, 0));
                return;
            }
            canvas.Tag = null;
            int x = e.X;
            int y = e.Y;
            x = (int)(x / pSize - 127 - offsetX);
            y = (int)(y / pSize - 127 - offsetY);

            if (x < -127 || x > 254) x = -1000;
            if (y < -127 || y > 254) x = -1000;
            onMouseMove.Invoke(x, y);
        }

        //---
        protected void mouseLeave(object obj, EventArgs e)
        {
            onMouseMove.Invoke(-1000, -1000);
        }

        //---
        public void zoom(int x, int y, float z)
        {
            nodraw = true;

            double xbarpr = (double)x / canvas.Width;
            double xpicpr = (hbar.Value + hbar.LargeChange * xbarpr) / hbar.Maximum;

            double ybarpr = (double)y / canvas.Height;
            double ypicpr = (vbar.Value + vbar.LargeChange * ybarpr) / vbar.Maximum;

            pixSize = z;
            hbar.Maximum = (int)(Basis.dim * pSize);
            vbar.Maximum = (int)(Basis.dim * pSize);

            int hPosition = (int)(Math.Round(hbar.Maximum * xpicpr - hbar.LargeChange * xbarpr, 0));
            int vPosition = (int)(Math.Round(vbar.Maximum * ypicpr - vbar.LargeChange * ybarpr, 0));

            if (hPosition > hbar.Maximum - hbar.LargeChange) hPosition = hbar.Maximum - hbar.LargeChange;
            if (hPosition < 0) hPosition = 0;

            if (vPosition > vbar.Maximum - vbar.LargeChange) vPosition = vbar.Maximum - vbar.LargeChange;
            if (vPosition < 0) vPosition = 0;

            hbar.Value = hPosition;
            vbar.Value = vPosition;
            scrollCanvasH(this, new ScrollEventArgs(ScrollEventType.EndScroll, 0));
            scrollCanvasV(this, new ScrollEventArgs(ScrollEventType.EndScroll, 0));

            x = (int)(x / pSize - 127 - offsetX);
            y = (int)(y / pSize - 127 - offsetY);

            if (x < -127 || x > 254) x = -1000;
            if (y < -127 || y > 254) x = -1000;
            onMouseMove.Invoke(x, y);
            nodraw = false;
            forceDraw();
        }
    }
}
