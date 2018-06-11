using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /* public enum eState
     {
         eInactiv, eSelected, eHotTrack, eERR
     }
     */

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
        public const int dim = 382;

        //---
        public Basis()
        {
            pixels = new Pix[dim, dim];
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
}
