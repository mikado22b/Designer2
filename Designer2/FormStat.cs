using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Designer2
{
    public partial class FormStat : Form
    {
        //---
        public FormStat()
        {
            InitializeComponent();
        }

        //---
        public FormStat(ICOlist icL)
        {
            InitializeComponent();
            tsWarning.Visible = false;

            treeView.ImageList = imageList;
            treeView.SelectedImageIndex = 1;
            treeView.Nodes.Add(new TreeNode("Icon collection ."));
            treeView.Nodes[0].ImageIndex = (int)eImage.iICOlist;
            treeView.Nodes[0].SelectedImageIndex = (int)eImage.iICOlist;

            int itCount = 0;
            int flashMemory = 2 + 2 * icL.getICOcount();
            for (int i = 0; i < icL.getICOcount(); i++)
            {
                itCount += icL[i].getItemCount();
                flashMemory += icL[i].getICOlenght();
            }
            tsLabel1.Text = " " + icL.getICOcount().ToString();
            tsLabel2.Text = " " + itCount.ToString();
            tsLabel3.Text = " " + flashMemory.ToString();

            treeView.Nodes[0].Nodes.Add("Total items count : " + itCount);
            if (itCount == 0)
            {
                treeView.Nodes[0].Nodes[0].ImageIndex = (int)eImage.iStdWar;
                treeView.Nodes[0].Nodes[0].SelectedImageIndex = (int)eImage.iStdWar;
                tsWarning.Visible = true;
            }

            treeView.Nodes[0].Nodes.Add("Total bytes in flash : " + flashMemory);
            treeView.Nodes[0].Nodes[1].ImageIndex = (int)eImage.iFlash;
            treeView.Nodes[0].Nodes[1].SelectedImageIndex = (int)eImage.iFlash;

            treeView.Nodes[0].Nodes.Add(new TreeNode("Colors."));
            treeView.Nodes[0].Nodes[2].ImageIndex = (int)eImage.iColor;
            treeView.Nodes[0].Nodes[2].SelectedImageIndex = (int)eImage.iColor;

            treeView.Nodes[0].Nodes[2].Nodes.Add("Selected skin : " + color.Skin.ToString());
            treeView.Nodes[0].Nodes[2].Nodes[0].ImageIndex = (int)eImage.iSkin;
            treeView.Nodes[0].Nodes[2].Nodes[0].SelectedImageIndex = (int)eImage.iSkin;

            treeView.Nodes[0].Nodes[2].Nodes.Add("Default color : " + color.Default.ToString());
            treeView.Nodes[0].Nodes[2].Nodes[1].ImageIndex = (int)eImage.iColor;
            treeView.Nodes[0].Nodes[2].Nodes[1].SelectedImageIndex = (int)eImage.iColor;

            treeView.Nodes[0].Nodes[2].Nodes[1].BackColor = color.setColor(color.Default);
            treeView.Nodes[0].Nodes[2].Nodes[1].ForeColor = color.contrast(color.setColor(color.Default));

            string ic;
            if (icL.getICOcount() == 0)
            {
                ic = "Empty collection.";
                tsWarning.Visible = true;
            }
            else ic = "Icons count : " + icL.getICOcount();
            treeView.Nodes[0].Nodes.Add(new TreeNode(ic));
            if (icL.getICOcount() == 0)
            {
                treeView.Nodes[0].Nodes[3].ImageIndex = (int)eImage.iWarning;
                treeView.Nodes[0].Nodes[3].SelectedImageIndex = (int)eImage.iWarning;
                treeView.Nodes[0].Nodes[3].ForeColor = Color.Black;
                treeView.Nodes[0].Nodes[3].BackColor = Color.Yellow;
            }
            else
            {
                bool ind = false;
                for (int z = 0; z < icL.getICOcount(); z++)
                {
                    if (icL[z].test().Count > 0)
                    {
                        ind = true;
                        break;
                    }
                }
                if (ind)
                {
                    treeView.Nodes[0].Nodes[3].ImageIndex = (int)eImage.iListWar;
                    treeView.Nodes[0].Nodes[3].SelectedImageIndex = (int)eImage.iListWar;
                }
                else
                {
                    treeView.Nodes[0].Nodes[3].ImageIndex = (int)eImage.iList;
                    treeView.Nodes[0].Nodes[3].SelectedImageIndex = (int)eImage.iList;
                }
            }

            for (int i = 0; i < icL.getICOcount(); i++)
            {
                treeView.Nodes[0].Nodes[3].Nodes.Add("Icons.", icL[i].Name);
                if (icL[i].test().Count == 0)
                {
                    treeView.Nodes[0].Nodes[3].Nodes[i].ImageIndex = (int)eImage.iICO;
                    treeView.Nodes[0].Nodes[3].Nodes[i].SelectedImageIndex = (int)eImage.iICO;
                }
                else
                {
                    treeView.Nodes[0].Nodes[3].Nodes[i].ImageIndex = (int)eImage.iICOwar;
                    treeView.Nodes[0].Nodes[3].Nodes[i].SelectedImageIndex = (int)eImage.iICOwar;
                    tsWarning.Visible = true;
                }

                fillICOnode(i, icL);
            }
        }

        //---
        protected void fillICOnode(int i, ICOlist icL)
        {
            int baseNode = 3;
            List<string> lErr = icL[i].test();
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add("Items count : " +
                                                icL[i].getItemCount());
            if (icL[i].getItemCount() > 0)
            {
                string[] types = icL[i].getExItemCount();
                for (int z = 0; z < types.Length; z++)
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[0].Nodes.Add(types[z]);
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[0].Nodes[z].ImageIndex = z + (int)eImage.iPoint;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[0].Nodes[z].SelectedImageIndex = z + (int)eImage.iPoint;
                }
            }
            else
            {
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[0].ImageIndex = (int)eImage.iStdWar;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[0].SelectedImageIndex = (int)eImage.iStdWar;
            }
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add("Bytes in flash : " +
                                                icL[i].getICOlenght());
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[1].ImageIndex = (int)eImage.iFlash;
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[1].SelectedImageIndex = (int)eImage.iFlash;

            int address = icL.getICOcount() * 2 + 1;
            for (int z = 0; z < i; z++)
            {
                address += icL[z].getICOlenght();
            }

            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add("Address : " + address);
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[2].ImageIndex = (int)eImage.iAddress;
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[2].SelectedImageIndex = (int)eImage.iAddress;

            if (icL[i].getItemCount() == 0)
            {
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add("Empty ICO.");
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].ImageIndex = (int)eImage.iWarning;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].SelectedImageIndex = (int)eImage.iWarning;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].ForeColor = Color.Black;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].BackColor = Color.Yellow;
                return;
            }

            Rectangle r = icL[i].rect();
            if (r.X != -1000)
            {
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add(new TreeNode("ICO position."));
                if (r.X < 0 || r.Y < 0)
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].ImageIndex = (int)eImage.iPositionWar;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].SelectedImageIndex = (int)eImage.iPositionWar;
                }
                else
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].ImageIndex = (int)eImage.iPosition;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].SelectedImageIndex = (int)eImage.iPosition;
                }
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes.Add("Origin point X= " + r.X);
                if (r.X < 0)
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[0].ImageIndex = (int)eImage.iXposWar;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[0].SelectedImageIndex = (int)eImage.iXposWar;
                }
                else
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[0].ImageIndex = (int)eImage.iXpos;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[0].SelectedImageIndex = (int)eImage.iXpos;
                }
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes.Add("Origin point Y= " + r.Y);
                if (r.Y < 0)
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[1].ImageIndex = (int)eImage.iYposWar;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[1].SelectedImageIndex = (int)eImage.iYposWar;
                }
                else
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[1].ImageIndex = (int)eImage.iYpos;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[1].SelectedImageIndex = (int)eImage.iYpos;
                }

                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes.Add("Max X= " + (r.X + r.Width));
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[2].ImageIndex = (int)eImage.iXpos;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[2].SelectedImageIndex = (int)eImage.iXpos;

                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes.Add("Max Y= " + (r.Y + r.Height));
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[3].ImageIndex = (int)eImage.iYpos;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[3].SelectedImageIndex = (int)eImage.iYpos;

                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes.Add("Icon width = " + r.Width);
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[4].ImageIndex = (int)eImage.iWidth;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[4].SelectedImageIndex = (int)eImage.iWidth;

                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes.Add("Icon height = " + r.Height);
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[5].ImageIndex = (int)eImage.iHeight;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].Nodes[5].SelectedImageIndex = (int)eImage.iHeight;
            }
            else
            {
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add("This icon has no visible items.");
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].ImageIndex = (int)eImage.iWarning;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[3].SelectedImageIndex = (int)eImage.iWarning;
            }

            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add(new TreeNode("Items list."));
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[4].ImageIndex = (int)eImage.iList;
            treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[4].SelectedImageIndex = (int)eImage.iList;

            for (int z = 0; z < icL[i].getItemCount(); z++)
            {
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[4].Nodes.Add(icL[i][z].property());
                if ((bool)(treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[4].Nodes[z].Tag) == false)
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[4].ImageIndex = (int)eImage.iListWar;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[4].SelectedImageIndex = (int)eImage.iListWar;
                }
            }

            if (lErr.Count > 0)
            {
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes.Add(new TreeNode("Warning !"));
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].ImageIndex = (int)eImage.iWarning;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].SelectedImageIndex = (int)eImage.iWarning;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].ForeColor = Color.Black;
                treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].BackColor = Color.Yellow;

                for (int z = 0; z < lErr.Count; z++)
                {
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].Nodes.Add(lErr[z]);
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].Nodes[z].ImageIndex = (int)eImage.iEmpty;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].Nodes[z].SelectedImageIndex = (int)eImage.iEmpty;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].Nodes[z].ForeColor = Color.Black;
                    treeView.Nodes[0].Nodes[baseNode].Nodes[i].Nodes[5].Nodes[z].BackColor = Color.Yellow;
                }
            }
        }

        //---
        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.CollapseAll();
        }

        //---
        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.ExpandAll();
        }

        //---
        private void closewindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //---
    }
}
