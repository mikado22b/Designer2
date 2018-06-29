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
        protected float scale = 0.5f;
        protected int offsetX = 0;
        protected int offsetY = 0;
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
            selPoint = Color.Navy;
            hPoint = Color.Red;
            fu = new Basis();
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
            tab.Add(canvas);

            hbar.Left = left + strip + 1;
            hbar.Top = top + strip + canvas.Height;
            hbar.Height = strip;
            hbar.Width = canvas.Width - 2;
            hbar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            hbar.BackColor = SystemColors.ControlLight;
            hbar.Maximum = 382;
            tab.Add(hbar);

            vbar.Left = left + strip + canvas.Width;
            vbar.Top = top + strip + 1;
            vbar.Height = height - 2 * strip - 2;
            vbar.Width = strip;
            vbar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            vbar.BackColor = SystemColors.ControlLight;
            vbar.Maximum = 382;
            tab.Add(vbar);

            return tab;
        }

        //---
        public float pixSize
        {
            set
            {
                bool fl = false;
                if (scale != value) fl = true;
                scale = value;
                if (fl) forceDraw();
            }
            get
            {
                return scale;
            }
        }

        //---
        public int shiftX
        {
            set
            {
                bool fl = false;
                if (offsetX != value) fl = true;
                offsetX = value;
                if (fl) forceDraw();
            }
            get
            {
                return offsetX;
            }
        }

        //---
        public int shiftY
        {
            set
            {
                bool fl = false;
                if (offsetY != value) fl = true;
                offsetY = value;
                if (fl) forceDraw();
            }
            get { return offsetY; }
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
                sel = value;
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
                selPoint = value;
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
                hPoint = value;
            }
            get
            {
                return hPoint;
            }
        }

        //---
        protected void forceDraw()
        {
            draw(fu, true);
        }

        //---
        public void draw(Basis b, bool allDraw = false)
        {
            Graphics gfx = canvas.CreateGraphics();
            gfx.ScaleTransform(scale, scale);
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
    }
}
