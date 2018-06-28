using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Assembly = System.Reflection.Assembly;

namespace Designer2
{
    public class Setting
    {
        protected Color bgCl;
        protected string fileSet;
        protected string[] recentFiles = new string[10];
        protected bool al = true;
        protected Size w_size = new Size();
        protected Point w_point = new Point();
        protected bool w_s = false;

        protected const char sep = '*';
        protected string[] tags = { "[BGCL]", "[LF]", "[AL]", "[WINDOW]", };

        //---
        public Setting(string fs)
        {
            fileSet = fs;
            clearFile();
        }

        //---
        public void save()
        {
            DateTime localDate = DateTime.Now;

            int indx = 0;
            string settings = "Automatically created file by Designer2.\r\n" +
                "Created at " + localDate.ToString() +
                "\r\nPlease, do not change anything manually!\r\n";
            //save panel1 background color
            settings += tags[indx] +
                bgCl.ToArgb().ToString() +
                tags[indx] + "\r\n";
            indx++;
            //save last used file (max 10)
            settings += tags[indx] + "\r\n";
            for (int i = 0; i < 10; i++)
            {
                recentFiles[i] = recentFiles[i].Replace(sep, ' ');
                recentFiles[i] = recentFiles[i].Trim();

                if (recentFiles[i] != "")
                    settings += recentFiles[i] + sep + "\r\n";
                else break;
            }
            settings += tags[indx] + "\r\n";
            indx++;
            //save auto load last project
            settings += tags[indx] + al.ToString();

            settings += tags[indx] + "\r\n";
            indx++;
            //save remember last window size
            settings += tags[indx] + "\r\n" + w_s.ToString() + sep + "\r\n" +
                w_size.Width.ToString() + sep + "\r\n" +
                w_size.Height.ToString() + sep + "\r\n" +
                w_point.X.ToString() + sep + "\r\n" +
                w_point.Y.ToString() + sep + "\r\n" +
                tags[indx] + "\r\n";
            indx++;
            //save skin

            File.WriteAllText(fileSet, settings, Encoding.Default);
        }

        //---
        public bool read()
        {
            string settings = "";
            if (File.Exists(fileSet))
                settings = File.ReadAllText(fileSet, Encoding.Default);

            int indx = 0;
            //read background color
            bool f = true;
            int s = settings.IndexOf(tags[indx].ToString());
            int e = settings.LastIndexOf(tags[indx].ToString());
            if (s < 0 || e < 0)
            {
                f = false;
                bgColor = Color.Black;
            }
            else
            {
                s += tags[indx].ToString().Count();
                string t = settings.Substring(s, e - s);
                int result = 0;
                f = int.TryParse(t, out result);
                if (f) bgColor = Color.FromArgb(result);
                else bgColor = Color.Black;
            }
            //read recently used file (max10)
            indx++;
            s = settings.IndexOf(tags[indx].ToString());
            e = settings.LastIndexOf(tags[indx].ToString());
            if (s < 0 || e < 0)
            {
                f = false;
            }
            else
            {
                s += tags[indx].ToString().Count();
                string t = settings.Substring(s, e - s);
                char[] spl = { sep };
                string[] temp = t.Split(spl, 10, StringSplitOptions.RemoveEmptyEntries);

                for (int i = temp.Count() - 1; i >= 0; i--)

                {
                    temp[i] = temp[i].Replace(sep, ' ');
                    temp[i] = temp[i].Trim();
                    addFile(temp[i]);
                }
            }
            //read auto load
            indx++;
            s = settings.IndexOf(tags[indx].ToString());
            e = settings.LastIndexOf(tags[indx].ToString());
            if (s < 0 || e < 0)
            {
                f = false;
            }
            else
            {
                s += tags[indx].ToString().Count();
                string t = settings.Substring(s, e - s);
                if (t == "True") autoLoad = true;
                else autoLoad = false;
            }
            //read windowSize
            indx++;
            s = settings.IndexOf(tags[indx].ToString());
            e = settings.LastIndexOf(tags[indx].ToString());
            if (s < 0 || e < 0)
            {
                f = false;
            }
            else
            {
                s += tags[indx].ToString().Count();
                string t = settings.Substring(s, e - s);
                char[] spl = { sep };
                string[] temp = t.Split(spl, 5, StringSplitOptions.RemoveEmptyEntries);

                for (int i = temp.Count() - 1; i >= 0; i--)

                {
                    temp[i] = temp[i].Replace(sep, ' ');
                    temp[i] = temp[i].Trim();
                }
                readWindowSize(temp);
            }

            return f;
        }

        //---
        public Color bgColor
        {
            set
            {
                bgCl = value;
            }
            get
            {
                return bgCl;
            }
        }

        //---
        public bool autoLoad
        {
            set
            {
                al = value;
            }
            get
            {
                return al;
            }
        }

        //---
        public Size windowSize
        {
            set
            {
                int w = value.Width;
                int h = value.Height;
                w_size = new Size(w, h);
            }
            get
            {
                return w_size;
            }
        }

        //---
        public Point windowLocation
        {
            set
            {
                int x = value.X;
                int y = value.Y;
                w_point = new Point(x, y);
            }
            get
            {
                return w_point;
            }
        }

        //---
        public bool windowSave
        {
            set
            {
                w_s = value;
            }
            get
            {
                return w_s;
            }
        }

        //---
        private void readWindowSize(string[] t)
        {
            if (t.Count() != 5)
            {
                return;
            }
            if (t[0] == "True") windowSave = true;
            else windowSave = false;
            int w = 0;
            int.TryParse(t[1], out w);
            int h = 0;
            int.TryParse(t[2], out h);
            int x = 0;
            int.TryParse(t[3], out x);
            int y = 0;
            int.TryParse(t[4], out y);

            windowSize = new Size(w, h);
            windowLocation = new Point(x, y);
        }

        //---
        public void addFile(string f)
        {
            if (f == "") return;
            delFile(f);
            for (int i = 9; i > 0; i--)
            {
                recentFiles[i] = recentFiles[i - 1];
            }
            recentFiles[0] = f;
        }

        //---

        public void delFile(string f)
        {
            for (int i = 0; i < 10; i++)
            {
                if (recentFiles[i] == f) recentFiles[i] = "";
                if (recentFiles[i] == "" && i < 9)
                {
                    for (int ii = 1; i + ii < 10; ii++)

                        if (recentFiles[i + ii] != "")
                        {
                            recentFiles[i] = recentFiles[i + ii];
                            recentFiles[i + ii] = "";
                            break;
                        }
                }
            }
        }

        //---
        public void clearFile()
        {
            for (int i = 0; i < 10; i++)
                recentFiles[i] = "";
        }

        //---
        public string[] getRecentFiles()
        {
            return recentFiles;
        }

        //---
    }

    //---R

    public class ICOlist
    {
        protected List<ICO> data = new List<ICO>();
        protected bool chd;

        public delegate void data_Change(object sender, EventArgs e);

        public event data_Change eventData_Change;

        public delegate void icoSequence_Change(object sender, EventArgs e);

        public event icoSequence_Change eventIcoSequence_Change;

        public delegate void ICOitemSequence_change(object sender, EventArgs e);

        public event ICOitemSequence_change eventICOitemSequence_Change;

        //---
        public ICOlist()
        {
            data.Clear();
            change = false;
            eventIcoSequence_Change?.Invoke(this, new EventArgs());
        }

        //---
        public ICOlist(string s)
        {
            data.Clear();
            parse(s);
            change = false;
            eventIcoSequence_Change?.Invoke(this, new EventArgs());
        }

        //---
        public ICO this[int ind]
        {
            get
            {
                if (data.Count == 0) return null;
                return data[ind];
            }
        }

        //---
        protected void change_ICO_Item_Sequence(object sender, EventArgs e)
        {
            eventICOitemSequence_Change?.Invoke(this, new EventArgs());
            change = true;
        }

        //---
        public void clear()
        {
            data.Clear();
            change = false;
            eventIcoSequence_Change?.Invoke(this, new EventArgs());
        }

        //---
        public void add(ICO ic)
        {
            if (isGoodName(ic.Name) < 0)
            {
                data.Add(ic);
                data[data.Count - 1].eventData_Change += new ICO.data_Change(change_ICO_Item_Sequence);
                change = true;
                eventIcoSequence_Change?.Invoke(this, new EventArgs());
            }
        }

        //---
        public void insert(int ind, ICO ic)
        {
            if (testIndex(ind) == false) return;
            if (isGoodName(ic.Name) < 0)
            {
                data.Insert(ind, ic);
                change = true;
                eventIcoSequence_Change?.Invoke(this, new EventArgs());
                ic.eventData_Change += new ICO.data_Change(change_ICO_Item_Sequence);
            }
        }

        //---
        public void del(int ind)
        {
            if (testIndex(ind) == false) return;
            if (data[ind].isEmpty() || DialogResult.Yes ==
                MessageBox.Show("Pressing the 'YES' button will result in the\r\n" +
                     " loss of data. Continue anyway?",
                     "Warning.",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Warning,
                     MessageBoxDefaultButton.Button2))
            {
                data.RemoveAt(ind);
                change = true;
                eventIcoSequence_Change?.Invoke(this, new EventArgs());
                return;
            }
        }

        //---
        public void up(int ind)
        {
            if (ind == data.Count - 1) ind = -1;
            if (testIndex(ind) == false) return;
            ICO temp = data[ind];
            data.RemoveAt(ind);
            data.Insert(ind + 1, temp);
            change = true;
            eventIcoSequence_Change?.Invoke(this, new EventArgs());
        }

        //---
        public void down(int ind)
        {
            if (ind == 0) ind = -1;
            if (testIndex(ind) == false) return;
            ICO temp = data[ind];
            data.RemoveAt(ind);
            data.Insert(ind - 1, temp);
            change = true;
            eventIcoSequence_Change?.Invoke(this, new EventArgs());
        }

        //---
        protected bool testIndex(int i)
        {
            if (i < 0 || i >= data.Count())
            {
                MessageBox.Show("Class ICOlist, method testIndex index out of range.",
                     "Error.",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //---
        protected bool isAbsent(string n)
        {
            for (int i = 0; i < data.Count; i++)
                if (data[i].Name == n) return false;
            return true;
        }

        //---
        public int isGoodName(string n)
        {
            if (n.Length == 0) return 0;

            if (n[0] != '_' && !char.IsLetter(n[0])) return 1;

            for (int i = 0; i < n.Length; i++)
            {
                if (char.IsLetterOrDigit(n[i]) || n[i] == '_') continue;
                return 2;
            }
            if (isAbsent(n) == false) return 3;
            return -1;
        }

        //---
        public bool empty()
        {
            if (data.Count == 0) return true;
            return false;
        }

        //---
        public override string ToString()
        {
            DateTime localDate = DateTime.Now;
            string s = "//[Designer2] This part of the code is generated automatically by Designer2.\r\n";
            s += "//  Created at " + localDate.ToString() + "\r\n";
            s += "//  Please do not change anything manually.\r\n//\r\n";
            s += "enum ICOS {";
            for (int i = 0; i < data.Count; i++)
            {
                s += data[i].Name;
                if (i == 0) s += " = 0";
                if (i < data.Count - 1) s += " , ";
            }
            s += "};\r\n//\r\n";
            s += "static int tabIco[] = { " + "\r\n";
            s += "// Prologue" + "\r\n";
            s += data.Count.ToString() + " ,  //";
            if (data.Count == 0) s += "### WARNING! EMPTY COLLECTION ### ";
            s += " Number of icons in the collection" + "\r\n";
            if (data.Count > 0) s += "// An array of icon addresses" + "\r\n";
            int address = data.Count * 2 + 1;
            for (int i = 0; i < data.Count; i++)
            {
                s += relAddressToString(address, i);
                address += data[i].getICOlenght();
            }

            for (int i = 0; i < data.Count; i++)
            {
                s += "// Icon No.#" + i.ToString() + "   ->";
                s += data[i].Name + "\r\n";
                s += data[i].ToString();
            }
            s += "//" + "\r\n";
            s += ((int)eCommand.last).ToString() + " }; // End collection marker\r\n";
            s += "// The end of the code generated automatically. [Designer2]";

            return s;
        }

        //---
        protected string relAddressToString(int addr, int ico)
        {
            if (addr > 127 * 127)
            {
                MessageBox.Show("The address of the Icon \r\n" +
                    data[ico].Name + " is too large.\r\n" +
                    "The maximum value is 16129.\r\n" +
                    "This icon can not be drawn.",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return "-1, -1, // ERROR ADR " + addr.ToString() + "\r\n";
            }
            int msb = addr / 127;
            int lsb = addr - msb * 127;

            return msb.ToString() + ", " + lsb.ToString() +
                               ",  //  #" + addr.ToString() + "\r\n";
        }

        //---
        public void parse(string l)
        {
            data.Clear();
            change = false;
            eventIcoSequence_Change?.Invoke(this, new EventArgs());
        }

        //---
        public bool change
        {
            set
            {
                chd = value;
                eventData_Change?.Invoke(this, new EventArgs());
            }
            get
            {
                return chd;
            }
        }

        //---
        public List<string> getNames()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                list.Add(data[i].Name);
            }
            return list;
        }

        //---
        public int getICOcount()
        {
            return data.Count;
        }
    }

    //---R
    public class ICO
    {
        protected string name;
        protected List<Item> data;
        protected Basis canvas;
        protected bool chd;
        protected int sel = -1;

        public delegate void data_Change(object sender, EventArgs e);

        public event data_Change eventData_Change;

        public delegate void selectedIndex_Change(object sender, EventArgs e);

        public event selectedIndex_Change eventSelectedIndex_Change;

        //---
        public ICO(string n)
        {
            name = n;
            data = new List<Item>();
            selectedIndex = -1;
            canvas = new Basis();
        }

        //---
        public Item this[int ind]
        {
            get
            {
                if (testIndex(ind) == false) return null;
                return data[ind];
            }
        }

        //---
        public string Name
        {
            set
            {
                name = value;
                eventData_Change?.Invoke(this, new EventArgs());
            }
            get
            {
                return name;
            }
        }

        //---
        public int selectedIndex
        {
            set
            {
                sel = value;

                eventSelectedIndex_Change?.Invoke(this, new EventArgs());
            }
            get
            {
                return sel;
            }
        }

        //---
        public void clear()
        {
            data.Clear();
            change = true;
            selectedIndex = -1;
            eventData_Change?.Invoke(this, new EventArgs());
        }

        //---
        public void add(Item it)
        {
            data.Add(it);
            change = true;
            selectedIndex = data.Count - 1;
            eventData_Change?.Invoke(this, new EventArgs());
        }

        //---
        public void insert(int ind, Item it)
        {
            if (testIndex(ind) == false) return;

            data.Insert(ind, it);
            change = true;
            selectedIndex = ind;
            eventData_Change?.Invoke(this, new EventArgs());
        }

        //---
        public void del(int ind)
        {
            if (testIndex(ind) == false) return;
            if (DialogResult.Yes ==
                MessageBox.Show("Pressing the 'YES' button will result in the\r\n" +
                     " loss of data. Continue anyway?",
                     "Warning.",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Warning,
                     MessageBoxDefaultButton.Button2))
            {
                data.RemoveAt(ind);

                if (data.Count == 0) selectedIndex = -1;
                else if (sel >= data.Count) selectedIndex = data.Count - 1;
                //eventData_Change?.Invoke();
                change = true;
                return;
            }
        }

        //---
        public void up(int ind)
        {
            if (ind == data.Count - 1) ind = -1;
            if (testIndex(ind) == false) return;
            Item temp = data[ind];
            data.RemoveAt(ind);
            data.Insert(ind + 1, temp);
            selectedIndex = ind + 1;
            change = true;
            eventData_Change?.Invoke(this, new EventArgs());
        }

        //---
        public void down(int ind)
        {
            if (ind == 0) ind = -1;
            if (testIndex(ind) == false) return;
            Item temp = data[ind];
            data.RemoveAt(ind);
            data.Insert(ind - 1, temp);
            selectedIndex = ind - 1;
            change = true;
            eventData_Change?.Invoke(this, new EventArgs());
        }

        //---
        protected bool testIndex(int i)
        {
            if (i < 0 || i >= data.Count())
            {
                MessageBox.Show("Class ICO, method testIndex index out of range.",
                     "Error.",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        //---
        public bool isEmpty()
        {
            if (data.Count > 0) return false;
            return true;
        }

        //---
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < data.Count; i++)
            {
                s += data[i].ToString();
            }

            s += ((int)eCommand.end).ToString() + ",  // ";
            if (data.Count == 0) s += "### WARNING! EMPTY ICO ### ";
            s += "End ICO " + name + "\r\n";

            return s;
        }

        //---
        public void parse(string s)
        {
        }

        //---
        public bool change
        {
            set
            {
                chd = value;
                eventData_Change?.Invoke(this, new EventArgs());
            }
            get
            {
                return chd;
            }
        }

        //---
        public Basis draw(int selected, eColor def)
        {
            Basis c = new Basis();

            eColor cl = def;
            for (int i = 0; i < data.Count; i++)
            {
                if (i == selected && data[i].GetType() != typeof(color)) continue;

                cl = data[i].draw(ref c, cl, i);
            }
            if (selected >= 0)
            {
                data[selected].draw(ref c, eColor.Selected, selected);
            }
            bool changeCanvas = false;
            c = compare(ref changeCanvas, c);
            canvas = c;
            return c;
        }

        //---
        public void painted()
        {
            canvas.painted();
        }

        //---
        protected Basis compare(ref bool drw, Basis b)
        {
            drw = false;
            for (int i = 0; i < Basis.dim; i++)
            {
                for (int z = 0; z < Basis.dim; z++)
                {
                    if (b[i, z] == canvas[i, z]) b[i, z].toDraw = false;
                    else drw = true;
                }
            }

            return b;
        }

        //---
        public int getItemCount()
        {
            return data.Count();
        }

        //---
        public int getICOlenght()
        {
            int count = 0;
            for (int i = 0; i < data.Count; i++)
            {
                count += data[i].itemLenght();
            }
            count++;
            return count;
        }

        //---
        public string[] getExItemCount()
        {
            var subclassTypes = Assembly
            .GetAssembly(typeof(Item))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Item)));

            Type[] ty = subclassTypes.ToArray();
            int[] types = new int[ty.Length];

            for (int i = 0; i < data.Count; i++)
            {
                for (int z = 0; z < ty.Length; z++)
                {
                    if (data[i].GetType() == ty[z])
                    {
                        types[z]++;
                        continue;
                    }
                }
            }
            string[] spec = new string[ty.Length];

            for (int i = 0; i < spec.Length; i++)
            {
                spec[i] = ty[i].ToString().Substring(10) +
                    " : " + types[i].ToString();
            }

            return spec;
        }

        //---
        public Rectangle rect()
        {
            if (data.Count == 0) return new Rectangle(-1000, -1000, -1000, -1000);

            int minX = 1000;
            int minY = 1000;
            int maxX = -1000;
            int maxY = -1000;

            for (int i = 0; i < data.Count; i++)
            {
                Rectangle rt = data[i].getRect();
                if (rt.X == -1000) continue;

                if (rt.X < minX) minX = rt.X;
                if (rt.Y < minY) minY = rt.Y;
                if (rt.X + rt.Width > maxX) maxX = rt.X + rt.Width;

                if (rt.Y + rt.Height > maxY) maxY = rt.Y + rt.Height;
            }
            if (minX == 1000)
            {
                minX = -1000;
                minY = minX;
            }
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        //---
        public List<string> getNames()
        {
            List<string> tl = new List<string>();

            for (int i = 0; i < data.Count; i++)
                tl.Add(data[i].GetType().Name + data[i].getName());

            return tl;
        }

        //---
        public List<string> test()
        {
            List<string> tst = new List<string>();
            if (data.Count == 0)
            {
                tst.Add("Empty ICO.");
                return tst;
            }
            Rectangle r = rect();
            if (r.X == -1000)
            {
                tst.Add("ICO has no visible elements.");
            }
            /*
                        else

                        if (data[0].GetType() != typeof(color))
                        {
                            tst.Add("First item is not a color.");
                        }

                        if (data[0].GetType() == typeof(color) &&
                            data[0].property().Nodes[0].Text == eColor.Def.ToString())
                        {
                            tst.Add("First color = Def have no effects.");
                        }
            */

            for (int i = 0; i < data.Count; i++)
            {
                List<errClass> errL = data[i].test();

                for (int z = 0; z < errL.Count; z++)
                {
                    tst.Add(errL[z].Text);
                }
            }

            tst.AddRange(exTest());

            return tst;
        }

        //---
        protected List<string> exTest()
        {
            List<string> tst = new List<string>();

            tst.AddRange(testColor());
            tst.AddRange(testmLine());
            tst.AddRange(testDupl());

            return tst;
        }

        //---
        protected List<string> testColor()
        {
            List<string> tst = new List<string>();
            string tstCl = eColor.Def.ToString();
            bool first = true;
            bool wasCl = false;
            string lastCl = "";

            if (data.Count > 0 && data[0].GetType() != typeof(color))
            {
                tst.Add("First item is not a color.");
            }

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].GetType() == typeof(color))
                    if (first)
                    {
                        first = false;
                        if (data[i].property().Nodes[0].Text == tstCl)
                        {
                            tst.Add("Color" + data[i].getName() +
                                " First occurrence of color = Def have no effects.");
                        }
                        tstCl = data[i].property().Nodes[0].Text;
                        wasCl = true;
                        lastCl = "Color" + data[i].getName();
                    }
                    else
                    {
                        if (data[i - 1].GetType() == typeof(color))
                        {
                            tst.Add("Between Color" + data[i - 1].getName() +
                                " & Color" + data[i].getName() +
                                  " there are no visible elements.");
                        }

                        if (data[i].property().Nodes[0].Text == tstCl)
                        {
                            tst.Add(lastCl + " & Color" + data[i].getName() +
                                  " are equal.(no effect)");
                        }

                        tstCl = data[i].property().Nodes[0].Text;
                        wasCl = true;
                        lastCl = "Color" + data[i].getName();
                    }
            }

            if (data.Count > 0 && data[data.Count - 1].GetType() == typeof(color))
            {
                tst.Add("Color" + data[data.Count - 1].getName() +
                    " as the last element has no effect.");
            }

            if (wasCl == false)
            {
                tst.Add("No color defined.");
            }
            return tst;
        }

        //---
        protected List<string> testmLine()
        {
            List<string> tst = new List<string>();
            Point tp = new Point(-1, -1);
            string lastLine = "";
            string cl = eColor.Def.ToString();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].GetType() == typeof(color))
                {
                    if (cl != data[i].property().Nodes[0].Text)
                    {
                        cl = data[i].property().Nodes[0].Text;
                        tp = new Point(-1, -1);
                    }
                }
                if (data[i].GetType() == typeof(mLine))
                {
                    if (((mLine)data[i]).closed == false)
                    {
                        if (tp == ((mLine)data[i]).Points[0])
                        {
                            tst.Add(lastLine + " & mLine" + data[i].getName() +
                                " may be joined together.");
                        }
                        lastLine = "mLine" + data[i].getName();
                        tp = ((mLine)data[i]).Points[((mLine)data[i]).Points.Count - 1];
                    }
                }
            }

            return tst;
        }

        //---
        protected List<string> testDupl()
        {
            List<string> tst = new List<string>();
            bool[] fl = new bool[data.Count];
            for (int i = 0; i < fl.Length; i++)
                fl[i] = false;

            for (int i = 0; i < data.Count - 1; i++)
            {
                if (fl[i]) continue;
                fl[i] = true;
                if (data[i].GetType() == typeof(color)) continue;

                List<int> indL = new List<int>();
                for (int z = i + 1; z < data.Count; z++)
                {
                    if (fl[z]) continue;

                    if (data[i].GetType() == data[z].GetType())
                    {
                        if (data[i].GetType() == typeof(point))
                            if ((point)data[i] == (point)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(mLine))
                            if ((mLine)data[i] == (mLine)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(circle))
                            if ((circle)data[i] == (circle)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(ellipse))
                            if ((ellipse)data[i] == (ellipse)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(triangle))
                            if ((triangle)data[i] == (triangle)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(rectangle))
                            if ((rectangle)data[i] == (rectangle)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(roundRect))
                            if ((roundRect)data[i] == (roundRect)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(polygon))
                            if ((polygon)data[i] == (polygon)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                        if (data[i].GetType() == typeof(arc))
                            if ((arc)data[i] == (arc)data[z])
                            {
                                indL.Add(z);
                                fl[z] = true;
                            }
                    }
                }
                if (indL.Count > 0)
                {
                    if (indL.Count < 3)
                    {
                        string err = data[i].GetType().Name + data[i].getName();
                        for (int h = 0; h < indL.Count; h++)
                        {
                            err += " & " + data[indL[h]].GetType().Name + data[indL[h]].getName();
                        }
                        err += " are equal.";
                        tst.Add(err);
                    }
                    else
                    {
                        tst.Add("This items :");
                        tst.Add(data[i].GetType().Name + data[i].getName() + " ,");
                        for (int h = 0; h < indL.Count; h++)
                        {
                            tst.Add(data[indL[h]].GetType().Name + data[indL[h]].getName() + " ,");
                        }

                        tst.Add("are equal.");
                    }
                }
            }

            return tst;
        }

        //---
        public string getName(int i)
        {
            if (i < 0 || i >= data.Count) return "Error!";
            return data[i].GetType().Name + data[i].getName();
        }
    }

    //---R
    internal static class Utils
    {
        public static DateTime GetLinkerDateTime(this Assembly assembly, TimeZoneInfo tzi = null)
        {
            // Constants related to the Windows PE file format.
            const int PE_HEADER_OFFSET = 60;
            const int LINKER_TIMESTAMP_OFFSET = 8;

            // Discover the base memory address where our assembly is loaded
            var entryModule = assembly.ManifestModule;
            var hMod = Marshal.GetHINSTANCE(entryModule);
            if (hMod == IntPtr.Zero - 1) throw new Exception("Failed to get HINSTANCE.");

            // Read the linker time stamp
            var offset = Marshal.ReadInt32(hMod, PE_HEADER_OFFSET);
            var secondsSince1970 = Marshal.ReadInt32(hMod, offset + LINKER_TIMESTAMP_OFFSET);

            // Convert the time stamp to a DateTime
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
            var dt = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tzi ?? TimeZoneInfo.Local);
            return dt;
        }
    }
}
