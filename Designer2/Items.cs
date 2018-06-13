using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Designer2
{
    #region Items def

    public class Item
    {
        //---
        protected string name;

        protected const int il = 4;
        protected const int nameWidth = 96;
        protected const int labelWidth = 34;
        protected const int nudWidth = 50;
        protected const int MIN = 0;
        protected const int MAX = 127;
        protected const int buttonSize = 35;
        protected const int picWidth = 34;
        protected const int picHeight = 20;

        //---
        public Item()
        {
            Random r = new Random();
            name = r.Next(1000, 9999).ToString();
        }

        //---
        public string getName()
        {
            return "_" + name;
        }

        //---
        public virtual TreeNode property()
        {
            TreeNode n = new TreeNode(name);
            n.Tag = false;
            return n;
        }

        //---
        public override string ToString()
        {
            return "Item object";
        }

        //---
        public virtual int itemLenght()
        {
            return 0;
        }

        //---
        public virtual eColor draw(ref Basis canva, eColor cl, int tag)
        {
            return cl;
        }

        //---
        protected void drawPixel(ref Basis canva, int x, int y, eColor cl, int tag)
        {
            if (x < 0 || y < 0 || x >= Basis.dim || y >= Basis.dim) return;
            canva[x, y].color = cl;
            canva[x, y].add(tag);
        }

        //---
        protected void drawLine(ref Basis canva, Point p1, Point p2, eColor cl, int tag)
        {
            int x1 = p1.X;
            int y1 = p1.Y;
            int x2 = p2.X;
            int y2 = p2.Y; ;
            drawLine(ref canva, x1, y1, x2, y2, cl, tag);
        }

        //---
        protected void drawLine(ref Basis canva, int x1, int y1, int x2, int y2, eColor cl, int tag)
        {
            int d, dx, dy, ai, bi, xi, yi;
            int x = x1, y = y1;

            if (x1 < x2)
            {
                xi = 1;
                dx = x2 - x1;
            }
            else
            {
                xi = -1;
                dx = x1 - x2;
            }

            if (y1 < y2)
            {
                yi = 1;
                dy = y2 - y1;
            }
            else
            {
                yi = -1;
                dy = y1 - y2;
            }
            drawPixel(ref canva, x, y, cl, tag);
            // canva[x, y].color = cl;

            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;

                while (x != x2)
                {
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;

                        drawPixel(ref canva, x, y, cl, tag);
                        // canva[x, y].color = cl;
                    }
                }
            }
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;

                while (y != y2)
                {
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    drawPixel(ref canva, x, y, cl, tag);
                    //canva[x, y].color = cl;
                }
            }
        }

        //---
        protected void drawCircleHelper(ref Basis canva, int x0, int y0, int r,
                                                        eQuarter q, eColor cl, int tag)
        {
            int f = 1 - r;
            int ddF_x = 1;
            int ddF_y = -r - r;
            int x = 0;

            while (x < r)
            {
                if (f >= 0)
                {
                    r--;
                    ddF_y += 2;
                    f += ddF_y;
                }
                x++;
                ddF_x += 2;
                f += ddF_x;

                switch (q)
                {
                    case eQuarter.qUL:
                        drawPixel(ref canva, x0 - r, y0 - x, cl, tag);
                        drawPixel(ref canva, x0 - x, y0 - r, cl, tag);
                        break;

                    case eQuarter.qUR:
                        drawPixel(ref canva, x0 + x, y0 - r, cl, tag);
                        drawPixel(ref canva, x0 + r, y0 - x, cl, tag);
                        break;

                    case eQuarter.qDL:
                        drawPixel(ref canva, x0 - r, y0 + x, cl, tag);
                        drawPixel(ref canva, x0 - x, y0 + r, cl, tag);
                        break;

                    case eQuarter.qDR:
                        drawPixel(ref canva, x0 + x, y0 + r, cl, tag);
                        drawPixel(ref canva, x0 + r, y0 + x, cl, tag);
                        break;

                    default:
                        break;
                }
            }
        }

        //---
        protected void fillCircleHelper(ref Basis canva, int x0, int y0, int r,
                                                uint cornername, int delta, eColor cl, int tag)
        {
            int f = 1 - r;
            int ddF_x = 1;
            int ddF_y = -r - r;
            int x = 0;

            delta++;
            while (x < r)
            {
                if (f >= 0)
                {
                    r--;
                    ddF_y += 2;
                    f += ddF_y;
                }
                x++;
                ddF_x += 2;
                f += ddF_x;

                if ((cornername & 0x1) == 1)
                {
                    drawLine(ref canva, x0 + x, y0 - r, (x0 + x) + r + r + delta, y0 - r, cl, tag);
                    drawLine(ref canva, x0 + r, y0 - x, (x0 + r) + x + x + delta, y0 - x, cl, tag);
                }
                if ((cornername & 0x2) == 2)
                {
                    drawLine(ref canva, x0 - x, y0 - r, (x0 - x) + r + r + delta, y0 - r, cl, tag);
                    drawLine(ref canva, x0 - r, y0 - x, (x0 - r) + x + x + delta, y0 - x, cl, tag);
                }
            }
        }

        //---
        private static void swap(ref int x, ref int y)
        {
            int t = x;
            x = y;
            y = t;
        }

        //---
        protected void fillTriangle(ref Basis canva, Point p0, Point p1, Point p2, eColor cl, int tag)
        {
            int x1 = p0.X;
            int y1 = p0.Y;
            int x2 = p1.X;
            int y2 = p1.Y;
            int x3 = p2.X;
            int y3 = p2.Y;
            // Fill a triangle - uses Bressenham method (from TFT_HX8357_Due.cpp for compatibility appearance)
            int t1x, t2x, y, minx, maxx, t1xp, t2xp;
            bool changed1 = false;
            bool changed2 = false;
            int signx1, signx2, dx1, dy1, dx2, dy2;
            uint e1, e2;
            // Sort vertices
            if (y1 > y2) { swap(ref y1, ref y2); swap(ref x1, ref x2); }
            if (y1 > y3) { swap(ref y1, ref y3); swap(ref x1, ref x3); }
            if (y2 > y3) { swap(ref y2, ref y3); swap(ref x2, ref x3); }

            t1x = t2x = x1; y = y1;   // Starting points

            dx1 = x2 - x1; if (dx1 < 0) { dx1 = -dx1; signx1 = -1; } else signx1 = 1;
            dy1 = y2 - y1;

            dx2 = x3 - x1; if (dx2 < 0) { dx2 = -dx2; signx2 = -1; } else signx2 = 1;
            dy2 = y3 - y1;

            if (dy1 > dx1)
            {   // swap values
                swap(ref dx1, ref dy1);
                changed1 = true;
            }
            if (dy2 > dx2)
            {   // swap values
                swap(ref dy2, ref dx2);
                changed2 = true;
            }

            e2 = (uint)dx2 >> 1;
            // Flat top, just process the second half
            if (y1 == y2) goto next;
            e1 = (uint)dx1 >> 1;

            for (uint i = 0; i < dx1;)
            {
                t1xp = 0; t2xp = 0;
                if (t1x < t2x) { minx = t1x; maxx = t2x; }
                else { minx = t2x; maxx = t1x; }
                // process first line until y value is about to change
                while (i < dx1)
                {
                    i++;
                    e1 += (uint)dy1;
                    while (e1 >= dx1)
                    {
                        e1 -= (uint)dx1;
                        if (changed1) t1xp = signx1;//t1x += signx1;
                        else goto next1;
                    }
                    if (changed1) break;
                    else t1x += signx1;
                }
                // Move line
                next1:
                // process second line until y value is about to change
                while (true)
                {
                    e2 += (uint)dy2;
                    while (e2 >= dx2)
                    {
                        e2 -= (uint)dx2;
                        if (changed2) t2xp = signx2;//t2x += signx2;
                        else goto next2;
                    }
                    if (changed2) break;
                    else t2x += signx2;
                }
                next2:
                if (minx > t1x) minx = t1x; if (minx > t2x) minx = t2x;
                if (maxx < t1x) maxx = t1x; if (maxx < t2x) maxx = t2x;
                drawLine(ref canva, minx, y, minx + maxx - minx, y, cl, tag);
                // Draw line from min to max points found on the y Now increase y
                if (!changed1) t1x += signx1;
                t1x += t1xp;
                if (!changed2) t2x += signx2;
                t2x += t2xp;
                y += 1;
                if (y == y2) break;
            }
            next:
            // Second half
            dx1 = x3 - x2; if (dx1 < 0) { dx1 = -dx1; signx1 = -1; } else signx1 = 1;
            dy1 = y3 - y2;
            t1x = x2;

            if (dy1 > dx1)
            {   // swap values
                swap(ref dy1, ref dx1);
                changed1 = true;
            }
            else changed1 = false;

            e1 = (uint)dx1 >> 1;

            for (uint i = 0; i <= dx1; i++)
            {
                t1xp = 0; t2xp = 0;
                if (t1x < t2x) { minx = t1x; maxx = t2x; }
                else { minx = t2x; maxx = t1x; }
                // process first line until y value is about to change
                while (i < dx1)
                {
                    e1 += (uint)dy1;
                    while (e1 >= dx1)
                    {
                        e1 -= (uint)dx1;
                        if (changed1) { t1xp = signx1; break; }//t1x += signx1;
                        else goto next3;
                    }
                    if (changed1) break;
                    else t1x += signx1;
                    if (i < dx1) i++;
                }
                next3:
                // process second line until y value is about to change
                while (t2x != x3)
                {
                    e2 += (uint)dy2;
                    while (e2 >= dx2)
                    {
                        e2 -= (uint)dx2;
                        if (changed2) t2xp = signx2;
                        else goto next4;
                    }
                    if (changed2) break;
                    else t2x += signx2;
                }
                next4:

                if (minx > t1x) minx = t1x; if (minx > t2x) minx = t2x;
                if (maxx < t1x) maxx = t1x; if (maxx < t2x) maxx = t2x;
                drawLine(ref canva, minx, y, minx + maxx - minx, y, cl, tag);
                // Draw line from min to max points found on the y

                // Now increase y
                if (!changed1) t1x += signx1;
                t1x += t1xp;
                if (!changed2) t2x += signx2;
                t2x += t2xp;
                y += 1;
                if (y > y3) break;
            }
        }

        //---
        public virtual void activePoint(int n)
        {
        }

        //---
        public virtual eKey move(eDirection dir)
        {
            return eKey.kNone;
        }

        //---
        public virtual List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();
            Label l = new Label();
            l.Text = "Item error.";
            tab.Add(l);
            return tab;
        }

        //---
        protected void loadInit(ref PictureBox pb, Bitmap bmp, ref Label name, string n)
        {
            pb.Height = picHeight;
            pb.Width = picWidth;
            bmp.MakeTransparent(Color.White);
            pb.Image = bmp;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            name.Text = n;
            name.Left = pb.Width;
            name.Width = nameWidth;
        }

        //---
        public virtual void move(Object sender, EventArgs e)
        { }

        //---
        public virtual Rectangle getRect()
        {
            return new Rectangle();
        }

        //---
        public virtual List<errClass> test()
        {
            return new List<errClass>();
        }

        //---
        public virtual bool parse(List<byte> sp, ref int i)
        {
            return false;
        }

        //---
        public static bool operator ==(Item obj1, Item obj2)
        {
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
        public static bool operator !=(Item obj1, Item obj2)
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

            return obj.GetType() == GetType() && Equals((Item)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return name.GetHashCode();
            }
        }
    }

    //---G
    public class point : Item
    {
        //---
        protected Point p = new Point();

        protected NumericUpDown nudx;
        protected NumericUpDown nudy;

        //---
        public point()
        {
            p.X = MIN;
            p.Y = MIN;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            name.Tag = true;
            name.ImageIndex = (int)eImage.iPoint;
            name.SelectedImageIndex = (int)eImage.iPoint;
            name.Nodes.Add("X= " + p.X);
            name.Nodes[0].ImageIndex = (int)eImage.iXpos;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iXpos;
            name.Nodes.Add("Y= " + p.Y);
            name.Nodes[1].ImageIndex = (int)eImage.iYpos;
            name.Nodes[1].SelectedImageIndex = (int)eImage.iYpos;

            return name;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            eColor cl2 = cl;
            if (cl2 == eColor.Selected) cl2 = eColor.HotPoint;
            drawPixel(ref canva, p.X, p.Y, cl2, tag);

            return cl;
        }

        //---
        public override eKey move(eDirection dir)
        {
            return base.move(dir);
        }

        //---
        public override Rectangle getRect()
        {
            return new Rectangle(p.X, p.Y, 1, 1);
        }

        //---
        public override string ToString()
        {
            return ((int)eCommand.dPoint).ToString() + ", " +
                p.X.ToString() + ", " + p.Y.ToString() + ", ";
        }

        //---
        public override int itemLenght()
        {
            return 3;
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();

            PictureBox pb = new PictureBox();
            Label name = new Label();
            Bitmap tr = Properties.Resources.Point;
            loadInit(ref pb, tr, ref name, "Point" + getName());

            tab.Add(pb);
            tab.Add(name);

            Label x = new Label();
            x.Text = "X=";
            x.Top = name.Top + name.Height * 3 + il;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = il;
            y.Width = x.Width;
            tab.Add(y);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = p.X;
            nudx.ValueChanged += new EventHandler(valueXchange);

            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = y.Width + il;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = p.Y;
            nudy.ValueChanged += new EventHandler(valueYchange);

            tab.Add(nudy);

            Button up = new Button();
            up.Text = "";
            up.Height = nudx.Height + 6;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = x.Top + x.Height + il / 2 - (int)(up.Height * 1.7);
            up.Left = nudy.Left + nudy.Width + il * 2 + up.Width;
            up.Tag = eDirection.iDown;
            up.Click += new EventHandler(valueYchange);

            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);

            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iUp;
            down.Click += new EventHandler(valueYchange);

            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);

            tab.Add(left);

            PictureBox pbc = new PictureBox();
            pbc.Height = up.Height;
            pbc.Width = up.Width;
            tr = Properties.Resources.Point;
            tr.MakeTransparent(Color.White);
            pbc.Image = tr;
            pbc.SizeMode = PictureBoxSizeMode.Zoom;
            pbc.Top = right.Top;
            pbc.Left = up.Left;
            tab.Add(pbc);

            return tab;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                p.X = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (p.X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else p.X--;
                }
                else
                {
                    if (p.X >= MAX)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else p.X++;
                }
            }
            nudx.Value = p.X;
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                p.Y = (int)(sender as NumericUpDown).Value;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iDown)
                {
                    if (p.Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else p.Y--;
                }
                else
                {
                    if (p.Y >= 127)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else p.Y++;
                }
            }
            nudy.Value = p.Y;
        }

        //---
        public override List<errClass> test()
        {
            return new List<errClass>();
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(point obj1, point obj2)
        {
            if (obj1.p == obj2.p) return true;

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
        public static bool operator !=(point obj1, point obj2)
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

            return obj.GetType() == GetType() && Equals((point)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (p.X.ToString() + p.Y.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class mLine : Item
    {
        //---
        protected List<Point> lPt = new List<Point>();

        protected int active = -1;
        protected int selectedPoint;
        protected bool closeLine = false;

        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected ListBox lbml;
        protected Button bd;
        protected PictureBox pb;
        protected PictureBox centerMl;

        //---
        public mLine()
        {
            lPt.Clear();
            lPt.Add(new Point(MIN, MIN));
            lPt.Add(new Point(5, 5));
            selectedPoint = 0;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            lbml = new ListBox();
            bd = new Button();
            pb = new PictureBox();
            centerMl = new PictureBox();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            eImage im;
            eImage im2;
            List<errClass> errL = test();
            bool ok = errL.Count == 0;
            name.Tag = ok; ;
            string clo;
            if (closeLine)
            {
                clo = "Yes";
                if (ok) im = eImage.imLineClose;
                else im = eImage.imLineCloseWar;
                im2 = eImage.imLineClose;
            }
            else
            {
                clo = "No";
                if (ok) im = eImage.imLine;
                else im = eImage.imLineWar;
                im2 = eImage.imLine;
            }
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            bool lpWar = false;

            List<bool> warP = new List<bool>();
            for (int i = 0; i < lPt.Count; i++)
            {
                warP.Add(false);
            }
            for (int i = 0; i < errL.Count; i++)
            {
                if (errL[i].ID == eTest.errmlDupPoint || errL[i].ID == eTest.errmlInline)
                {
                    for (int z = 0; z < errL[i].count; z++)
                    {
                        warP[errL[i].No[z]] = true;
                    }

                    lpWar = true;
                }
                if (errL[i].ID == eTest.errmlNoEfect)
                {
                    im2 = eImage.imLineCloseWar;
                }
            }

            name.Nodes.Add("Point count = " + lPt.Count);
            if (lpWar)
            {
                name.Nodes[0].ImageIndex = (int)eImage.iListWar;
                name.Nodes[0].SelectedImageIndex = (int)eImage.iListWar;
            }
            else
            {
                name.Nodes[0].ImageIndex = (int)eImage.iList;
                name.Nodes[0].SelectedImageIndex = (int)eImage.iList;
            }

            for (int i = 0; i < lPt.Count; i++)
            {
                name.Nodes[0].Nodes.Add("Point #" + i);
                name.Nodes[0].Nodes[i].Nodes.Add("X= " + lPt[i].X);
                name.Nodes[0].Nodes[i].Nodes.Add("Y= " + lPt[i].Y);
                if (warP[i])
                {
                    name.Nodes[0].Nodes[i].ImageIndex = (int)eImage.iPointWar;
                    name.Nodes[0].Nodes[i].SelectedImageIndex = (int)eImage.iPointWar;

                    name.Nodes[0].Nodes[i].Nodes[0].ImageIndex = (int)eImage.iXposWar;
                    name.Nodes[0].Nodes[i].Nodes[0].SelectedImageIndex = (int)eImage.iXposWar;

                    name.Nodes[0].Nodes[i].Nodes[1].ImageIndex = (int)eImage.iYposWar;
                    name.Nodes[0].Nodes[i].Nodes[1].SelectedImageIndex = (int)eImage.iYposWar;
                }
                else
                {
                    name.Nodes[0].Nodes[i].ImageIndex = (int)eImage.iPoint;
                    name.Nodes[0].Nodes[i].SelectedImageIndex = (int)eImage.iPoint;

                    name.Nodes[0].Nodes[i].Nodes[0].ImageIndex = (int)eImage.iXpos;
                    name.Nodes[0].Nodes[i].Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

                    name.Nodes[0].Nodes[i].Nodes[1].ImageIndex = (int)eImage.iYpos;
                    name.Nodes[0].Nodes[i].Nodes[1].SelectedImageIndex = (int)eImage.iYpos;
                }
            }

            name.Nodes.Add("Closed mLine : " + clo);
            name.Nodes[1].ImageIndex = (int)im2;
            name.Nodes[1].SelectedImageIndex = (int)im2;
            return name;
        }

        //---
        public override string ToString()
        {
            if (lPt.Count < 1) return "";
            string l;
            if (closeLine) l = ((int)eCommand.dclmLine).ToString();
            else l = ((int)eCommand.dmLine).ToString();

            for (int i = 0; i < lPt.Count; i++)
            {
                l += ", ";
                l += lPt[i].X.ToString() + ", ";
                l += lPt[i].Y.ToString();
            }
            l += ", ";

            return l;
        }

        //---
        public override int itemLenght()
        {
            return 1 + lPt.Count * 2;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            for (int i = 1; i < lPt.Count; i++)
            {
                drawLine(ref canva, lPt[i - 1], lPt[i], cl, tag);
            }
            if (closeLine && lPt.Count > 2)
                drawLine(ref canva, lPt[lPt.Count - 1], lPt[0], cl, tag);

            if (cl == eColor.Selected)
            {
                for (int i = 0; i < lPt.Count; i++)
                {
                    drawPixel(ref canva, lPt[i].X, lPt[i].Y, eColor.SelectedPoint, tag);
                    if (selectedPoint == i)
                        drawPixel(ref canva, lPt[i].X, lPt[i].Y, eColor.HotPoint, tag);
                }
            }

            return cl;
        }

        //---
        public override void activePoint(int n)
        {
            active = n;
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();

            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr;
            if (closeLine) tr = Properties.Resources.mLineClosed;
            else tr = Properties.Resources.mLine;
            loadInit(ref pb, tr, ref name, "mLine" + getName());

            tab.Add(pb);
            tab.Add(name);

            Label x = new Label();
            x.Text = "X=";
            x.Top = name.Top + name.Height + il;
            x.Left = il;
            x.Width = labelWidth - 3 * il;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = x.Left;
            y.Width = x.Width;
            tab.Add(y);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Left + x.Width + il;
            nudx.Width = nudWidth - il * 2;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = this.lPt[selectedPoint].X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = nudx.Left;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = this.lPt[selectedPoint].Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            Button up = new Button();
            up.Text = "";
            up.Height = 25;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = nudx.Top;
            up.Left = nudx.Left + nudx.Width + up.Width + il;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            PictureBox center = new PictureBox();
            center.Text = up.Text;
            center.Height = up.Height;
            center.Width = up.Width;
            tr = Properties.Resources.Point;
            tr.MakeTransparent(Color.White);
            center.Image = tr;
            center.SizeMode = PictureBoxSizeMode.Zoom;
            center.Top = right.Top;
            center.Left = up.Left;
            tab.Add(center);

            Button upMl = new Button();
            upMl.Text = "";
            upMl.Height = 25;
            upMl.Width = upMl.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            upMl.BackgroundImage = tr;
            upMl.BackgroundImageLayout = ImageLayout.Zoom;
            upMl.Top = down.Top + down.Height + il; ;
            upMl.Left = up.Left;
            upMl.Tag = eDirection.iUp;
            upMl.Click += new EventHandler(move);
            tab.Add(upMl);

            Button rightMl = new Button();
            rightMl.Text = upMl.Text;
            rightMl.Height = upMl.Height;
            rightMl.Width = upMl.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            rightMl.BackgroundImage = tr;
            rightMl.BackgroundImageLayout = ImageLayout.Zoom;
            rightMl.Top = upMl.Top + upMl.Height;
            rightMl.Left = upMl.Left + upMl.Width;
            rightMl.Tag = eDirection.iRight;
            rightMl.Click += new EventHandler(move);
            tab.Add(rightMl);

            Button downMl = new Button();
            downMl.Text = upMl.Text;
            downMl.Height = upMl.Height;
            downMl.Width = upMl.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            downMl.BackgroundImage = tr;
            downMl.BackgroundImageLayout = ImageLayout.Zoom;
            downMl.Top = rightMl.Top + upMl.Height;
            downMl.Left = upMl.Left;
            downMl.Tag = eDirection.iDown;
            downMl.Click += new EventHandler(move);
            tab.Add(downMl);

            Button leftMl = new Button();
            leftMl.Text = upMl.Text;
            leftMl.Height = upMl.Height;
            leftMl.Width = upMl.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            leftMl.BackgroundImage = tr;
            leftMl.BackgroundImageLayout = ImageLayout.Zoom;
            leftMl.Top = rightMl.Top;
            leftMl.Left = upMl.Left - upMl.Width;
            leftMl.Tag = eDirection.iLeft;
            leftMl.Click += new EventHandler(move);
            tab.Add(leftMl);

            centerMl = new PictureBox();
            centerMl.Text = upMl.Text;
            centerMl.Height = upMl.Height;
            centerMl.Width = upMl.Width;
            if (closeLine) tr = Properties.Resources.mLineClosed;
            else tr = Properties.Resources.mLine;
            tr.MakeTransparent(Color.White);
            centerMl.Image = tr;
            centerMl.SizeMode = PictureBoxSizeMode.Zoom;
            centerMl.Top = rightMl.Top;
            centerMl.Left = upMl.Left;
            tab.Add(centerMl);

            CheckBox cb = new CheckBox();
            cb.Top = y.Top + y.Height + il;
            cb.Left = x.Left;
            cb.Width = 70;
            cb.Text = "Closed";
            cb.Checked = closeLine;
            cb.CheckedChanged += new EventHandler(closedChange);
            tab.Add(cb);

            lbml = new ListBox();
            lbml.Name = "lbMl";
            lbml.Top = cb.Top + cb.Height + il;
            lbml.Left = il;
            lbml.Width = 70;
            lbml.Height = 190;

            refreshList();
            lbml.SelectedIndexChanged += new EventHandler(selIndChange);

            tab.Add(lbml);

            Button ba = new Button();
            ba.Text = "&Add";
            ba.Top = downMl.Top + downMl.Height + il * 3;
            ba.Left = lbml.Left + lbml.Width + il;
            ba.Width = 70;
            ba.Click += new EventHandler(add);
            tab.Add(ba);

            Button bi = new Button();
            bi.Text = "&Insert";
            bi.Top = ba.Top + ba.Height + il;
            bi.Left = ba.Left;
            bi.Width = ba.Width;
            bi.Click += new EventHandler(ins);
            tab.Add(bi);

            bd = new Button();
            bd.Text = "&Del";
            bd.Top = bi.Top + bi.Height + il;
            bd.Left = ba.Left;
            bd.Width = ba.Width;
            if (this.lPt.Count < 3) bd.Enabled = false;
            else bd.Enabled = true;
            bd.Click += new EventHandler(del);
            tab.Add(bd);

            return tab;
        }

        //---
        protected void refreshList()
        {
            List<string> points = getPoints();
            lbml.Items.Clear();
            for (int i = 0; i < points.Count; i++)
                lbml.Items.Add(points[i]);
            lbml.SelectedIndex = selectedPoint;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                int x = (int)(sender as NumericUpDown).Value;
                lPt[selectedPoint] = new Point(x, lPt[selectedPoint].Y);
                refreshList();
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (lPt[selectedPoint].X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        int x = lPt[selectedPoint].X;
                        x--;
                        lPt[selectedPoint] = new Point(x, lPt[selectedPoint].Y);
                    }
                }
                else
                {
                    if (lPt[selectedPoint].X >= MAX)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        int x = lPt[selectedPoint].X;
                        x++;
                        lPt[selectedPoint] = new Point(x, lPt[selectedPoint].Y);
                    }
                }
            }
            nudx.Value = lPt[selectedPoint].X;
            refreshList();
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                int y = (int)(sender as NumericUpDown).Value;
                lPt[selectedPoint] = new Point(lPt[selectedPoint].X, y);
                refreshList();
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (lPt[selectedPoint].Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else
                    {
                        int y = lPt[selectedPoint].Y;
                        y--;
                        lPt[selectedPoint] = new Point(lPt[selectedPoint].X, y);
                    }
                }
                else
                {
                    if (lPt[selectedPoint].Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else
                    {
                        int y = lPt[selectedPoint].Y;
                        y++;
                        lPt[selectedPoint] = new Point(lPt[selectedPoint].X, y);
                    }
                }
            }
            nudy.Value = lPt[selectedPoint].Y;
            refreshList();
        }

        //---

        protected void selIndChange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ListBox))
            {
                int ind = (sender as ListBox).SelectedIndex;
                if (ind < 0) return;
                selectedPoint = ind;
                nudx.Value = lPt[ind].X;
                nudy.Value = lPt[ind].Y;
            }
        }

        //---
        public override void move(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                eDirection v = (eDirection)((sender as Button).Tag);
                switch (v)
                {
                    case eDirection.iLeft:
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            if (lPt[i].X == MIN)
                            {
                                MessageBox.Show("This mLine can not be moved to the left.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            int x = lPt[i].X;
                            x--;
                            lPt[i] = new Point(x, lPt[i].Y);
                        }

                        break;
                    //---------------------------------
                    case eDirection.iUp:
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            if (lPt[i].Y == MIN)
                            {
                                MessageBox.Show("This mLine can not be moved to the up.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            int y = lPt[i].Y;
                            y--;
                            lPt[i] = new Point(lPt[i].X, y);
                        }
                        break;
                    //----------------------------------
                    case eDirection.iRight:
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            if (lPt[i].X == MAX)
                            {
                                MessageBox.Show("This mLine can not be moved to the right.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            int x = lPt[i].X;
                            x++;
                            lPt[i] = new Point(x, lPt[i].Y);
                        }
                        break;
                    //---------------------------------
                    case eDirection.iDown:
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            if (lPt[i].Y == MAX)
                            {
                                MessageBox.Show("This mLine can not be moved to the down.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < lPt.Count; i++)
                        {
                            int y = lPt[i].Y;
                            y++;
                            lPt[i] = new Point(lPt[i].X, y);
                        }
                        break;
                    //---------------------------------
                    default:
                        MessageBox.Show("Unknown command.",
                                                 "Error.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Error);
                        return;
                }
                refreshList();
            }
        }

        //---
        public override Rectangle getRect()
        {
            Point min = lPt[0];
            Point max = lPt[0];
            for (int i = 1; i < lPt.Count; i++)
            {
                if (lPt[i].X < min.X) min.X = lPt[i].X;
                if (lPt[i].Y < min.Y) min.Y = lPt[i].Y;
                if (lPt[i].X > max.X) max.X = lPt[i].X;
                if (lPt[i].Y > max.Y) max.Y = lPt[i].Y;
            }

            return new Rectangle(min.X, min.Y, max.X - min.X + 1, max.Y - min.Y + 1);
        }

        //---
        protected void add(object sender, EventArgs e)
        {
            int x = lPt[lPt.Count - 1].X + 5;
            int y = lPt[lPt.Count - 1].Y + 5;
            if (x > MAX) x -= MAX;
            if (y > MAX) y -= MAX;
            lPt.Add(new Point(x, y));
            selectedPoint = lPt.Count - 1;
            bd.Enabled = true;
            refreshList();
        }

        //---
        protected void ins(object sender, EventArgs e)
        {
            int x = lPt[selectedPoint].X - 5;
            int y = lPt[selectedPoint].Y - 5;
            if (x < MIN) x += MAX;
            if (y < MIN) y += MAX;
            lPt.Insert(selectedPoint, new Point(x, y));

            bd.Enabled = true;
            refreshList();
        }

        //---
        protected void del(object sender, EventArgs e)
        {
            lPt.RemoveAt(selectedPoint);
            if (selectedPoint >= lPt.Count) selectedPoint = lPt.Count - 1;

            if (lPt.Count <= 2) bd.Enabled = false;
            else bd.Enabled = true;
            refreshList();
        }

        //---
        protected void closedChange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(CheckBox))
            {
                closeLine = (sender as CheckBox).Checked;
            }
            Bitmap tr;
            if (closeLine) tr = Properties.Resources.mLineClosed;
            else tr = Properties.Resources.mLine;
            tr.MakeTransparent(Color.White);

            pb.Image = tr;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            centerMl.Image = tr;
            centerMl.SizeMode = PictureBoxSizeMode.Zoom;
        }

        //---
        protected List<string> getPoints()
        {
            List<string> lptemp = new List<string>();
            for (int i = 0; i < lPt.Count; i++)
                lptemp.Add("P" + i.ToString() + " (" + lPt[i].X.ToString() + "," + lPt[i].Y.ToString() + ")");
            return lptemp;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();
            if (lPt.Count > 2 && closeLine)
            {
                if (lPt[0] == lPt[lPt.Count - 1]) ts.Add(new errClass(eTest.errmlDupPoint,
                    new int[2] { 0, lPt.Count - 1, },
                    "mLine" + this.getName() + ": Point #0 & #" + (lPt.Count - 1).ToString() +
                    " have the same coordinates."));
            }
            if (ts.Count == 0)
            {
                for (int i = 0; i < lPt.Count - 1; i++)
                {
                    if (lPt[i] == lPt[i + 1])
                    {
                        ts.Add(new errClass(eTest.errmlDupPoint,
                            new int[2] { i, i + 1 },
                       "mLine" + this.getName() + ": Point #" + i.ToString() + " & #" + (i + 1).ToString() +
                        " have the same coordinates."));
                    }
                }
            }
            if (lPt.Count == 2 && closeLine) ts.Add(new errClass(eTest.errmlNoEfect,
                new int[0] { },
                "mLine" + this.getName() + ": Select close line have no effect."));

            for (int i = 0; i < lPt.Count - 2; i++)
            {
                if ((lPt[i + 1].X - lPt[i].X) != 0 && (lPt[i + 2].X - lPt[i].X) != 0)
                {
                    double a = (double)(lPt[i + 1].Y - lPt[i].Y) / (double)(lPt[i + 1].X - lPt[i].X);
                    double b = (double)(lPt[i + 2].Y - lPt[i].Y) / (double)(lPt[i + 2].X - lPt[i].X);
                    if (a == b) ts.Add(new errClass(eTest.errmlInline,
                        new int[3] { i, i + 1, i + 2 },
                        "mLine" + this.getName() + ": This points : #" + i.ToString() +
                        " , #" + (i + 1).ToString() + " , #" + (i + 2).ToString() +
                        " are collinear."));
                }
                else
                if ((lPt[i + 1].X - lPt[i].X) == 0 && (lPt[i + 2].X - lPt[i].X) == 0)
                    ts.Add(new errClass(eTest.errmlInline,
                        new int[3] { i, i + 1, i + 2 },
                       "mLine" + this.getName() + ": This points : #" + i.ToString() +
                       " , #" + (i + 1).ToString() + " , #" + (i + 2).ToString() +
                       " are collinear."));
            }
            if (closeLine && lPt.Count > 3)
            {
                for (int i = lPt.Count - 2; i < lPt.Count; i++)
                {
                    int x1 = i + 1;
                    if (x1 == lPt.Count) x1 = 0;
                    int x2 = i + 2;
                    if (x2 >= lPt.Count) x2 -= lPt.Count;
                    if ((lPt[x1].X - lPt[i].X) != 0 && (lPt[x2].X - lPt[i].X) != 0)
                    {
                        double a = (double)(lPt[x1].Y - lPt[i].Y) / (double)(lPt[x1].X - lPt[i].X);
                        double b = (double)(lPt[x2].Y - lPt[i].Y) / (double)(lPt[x2].X - lPt[i].X);
                        if (a == b) ts.Add(new errClass(eTest.errmlInline,
                            new int[3] { i, x1, x2 },
                            "mLine" + this.getName() + ": This points : #" + i.ToString() +
                            " , #" + x1.ToString() + " , #" + x2.ToString() +
                            " are collinear."));
                    }
                    else
                if ((lPt[x1].X - lPt[i].X) == 0 && (lPt[x2].X - lPt[i].X) == 0)
                        ts.Add(new errClass(eTest.errmlInline,
                            new int[3] { i, x1, x2 },
                           "mLine" + this.getName() + ": This points : #" + i.ToString() +
                           " , #" + x1.ToString() + " , #" + x2.ToString() +
                           " are collinear."));
                }
            }

            return ts;
        }

        //---
        public bool closed
        {
            get
            {
                return closeLine;
            }
        }

        //---
        public List<Point> Points
        {
            get
            {
                return lPt;
            }
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(mLine obj1, mLine obj2)
        {
            if (obj1.closeLine == obj2.closeLine && obj1.lPt.Count == obj2.lPt.Count)
            {
                bool fl = true;
                for (int i = 0; i < obj1.lPt.Count; i++)
                {
                    if (obj1.lPt[i] != obj2.lPt[i])
                    {
                        fl = false;
                        break;
                    }
                }
                if (fl) return true;
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
        public static bool operator !=(mLine obj1, mLine obj2)
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

            return obj.GetType() == GetType() && Equals((mLine)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (lPt.ToString() + closeLine.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class circle : Item
    {
        //---
        protected Point c;

        protected byte r;
        protected bool fill;
        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected NumericUpDown nudr;
        protected CheckBox f;
        protected PictureBox pb;
        protected PictureBox pbc;

        //---
        public circle()
        {
            fill = false;
            c = new Point(1, 1);
            r = 1;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            nudr = new NumericUpDown();
            f = new CheckBox();
            pb = new PictureBox();
            pbc = new PictureBox();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            bool ok = test().Count == 0;
            name.Tag = ok;
            eImage im;
            eImage im2;
            string fl;
            if (fill)
            {
                if (ok) im = eImage.iCircleFill;
                else im = eImage.iCircleFillWar;
                im2 = eImage.iCircleFill;
                fl = "Yes";
            }
            else
            {
                if (ok) im = eImage.iCircle;
                else im = eImage.iCircleWar;
                im2 = eImage.iCircle;
                fl = "No";
            }
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            name.Nodes.Add("X= " + c.X);
            name.Nodes[0].ImageIndex = (int)eImage.iXpos;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

            name.Nodes.Add("Y= " + c.Y);
            name.Nodes[1].ImageIndex = (int)eImage.iYpos;
            name.Nodes[1].SelectedImageIndex = (int)eImage.iYpos;

            name.Nodes.Add("R= " + r);
            if (r > 0)
            {
                name.Nodes[2].ImageIndex = (int)eImage.iRadiusCircle;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iRadiusCircle;
            }
            else
            {
                name.Nodes[2].ImageIndex = (int)eImage.iRadiusCircleWar;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iRadiusCircleWar;
            }
            name.Nodes.Add("Filled circle : " + fl);
            name.Nodes[3].ImageIndex = (int)im2;
            name.Nodes[3].SelectedImageIndex = (int)im2;
            return name;
        }

        //---
        public override string ToString()
        {
            string l = "";
            if (fill) l = ((int)eCommand.fCircle).ToString();
            else l = ((int)eCommand.dCircle).ToString();
            l += ", " + c.X.ToString() + ", ";
            l += c.Y.ToString() + ", ";
            l += r.ToString() + ", ";
            return l;
        }

        //---
        public override int itemLenght()
        {
            return 4;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            int x0 = c.X;
            int y0 = c.Y;
            if (fill)
            {
                drawLine(ref canva, x0, y0 - r, x0, y0 - r + r + r + 1, cl, tag);
                fillCircleHelper(ref canva, x0, y0, r, 3, 0, cl, tag);
            }
            else
            {
                int f = 1 - r;
                int ddF_x = 1;
                int ddF_y = -r - r;
                int x = 0;

                drawPixel(ref canva, x0, y0 + r, cl, tag);
                drawPixel(ref canva, x0, y0 - r, cl, tag);
                drawPixel(ref canva, x0 + r, y0, cl, tag);
                drawPixel(ref canva, x0 - r, y0, cl, tag);

                while (x < r)
                {
                    if (f >= 0)
                    {
                        r--;
                        ddF_y += 2;
                        f += ddF_y;
                    }
                    x++;
                    ddF_x += 2;
                    f += ddF_x;

                    drawPixel(ref canva, x0 + x, y0 + r, cl, tag);
                    drawPixel(ref canva, x0 - x, y0 + r, cl, tag);
                    drawPixel(ref canva, x0 + x, y0 - r, cl, tag);
                    drawPixel(ref canva, x0 - x, y0 - r, cl, tag);
                    drawPixel(ref canva, x0 + r, y0 + x, cl, tag);
                    drawPixel(ref canva, x0 - r, y0 + x, cl, tag);
                    drawPixel(ref canva, x0 + r, y0 - x, cl, tag);
                    drawPixel(ref canva, x0 - r, y0 - x, cl, tag);
                }
            }
            if (cl == eColor.Selected)
            {
                drawPixel(ref canva, c.X, c.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X - r, c.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X, c.Y - r, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X + r, c.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X, c.Y + r, eColor.SelectedPoint, tag);
            }

            return cl;
        }

        //---
        public override Rectangle getRect()
        {
            return new Rectangle(c.X - r, c.Y - r, r * 2, r * 2);
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();

            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr;
            if (fill) tr = Properties.Resources.CircleFill;
            else tr = Properties.Resources.Circle;
            loadInit(ref pb, tr, ref name, "Circle" + getName());

            tab.Add(pb);
            tab.Add(name);

            f = new CheckBox();
            f.Top = name.Top + name.Height + il;
            f.Left = il + labelWidth;
            f.Text = "Fill";
            f.Checked = fill;
            f.CheckedChanged += new EventHandler(fillChange);
            tab.Add(f);

            Label x = new Label();
            x.Text = "X=";
            x.Top = f.Top + f.Height + il * 6;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = il;
            y.Width = x.Width;
            tab.Add(y);

            Label ra = new Label();
            ra.Text = "R=";
            ra.Top = y.Top + y.Height + il * 6;
            ra.Left = il;
            ra.Width = x.Width;
            tab.Add(ra);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = c.X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = y.Width + il;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = c.Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            nudr = new NumericUpDown();
            nudr.Top = ra.Top - 3;
            nudr.Left = ra.Width + il;
            nudr.Width = nudx.Width;
            nudr.Minimum = nudx.Minimum;
            nudr.Maximum = nudx.Maximum;
            nudr.Value = r;
            nudr.ValueChanged += new EventHandler(valueRchange);
            tab.Add(nudr);

            Button up = new Button();
            up.Text = "";
            up.Height = nudx.Height + 6;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = nudx.Top + nudx.Height + il / 2 - (int)(up.Height * 1.5);
            up.Left = nudx.Left + nudx.Width + il * 2 + up.Width;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            pbc = new PictureBox();
            pbc.Height = up.Height;
            pbc.Width = up.Width;
            if (fill) tr = Properties.Resources.CircleFill;
            else tr = Properties.Resources.Circle;
            tr.MakeTransparent(Color.White);
            pbc.Image = tr;
            pbc.SizeMode = PictureBoxSizeMode.Zoom;
            pbc.Top = right.Top;
            pbc.Left = up.Left;
            tab.Add(pbc);

            Button shrink = new Button();
            shrink.Text = up.Text;
            shrink.Height = up.Height;
            shrink.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrink.BackgroundImage = tr;
            shrink.BackgroundImageLayout = ImageLayout.Zoom;
            shrink.Top = nudr.Top + nudr.Height / 2 - shrink.Height / 2;
            shrink.Left = nudr.Left + nudr.Width + 2 * il;
            shrink.Tag = eDirection.iLeft;
            shrink.Click += new EventHandler(valueRchange);
            tab.Add(shrink);

            Button increase = new Button();
            increase.Text = up.Text;
            increase.Height = up.Height;
            increase.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increase.BackgroundImage = tr;
            increase.BackgroundImageLayout = ImageLayout.Zoom;
            increase.Top = shrink.Top;
            increase.Left = right.Left;
            increase.Tag = eDirection.iRight;
            increase.Click += new EventHandler(valueRchange);
            tab.Add(increase);

            PictureBox pbr = new PictureBox();

            pbr.Height = up.Height;
            pbr.Width = up.Width;
            pbr.Top = shrink.Top;
            pbr.Left = shrink.Left + shrink.Width;

            tr = Properties.Resources.Radius;
            tr.MakeTransparent(Color.White);

            pbr.Image = tr;
            pbr.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbr);

            return tab;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.X = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (c.X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X--;
                }
                else
                {
                    if (c.X >= 127)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X++;
                }
            }
            nudx.Value = c.X;
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.Y = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (c.Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y--;
                }
                else
                {
                    if (c.Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y++;
                }
            }
            nudy.Value = c.Y;
        }

        //---
        protected void valueRchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r = (byte)(sender as NumericUpDown).Value;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (r == MIN)
                    {
                        MessageBox.Show("R=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r--;
                }
                else
                {
                    if (r >= MAX)
                    {
                        MessageBox.Show("R=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r++;
                }
            }
            nudr.Value = r;
        }

        //---
        protected void fillChange(object sender, EventArgs e)
        {
            bool useCheckBox = true;
            if (sender.GetType() == typeof(CheckBox))
            {
                fill = (sender as CheckBox).Checked;
                useCheckBox = false;
            }
            if (useCheckBox) f.Checked = fill;

            Bitmap tr;

            if (fill) tr = Properties.Resources.CircleFill;
            else tr = Properties.Resources.Circle;

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbc.Image = tr;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();
            if (r == 0) ts.Add(new errClass(eTest.errCirRedToPoint,
                new int[0] { },
                "Circle" + this.getName() + ": Radius=0."));

            if (c.X - r < 0) ts.Add(new errClass(eTest.errOriXUnder0,
                new int[0] { },
                "Circle" + this.getName() + ": Boundary X<0."));

            if (c.Y - r < 0) ts.Add(new errClass(eTest.errOriYUnder0,
                new int[0] { },
                 "Circle" + this.getName() + ": Boundary Y<0."));
            return ts;
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(circle obj1, circle obj2)
        {
            if (obj1.c == obj2.c && obj1.r == obj2.r &&
                obj1.fill == obj2.fill) return true;
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
        public static bool operator !=(circle obj1, circle obj2)
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

            return obj.GetType() == GetType() && Equals((circle)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (c.X.ToString() + c.Y.ToString() + r.ToString() + fill.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class ellipse : Item
    {
        //---
        protected Point c;

        protected byte rx;
        protected byte ry;
        protected bool fill;
        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected NumericUpDown nudrx;
        protected NumericUpDown nudry;
        protected CheckBox f;
        protected PictureBox pb;
        protected PictureBox pbc;

        //---
        public ellipse()
        {
            fill = false;
            c = new Point(3, 2);
            rx = 3;
            ry = 2;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            nudrx = new NumericUpDown();
            nudry = new NumericUpDown();
            f = new CheckBox();
            pb = new PictureBox();
            pbc = new PictureBox();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            bool ok = test().Count == 0;
            name.Tag = ok;
            eImage im;
            eImage im2;
            string fl;
            if (fill)
            {
                if (ok) im = eImage.iEllipseFill;
                else im = eImage.iEllipseFillWar;
                im2 = eImage.iEllipseFill;
                fl = "Yes";
            }
            else
            {
                if (ok) im = eImage.iEllipse;
                else im = eImage.iEllipseWar;
                im2 = eImage.iEllipse;
                fl = "No";
            }
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            name.Nodes.Add("X= " + c.X);
            name.Nodes[0].ImageIndex = (int)eImage.iXpos;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

            name.Nodes.Add("Y= " + c.Y);
            name.Nodes[1].ImageIndex = (int)eImage.iYpos;
            name.Nodes[1].SelectedImageIndex = (int)eImage.iYpos;

            name.Nodes.Add("Rx= " + rx);
            if (rx > 0)
            {
                name.Nodes[2].ImageIndex = (int)eImage.iEllipseRxRadius;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iEllipseRxRadius;
            }
            else
            {
                name.Nodes[2].ImageIndex = (int)eImage.iEllipseRxRadiusWar;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iEllipseRxRadiusWar;
            }

            name.Nodes.Add("Ry= " + ry);
            if (rx > 0)
            {
                name.Nodes[3].ImageIndex = (int)eImage.iEllipseRyRadius;
                name.Nodes[3].SelectedImageIndex = (int)eImage.iEllipseRyRadius;
            }
            else
            {
                name.Nodes[3].ImageIndex = (int)eImage.iEllipseRyRadiusWar;
                name.Nodes[3].SelectedImageIndex = (int)eImage.iEllipseRyRadiusWar;
            }
            name.Nodes.Add("Filled ellipse : " + fl);
            name.Nodes[4].ImageIndex = (int)im2;
            name.Nodes[4].SelectedImageIndex = (int)im2;
            return name;
        }

        //---
        public override string ToString()
        {
            string l = "";
            if (fill) l = ((int)eCommand.fEllipse).ToString();
            else l = ((int)eCommand.dEllipse).ToString();
            l += ", " + c.X.ToString() + ", ";
            l += c.Y.ToString() + ", ";
            l += rx.ToString() + ", ";
            l += ry.ToString() + ", ";
            return l;
        }

        //---
        public override int itemLenght()
        {
            return 5;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            int x0 = c.X;
            int y0 = c.Y;
            if (fill)
            {
                if (rx < 2) return cl;
                if (ry < 2) return cl;
                int x, y;
                int rx2 = rx * rx;
                int ry2 = ry * ry;
                int fx2 = 4 * rx2;
                int fy2 = 4 * ry2;
                int s;

                for (x = 0, y = ry, s = 2 * ry2 + rx2 * (1 - 2 * ry); ry2 * x <= rx2 * y; x++)
                {
                    drawLine(ref canva, x0 - x, y0 - y, (x0 - x) + x + x + 1, y0 - y, cl, tag);
                    drawLine(ref canva, x0 - x, y0 + y, (x0 - x) + x + x + 1, y0 - y, cl, tag);

                    if (s >= 0)
                    {
                        s += fx2 * (1 - y);
                        y--;
                    }
                    s += ry2 * ((4 * x) + 6);
                }

                for (x = rx, y = 0, s = 2 * rx2 + ry2 * (1 - 2 * rx); rx2 * y <= ry2 * x; y++)
                {
                    drawLine(ref canva, x0 - x, y0 - y, (x0 - x) + x + x + 1, y0 - y, cl, tag);
                    drawLine(ref canva, x0 - x, y0 + y, (x0 - x) + x + x + 1, y0 + y, cl, tag);

                    if (s >= 0)
                    {
                        s += fy2 * (1 - x);
                        x--;
                    }
                    s += rx2 * ((4 * y) + 6);
                }
            }
            else
            {
                if (rx < 2) return cl;
                if (ry < 2) return cl;
                int x, y;
                int rx2 = rx * rx;
                int ry2 = ry * ry;
                int fx2 = 4 * rx2;
                int fy2 = 4 * ry2;
                int s;

                for (x = 0, y = ry, s = 2 * ry2 + rx2 * (1 - 2 * ry); ry2 * x <= rx2 * y; x++)
                {
                    drawPixel(ref canva, x0 + x, y0 + y, cl, tag);
                    drawPixel(ref canva, x0 - x, y0 + y, cl, tag);
                    drawPixel(ref canva, x0 + x, y0 - y, cl, tag);
                    drawPixel(ref canva, x0 - x, y0 - y, cl, tag);
                    if (s >= 0)
                    {
                        s += fx2 * (1 - y);
                        y--;
                    }
                    s += ry2 * ((4 * x) + 6);
                }

                for (x = rx, y = 0, s = 2 * rx2 + ry2 * (1 - 2 * rx); rx2 * y <= ry2 * x; y++)
                {
                    drawPixel(ref canva, x0 + x, y0 + y, cl, tag);
                    drawPixel(ref canva, x0 - x, y0 + y, cl, tag);
                    drawPixel(ref canva, x0 + x, y0 - y, cl, tag);
                    drawPixel(ref canva, x0 - x, y0 - y, cl, tag);
                    if (s >= 0)
                    {
                        s += fy2 * (1 - x);
                        x--;
                    }
                    s += rx2 * ((4 * y) + 6);
                }
            }
            if (cl == eColor.Selected)
            {
                drawPixel(ref canva, c.X, c.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X - rx, c.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X, c.Y - ry, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X + rx, c.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, c.X, c.Y + ry, eColor.SelectedPoint, tag);
            }

            return cl;
        }

        //---
        public override Rectangle getRect()
        {
            return new Rectangle(c.X - rx, c.Y - ry, rx * 2, ry * 2);
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();

            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr;
            if (fill) tr = Properties.Resources.EllipseFill;
            else tr = Properties.Resources.Ellipse;
            loadInit(ref pb, tr, ref name, "Ellipse" + getName());

            tab.Add(pb);
            tab.Add(name);

            f = new CheckBox();
            f.Top = name.Top + name.Height + il;
            f.Left = il + labelWidth;
            f.Text = "Fill";
            f.Checked = fill;
            f.CheckedChanged += new EventHandler(fillChange);
            tab.Add(f);

            Label x = new Label();
            x.Text = "X=";
            x.Top = f.Top + f.Height + il * 6;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = il;
            y.Width = x.Width;
            tab.Add(y);

            Label lrx = new Label();
            lrx.Text = "Rx=";
            lrx.Top = y.Top + y.Height + il * 6;
            lrx.Left = il;
            lrx.Width = x.Width;
            tab.Add(lrx);

            Label lry = new Label();
            lry.Text = "Ry=";
            lry.Top = lrx.Top + lrx.Height + il;
            lry.Left = il;
            lry.Width = x.Width;
            tab.Add(lry);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = c.X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = y.Width + il;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = c.Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            nudrx = new NumericUpDown();
            nudrx.Top = lrx.Top - 3;
            nudrx.Left = lrx.Width + il;
            nudrx.Width = nudx.Width;
            nudrx.Minimum = 2;
            nudrx.Maximum = nudx.Maximum;
            nudrx.Value = rx;
            nudrx.ValueChanged += new EventHandler(valueRxchange);
            tab.Add(nudrx);

            nudry = new NumericUpDown();
            nudry.Top = lry.Top - 3;
            nudry.Left = lry.Width + il;
            nudry.Width = nudx.Width;
            nudry.Minimum = 2;
            nudry.Maximum = nudx.Maximum;
            nudry.Value = ry;
            nudry.ValueChanged += new EventHandler(valueRychange);
            tab.Add(nudry);

            Button up = new Button();
            up.Text = "";
            up.Height = nudx.Height + 6;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = nudx.Top + nudx.Height + il / 2 - (int)(up.Height * 1.5);
            up.Left = nudx.Left + nudx.Width + il * 2 + up.Width;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            pbc = new PictureBox();
            pbc.Height = up.Height;
            pbc.Width = up.Width;
            if (fill) tr = Properties.Resources.EllipseFill;
            else tr = Properties.Resources.Ellipse;
            tr.MakeTransparent(Color.White);
            pbc.Image = tr;
            pbc.SizeMode = PictureBoxSizeMode.Zoom;
            pbc.Top = right.Top;
            pbc.Left = up.Left;
            tab.Add(pbc);

            Button shrinkx = new Button();
            shrinkx.Text = up.Text;
            shrinkx.Height = up.Height;
            shrinkx.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrinkx.BackgroundImage = tr;
            shrinkx.BackgroundImageLayout = ImageLayout.Zoom;
            shrinkx.Top = nudrx.Top + nudrx.Height / 2 - shrinkx.Height / 2;
            shrinkx.Left = nudrx.Left + nudrx.Width + 2 * il;
            shrinkx.Tag = eDirection.iLeft;
            shrinkx.Click += new EventHandler(valueRxchange);
            tab.Add(shrinkx);

            Button increasex = new Button();
            increasex.Text = up.Text;
            increasex.Height = up.Height;
            increasex.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increasex.BackgroundImage = tr;
            increasex.BackgroundImageLayout = ImageLayout.Zoom;
            increasex.Top = shrinkx.Top;
            increasex.Left = right.Left;
            increasex.Tag = eDirection.iRight;
            increasex.Click += new EventHandler(valueRxchange);
            tab.Add(increasex);

            PictureBox pbrx = new PictureBox();

            pbrx.Height = up.Height;
            pbrx.Width = up.Width;
            pbrx.Top = shrinkx.Top;
            pbrx.Left = shrinkx.Left + shrinkx.Width;

            tr = Properties.Resources.EllipseRadiusX;
            tr.MakeTransparent(Color.White);

            pbrx.Image = tr;
            pbrx.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbrx);

            Button shrinky = new Button();
            shrinky.Text = up.Text;
            shrinky.Height = up.Height;
            shrinky.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrinky.BackgroundImage = tr;
            shrinky.BackgroundImageLayout = ImageLayout.Zoom;
            shrinky.Top = nudry.Top + nudry.Height / 2 - shrinky.Height / 2;
            shrinky.Left = nudry.Left + nudry.Width + 2 * il;
            shrinky.Tag = eDirection.iLeft;
            shrinky.Click += new EventHandler(valueRychange);
            tab.Add(shrinky);

            Button increasey = new Button();
            increasey.Text = up.Text;
            increasey.Height = up.Height;
            increasey.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increasey.BackgroundImage = tr;
            increasey.BackgroundImageLayout = ImageLayout.Zoom;
            increasey.Top = shrinky.Top;
            increasey.Left = right.Left;
            increasey.Tag = eDirection.iRight;
            increasey.Click += new EventHandler(valueRychange);
            tab.Add(increasey);

            PictureBox pbr = new PictureBox();

            pbr.Height = up.Height;
            pbr.Width = up.Width;
            pbr.Top = shrinky.Top;
            pbr.Left = shrinky.Left + shrinky.Width;

            tr = Properties.Resources.EllipseRadiusY;
            tr.MakeTransparent(Color.White);

            pbr.Image = tr;
            pbr.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbr);

            return tab;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.X = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (c.X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X--;
                }
                else
                {
                    if (c.X >= 127)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X++;
                }
            }
            nudx.Value = c.X;
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.Y = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (c.Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y--;
                }
                else
                {
                    if (c.Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y++;
                }
            }
            nudy.Value = c.Y;
        }

        //---
        protected void valueRxchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                rx = (byte)(sender as NumericUpDown).Value;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (rx == 2)
                    {
                        MessageBox.Show("Rx=2 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else rx--;
                }
                else
                {
                    if (rx >= MAX)
                    {
                        MessageBox.Show("Rx=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else rx++;
                }
            }
            nudrx.Value = rx;
        }

        //---
        protected void valueRychange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                ry = (byte)(sender as NumericUpDown).Value;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (ry == 2)
                    {
                        MessageBox.Show("Ry=2 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else ry--;
                }
                else
                {
                    if (ry >= MAX)
                    {
                        MessageBox.Show("Ry=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else ry++;
                }
            }
            nudry.Value = ry;
        }

        //---
        protected void fillChange(object sender, EventArgs e)
        {
            bool useCheckBox = true;
            if (sender.GetType() == typeof(CheckBox))
            {
                fill = (sender as CheckBox).Checked;
                useCheckBox = false;
            }
            if (useCheckBox) f.Checked = fill;

            Bitmap tr;

            if (fill) tr = Properties.Resources.EllipseFill;
            else tr = Properties.Resources.Ellipse;

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbc.Image = tr;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();
            if (rx == ry) ts.Add(new errClass(eTest.errEllipRXY,
                new int[0] { },
                "Ellipse" + this.getName() + ": Radius x=y ->circle."));

            if (c.X - rx < 0) ts.Add(new errClass(eTest.errOriXUnder0,
                new int[0] { },
                "Ellipse" + this.getName() + ": Boundary X<0."));

            if (c.Y - ry < 0) ts.Add(new errClass(eTest.errOriYUnder0,
                new int[0] { },
                 "Ellipse" + this.getName() + ": Boundary Y<0."));
            return ts;
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(ellipse obj1, ellipse obj2)
        {
            if (obj1.c == obj2.c && obj1.rx == obj2.rx &&
               obj1.ry == obj2.ry &&
               obj1.fill == obj2.fill) return true;
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
        public static bool operator !=(ellipse obj1, ellipse obj2)
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

            return obj.GetType() == GetType() && Equals((ellipse)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (c.X.ToString() + c.Y.ToString() + rx.ToString()
                    + ry.ToString() + fill.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class triangle : Item
    {
        //---
        protected bool fill;

        protected List<Point> lPt = new List<Point>();
        protected int selectedPoint;

        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected CheckBox f;
        protected PictureBox pb;
        protected PictureBox pbt;
        protected ListBox lb;

        //---
        public triangle()
        {
            lPt.Add(new Point(MIN, MIN));
            lPt.Add(new Point(1, MIN));
            lPt.Add(new Point(MIN, 1));

            fill = false;
            selectedPoint = 0;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            f = new CheckBox();
            pb = new PictureBox();
            pbt = new PictureBox();
            lb = new ListBox();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            List<errClass> tst = test();
            bool ok = tst.Count == 0;
            name.Tag = ok;
            eImage im;
            eImage im2;
            string abc = "ABC";
            string fl;
            if (fill)
            {
                fl = "Yes";
                if (ok) im = eImage.iTriangleFill;
                else im = eImage.iTriangleFillWar;
                im2 = eImage.iTriangleFill;
            }
            else
            {
                fl = "No";
                if (ok) im = eImage.iTriangle;
                else im = eImage.iTriangleWar;
                im2 = eImage.iTriangle;
            }
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            bool[] warPoint = new bool[3];
            warPoint[0] = false;
            warPoint[1] = false;
            warPoint[2] = false;
            if (tst.Count != 0)
            {
                if (tst[0].ID == eTest.errTrRedToPoint || tst[0].ID == eTest.errTrRedToLine)
                {
                    warPoint[0] = true;
                    warPoint[1] = true;
                    warPoint[2] = true;
                }
                else
                {
                    warPoint[tst[0].No[0]] = true;
                    warPoint[tst[0].No[1]] = true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                name.Nodes.Add("Point " + abc[i]);
                name.Nodes[i].Nodes.Add("X= " + lPt[i].X);
                name.Nodes[i].Nodes.Add("Y= " + lPt[i].Y);
                if (warPoint[i])
                {
                    name.Nodes[i].ImageIndex = (int)eImage.iPointWar;
                    name.Nodes[i].SelectedImageIndex = (int)eImage.iPointWar;

                    name.Nodes[i].Nodes[0].ImageIndex = (int)eImage.iXposWar;
                    name.Nodes[i].Nodes[0].SelectedImageIndex = (int)eImage.iXposWar;

                    name.Nodes[i].Nodes[1].ImageIndex = (int)eImage.iYposWar;
                    name.Nodes[i].Nodes[1].SelectedImageIndex = (int)eImage.iYposWar;
                }
                else
                {
                    name.Nodes[i].ImageIndex = (int)eImage.iPoint;
                    name.Nodes[i].SelectedImageIndex = (int)eImage.iPoint;

                    name.Nodes[i].Nodes[0].ImageIndex = (int)eImage.iXpos;
                    name.Nodes[i].Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

                    name.Nodes[i].Nodes[1].ImageIndex = (int)eImage.iYpos;
                    name.Nodes[i].Nodes[1].SelectedImageIndex = (int)eImage.iYpos;
                }
            }

            name.Nodes.Add("Filled triangle : " + fl);
            name.Nodes[3].ImageIndex = (int)im2;
            name.Nodes[3].SelectedImageIndex = (int)im2;
            return name;
        }

        //---
        public override string ToString()
        {
            string l = "";
            if (fill) l = ((int)eCommand.fTriangle).ToString();
            else l = ((int)eCommand.dTriangle).ToString();
            l += ", " + lPt[0].X.ToString() + ", ";
            l += lPt[0].Y.ToString() + ", ";
            l += lPt[1].X.ToString() + ", ";
            l += lPt[1].Y.ToString() + ", ";
            l += lPt[2].X.ToString() + ", ";
            l += lPt[2].Y.ToString() + ", ";
            return l;
        }

        //---
        public override int itemLenght()
        {
            return 7;
        }

        //---

        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            if (fill)
            {
                fillTriangle(ref canva, lPt[0], lPt[1], lPt[2], cl, tag);
            }
            else
            {
                drawLine(ref canva, lPt[0], lPt[1], cl, tag);
                drawLine(ref canva, lPt[1], lPt[2], cl, tag);
                drawLine(ref canva, lPt[2], lPt[0], cl, tag);
            }

            if (cl == eColor.SelectedPoint)
            {
                for (int i = 0; i < 3; i++)
                {
                    eColor cls = eColor.SelectedPoint;
                    if (selectedPoint == i) cls = eColor.HotPoint;
                    drawPixel(ref canva, lPt[i].X, lPt[i].Y, cls, tag);
                }
            }

            return cl;
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();
            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr;
            if (fill) tr = Properties.Resources.TriangleFill;
            else tr = Properties.Resources.Triangle;
            loadInit(ref pb, tr, ref name, "Triangle" + getName());

            tab.Add(pb);
            tab.Add(name);

            Label x = new Label();
            x.Text = "X=";
            x.Top = name.Top + name.Height + il;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = il;
            y.Width = x.Width;
            tab.Add(y);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = lPt[selectedPoint].X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = y.Width + il;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = lPt[selectedPoint].Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            Label ver = new Label();
            ver.Text = "Vertices :";
            ver.Top = y.Top + y.Height + 2 * il;
            ver.Left = x.Left;
            //ver.Width = nameWidth;
            tab.Add(ver);

            lb = new ListBox();
            lb.Width = 82;
            lb.Height = 48;
            lb.Top = ver.Top + ver.Height;
            lb.Left = nudy.Left + nudy.Width - lb.Width;
            refreshList();
            lb.SelectedIndexChanged += new EventHandler(selIndChange);
            tab.Add(lb);

            f = new CheckBox();
            f.Top = nudx.Top;
            f.Left = nudx.Left + nudx.Width + 5 * il;
            f.Text = "Fill";
            f.Checked = fill;
            f.CheckedChanged += new EventHandler(fillChange);
            tab.Add(f);

            Button up = new Button();
            up.Text = "";
            up.Height = 25;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = lb.Top + lb.Height + 2 * il;
            up.Left = up.Width + il / 2;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            PictureBox center = new PictureBox();
            center.Text = up.Text;
            center.Height = up.Height;
            center.Width = up.Width;
            tr = Properties.Resources.Point;
            tr.MakeTransparent(Color.White);
            center.Image = tr;
            center.SizeMode = PictureBoxSizeMode.Zoom;
            center.Top = right.Top;
            center.Left = up.Left;
            tab.Add(center);

            Button upT = new Button();
            upT.Text = "";
            upT.Height = up.Height;
            upT.Width = up.Width;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            upT.BackgroundImage = tr;
            upT.BackgroundImageLayout = ImageLayout.Zoom;
            upT.Top = up.Top;
            upT.Left = right.Left + 2 * right.Width + il;
            upT.Tag = eDirection.iUp;
            upT.Click += new EventHandler(move);
            tab.Add(upT);

            Button rightT = new Button();
            rightT.Text = up.Text;
            rightT.Height = up.Height;
            rightT.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            rightT.BackgroundImage = tr;
            rightT.BackgroundImageLayout = ImageLayout.Zoom;
            rightT.Top = upT.Top + upT.Height;
            rightT.Left = upT.Left + upT.Width;
            rightT.Tag = eDirection.iRight;
            rightT.Click += new EventHandler(move);
            tab.Add(rightT);

            Button downT = new Button();
            downT.Text = up.Text;
            downT.Height = up.Height;
            downT.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            downT.BackgroundImage = tr;
            downT.BackgroundImageLayout = ImageLayout.Zoom;
            downT.Top = rightT.Top + rightT.Height;
            downT.Left = upT.Left;
            downT.Tag = eDirection.iDown;
            downT.Click += new EventHandler(move);
            tab.Add(downT);

            Button leftT = new Button();
            leftT.Text = up.Text;
            leftT.Height = up.Height;
            leftT.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            leftT.BackgroundImage = tr;
            leftT.BackgroundImageLayout = ImageLayout.Zoom;
            leftT.Top = rightT.Top;
            leftT.Left = upT.Left - upT.Width;
            leftT.Tag = eDirection.iLeft;
            leftT.Click += new EventHandler(move);
            tab.Add(leftT);

            pbt = new PictureBox();
            pbt.Height = up.Height;
            pbt.Width = up.Width;
            if (fill) tr = Properties.Resources.TriangleFill;
            else tr = Properties.Resources.Triangle;
            tr.MakeTransparent(Color.White);
            pbt.Image = tr;
            pbt.SizeMode = PictureBoxSizeMode.Zoom;
            pbt.Top = rightT.Top;
            pbt.Left = upT.Left;
            tab.Add(pbt);

            return tab;
        }

        //---
        protected void refreshList()
        {
            List<string> points = getPoints();
            lb.Items.Clear();
            for (int i = 0; i < points.Count; i++)
                lb.Items.Add(points[i]);
            lb.SelectedIndex = selectedPoint;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                int x = (int)(sender as NumericUpDown).Value;
                lPt[selectedPoint] = new Point(x, lPt[selectedPoint].Y);
                refreshList();
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (lPt[selectedPoint].X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        int x = lPt[selectedPoint].X;
                        x--;
                        lPt[selectedPoint] = new Point(x, lPt[selectedPoint].Y);
                    }
                }
                else
                {
                    if (lPt[selectedPoint].X >= MAX)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        int x = lPt[selectedPoint].X;
                        x++;
                        lPt[selectedPoint] = new Point(x, lPt[selectedPoint].Y);
                    }
                }
            }
            nudx.Value = lPt[selectedPoint].X;
            refreshList();
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                int y = (int)(sender as NumericUpDown).Value;
                lPt[selectedPoint] = new Point(lPt[selectedPoint].X, y);
                refreshList();
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (lPt[selectedPoint].Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else
                    {
                        int y = lPt[selectedPoint].Y;
                        y--;
                        lPt[selectedPoint] = new Point(lPt[selectedPoint].X, y);
                    }
                }
                else
                {
                    if (lPt[selectedPoint].Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else
                    {
                        int y = lPt[selectedPoint].Y;
                        y++;
                        lPt[selectedPoint] = new Point(lPt[selectedPoint].X, y);
                    }
                }
            }
            nudy.Value = lPt[selectedPoint].Y;
            refreshList();
        }

        //---
        protected void fillChange(object sender, EventArgs e)
        {
            bool useCheckBox = true;
            if (sender.GetType() == typeof(CheckBox))
            {
                fill = (sender as CheckBox).Checked;
                useCheckBox = false;
            }
            if (useCheckBox) f.Checked = fill;

            Bitmap tr;

            if (fill) tr = Properties.Resources.TriangleFill;
            else tr = Properties.Resources.Triangle;

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbt.Image = tr;
        }

        //---
        protected void selIndChange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ListBox))
            {
                int ind = (sender as ListBox).SelectedIndex;
                if (ind < 0) return;
                selectedPoint = ind;
                nudx.Value = lPt[ind].X;
                nudy.Value = lPt[ind].Y;
            }
        }

        //---
        public override void move(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                eDirection v = (eDirection)((sender as Button).Tag);
                switch (v)
                {
                    case eDirection.iLeft:
                        for (int i = 0; i < 3; i++)
                        {
                            if (lPt[i].X == MIN)
                            {
                                MessageBox.Show("This triangle can not be moved to the left.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            int x = lPt[i].X;
                            x--;
                            lPt[i] = new Point(x, lPt[i].Y);
                        }

                        break;
                    //---------------------------------
                    case eDirection.iUp:
                        for (int i = 0; i < 3; i++)
                        {
                            if (lPt[i].Y == MIN)
                            {
                                MessageBox.Show("This triangle can not be moved to the up.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            int y = lPt[i].Y;
                            y--;
                            lPt[i] = new Point(lPt[i].X, y);
                        }
                        break;
                    //----------------------------------
                    case eDirection.iRight:
                        for (int i = 0; i < 3; i++)
                        {
                            if (lPt[i].X == MAX)
                            {
                                MessageBox.Show("This triangle can not be moved to the right.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            int x = lPt[i].X;
                            x++;
                            lPt[i] = new Point(x, lPt[i].Y);
                        }
                        break;
                    //---------------------------------
                    case eDirection.iDown:
                        for (int i = 0; i < 3; i++)
                        {
                            if (lPt[i].Y == MAX)
                            {
                                MessageBox.Show("This triangle can not be moved to the down.",
                                                 "Warning.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            int y = lPt[i].Y;
                            y++;
                            lPt[i] = new Point(lPt[i].X, y);
                        }
                        break;
                    //---------------------------------
                    default:
                        MessageBox.Show("Unknown command.",
                                                 "Error.",
                                                  MessageBoxButtons.OK,
                                                   MessageBoxIcon.Error);
                        return;
                }
                refreshList();
            }
        }

        //---
        public override Rectangle getRect()
        {
            Point min = lPt[0];
            Point max = lPt[0];
            for (int i = 1; i < 3; i++)
            {
                if (lPt[i].X < min.X) min.X = lPt[i].X;
                if (lPt[i].Y < min.Y) min.Y = lPt[i].Y;
                if (lPt[i].X > max.X) max.X = lPt[i].X;
                if (lPt[i].Y > max.Y) max.Y = lPt[i].Y;
            }

            return new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
        }

        //---
        protected List<string> getPoints()
        {
            List<string> lptemp = new List<string>();
            string abc = "ABC";
            for (int i = 0; i < 3; i++)
                lptemp.Add(abc[i] + " (" + lPt[i].X.ToString() + "," + lPt[i].Y.ToString() + ")");
            return lptemp;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();
            if (lPt[0] == lPt[1] && lPt[1] == lPt[2])
            {
                ts.Add(new errClass(eTest.errTrRedToPoint,
                    new int[0] { },
                    "Triangle" + this.getName() + ": All vertices have the same coordinates."));
                return ts;
            }

            if (lPt[0] == lPt[1])
            {
                ts.Add(new errClass(eTest.errTrDupPoint,
                    new int[2] { 0, 1 },
                    "Triangle" + this.getName() + ": Points A & B have the same coordinates."));
                return ts;
            }

            if (lPt[1] == lPt[2])
            {
                ts.Add(new errClass(eTest.errTrDupPoint,
                    new int[2] { 1, 2 },
                    "Triangle" + this.getName() + ": Points B & C have the same coordinates."));
                return ts;
            }

            if (lPt[2] == lPt[0])
            {
                ts.Add(new errClass(eTest.errTrDupPoint,
                    new int[2] { 0, 2 },
                    "Triangle" + this.getName() + ": Points A & C have the same coordinates."));
                return ts;
            }

            if ((lPt[1].X - lPt[0].X) != 0 && (lPt[2].X - lPt[0].X) != 0)
            {
                double a = (double)(lPt[1].Y - lPt[0].Y) / (double)(lPt[1].X - lPt[0].X);
                double b = (double)(lPt[2].Y - lPt[0].Y) / (double)(lPt[2].X - lPt[0].X);
                if (a == b) ts.Add(new errClass(eTest.errTrRedToLine,
                    new int[2] { -1, -1 },
                    "Triangle" + this.getName() + ": The vertices are collinear."));
            }
            else
            if ((lPt[1].X - lPt[0].X) == 0 && (lPt[2].X - lPt[0].X) == 0)
                ts.Add(new errClass(eTest.errTrRedToLine,
                    new int[2] { -1, -1 },
                   "Triangle" + this.getName() + ": The vertices are collinear."));

            return ts;
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(triangle obj1, triangle obj2)
        {
            if (obj1.fill == obj2.fill)
            {
                bool fl = true;
                for (int i = 0; i < 3; i++)
                {
                    if (obj1.lPt[i] != obj2.lPt[i])
                    {
                        fl = false;
                        break;
                    }
                }
                if (fl) return true;
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
        public static bool operator !=(triangle obj1, triangle obj2)
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

            return obj.GetType() == GetType() && Equals((triangle)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (lPt.ToString() + fill.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class rectangle : Item
    {
        //---
        protected bool fill;

        protected Rectangle r;

        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected NumericUpDown nudw;
        protected NumericUpDown nudh;
        protected CheckBox f;
        protected PictureBox pb;
        protected PictureBox pbc;

        //---

        public rectangle()
        {
            r = new Rectangle(MIN, MIN, 1, 1);
            fill = false;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            nudw = new NumericUpDown();
            nudh = new NumericUpDown();
            f = new CheckBox();
            pb = new PictureBox();
            pbc = new PictureBox();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            bool ok = test().Count == 0;
            name.Tag = ok;
            eImage im;
            eImage im2;
            string fl;
            if (fill)
            {
                fl = "Yes";
                if (ok) im = eImage.iRectangleFill;
                else im = eImage.iRectangleFillWar;
                im2 = eImage.iRectangleFill;
            }
            else
            {
                fl = "No";
                if (ok) im = eImage.iRectangle;
                else im = eImage.iRectangleWar;
                im2 = eImage.iRectangle;
            }
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            name.Nodes.Add("X= " + r.X);
            name.Nodes[0].ImageIndex = (int)eImage.iXpos;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

            name.Nodes.Add("Y= " + r.Y);
            name.Nodes[1].ImageIndex = (int)eImage.iYpos;
            name.Nodes[1].SelectedImageIndex = (int)eImage.iYpos;

            name.Nodes.Add("Width= " + r.Width);
            if (r.Width > 0)
            {
                name.Nodes[2].ImageIndex = (int)eImage.iWidth;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iWidth;
            }
            else
            {
                name.Nodes[2].ImageIndex = (int)eImage.iWidthWar;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iWidthWar;
            }
            name.Nodes.Add("Height= " + r.Height);
            if (r.Height > 0)
            {
                name.Nodes[3].ImageIndex = (int)eImage.iHeight;
                name.Nodes[3].SelectedImageIndex = (int)eImage.iHeight;
            }
            else
            {
                name.Nodes[3].ImageIndex = (int)eImage.iHeightWar;
                name.Nodes[3].SelectedImageIndex = (int)eImage.iHeightWar;
            }
            name.Nodes.Add("Filled rectangle : " + fl);
            name.Nodes[4].ImageIndex = (int)im2;
            name.Nodes[4].SelectedImageIndex = (int)im2;
            return name;
        }

        //---
        public override string ToString()
        {
            string l = "";
            if (fill) l = ((int)eCommand.fRectangle).ToString();
            else l = ((int)eCommand.dRectangle).ToString();
            l += ", " + r.X + ", ";
            l += r.Y.ToString() + ",";
            l += r.Size.Width.ToString() + ", ";
            l += r.Size.Height.ToString() + ", ";

            return l;
        }

        //---
        public override int itemLenght()
        {
            return 5;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            drawLine(ref canva, r.X, r.Y, r.X + r.Width, r.Y, cl, tag);
            drawLine(ref canva, r.X, r.Y + r.Height - 1, r.X + r.Width, r.Y, cl, tag);
            drawLine(ref canva, r.X, r.Y, r.X, r.Y + r.Height, cl, tag);
            drawLine(ref canva, r.X + r.Width - 1, r.Y, r.X, r.Y + r.Height, cl, tag);
            if (fill)
            {
                for (int i = 1; i < r.Height - 1 + 1; i++)
                {
                    drawLine(ref canva, r.X, r.Y, r.X + r.Width - 1, r.Y + i, cl, tag);
                }
            }

            if (cl == eColor.Selected)
            {
                drawPixel(ref canva, r.X, r.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, r.X + r.Width, r.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, r.X, r.Y + r.Height, eColor.SelectedPoint, tag);
                drawPixel(ref canva, r.X + r.Width, r.Y + r.Height, eColor.SelectedPoint, tag);
            }

            return cl;
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();
            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr;
            if (fill) tr = Properties.Resources.RectangleFill;
            else tr = Properties.Resources.Rectangle;
            loadInit(ref pb, tr, ref name, "Rectangle" + getName());

            tab.Add(pb);
            tab.Add(name);

            f = new CheckBox();
            f.Top = name.Top + name.Height + il;
            f.Left = il + labelWidth;
            f.Text = "Fill";
            f.Checked = fill;
            f.CheckedChanged += new EventHandler(fillChange);
            tab.Add(f);

            Label x = new Label();
            x.Text = "X=";
            x.Top = f.Top + f.Height + il * 6;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = il;
            y.Width = x.Width;
            tab.Add(y);

            Label h = new Label();
            h.Text = "H=";
            h.Top = y.Top + y.Height + il * 8;
            h.Left = il;
            h.Width = x.Width;
            tab.Add(h);

            Label w = new Label();
            w.Text = "W=";
            w.Top = h.Top + h.Height + il;
            w.Left = il;
            w.Width = x.Width;
            tab.Add(w);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = r.X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = nudx.Left;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = r.Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            nudh = new NumericUpDown();
            nudh.Top = h.Top - 3;
            nudh.Left = nudx.Left;
            nudh.Width = nudx.Width;
            nudh.Minimum = nudx.Minimum;
            nudh.Maximum = nudx.Maximum;
            nudh.Value = r.Height;
            nudh.ValueChanged += new EventHandler(valueHchange);
            tab.Add(nudh);

            nudw = new NumericUpDown();
            nudw.Top = w.Top - 3;
            nudw.Left = nudx.Left;
            nudw.Width = nudx.Width;
            nudw.Minimum = nudx.Minimum;
            nudw.Maximum = nudx.Maximum;
            nudw.Value = r.Width;
            nudw.ValueChanged += new EventHandler(valueWchange);
            tab.Add(nudw);

            Button up = new Button();
            up.Text = "";
            up.Height = nudx.Height + 6;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = nudx.Top + nudx.Height + il / 2 - (int)(up.Height * 1.5);
            up.Left = nudx.Left + nudx.Width + il * 2 + up.Width;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            pbc = new PictureBox();
            pbc.Height = up.Height;
            pbc.Width = up.Width;
            if (fill) tr = Properties.Resources.RectangleFill;
            else tr = Properties.Resources.Rectangle;
            tr.MakeTransparent(Color.White);
            pbc.Image = tr;
            pbc.SizeMode = PictureBoxSizeMode.Zoom;
            pbc.Top = right.Top;
            pbc.Left = up.Left;
            tab.Add(pbc);

            Button shrh = new Button();
            shrh.Text = "";
            shrh.Height = up.Height;
            shrh.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrh.BackgroundImage = tr;
            shrh.BackgroundImageLayout = ImageLayout.Zoom;
            shrh.Top = nudh.Top + nudh.Height + il / 2 - (int)(up.Height * 1.5);
            shrh.Left = nudh.Left + nudh.Width + il * 2 + up.Width;
            shrh.Tag = eDirection.iUp;
            shrh.Click += new EventHandler(valueHchange);
            tab.Add(shrh);

            Button shrr = new Button();
            shrr.Text = up.Text;
            shrr.Height = up.Height;
            shrr.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            shrr.BackgroundImage = tr;
            shrr.BackgroundImageLayout = ImageLayout.Zoom;
            shrr.Top = shrh.Top + shrh.Height;
            shrr.Left = shrh.Left + shrh.Width;
            shrr.Tag = eDirection.iRight;
            shrr.Click += new EventHandler(valueWchange);
            tab.Add(shrr);

            Button shrd = new Button();
            shrd.Text = up.Text;
            shrd.Height = up.Height;
            shrd.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            shrd.BackgroundImage = tr;
            shrd.BackgroundImageLayout = ImageLayout.Zoom;
            shrd.Top = shrr.Top + shrr.Height;
            shrd.Left = shrh.Left;
            shrd.Tag = eDirection.iDown;
            shrd.Click += new EventHandler(valueHchange);
            tab.Add(shrd);

            Button shrl = new Button();
            shrl.Text = up.Text;
            shrl.Height = up.Height;
            shrl.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrl.BackgroundImage = tr;
            shrl.BackgroundImageLayout = ImageLayout.Zoom;
            shrl.Top = shrr.Top;
            shrl.Left = shrh.Left - shrh.Width;
            shrl.Tag = eDirection.iLeft;
            shrl.Click += new EventHandler(valueWchange);
            tab.Add(shrl);

            PictureBox pbc2 = new PictureBox();
            pbc2.Height = up.Height;
            pbc2.Width = up.Width;
            tr = Properties.Resources.RectangleSize;
            tr.MakeTransparent(Color.White);
            pbc2.Image = tr;
            pbc2.SizeMode = PictureBoxSizeMode.Zoom;
            pbc2.Top = shrr.Top;
            pbc2.Left = shrh.Left;
            tab.Add(pbc2);

            return tab;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r.X = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (r.X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else r.X--;
                }
                else
                {
                    if (r.X >= 127)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else r.X++;
                }
            }
            nudx.Value = r.X;
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r.Y = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (r.Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r.Y--;
                }
                else
                {
                    if (r.Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r.Y++;
                }
            }
            nudy.Value = r.Y;
        }

        //---
        protected void valueWchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r.Width = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (r.Width == MIN)
                    {
                        MessageBox.Show("W=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else r.Width--;
                }
                else
                {
                    if (r.Width >= MAX)
                    {
                        MessageBox.Show("W=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else r.Width++;
                }
            }
            nudw.Value = r.Width;
        }

        //---
        protected void valueHchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r.Height = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (r.Height == MIN)
                    {
                        MessageBox.Show("H=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r.Height--;
                }
                else
                {
                    if (r.Height >= MAX)
                    {
                        MessageBox.Show("H=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r.Height++;
                }
            }
            nudh.Value = r.Height;
        }

        //---
        protected void fillChange(object sender, EventArgs e)
        {
            bool useCheckBox = true;
            if (sender.GetType() == typeof(CheckBox))
            {
                fill = (sender as CheckBox).Checked;
                useCheckBox = false;
            }
            if (useCheckBox) f.Checked = fill;

            Bitmap tr;

            if (fill) tr = Properties.Resources.RectangleFill;
            else tr = Properties.Resources.Rectangle;

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbc.Image = tr;
        }

        //---
        public override Rectangle getRect()
        {
            return r;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();
            if (r.Width == 0 && r.Height == 0)
            {
                ts.Add(new errClass(eTest.errRecRedToPoint,
                    new int[0] { },
                    "Rectangle" + this.getName() + ": Reduced to point."));
                return ts;
            }
            if (r.Width == 0)
            {
                ts.Add(new errClass(eTest.errRecWidth,
                    new int[0],
                    "Rectangle" + this.getName() + ": Width =0."));
                return ts;
            }
            if (r.Height == 0)
            {
                ts.Add(new errClass(eTest.errRecHeight,
                    new int[0],
                  "Rectangle" + this.getName() + ": Height =0."));
                return ts;
            }
            return ts;
        }

        //---

        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(rectangle obj1, rectangle obj2)
        {
            if (obj1.r == obj2.r && obj1.fill == obj2.fill) return true;
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
        public static bool operator !=(rectangle obj1, rectangle obj2)
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

            return obj.GetType() == GetType() && Equals((rectangle)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (r.ToString() + fill.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class roundRect : Item
    {
        //---
        protected bool fill;

        protected Rectangle rec;
        protected byte r;

        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected NumericUpDown nudw;
        protected NumericUpDown nudh;
        protected NumericUpDown nur;
        protected CheckBox f;
        protected PictureBox pb;
        protected PictureBox pbc;

        //---

        public roundRect()
        {
            rec = new Rectangle(MIN, MIN, 3, 3);
            r = 1;
            fill = false;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            nudw = new NumericUpDown();
            nudh = new NumericUpDown();
            nur = new NumericUpDown();
            f = new CheckBox();
            pb = new PictureBox();
            pbc = new PictureBox();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            bool ok = test().Count == 0;
            name.Tag = ok;
            eImage im;
            eImage im2;
            string fl;
            if (fill)
            {
                fl = "Yes";
                im = eImage.iRoundRectFill;
            }
            else
            {
                fl = "No";
                im = eImage.iRoundRect;
            }
            im2 = im;
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            name.Nodes.Add("X= " + rec.X);
            name.Nodes[0].ImageIndex = (int)eImage.iXpos;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

            name.Nodes.Add("Y= " + rec.Y);
            name.Nodes[1].ImageIndex = (int)eImage.iYpos;
            name.Nodes[1].SelectedImageIndex = (int)eImage.iYpos;

            name.Nodes.Add("Width= " + rec.Width);

            name.Nodes[2].ImageIndex = (int)eImage.iWidth;
            name.Nodes[2].SelectedImageIndex = (int)eImage.iWidth;

            name.Nodes.Add("Height= " + rec.Height);

            name.Nodes[3].ImageIndex = (int)eImage.iHeight;
            name.Nodes[3].SelectedImageIndex = (int)eImage.iHeight;

            name.Nodes.Add("Corner R= " + r);

            name.Nodes[4].ImageIndex = (int)eImage.iRadiusArc;
            name.Nodes[4].SelectedImageIndex = (int)eImage.iRadiusArc;

            name.Nodes.Add("Filled rectangle : " + fl);
            name.Nodes[5].ImageIndex = (int)im2;
            name.Nodes[4].SelectedImageIndex = (int)im2;
            return name;
        }

        //---
        public override string ToString()
        {
            string l = "";
            if (fill) l = ((int)eCommand.fRectangle).ToString();
            else l = ((int)eCommand.dRectangle).ToString();
            l += ", " + rec.X + ", ";
            l += rec.Y.ToString() + ",";
            l += rec.Size.Width.ToString() + ", ";
            l += rec.Size.Height.ToString() + ", ";
            l += r.ToString();

            return l;
        }

        //---
        public override int itemLenght()
        {
            return 6;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            if (rec.Width < 2 * r || rec.Height < 2 * r) return cl; // Stop iteration problems on corners
            if (fill)
            {
                for (int i = 0; i < rec.Height; i++)
                {
                    drawLine(ref canva, rec.X, rec.Y + i, rec.X + rec.Width, rec.Y + i, cl, tag);
                }

                // drarec.Width four corners
                fillCircleHelper(ref canva, rec.X + rec.Width - r - 1, rec.Y + r, r, 1, rec.Height - r - r - 1, cl, tag);
                fillCircleHelper(ref canva, rec.X + r, rec.Y + r, r, 2, rec.Height - r - r - 1, cl, tag);
            }
            else
            {
                drawLine(ref canva,
                    rec.X + r, rec.Y, rec.X + r + rec.Width - r - r, rec.Y,
                    cl, tag); // Top
                drawLine(ref canva,
                    rec.X + r, rec.Y + rec.Height - 1, rec.X + r + rec.Width - r - r, rec.Y + rec.Height - 1,
                    cl, tag); // Bottom
                drawLine(ref canva,
                    rec.X, rec.Y + r, rec.X, rec.Y + r + rec.Height - r - r,
                    cl, tag); // Left
                drawLine(ref canva,
                    rec.X + rec.Width - 1, rec.Y + r, rec.X + rec.Width - 1, rec.Y + r + rec.Height - r - r,
                    cl, tag); // Right
                              // draw four corners
                drawCircleHelper(ref canva,
                    rec.X + r, rec.Y + r, r, (eQuarter)1,
                    cl, tag);
                drawCircleHelper(ref canva,
                    rec.X + rec.Width - r - 1, rec.Y + r, r, (eQuarter)2,
                    cl, tag);
                drawCircleHelper(ref canva,
                    rec.X + rec.Width - r - 1, rec.Y + rec.Height - r - 1, r, (eQuarter)4,
                    cl, tag);
                drawCircleHelper(ref canva,
                    rec.X + r, rec.Y + rec.Height - r - 1, r, (eQuarter)8,
                    cl, tag);
            }

            if (cl == eColor.Selected)
            {
                drawPixel(ref canva, rec.X, rec.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, rec.X + rec.Width, rec.Y, eColor.SelectedPoint, tag);
                drawPixel(ref canva, rec.X, rec.Y + rec.Height, eColor.SelectedPoint, tag);
                drawPixel(ref canva, rec.X + rec.Width, rec.Y + rec.Height, eColor.SelectedPoint, tag);
            }

            return cl;
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();
            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr;
            if (fill) tr = Properties.Resources.roundRectFill;
            else tr = Properties.Resources.roundRect;
            loadInit(ref pb, tr, ref name, "RoundRect" + getName());

            tab.Add(pb);
            tab.Add(name);

            f = new CheckBox();
            f.Top = name.Top + name.Height + il;
            f.Left = il + labelWidth;
            f.Text = "Fill";
            f.Checked = fill;
            f.CheckedChanged += new EventHandler(fillChange);
            tab.Add(f);

            Label x = new Label();
            x.Text = "X=";
            x.Top = f.Top + f.Height + il * 6;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = il;
            y.Width = x.Width;
            tab.Add(y);

            Label h = new Label();
            h.Text = "H=";
            h.Top = y.Top + y.Height + il * 8;
            h.Left = il;
            h.Width = x.Width;
            tab.Add(h);

            Label w = new Label();
            w.Text = "W=";
            w.Top = h.Top + h.Height + il;
            w.Left = il;
            w.Width = x.Width;
            tab.Add(w);

            Label lr = new Label();
            lr.Text = "R=";
            lr.Top = w.Top + w.Height + il * 6;
            lr.Left = il;
            lr.Width = x.Width;
            tab.Add(lr);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = rec.X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = nudx.Left;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = rec.Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            nudh = new NumericUpDown();
            nudh.Top = h.Top - 3;
            nudh.Left = nudx.Left;
            nudh.Width = nudx.Width;
            nudh.Minimum = nudx.Minimum;
            nudh.Maximum = nudx.Maximum;
            nudh.Value = rec.Height;
            nudh.ValueChanged += new EventHandler(valueHchange);
            tab.Add(nudh);

            nudw = new NumericUpDown();
            nudw.Top = w.Top - 3;
            nudw.Left = nudx.Left;
            nudw.Width = nudx.Width;
            nudw.Minimum = nudx.Minimum;
            nudw.Maximum = nudx.Maximum;
            nudw.Value = rec.Width;
            nudw.ValueChanged += new EventHandler(valueWchange);
            tab.Add(nudw);

            nur = new NumericUpDown();
            nur.Top = lr.Top - 3;
            nur.Left = nudx.Left;
            nur.Width = nudx.Width;
            nur.Minimum = 1;
            nur.Maximum = nudx.Maximum;
            nur.Value = r;
            nur.ValueChanged += new EventHandler(valueRchange);
            tab.Add(nur);

            Button up = new Button();
            up.Text = "";
            up.Height = nudx.Height + 6;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = nudx.Top + nudx.Height + il / 2 - (int)(up.Height * 1.5);
            up.Left = nudx.Left + nudx.Width + il * 2 + up.Width;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            pbc = new PictureBox();
            pbc.Height = up.Height;
            pbc.Width = up.Width;
            if (fill) tr = Properties.Resources.roundRectFill;
            else tr = Properties.Resources.roundRect;
            tr.MakeTransparent(Color.White);
            pbc.Image = tr;
            pbc.SizeMode = PictureBoxSizeMode.Zoom;
            pbc.Top = right.Top;
            pbc.Left = up.Left;
            tab.Add(pbc);

            Button shrh = new Button();
            shrh.Text = "";
            shrh.Height = up.Height;
            shrh.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrh.BackgroundImage = tr;
            shrh.BackgroundImageLayout = ImageLayout.Zoom;
            shrh.Top = nudh.Top + nudh.Height + il / 2 - (int)(up.Height * 1.5);
            shrh.Left = nudh.Left + nudh.Width + il * 2 + up.Width;
            shrh.Tag = eDirection.iUp;
            shrh.Click += new EventHandler(valueHchange);
            tab.Add(shrh);

            Button shrr = new Button();
            shrr.Text = up.Text;
            shrr.Height = up.Height;
            shrr.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            shrr.BackgroundImage = tr;
            shrr.BackgroundImageLayout = ImageLayout.Zoom;
            shrr.Top = shrh.Top + shrh.Height;
            shrr.Left = shrh.Left + shrh.Width;
            shrr.Tag = eDirection.iRight;
            shrr.Click += new EventHandler(valueWchange);
            tab.Add(shrr);

            Button shrd = new Button();
            shrd.Text = up.Text;
            shrd.Height = up.Height;
            shrd.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            shrd.BackgroundImage = tr;
            shrd.BackgroundImageLayout = ImageLayout.Zoom;
            shrd.Top = shrr.Top + shrr.Height;
            shrd.Left = shrh.Left;
            shrd.Tag = eDirection.iDown;
            shrd.Click += new EventHandler(valueHchange);
            tab.Add(shrd);

            Button shrl = new Button();
            shrl.Text = up.Text;
            shrl.Height = up.Height;
            shrl.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrl.BackgroundImage = tr;
            shrl.BackgroundImageLayout = ImageLayout.Zoom;
            shrl.Top = shrr.Top;
            shrl.Left = shrh.Left - shrh.Width;
            shrl.Tag = eDirection.iLeft;
            shrl.Click += new EventHandler(valueWchange);
            tab.Add(shrl);

            PictureBox pbc2 = new PictureBox();
            pbc2.Height = up.Height;
            pbc2.Width = up.Width;
            tr = Properties.Resources.RectangleSize;
            tr.MakeTransparent(Color.White);
            pbc2.Image = tr;
            pbc2.SizeMode = PictureBoxSizeMode.Zoom;
            pbc2.Top = shrr.Top;
            pbc2.Left = shrh.Left;
            tab.Add(pbc2);

            Button shrink = new Button();
            shrink.Text = "";
            shrink.Height = up.Height;
            shrink.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrink.BackgroundImage = tr;
            shrink.BackgroundImageLayout = ImageLayout.Zoom;
            shrink.Top = nur.Top + nur.Height / 2 - shrink.Height / 2;
            shrink.Left = nur.Left + nur.Width + 2 * il;
            shrink.Tag = eDirection.iLeft;
            shrink.Click += new EventHandler(valueRchange);
            tab.Add(shrink);

            Button increase = new Button();
            increase.Text = shrink.Text;
            increase.Height = shrink.Height;
            increase.Width = shrink.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increase.BackgroundImage = tr;
            increase.BackgroundImageLayout = ImageLayout.Zoom;
            increase.Top = shrink.Top;
            increase.Left = shrink.Left + 2 * shrink.Width;
            increase.Tag = eDirection.iRight;
            increase.Click += new EventHandler(valueRchange);
            tab.Add(increase);

            PictureBox pbr = new PictureBox();

            pbr.Height = shrink.Height;
            pbr.Width = shrink.Width;
            pbr.Top = shrink.Top;
            pbr.Left = shrink.Left + shrink.Width;

            tr = Properties.Resources.Arc;
            tr.MakeTransparent(Color.White);

            pbr.Image = tr;
            pbr.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbr);

            return tab;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                rec.X = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (rec.X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else rec.X--;
                }
                else
                {
                    if (rec.X >= 127)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else rec.X++;
                }
            }
            nudx.Value = rec.X;
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                rec.Y = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (rec.Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else rec.Y--;
                }
                else
                {
                    if (rec.Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else rec.Y++;
                }
            }
            nudy.Value = rec.Y;
        }

        //---
        protected void valueWchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                rec.Width = (int)(sender as NumericUpDown).Value;
                if (r * 2 > rec.Width)
                {
                    rec.Width = r * 2;
                    (sender as NumericUpDown).Value = rec.Width;
                }
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if ((rec.Width - 1) < 2 * r)
                    {
                        MessageBox.Show("W=2*R is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);

                        return;
                    }
                    else rec.Width--;
                }
                else
                {
                    if (rec.Width >= MAX)
                    {
                        MessageBox.Show("W=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else rec.Width++;
                }
            }
            nudw.Value = rec.Width;
        }

        //---
        protected void valueHchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                rec.Height = (int)(sender as NumericUpDown).Value;

                if (r * 2 > rec.Height)
                {
                    rec.Height = r * 2;
                    (sender as NumericUpDown).Value = rec.Height;
                }
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if ((rec.Height - 1) < 2 * r)
                    {
                        MessageBox.Show("H=2*R is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else rec.Height--;
                }
                else
                {
                    if (rec.Height >= MAX)
                    {
                        MessageBox.Show("H=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else rec.Height++;
                }
            }
            nudh.Value = rec.Height;
        }

        //---
        protected void valueRchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r = (byte)(sender as NumericUpDown).Value;
                if (r * 2 > rec.Width || r * 2 > rec.Height)
                {
                    int low = rec.Width;
                    if (rec.Height < rec.Width) low = rec.Height;
                    r = (byte)(low >> 1);
                }
                nur.Value = r;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (r <= 1)
                    {
                        MessageBox.Show("R=1 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        r = 1;
                    }
                    else r--;
                }
                else
                {
                    if ((r + 1) * 2 > rec.Height || (r + 1) * 2 > rec.Width)
                    {
                        int low = rec.Width;
                        string strL = "Width";

                        if (rec.Height < rec.Width)
                        {
                            low = rec.Height;
                            strL = "Height";
                        }
                        r = (byte)(low >> 1);

                        MessageBox.Show("R*2=" + strL + " is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r++;
                }
            }
            nur.Value = r;
        }

        //---
        protected void fillChange(object sender, EventArgs e)
        {
            bool useCheckBox = true;
            if (sender.GetType() == typeof(CheckBox))
            {
                fill = (sender as CheckBox).Checked;
                useCheckBox = false;
            }
            if (useCheckBox) f.Checked = fill;

            Bitmap tr;

            if (fill) tr = Properties.Resources.roundRectFill;
            else tr = Properties.Resources.roundRect;

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbc.Image = tr;
        }

        //---
        public override Rectangle getRect()
        {
            return rec;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();

            return ts;
        }

        //---

        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(roundRect obj1, roundRect obj2)
        {
            if (obj1.rec == obj2.rec && obj1.fill == obj2.fill &&
                obj1.r == obj2.r) return true;
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
        public static bool operator !=(roundRect obj1, roundRect obj2)
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

            return obj.GetType() == GetType() && Equals((roundRect)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (r.ToString() + rec.ToString() + fill.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class polygon : Item
    {
        //---
        protected Point c;

        protected List<Point> lPt;

        protected byte r;
        protected bool fill;
        protected byte side;
        protected byte angle;
        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected NumericUpDown nudr;
        protected NumericUpDown nudsid;
        protected NumericUpDown nudang;

        protected CheckBox f;
        protected PictureBox pb;
        protected PictureBox pbc;

        //---
        public polygon()
        {
            fill = false;
            c = new Point(1, 1);
            r = 1;
            side = 3;
            angle = 0;
            lPt = getPoint();
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            nudr = new NumericUpDown();
            nudsid = new NumericUpDown();
            nudang = new NumericUpDown();
            f = new CheckBox();
            pb = new PictureBox();
            pbc = new PictureBox();
        }

        //---
        protected List<Point> getPoint()
        {
            List<Point> l = new List<Point>();

            for (int i = 0; i < side; i++)
            {
                int x = (int)(-r * Math.Cos((double)angle / 180 * Math.PI));
                int y = (int)(-r * Math.Sin((double)angle / 180 * Math.PI));
                l.Add(new Point(x + c.X, y + c.Y));
            }

            return l;
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            bool ok = test().Count == 0;
            name.Tag = ok;
            eImage im;
            eImage im2;
            string fl;
            if (fill)
            {
                im = eImage.iPolygonFill;

                fl = "Yes";
            }
            else
            {
                im = eImage.iPolygon;

                fl = "No";
            }
            im2 = im;
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            name.Nodes.Add("X= " + c.X);
            name.Nodes[0].ImageIndex = (int)eImage.iXpos;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

            name.Nodes.Add("Y= " + c.Y);
            name.Nodes[1].ImageIndex = (int)eImage.iYpos;
            name.Nodes[1].SelectedImageIndex = (int)eImage.iYpos;

            name.Nodes.Add("R= " + r);
            if (r > 0)
            {
                name.Nodes[2].ImageIndex = (int)eImage.iRadiusCircle;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iRadiusCircle;
            }
            else
            {
                name.Nodes[2].ImageIndex = (int)eImage.iRadiusCircleWar;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iRadiusCircleWar;
            }
            name.Nodes.Add("Number of sides" + side);
            name.Nodes[3].ImageIndex = (int)eImage.iStd;
            name.Nodes[3].SelectedImageIndex = (int)eImage.iStd;

            name.Nodes.Add("Origin angle =" + angle);
            name.Nodes[4].ImageIndex = (int)eImage.iRadiusArc;
            name.Nodes[4].SelectedImageIndex = (int)eImage.iRadiusArc;

            name.Nodes.Add("Filled polygon : " + fl);
            name.Nodes[5].ImageIndex = (int)im2;
            name.Nodes[5].SelectedImageIndex = (int)im2;
            return name;
        }

        //---
        public override string ToString()
        {
            string l = "";
            if (fill) l = ((int)eCommand.fCircle).ToString();
            else l = ((int)eCommand.dCircle).ToString();
            l += ", " + c.X.ToString() + ", ";
            l += c.Y.ToString() + ", ";
            l += r.ToString() + ", ";
            l += side.ToString();
            l += angle.ToString();
            return l;
        }

        //---
        public override int itemLenght()
        {
            return 6;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            if (fill)
            {
                for (int i = 0; i < lPt.Count - 1; i++)
                {
                    fillTriangle(ref canva, c, lPt[i], lPt[i + 1], cl, tag);
                }
                fillTriangle(ref canva, c, lPt[lPt.Count - 1], lPt[0], cl, tag);
            }
            else
            {
                for (int i = 0; i < lPt.Count - 1; i++)
                {
                    drawLine(ref canva, lPt[i], lPt[i + 1], cl, tag);
                }
                drawLine(ref canva, lPt[lPt.Count - 1], lPt[0], cl, tag);
            }
            if (cl == eColor.Selected)
            {
                drawPixel(ref canva, c.X, c.Y, eColor.SelectedPoint, tag);
                for (int i = 0; i < lPt.Count; i++)
                {
                    drawPixel(ref canva, lPt[i].X, lPt[i].Y, eColor.SelectedPoint, tag);
                }
            }

            return cl;
        }

        //---
        public override Rectangle getRect()
        {
            Point min = lPt[0];
            Point max = lPt[0];
            for (int i = 1; i < lPt.Count; i++)
            {
                if (lPt[i].X < min.X) min.X = lPt[i].X;
                if (lPt[i].Y < min.Y) min.Y = lPt[i].Y;
                if (lPt[i].X > max.X) max.X = lPt[i].X;
                if (lPt[i].Y > max.Y) max.Y = lPt[i].Y;
            }

            return new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();
            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr;
            if (fill) tr = Properties.Resources.PolygonFill;
            else tr = Properties.Resources.Polygon;
            loadInit(ref pb, tr, ref name, "Polygon" + getName());

            tab.Add(pb);
            tab.Add(name);

            f = new CheckBox();
            f.Top = name.Top + name.Height + il;
            f.Left = il + labelWidth;
            f.Text = "Fill";
            f.Checked = fill;
            f.CheckedChanged += new EventHandler(fillChange);
            tab.Add(f);

            Label x = new Label();
            x.Text = "X=";
            x.Top = f.Top + f.Height + il * 6;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = il;
            y.Width = x.Width;
            tab.Add(y);

            Label ra = new Label();
            ra.Text = "R=";
            ra.Top = y.Top + y.Height + il * 5;
            ra.Left = il;
            ra.Width = x.Width;
            tab.Add(ra);

            Label ls = new Label();
            ls.Text = "Sides";
            ls.Top = ra.Top + ra.Height + il;
            ls.Left = il;
            ls.Width = x.Width;
            tab.Add(ls);

            Label lang = new Label();
            lang.Text = "Angle";
            lang.Top = ls.Top + ls.Height + il;
            lang.Left = il;
            lang.Width = x.Width;
            tab.Add(lang);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = c.X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = y.Width + il;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = c.Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            nudr = new NumericUpDown();
            nudr.Top = ra.Top - 3;
            nudr.Left = ra.Width + il;
            nudr.Width = nudx.Width;
            nudr.Minimum = 1;
            nudr.Maximum = nudx.Maximum;
            nudr.Value = r;
            nudr.ValueChanged += new EventHandler(valueRchange);
            tab.Add(nudr);

            nudsid = new NumericUpDown();
            nudsid.Top = ls.Top - 3;
            nudsid.Left = ls.Width + il;
            nudsid.Width = nudx.Width;
            nudsid.Minimum = 3;
            nudsid.Maximum = 20;
            nudsid.Value = side;
            nudsid.ValueChanged += new EventHandler(valueSideChange);
            tab.Add(nudsid);

            nudang = new NumericUpDown();
            nudang.Top = lang.Top - 3;
            nudang.Left = ls.Width + il;
            nudang.Width = nudx.Width;
            nudang.Minimum = 0;
            nudang.Maximum = 120;
            nudang.Value = angle;
            nudang.ValueChanged += new EventHandler(valueAngChange);
            tab.Add(nudang);

            Button up = new Button();
            up.Text = "";
            up.Height = nudx.Height + 6;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = nudx.Top + nudx.Height + il / 2 - (int)(up.Height * 1.5);
            up.Left = nudx.Left + nudx.Width + il * 2 + up.Width;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            pbc = new PictureBox();
            pbc.Height = up.Height;
            pbc.Width = up.Width;
            if (fill) tr = Properties.Resources.PolygonFill;
            else tr = Properties.Resources.Polygon;
            tr.MakeTransparent(Color.White);
            pbc.Image = tr;
            pbc.SizeMode = PictureBoxSizeMode.Zoom;
            pbc.Top = right.Top;
            pbc.Left = up.Left;
            tab.Add(pbc);

            Button shrink = new Button();
            shrink.Text = up.Text;
            shrink.Height = nudr.Height + 6;
            shrink.Width = shrink.Height;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrink.BackgroundImage = tr;
            shrink.BackgroundImageLayout = ImageLayout.Zoom;
            shrink.Top = nudr.Top - 3;
            shrink.Left = nudr.Left + nudr.Width + il;
            shrink.Tag = eDirection.iLeft;
            shrink.Click += new EventHandler(valueRchange);
            tab.Add(shrink);

            Button increase = new Button();
            increase.Text = up.Text;
            increase.Height = shrink.Height;
            increase.Width = shrink.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increase.BackgroundImage = tr;
            increase.BackgroundImageLayout = ImageLayout.Zoom;
            increase.Top = shrink.Top;
            increase.Left = shrink.Left + 2 * shrink.Width;
            increase.Tag = eDirection.iRight;
            increase.Click += new EventHandler(valueRchange);
            tab.Add(increase);

            PictureBox pbr = new PictureBox();

            pbr.Height = shrink.Height;
            pbr.Width = shrink.Width;
            pbr.Top = shrink.Top;
            pbr.Left = shrink.Left + shrink.Width;

            tr = Properties.Resources.Radius;
            tr.MakeTransparent(Color.White);

            pbr.Image = tr;
            pbr.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbr);

            Button shrinkSide = new Button();
            shrinkSide.Text = up.Text;
            shrinkSide.Height = shrink.Height;
            shrinkSide.Width = shrinkSide.Height;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrinkSide.BackgroundImage = tr;
            shrinkSide.BackgroundImageLayout = ImageLayout.Zoom;
            shrinkSide.Top = nudsid.Top - 3;
            shrinkSide.Left = nudsid.Left + nudsid.Width + il;
            shrinkSide.Tag = eDirection.iLeft;
            shrinkSide.Click += new EventHandler(valueSideChange);
            tab.Add(shrinkSide);

            Button increaseSide = new Button();
            increaseSide.Text = up.Text;
            increaseSide.Height = shrink.Height;
            increaseSide.Width = shrink.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increaseSide.BackgroundImage = tr;
            increaseSide.BackgroundImageLayout = ImageLayout.Zoom;
            increaseSide.Top = shrinkSide.Top;
            increaseSide.Left = shrinkSide.Left + 2 * shrinkSide.Width;
            increaseSide.Tag = eDirection.iRight;
            increaseSide.Click += new EventHandler(valueSideChange);
            tab.Add(increaseSide);

            PictureBox pbSid = new PictureBox();

            pbSid.Height = shrink.Height;
            pbSid.Width = shrink.Width;
            pbSid.Top = shrinkSide.Top;
            pbSid.Left = shrinkSide.Left + shrinkSide.Width;

            tr = Properties.Resources.Count;
            tr.MakeTransparent(Color.White);

            pbSid.Image = tr;
            pbSid.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbSid);

            Button shrinkAng = new Button();
            shrinkAng.Text = up.Text;
            shrinkAng.Height = shrink.Height;
            shrinkAng.Width = shrinkAng.Height;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrinkAng.BackgroundImage = tr;
            shrinkAng.BackgroundImageLayout = ImageLayout.Zoom;
            shrinkAng.Top = nudang.Top - 3;
            shrinkAng.Left = nudang.Left + nudang.Width + il;
            shrinkAng.Tag = eDirection.iLeft;
            shrinkAng.Click += new EventHandler(valueAngChange);
            tab.Add(shrinkAng);

            Button increaseAng = new Button();
            increaseAng.Text = up.Text;
            increaseAng.Height = shrink.Height;
            increaseAng.Width = shrink.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increaseAng.BackgroundImage = tr;
            increaseAng.BackgroundImageLayout = ImageLayout.Zoom;
            increaseAng.Top = shrinkAng.Top;
            increaseAng.Left = shrinkAng.Left + 2 * shrinkAng.Width;
            increaseAng.Tag = eDirection.iRight;
            increaseAng.Click += new EventHandler(valueAngChange);
            tab.Add(increaseAng);

            PictureBox pbAng = new PictureBox();

            pbAng.Height = shrink.Height;
            pbAng.Width = shrink.Width;
            pbAng.Top = shrinkAng.Top;
            pbAng.Left = shrinkAng.Left + shrinkAng.Width;

            tr = Properties.Resources.ArcRadius;
            tr.MakeTransparent(Color.White);

            pbAng.Image = tr;
            pbAng.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbAng);

            return tab;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.X = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (c.X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X--;
                }
                else
                {
                    if (c.X >= 127)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X++;
                }
            }
            nudx.Value = c.X;
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.Y = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (c.Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y--;
                }
                else
                {
                    if (c.Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y++;
                }
            }
            nudy.Value = c.Y;
        }

        //---
        protected void valueRchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r = (byte)(sender as NumericUpDown).Value;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (r == 1)
                    {
                        MessageBox.Show("R=1 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r--;
                }
                else
                {
                    if (r >= MAX)
                    {
                        MessageBox.Show("R=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r++;
                }
            }
            nudr.Value = r;
        }

        //---
        protected void valueSideChange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                side = (byte)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (side <= 3)
                    {
                        MessageBox.Show("Side=3 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        side = 3;
                        nudsid.Value = side;
                        return;
                    }
                    else side--;
                }
                else
                {
                    if (side >= 20)
                    {
                        MessageBox.Show("Side=20 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        side = 20;
                        nudsid.Value = side;
                        return;
                    }
                    else side++;
                }
            }
            nudsid.Value = side;
        }

        //---
        protected void valueAngChange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                angle = (byte)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (side <= MIN)
                    {
                        MessageBox.Show("Angle=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        angle = MIN;
                        nudang.Value = angle;
                        return;
                    }
                    else angle--;
                }
                else
                {
                    if (angle >= 120)
                    {
                        MessageBox.Show("Angle=120 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        angle = 120;
                        nudang.Value = angle;
                        return;
                    }
                    else angle++;
                }
            }
            nudang.Value = angle;
        }

        //---
        protected void fillChange(object sender, EventArgs e)
        {
            bool useCheckBox = true;
            if (sender.GetType() == typeof(CheckBox))
            {
                fill = (sender as CheckBox).Checked;
                useCheckBox = false;
            }
            if (useCheckBox) f.Checked = fill;

            Bitmap tr;

            if (fill) tr = Properties.Resources.PolygonFill;
            else tr = Properties.Resources.Polygon;

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbc.Image = tr;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();

            for (int i = 0; i < lPt.Count; i++)
            {
                if (lPt[i].X < 0)
                {
                    ts.Add(new errClass(eTest.errOriXUnder0,
                  new int[0] { },
                  "Polygon" + this.getName() + ": Boundary X<0."));
                    break;
                }
            }
            for (int i = 0; i < lPt.Count; i++)
            {
                if (lPt[i].Y < 0)
                {
                    ts.Add(new errClass(eTest.errOriYUnder0,
                  new int[0] { },
                  "Polygon" + this.getName() + ": Boundary Y<0."));
                    break;
                }
            }
            return ts;
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(polygon obj1, polygon obj2)
        {
            if (obj1.c == obj2.c && obj1.r == obj2.r &&
                obj1.fill == obj2.fill &&
                obj1.side == obj2.side && obj1.angle == obj2.angle) return true;
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
        public static bool operator !=(polygon obj1, polygon obj2)
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

            return obj.GetType() == GetType() && Equals((polygon)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (c.X.ToString() + c.Y.ToString() + r.ToString() +
                    fill.ToString() + side.ToString() + angle.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class arc : Item
    {
        //---
        protected bool fill;

        protected Point c;
        protected byte r;
        protected eQuarter q;
        protected NumericUpDown nudx;
        protected NumericUpDown nudy;
        protected NumericUpDown nudr;
        protected CheckBox f;
        protected PictureBox pb;
        protected PictureBox pbc;
        protected ComboBox cb;

        //---
        public arc()
        {
            fill = false;
            c = new Point(1, 1);
            r = 1;
            q = eQuarter.qUR;
            nudx = new NumericUpDown();
            nudy = new NumericUpDown();
            nudr = new NumericUpDown();
            f = new CheckBox();
            pb = new PictureBox();
            pbc = new PictureBox();
            cb = new ComboBox();
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            bool ok = test().Count == 0;
            name.Tag = ok;
            eImage im;
            eImage im2;
            if (fill)
            {
                switch (q)
                {
                    case eQuarter.qUL:
                        if (ok) im = eImage.iArcUlFill;
                        else im = eImage.iArcULfillWar;
                        im2 = eImage.iArcUlFill;
                        break;

                    case eQuarter.qUR:
                        if (ok) im = eImage.iArcUrFill;
                        else im = eImage.iArcURfillWar;
                        im2 = eImage.iArcUrFill;
                        break;

                    case eQuarter.qDL:
                        if (ok) im = eImage.iArcDlFill;
                        else im = eImage.iArcDLfillWar;
                        im2 = eImage.iArcDlFill;
                        break;

                    case eQuarter.qDR:
                        if (ok) im = eImage.iArcDrFill;
                        else im = eImage.iArcDRfillWar;
                        im2 = eImage.iArcDrFill;
                        break;

                    default:
                        im = eImage.iEmpty;
                        im2 = im;
                        break;
                }
            }
            else
            {
                switch (q)
                {
                    case eQuarter.qUL:
                        if (ok) im = eImage.iArc;
                        else im = eImage.iArcULwar;
                        im2 = eImage.iArc;
                        break;

                    case eQuarter.qUR:
                        if (ok) im = eImage.iArcUr;
                        else im = eImage.iArcURwar;
                        im2 = eImage.iArcUr;
                        break;

                    case eQuarter.qDL:
                        if (ok) im = eImage.iArcDl;
                        else im = eImage.iArcDLwar;
                        im2 = eImage.iArcDl;
                        break;

                    case eQuarter.qDR:
                        if (ok) im = eImage.iArcDr;
                        else im = eImage.iArcDRwar;
                        im2 = eImage.iArcDr;
                        break;

                    default:
                        im = eImage.iEmpty;
                        im2 = im;
                        break;
                }
            }
            name.ImageIndex = (int)im;
            name.SelectedImageIndex = (int)im;

            name.Nodes.Add("X= " + c.X);
            name.Nodes[0].ImageIndex = (int)eImage.iXpos;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iXpos;

            name.Nodes.Add("Y= " + c.Y);
            name.Nodes[1].ImageIndex = (int)eImage.iYpos;
            name.Nodes[1].SelectedImageIndex = (int)eImage.iYpos;

            name.Nodes.Add("R= " + r);
            if (r > 0)
            {
                name.Nodes[2].ImageIndex = (int)eImage.iRadiusArc;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iRadiusArc;
            }
            else
            {
                name.Nodes[2].ImageIndex = (int)eImage.iRadiusArcWar;
                name.Nodes[2].SelectedImageIndex = (int)eImage.iRadiusArcWar;
            }
            string fl = "No";
            if (fill) fl = "Yes";
            name.Nodes.Add("Filled arc : " + fl);
            name.Nodes[3].ImageIndex = (int)im2;
            name.Nodes[3].SelectedImageIndex = (int)im2;

            string qs = "";
            switch (q)
            {
                case eQuarter.qUL:
                    qs = "upper left";
                    im = eImage.iUL;
                    break;

                case eQuarter.qUR:
                    qs = "upper right";
                    im = eImage.iUR;
                    break;

                case eQuarter.qDL:
                    qs = "down left";
                    im = eImage.iDL;
                    break;

                case eQuarter.qDR:
                    qs = "down right";
                    im = eImage.iDR;
                    break;

                default:
                    qs = "unknown";
                    im = eImage.iEmpty;
                    break;
            }

            name.Nodes.Add("Quarter " + qs);
            name.Nodes[4].ImageIndex = (int)im;
            name.Nodes[4].SelectedImageIndex = (int)im;
            return name;
        }

        //---
        public override string ToString()
        {
            string l = "";
            if (fill) l = ((int)eCommand.fArc).ToString();
            else l = ((int)eCommand.dArc).ToString();
            l += ", " + c.X.ToString() + ", ";
            l += c.Y.ToString() + ", ";
            l += r.ToString() + ", ";
            l += ((byte)q).ToString() + ", ";
            return l;
        }

        //---
        public override int itemLenght()
        {
            return 6;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            if (fill)
            {
                int f = 1 - r;
                int ddF_x = 1;
                int ddF_y = -r - r;
                int x = 0;
                int x0 = c.X;
                int y0 = c.Y;

                while (x < r)
                {
                    if (f >= 0)
                    {
                        r--;
                        ddF_y += 2;
                        f += ddF_y;
                    }
                    x++;
                    ddF_x += 2;
                    f += ddF_x;

                    switch (q)
                    {
                        case eQuarter.qUL:
                            drawLine(ref canva, x0 - r, y0 + x, x0, y0 + x, cl, tag);
                            drawLine(ref canva, x0 - x, y0 + r, x0, y0 + r, cl, tag);
                            break;

                        case eQuarter.qUR:
                            drawLine(ref canva, x0, y0 - r, x0 + x, y0 - r, cl, tag);
                            drawLine(ref canva, x0, y0 - x, x0 + r, y0 - x, cl, tag);
                            break;

                        case eQuarter.qDL:
                            drawLine(ref canva, x0 - r, y0 - x, x0, y0 - x, cl, tag);
                            drawLine(ref canva, x0 - x, y0 - r, x0, y0 - r, cl, tag);
                            break;

                        case eQuarter.qDR:
                            drawLine(ref canva, x0, y0 + r, x0 + x, y0 + r, cl, tag);
                            drawLine(ref canva, x0, y0 + x, x0 + r, y0 + x, cl, tag);
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                drawCircleHelper(ref canva, c.X, c.Y, r, q, cl, tag);
            }

            if (cl == eColor.Selected)
            {
                eColor cl2 = eColor.SelectedPoint;

                drawPixel(ref canva, c.X, c.Y, cl2, tag);

                switch (q)
                {
                    case eQuarter.qUL:
                        drawPixel(ref canva, c.X - r, c.Y, cl2, tag);
                        drawPixel(ref canva, c.X, c.Y - r, cl2, tag);
                        break;

                    case eQuarter.qUR:
                        drawPixel(ref canva, c.X + r, c.Y, cl2, tag);
                        drawPixel(ref canva, c.X, c.Y - r, cl2, tag);
                        break;

                    case eQuarter.qDL:
                        drawPixel(ref canva, c.X - r, c.Y, cl2, tag);
                        drawPixel(ref canva, c.X, c.Y + r, cl2, tag);
                        break;

                    case eQuarter.qDR:
                        drawPixel(ref canva, c.X + r, c.Y, cl2, tag);
                        drawPixel(ref canva, c.X, c.Y + r, cl2, tag);
                        break;

                    default:
                        break;
                }
            }
            return cl;
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();
            pb = new PictureBox();
            Label name = new Label();
            Bitmap tr = arcBitmap();
            loadInit(ref pb, tr, ref name, "Arc" + getName());

            tab.Add(pb);
            tab.Add(name);

            Label x = new Label();
            x.Text = "X=";
            x.Top = name.Top + name.Height + il;
            x.Left = il;
            x.Width = labelWidth;
            tab.Add(x);

            Label y = new Label();
            y.Text = "Y=";
            y.Top = x.Top + x.Height + il;
            y.Left = x.Left;
            y.Width = x.Width;
            tab.Add(y);

            Label ra = new Label();
            ra.Text = "R=";
            ra.Top = y.Top + y.Height + il;
            ra.Left = x.Left;
            ra.Width = x.Width;
            tab.Add(ra);

            nudx = new NumericUpDown();
            nudx.Top = x.Top - 3;
            nudx.Left = x.Width + il;
            nudx.Width = nudWidth;
            nudx.Minimum = MIN;
            nudx.Maximum = MAX;
            nudx.Value = c.X;
            nudx.ValueChanged += new EventHandler(valueXchange);
            tab.Add(nudx);

            nudy = new NumericUpDown();
            nudy.Top = y.Top - 3;
            nudy.Left = y.Width + il;
            nudy.Width = nudx.Width;
            nudy.Minimum = nudx.Minimum;
            nudy.Maximum = nudx.Maximum;
            nudy.Value = c.Y;
            nudy.ValueChanged += new EventHandler(valueYchange);
            tab.Add(nudy);

            nudr = new NumericUpDown();
            nudr.Top = ra.Top - 3;
            nudr.Left = ra.Width + il;
            nudr.Width = nudx.Width;
            nudr.Minimum = nudx.Minimum;
            nudr.Maximum = nudx.Maximum;
            nudr.Value = r;
            nudr.ValueChanged += new EventHandler(valueRchange);
            tab.Add(nudr);

            f = new CheckBox();
            f.Top = nudx.Top;
            f.Left = nudx.Left + nudx.Width + 5 * il;
            f.Text = "Fill";
            f.Checked = fill;
            f.CheckedChanged += new EventHandler(fillChange);
            tab.Add(f);

            cb = new ComboBox();
            for (int i = 1; i < 5; i++)
            {
                string t = ((eQuarter)i).ToString();
                t = t.Substring(1);
                cb.Items.Add(t);
            }
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Top = f.Top + f.Height + il;
            cb.Left = f.Left;
            cb.Width = 40;
            cb.SelectedIndex = (int)q - 1;
            cb.SelectedIndexChanged += new EventHandler(quarterChange);
            tab.Add(cb);

            PictureBox pbar = new PictureBox();
            pbar.Height = picHeight * 2;
            pbar.Width = pbar.Height;
            tr = Properties.Resources.AllArc;
            tr.MakeTransparent(Color.White);
            pbar.Image = tr;
            pbar.SizeMode = PictureBoxSizeMode.Zoom;
            pbar.Top = cb.Top + cb.Height + il;
            pbar.Left = cb.Left;
            tab.Add(pbar);

            Button up = new Button();
            up.Text = "";
            up.Height = buttonSize;
            up.Width = up.Height;

            tr = Properties.Resources.blackArrowUp;
            tr.MakeTransparent(Color.White);
            up.BackgroundImage = tr;
            up.BackgroundImageLayout = ImageLayout.Zoom;
            up.Top = nudr.Top + nudr.Height + 4 * il;
            up.Left = nudr.Left + nudr.Width - up.Width;
            up.Tag = eDirection.iUp;
            up.Click += new EventHandler(valueYchange);
            tab.Add(up);

            Button right = new Button();
            right.Text = up.Text;
            right.Height = up.Height;
            right.Width = up.Width;

            tr = Properties.Resources.blackArrowRight;
            tr.MakeTransparent(Color.White);
            right.BackgroundImage = tr;
            right.BackgroundImageLayout = ImageLayout.Zoom;
            right.Top = up.Top + up.Height;
            right.Left = up.Left + up.Width;
            right.Tag = eDirection.iRight;
            right.Click += new EventHandler(valueXchange);
            tab.Add(right);

            Button down = new Button();
            down.Text = up.Text;
            down.Height = up.Height;
            down.Width = up.Width;

            tr = Properties.Resources.blackArrowDown;
            tr.MakeTransparent(Color.White);
            down.BackgroundImage = tr;
            down.BackgroundImageLayout = ImageLayout.Zoom;
            down.Top = right.Top + up.Height;
            down.Left = up.Left;
            down.Tag = eDirection.iDown;
            down.Click += new EventHandler(valueYchange);
            tab.Add(down);

            Button left = new Button();
            left.Text = up.Text;
            left.Height = up.Height;
            left.Width = up.Width;

            tr = Properties.Resources.blackArrowLeft;
            tr.MakeTransparent(Color.White);
            left.BackgroundImage = tr;
            left.BackgroundImageLayout = ImageLayout.Zoom;
            left.Top = right.Top;
            left.Left = up.Left - up.Width;
            left.Tag = eDirection.iLeft;
            left.Click += new EventHandler(valueXchange);
            tab.Add(left);

            pbc = new PictureBox();
            pbc.Height = up.Height;
            pbc.Width = up.Width;
            tr = arcBitmap();
            tr.MakeTransparent(Color.White);
            pbc.Image = tr;
            pbc.SizeMode = PictureBoxSizeMode.Zoom;
            pbc.Top = right.Top;
            pbc.Left = up.Left;
            tab.Add(pbc);

            Button shrink = new Button();
            shrink.Text = up.Text;
            shrink.Height = up.Height;
            shrink.Width = up.Width;

            tr = Properties.Resources.Shrink;
            tr.MakeTransparent(Color.White);
            shrink.BackgroundImage = tr;
            shrink.BackgroundImageLayout = ImageLayout.Zoom;
            shrink.Top = down.Top + down.Height + 2 * il;
            shrink.Left = left.Left;
            shrink.Tag = eDirection.iLeft;
            shrink.Click += new EventHandler(valueRchange);
            tab.Add(shrink);

            Button increase = new Button();
            increase.Text = up.Text;
            increase.Height = up.Height;
            increase.Width = up.Width;

            tr = Properties.Resources.Increase;
            tr.MakeTransparent(Color.White);
            increase.BackgroundImage = tr;
            increase.BackgroundImageLayout = ImageLayout.Zoom;
            increase.Top = shrink.Top;
            increase.Left = right.Left;
            increase.Tag = eDirection.iRight;
            increase.Click += new EventHandler(valueRchange);
            tab.Add(increase);

            PictureBox pbr = new PictureBox();

            pbr.Height = up.Height;
            pbr.Width = up.Width;
            pbr.Top = shrink.Top;
            pbr.Left = shrink.Left + shrink.Width;

            tr = Properties.Resources.ArcRadius;
            tr.MakeTransparent(Color.White);

            pbr.Image = tr;
            pbr.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Add(pbr);

            return tab;
        }

        //---
        protected Bitmap arcBitmap()
        {
            Bitmap tr = null;
            if (fill)
            {
                switch (q)
                {
                    case eQuarter.qUL:
                        tr = Properties.Resources.ArcFill;
                        break;

                    case eQuarter.qUR:
                        tr = Properties.Resources.ArcFillUr;
                        break;

                    case eQuarter.qDL:
                        tr = Properties.Resources.ArcFillDl;
                        break;

                    case eQuarter.qDR:
                        tr = Properties.Resources.ArcFillDr;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (q)
                {
                    case eQuarter.qUL:
                        tr = Properties.Resources.Arc;
                        break;

                    case eQuarter.qUR:
                        tr = Properties.Resources.ArcUr;
                        break;

                    case eQuarter.qDL:
                        tr = Properties.Resources.ArcDl;
                        break;

                    case eQuarter.qDR:
                        tr = Properties.Resources.ArcDr;
                        break;

                    default:
                        break;
                }
            }
            return tr;
        }

        //---
        protected void valueXchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.X = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (c.X == MIN)
                    {
                        MessageBox.Show("X=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X--;
                }
                else
                {
                    if (c.X >= MAX)
                    {
                        MessageBox.Show("X=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        return;
                    }
                    else c.X++;
                }
            }
            nudx.Value = c.X;
        }

        //---
        protected void valueYchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                c.Y = (int)(sender as NumericUpDown).Value;
                return;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iUp)
                {
                    if (c.Y == MIN)
                    {
                        MessageBox.Show("Y=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y--;
                }
                else
                {
                    if (c.Y >= MAX)
                    {
                        MessageBox.Show("Y=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else c.Y++;
                }
            }
            nudy.Value = c.Y;
        }

        //---
        protected void valueRchange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(NumericUpDown))
            {
                r = (byte)(sender as NumericUpDown).Value;
            }
            else if (sender.GetType() == typeof(Button))
            {
                if ((eDirection)((sender as Button).Tag) == eDirection.iLeft)
                {
                    if (r == MIN)
                    {
                        MessageBox.Show("R=0 is the minimum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r--;
                }
                else
                {
                    if (r >= MAX)
                    {
                        MessageBox.Show("R=127 is the maximum value.",
                                         "Warning.",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                    }
                    else r++;
                }
            }
            nudr.Value = r;
        }

        //---
        protected void fillChange(object sender, EventArgs e)
        {
            bool useCheckBox = true;
            if (sender.GetType() == typeof(CheckBox))
            {
                fill = (sender as CheckBox).Checked;
                useCheckBox = false;
            }
            if (useCheckBox) f.Checked = fill;

            Bitmap tr = arcBitmap();

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbc.Image = tr;
        }

        //---
        protected void quarterChange(object sender, EventArgs e)
        {
            bool useComboBox = true;
            if (sender.GetType() == typeof(ComboBox))
            {
                q = (eQuarter)((sender as ComboBox).SelectedIndex + 1);
                useComboBox = false;
            }

            if (useComboBox) cb.SelectedIndex = (int)q - 1;
            Bitmap tr = arcBitmap();

            tr.MakeTransparent(Color.White);
            pb.Image = tr;
            pbc.Image = tr;
        }

        //---
        public override List<errClass> test()
        {
            List<errClass> ts = new List<errClass>();
            if (r == 0) ts.Add(new errClass(eTest.errArcRedToPoint,
                new int[0] { },
                "Arc" + this.getName() + ": Radius =0."));

            if (c.X - r < 0 && (q == eQuarter.qUL || q == eQuarter.qDL))
                ts.Add(new errClass(eTest.errOriXUnder0,
                    new int[0] { },
                    "Arc" + this.getName() + ": Boundary X<0."));

            if (c.Y - r < 0 && (q == eQuarter.qUL || q == eQuarter.qUR))
                ts.Add(new errClass(eTest.errOriYUnder0,
                    new int[0] { },
                    "Arc" + this.getName() + ": Boundary Y<0."));

            return ts;
        }

        //---
        public override Rectangle getRect()
        {
            int x = 0;
            int y = 0;
            switch (q)
            {
                case eQuarter.qUL:
                    x = c.X - r;
                    y = c.Y - r;
                    break;

                case eQuarter.qUR:
                    x = c.X;
                    y = c.Y - r;
                    break;

                case eQuarter.qDL:
                    x = c.X - r;
                    y = c.Y;
                    break;

                case eQuarter.qDR:
                    x = c.X;
                    y = c.Y;
                    break;

                default:
                    x = -1000;
                    y = -1000;
                    break;
            }

            return new Rectangle(x, y, r, r);
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(arc obj1, arc obj2)
        {
            if (obj1.c == obj2.c && obj1.r == obj2.r && obj1.q == obj2.q &&
                obj1.fill == obj2.fill) return true;
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
        public static bool operator !=(arc obj1, arc obj2)
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

            return obj.GetType() == GetType() && Equals((arc)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (c.ToString() + r.ToString() + q.ToString() + fill.ToString()).GetHashCode();
            }
        }
    }

    //---G
    public class color : Item
    {
        #region ColorData

        public static UInt16[,] skinColorTab = {
    {/*System skin Light */				0xffff,		//BackGround
										0xd77c,		//Workspace
										0xfe7e,		//Button
										0xc5ff,		//Bar
										0x018b,		//Border
										0x3267,		//Frame
										0x7103,		//Line
										0x0000,		//Text
										0xede0,		//SpecialText
										0xc220,		//Number
										0xd8ab,		//Highlight
										0xad75,		//Inactive
										0xfcc3,		//Warning
										0xf862,		//Alarm
										0x27e2,		//High
										0x2000,		//Low
										0xda21,		//Hot
										0xaf87,		//Mid
										0x0637,		//Could
										0xf888  },	//Indicator

	{/*System skin Dark */				0x0000,		//BackGround
										0x1103,		//Workspace
										0x7010,		//Button
										0x11c0,		//Bar
										0x03f3,		//Border
										0xe8e4,		//Frame
										0xa9e3,		//Line
										0xFFFF,		//Text
										0xFFF0,		//SpecialText
										0x0385,		//Number
										0x765e,		//Highlight
										0x4208,		//Inactive
										0xfbe0,		//Warning
										0xF800,		//Alarm
										0x47e3,		//High
										0xc6ff,		//Low
										0xfd83,		//Hot
										0xb486,		//Mid
										0x000C,		//Could
										0xb800  },	//Indicator

	{/*System skin Gray */				0x0000,		//BackGround
										0x18c3,		//Workspace
										0x4208,		//Button
										0xa530,		//Bar
										0xc5d7,		//Border
										0xbed7,		//Frame
										0x8c51,		//Line
										0xFFFF,		//Text
										0xde92,		//SpecialText
										0xb6f6,		//Number
										0xdeff,		//Highlight
										0x6b4c,		//Inactive
										0xcdf1,		//Warning
										0xe595,		//Alarm
										0xb6ff,		//High
										0xf597,		//Low
										0xfe79,		//Hot
										0x8c0a,		//Mid
										0x08c6,		//Could
										0xfcf5  },	//Indicator

	{/*System skin HiContrast */		0xfe9f,		//BackGround
										0xeb99,		//Workspace
										0xa7ff,		//Button
										0x9fb3,		//Bar
										0x0140,		//Border
										0x6001,		//Frame
										0x008b,		//Line
										0x0000,		//Text
										0xffe0,		//SpecialText
										0x204b,		//Number
										0x1904,		//Highlight
										0x3987,		//Inactive
										0xfae0,		//Warning
										0x98e2,		//Alarm
										0x001f,		//High
										0xf800,		//Low
										0xf808,		//Hot
										0x6c2d,		//Mid
										0x0013,		//Could
										0x3fe0  },	//Indicator

	{/*System skin Rainbow */			0x0005,		//BackGround
										0x0089,		//Workspace
										0x4000,		//Button
										0x4203,		//Bar
										0x061C,		//Border
										0x060c,		//Frame
										0xf126,		//Line
										0xffff,		//Text
										0xffe6,		//SpecialText
										0x07ed,		//Number
										0x379F,		//Highlight
										0x630d,		//Inactive
										0xc7e3,		//Warning
										0xf80d,		//Alarm
										0xa063,		//High
										0xc560,		//Low
										0xc862,		//Hot
										0x37e4,		//Mid
										0xc81f,		//Could
										0xf800  },	//Indicator

	{/*System skin Lea */				0x0060,		//BackGround
										0x1120,		//Workspace
										0x2220,		//Button
										0x6220,		//Bar
										0x9dc7,		//Border
										0x060a,		//Frame
										0x95c3,		//Line
										0xFFFF,		//Text
										0xFFF0,		//SpecialText
										0x8ea0,		//Number
										0x5fed,		//Highlight
										0x5a49,		//Inactive
										0xfd60,		//Warning
										0xd06a,		//Alarm
										0x07E0,		//High
										0xddfa,		//Low
										0x87f0,		//Hot
										0x7d25,		//Mid
										0x0928,		//Could
										0xfea0  },	//Indicator

	{/*System skin Fire */				0x1800,		//BackGround
										0x4000,		//Workspace
										0x70a5,		//Button
										0x6040,		//Bar
										0xd9e7,		//Border
										0xea24,		//Frame
										0xd824,		//Line
										0xffbc,		//Text
										0xfea6,		//SpecialText
										0xdc60,		//Number
										0xf9e4,		//Highlight
										0x4a87,		//Inactive
										0xfb80,		//Warning
										0x1c85,		//Alarm
										0xf991,		//High
										0xae5e,		//Low
										0xfc11,		//Hot
										0x9246,		//Mid
										0x5000,		//Could
										0xb6e0  },	//Indicator

{/*System skin Lavender */				0x1802,		//BackGround
										0x2804,		//Workspace
										0x6043,		//Button
										0x4885,		//Bar
										0xd9ef,		//Border
										0xeb52,		//Frame
										0xd94a,		//Line
										0xffff,		//Text
										0xdd24,		//SpecialText
										0x29ef,		//Number
										0xf818,		//Highlight
										0x3965,		//Inactive
										0x7ce0,		//Warning
										0xc184,		//Alarm
										0xa8c3,		//High
										0x4c2d,		//Low
										0x47a9,		//Hot
										0xeda5,		//Mid
										0x0292,		//Could
										0xd6f2  },	//Indicator

{/*System skin GoldenBrown */			0x2082,		//BackGround
										0x3880,		//Workspace
										0x5084,		//Button
										0x4081,		//Bar
										0x82a0,		//Border
										0x8360,		//Frame
										0xaa80,		//Line
										0xffff,		//Text
										0xfe20,		//SpecialText
										0xfd67,		//Number
										0xffe7,		//Highlight
										0x8b88,		//Inactive
										0x8b80,		//Warning
										0xfc20,		//Alarm
										0x8da0,		//High
										0xfac6,		//Low
										0x47a9,		//Hot
										0xbda8,		//Mid
										0x028a,		//Could
										0xaf09  },	//Indicator

	{/*System skin Sea */				0x0008,		//BackGround
										0x1043,		//Workspace
										0x0124,		//Button
										0x1183,		//Bar
										0x2c6c,		//Border
										0x061C,		//Frame
										0x001F,		//Line
										0xFFFF,		//Text
										0xb60b,		//SpecialText
										0x6b6a,		//Number
										0x06bf,		//Highlight
										0x0010,		//Inactive
										0xFDe3,		//Warning
										0xF800,		//Alarm
										0xa816,		//High
										0x08aa,		//Low
										0x0654,		//Hot
										0x73f4,		//Mid
										0x000C,		//Could
										0xe680  },	//Indicator
};

        //---
        public static UInt16[] indColorTab = {
                                        0,		//BackGround
										0,		//Workspace
										0,		//Button
										0,		//Bar
										0,		//Border
										0,		//Frame
										0,		//Line
										0,		//Text
										0,		//SpecialText
										0,		//Number
										0,		//Highlight
										0,		//Inactive
										0,		//Warning
										0,		//Alarm
										0,		//High
										0,		//Low
										0,		//Hot
										0,		//Mid
										0,		//Could
										0,		//Indicator
										0x0000,		//Black
										0xFFFF,		//White
										0xBDF7,		//LightGray
										0x738E,		//Gray
										0x2104,		//DarkGray
										0x6EDD,		//LightBlue
										0x001F,		//Blue
										0x0004,		//DarkBlue
										0xFBEF,		//LightRed
										0xF800,		//Red
										0x9000,		//DarkRed
										0x7FEF,		//LightGreen
										0x07E0,		//Green
										0x0120,		//DarkGreen
										0xFFF3,		//LightYellow
										0xFFE2,		//Yellow
										0xFB61,		//Orange
										0x3040,		//Brown
										0x680D,		//Purple
										0xFD97,      //Pink
                                        0x0,        //def color
                                        0x6EDD      //selected color
};

        #endregion ColorData

        //---
        protected eColor clr;

        protected static eColor def = eColor.Red;
        protected static eSkin skn = eSkin.Light;

        protected ComboBox cbclr;
        protected Panel p;
        protected ComboBox cbskn;
        protected ComboBox cbdef;
        protected Panel p2;

        //---
        public color()
        {
            clr = eColor.White;
            cbclr = new ComboBox();
            p = new Panel();
            cbskn = new ComboBox();
            cbdef = new ComboBox();
            p2 = new Panel();
        }

        //---
        static color()
        {
            for (int i = 0; i < 20; i++)
                indColorTab[i] = skinColorTab[0, i];
        }

        //---
        public override TreeNode property()
        {
            TreeNode name = new TreeNode(this.GetType().Name + getName());
            name.Tag = true;
            name.ImageIndex = (int)eImage.iColor;
            name.SelectedImageIndex = (int)eImage.iColor;

            name.Nodes.Add(clr.ToString());
            name.Nodes[0].ImageIndex = (int)eImage.iEmpty;
            name.Nodes[0].SelectedImageIndex = (int)eImage.iEmpty;

            Color fCl = contrast(setColor(clr));

            name.Nodes[0].BackColor = setColor(clr);
            name.Nodes[0].ForeColor = fCl;
            return name;
        }

        //---
        public static eColor Default
        {
            set
            {
                def = value;
            }
            get
            {
                return def;
            }
        }

        //---
        public static eSkin Skin
        {
            set
            {
                skn = value;
            }
            get
            {
                return skn;
            }
        }

        //---
        public override string ToString()
        {
            return ((int)eCommand.cColor).ToString() + ", " +
                ((byte)clr).ToString() + ", ";
        }

        //---
        public override int itemLenght()
        {
            return 2;
        }

        //---
        public override eColor draw(ref Basis canva, eColor cl, int tag)
        {
            return clr;
        }

        //---
        public override List<Control> loadItem()
        {
            List<Control> tab = new List<Control>();

            PictureBox pb = new PictureBox();
            Label name = new Label();
            Bitmap tr = Properties.Resources.Color;
            loadInit(ref pb, tr, ref name, "Color" + getName());

            tab.Add(pb);
            tab.Add(name);

            Label a = new Label();
            a.Text = "Set the active color.";
            a.Width = 200;
            a.Left = il * 2;
            a.Top = name.Top + name.Height + il;
            tab.Add(a);

            cbclr = new ComboBox();
            cbclr.Items.AddRange(Enum.GetNames(typeof(eColor)));
            cbclr.Items.RemoveAt(cbclr.Items.Count - 1);
            cbclr.Items.RemoveAt(cbclr.Items.Count - 1);
            cbclr.Items.RemoveAt(cbclr.Items.Count - 1);
            cbclr.Items.RemoveAt(cbclr.Items.Count - 1);

            cbclr.Top = a.Top + a.Height;
            cbclr.Width = 120;
            cbclr.Left = a.Left;
            cbclr.SelectedIndex = (int)clr;
            cbclr.SelectedIndexChanged += new EventHandler(colorSelectedIndexChange);
            tab.Add(cbclr);

            p = new Panel();
            p.Height = cbclr.Height;
            p.Width = cbclr.Width;
            p.Left = a.Left;
            p.Top = cbclr.Top + cbclr.Height + il;
            p.BackColor = setColor(clr);
            tab.Add(p);

            Label b = new Label();
            b.Text = "Selected skin.";
            b.Left = a.Left;
            b.Top = p.Top + p.Height + 6 * il;
            b.Width = a.Width;
            tab.Add(b);

            cbskn = new ComboBox();
            cbskn.Name = "cbskn";
            cbskn.Items.AddRange(Enum.GetNames(typeof(eSkin)));

            cbskn.Top = b.Top + b.Height;
            cbskn.Width = cbclr.Width; ;
            cbskn.Left = a.Left;
            cbskn.SelectedIndex = (int)skn;
            cbskn.SelectedIndexChanged += new EventHandler(skinSelectedIndexChange);
            tab.Add(cbskn);

            Label c = new Label();
            c.Text = "Selected default color.";
            c.Left = a.Left;
            c.Top = cbskn.Top + cbskn.Height + 6 * il;
            c.Width = a.Width;
            tab.Add(c);

            cbdef = new ComboBox();
            cbdef.Name = "cbdef";
            cbdef.Items.AddRange(Enum.GetNames(typeof(eColor)));
            cbdef.Items.RemoveAt(cbdef.Items.Count - 1);
            cbdef.Items.RemoveAt(cbdef.Items.Count - 1);
            cbdef.Items.RemoveAt(cbdef.Items.Count - 1);
            cbdef.Items.RemoveAt(cbdef.Items.Count - 1);
            cbdef.Items.RemoveAt(cbdef.Items.Count - 1);

            cbdef.Top = c.Top + c.Height;
            cbdef.Width = cbclr.Width;
            cbdef.Left = a.Left;
            cbdef.SelectedIndex = (int)def;
            cbdef.SelectedIndexChanged += new EventHandler(defSelectedIndexChange);
            tab.Add(cbdef);

            p2 = new Panel();
            p2.Height = cbdef.Height;
            p2.Width = cbdef.Width;
            p2.Left = a.Left;
            p2.Top = cbdef.Top + cbdef.Height + il;
            p2.BackColor = setColor(eColor.Def);
            tab.Add(p2);

            return tab;
        }

        //---
        protected void colorSelectedIndexChange(object sender, EventArgs e)
        {
            bool useComboBox = true;
            if (sender.GetType() == typeof(ComboBox))
            {
                clr = (eColor)((sender as ComboBox).SelectedIndex);
                useComboBox = false;
            }
            if (useComboBox) cbclr.SelectedIndex = (int)skn;
            p.BackColor = setColor(clr);
            p2.BackColor = setColor(eColor.Def);
        }

        //---
        protected void skinSelectedIndexChange(object sender, EventArgs e)
        {
            bool useComboBox = true;
            if (sender.GetType() == typeof(ComboBox))
            {
                skn = (eSkin)((sender as ComboBox).SelectedIndex);
                useComboBox = false;
            }
            if (useComboBox) cbskn.SelectedIndex = (int)skn;
            for (int i = 0; i < 20; i++)
                indColorTab[i] = skinColorTab[(int)skn, i];
            p.BackColor = setColor(clr);
            p2.BackColor = setColor(eColor.Def);
        }

        //---
        protected void defSelectedIndexChange(object sender, EventArgs e)
        {
            bool useComboBox = true;
            if (sender.GetType() == typeof(ComboBox))
            {
                def = (eColor)((sender as ComboBox).SelectedIndex);
                useComboBox = false;
            }
            if (useComboBox) cbdef.SelectedIndex = (int)def;
            p2.BackColor = setColor(eColor.Def);
            if (clr == eColor.Def) p.BackColor = setColor(clr);
        }

        //---
        public static Color setColor(eColor clrs)
        {
            UInt16 cl;
            if (clrs != eColor.Def)
            {
                cl = indColorTab[(int)clrs];
            }
            else
            {
                cl = indColorTab[(int)def];
            }
            int r = (cl >> 11) << 3;
            int g = ((cl >> 5) & 63) << 2;
            int b = (cl & 31) << 3;
            return Color.FromArgb(r, g, b);
        }

        //---
        public static Color contrast(Color cl)
        {
            Color col = Color.Black;
            int r = cl.R;
            int g = cl.G;
            int b = cl.B;
            if ((r + g + b) / 3.0 < 60) col = Color.White;

            return col;
        }

        //---
        /*
        protected void setDefColor()
        {
            UInt16 cl = indColorTab[(int)def];
            int r = (cl >> 11) << 3;
            int g = ((cl >> 5) & 63) << 2;
            int b = (cl & 31) << 3;
            p2.BackColor = Color.FromArgb(r, g, b);
        }
        */

        //---
        public override Rectangle getRect()
        {
            return new Rectangle(-1000, -1000, -1000, -1000);
        }

        //---
        public override List<errClass> test()
        {
            return new List<errClass>();
        }

        //---
        public override bool parse(List<byte> sp, ref int i)
        {
            return base.parse(sp, ref i);
        }

        //---
        public static bool operator ==(color obj1, color obj2)
        {
            if (obj1.clr == obj2.clr) return true;
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
        public static bool operator !=(color obj1, color obj2)
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

            return obj.GetType() == GetType() && Equals((color)obj);
        }

        //---
        public override int GetHashCode()
        {
            unchecked
            {
                return (clr.ToString()).GetHashCode();
            }
        }
    }

    #endregion Items def

    //---G
}
