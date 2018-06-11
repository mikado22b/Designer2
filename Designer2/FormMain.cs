using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Designer2
{
    public partial class FormMain : Form
    {
        public ICOlist icons = new ICOlist();
        private string pathToFile = "";
        private Setting setting;
        private ToolStripMenuItem[] tabTS = new ToolStripMenuItem[10];
        private bool firstShow = true;
        private string progName = "Designer2";

        //---
        public FormMain()
        {
            InitializeComponent();
        }

        //---

        #region startup

        //---
        private void FormMain_Load(object sender, EventArgs e)
        {
            comboBoxItems.Items.Clear();

            var subclassTypes = Assembly
             .GetAssembly(typeof(Item))
             .GetTypes()
             .Where(t => t.IsSubclassOf(typeof(Item)));

            comboBoxItems.Items.AddRange(subclassTypes.ToArray());
            for (int i = 0; i < comboBoxItems.Items.Count; i++)
            {
                comboBoxItems.Items[i] = comboBoxItems.Items[i].ToString().Substring(10);
                MenuItemsType.Items.Add(comboBoxItems.Items[i]);
            }

            comboBoxItems.SelectedIndex = comboBoxItems.Items.Count - 1;
            MenuItemsType.SelectedIndex = comboBoxItems.SelectedIndex;
            icons.eventIcoSequence_Change += new ICOlist.icoSequence_Change(actualizeIcon);
            icons.eventData_Change += new ICOlist.data_Change(dataChange);
            icons.eventICOitemSequence_Change += new ICOlist.ICOitemSequence_change(actualizeItem);

            tabTS[0] = file0Menu;
            tabTS[1] = file1Menu;
            tabTS[2] = file2Menu;
            tabTS[3] = file3Menu;
            tabTS[4] = file4Menu;
            tabTS[5] = file5Menu;
            tabTS[6] = file6Menu;
            tabTS[7] = file7Menu;
            tabTS[8] = file8Menu;
            tabTS[9] = file9Menu;

            MenuDefColor.Items.AddRange(Enum.GetNames(typeof(eColor)));
            MenuDefColor.Items.RemoveAt(MenuDefColor.Items.Count - 1);
            MenuDefColor.Items.RemoveAt(MenuDefColor.Items.Count - 1);
            MenuDefColor.Items.RemoveAt(MenuDefColor.Items.Count - 1);
            MenuDefColor.Items.RemoveAt(MenuDefColor.Items.Count - 1);
            MenuDefColor.Items.RemoveAt(MenuDefColor.Items.Count - 1);

            MenuDefColor.SelectedIndex = (int)color.Default;

            MenuSkin.Items.AddRange(Enum.GetNames(typeof(eSkin)));
            MenuSkin.SelectedIndex = (int)color.Skin;

            setting = new Setting(@"Setting.txt");

            if (setting.read() == false)
            {
                MessageBox.Show("There are errors in the configuration file.\r\n" +
                    "The program will fix them and run on.\r\n" +
                    "Please do not change the 'Settings.txt' file manually.",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                setting.clearFile();
                saveSettings();
            }
            canvasPanel.BackColor = setting.bgColor;
            recentFileUpdate();
            autoLoadUpDate();
            windowSaveUpdate();
            dataChange(sender, e);
            loadICOtoTab(-1);
        }

        //---
        private void saveSettings()
        {
            setting.bgColor = canvasPanel.BackColor;
            setting.autoLoad = MenuAutoLoad.Checked;
            setting.windowSize = this.Size;
            setting.windowLocation = this.Location;
            setting.windowSave = MenuRemember.Checked;

            setting.save();
        }

        //---
        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (firstShow)
            {
                firstShow = false;

                if (setting.autoLoad && setting.getRecentFiles()[0] != "")
                {
                    MessageBox.Show("Auto load file :\r\n" +
                        setting.getRecentFiles()[0],
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);

                    file0Menu_Click(file0Menu, e);
                }
                itemSelIndChange(sender, e);
            }
        }

        //---

        #endregion startup

        //---

        #region menu file

        //---
        private void MenuNew_Click(object sender, EventArgs e)
        {
            if (canNew() == true)
            {
                icons.clear();
                pathToFile = "";
                Text = progName;
            }
        }

        //---
        private void MenuPaste_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                MessageBox.Show("Clipboard have no text data.", "Information.",
                                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (canNew())
            {
                icons.clear();
                Text = progName;
                pathToFile = "";
                icons.parse(Clipboard.GetText());
            }
        }

        //---
        private void MenuCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(icons.ToString(), TextDataFormat.UnicodeText);
            MessageBox.Show("Data saved to clipboard.", "Information.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //---
        private void MenuLoad_Click(object sender, EventArgs e)
        {
            if (canNew())
            {
                if (pathToFile == "")
                {
                    openFileDialog1.FileName = @"";
                    openFileDialog1.InitialDirectory = @"c:\\";
                }
                else
                {
                    openFileDialog1.FileName = pathToFile;
                    openFileDialog1.InitialDirectory = pathToFile;
                }
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    loadFile(openFileDialog1.FileName);
                }
            }
        }

        //---
        private void MenuSave_Click(object sender, EventArgs e)
        {
            if (pathToFile == "")
            {
                MenuSaveAs_Click(sender, e);
                return;
            }
            saveFile();
        }

        //---
        private void MenuSaveAs_Click(object sender, EventArgs e)
        {
            if (pathToFile == "")
            {
                saveFileDialog1.FileName = @"";
                saveFileDialog1.InitialDirectory = @"c:\\";
            }
            else
            {
                saveFileDialog1.FileName = pathToFile;
                saveFileDialog1.InitialDirectory = pathToFile;
            }
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathToFile = saveFileDialog1.FileName;
                saveFile();
            }
        }

        //---
        private void file0Menu_Click(object sender, EventArgs e)
        {
            if (canNew())
            {
                if (File.Exists((sender as ToolStripMenuItem).Text.Substring(4)) == false)
                {
                    if (DialogResult.No == MessageBox.Show("No file found.\r\nPress 'Yes' removes an entry from the list." +
                         "\r\nPress 'No' cancels the selected option\r\nand returns to the main program.",
                                         "Error",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Error)) return;
                    setting.delFile((sender as ToolStripMenuItem).Text.Substring(4));
                    return;
                }
                loadFile((sender as ToolStripMenuItem).Text.Substring(4));
            }
        }

        //---
        private void MenuClearList_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are You sure?",
                  "Warning!",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Warning,
                  MessageBoxDefaultButton.Button2))
            {
                setting.clearFile();
                recentFileUpdate();
            }
        }

        //---
        private void MenuEnd_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion menu file

        //---

        #region menu Icons

        //---
        private void MenuIconsList_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(0);
        }

        //---
        private void MenuIconsAdd_Click(object sender, EventArgs e)
        {
            addInsert(-1);
        }

        //---
        private void MenuIconsInsert_Click(object sender, EventArgs e)
        {
            if (listBoxIcons.Items.Count == 0)
            {
                MenuIconsAdd_Click(sender, e);
                return;
            }
            if (listBoxIcons.SelectedIndex < 0)
            {
                MessageBox.Show("No Icon selected. Please indicate the one\r\n" +
                    "before which you want to insert a new one.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            addInsert(listBoxIcons.SelectedIndex);
        }

        //---
        private void MenuIconsDel_Click(object sender, EventArgs e)
        {
            if (listBoxIcons.Items.Count == 0)
            {
                MessageBox.Show("No Icon to delete.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBoxIcons.SelectedIndex < 0)
            {
                MessageBox.Show("Selected no Icon to delete.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ind = listBoxIcons.SelectedIndex;
            if (icons[ind].isEmpty() == false)
            {
                if (DialogResult.No ==
                 MessageBox.Show("Pressing the 'YES' button will result in the\r\n" +
                      " loss of data. Continue anyway?",
                      "Warning.",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Warning,
                      MessageBoxDefaultButton.Button2)) return;
            }
            icons[ind].clear();
            icons.del(ind);
            if (icons.empty()) return;

            if (ind >= listBoxIcons.Items.Count) ind = listBoxIcons.Items.Count - 1;

            listBoxIcons.SelectedIndex = ind;
        }

        //---
        private void MenuIconsMoveUp_Click(object sender, EventArgs e)
        {
            if (listBoxIcons.Items.Count == 0)
            {
                MessageBox.Show("No Icon to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBoxIcons.SelectedIndex < 0)
            {
                MessageBox.Show("Selected no Icon to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ind = listBoxIcons.SelectedIndex;
            if (ind == 0)
            {
                MessageBox.Show("You can't move up this Icon.", "Error.",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            icons.down(ind);
            listBoxIcons.SelectedIndex = ind - 1;
        }

        //---
        private void MenuIconsMoveDown_Click(object sender, EventArgs e)
        {
            if (listBoxIcons.Items.Count == 0)
            {
                MessageBox.Show("No Icon to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBoxIcons.SelectedIndex < 0)
            {
                MessageBox.Show("Selected no Icon to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ind = listBoxIcons.SelectedIndex;
            if (ind == listBoxIcons.Items.Count - 1)
            {
                MessageBox.Show("You can't move down this Icon.", "Error.",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            icons.up(ind);
            listBoxIcons.SelectedIndex = ind + 1;
        }

        //---
        private void menuIconChangeName_Click(object sender, EventArgs e)
        {
            {
                IcoName f = new IcoName(textBoxName.Text);
                string t = "";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    t = f.icoName;

                    if (t == textBoxName.Text) return;
                    int err = icons.isGoodName(t);
                    if (err < 0)
                    {
                        int i = listBoxIcons.SelectedIndex;
                        if (i < 0)
                        {
                            MessageBox.Show("No Icon has been selected for renaming.",
                                        "Error.",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                            return;
                        }
                        icons[i].Name = t;
                        actualizeIcon(sender, e);
                        listBoxIcons.SelectedIndex = i;

                        return;
                    }
                    string[] errTab = { "The name must have at least one character.",
                    "The first character of the name must be '_' or a letter.",
                    "Permissible characters are letters, numbers or '_'.",
                    "The name must be unique. This name already exists." };
                    MessageBox.Show(errTab[err],
                                        "Error.",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                }
            }
        }

        //---
        private void MenuIconsPreview_Click(object sender, EventArgs e)
        {
            FormPreview fp = new FormPreview();
            fp.textBox1.Text = icons.ToString();
            fp.textBox1.Select(0, 0);
            fp.Show();
        }

        #endregion menu Icons

        //---

        #region menu Items

        private void MenuItemsList_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(1);
        }

        //---
        private void MenuItemsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTypeChange(sender, e);
        }

        //---
        private void MenuItemsAdd_Click(object sender, EventArgs e)
        {
            addInsertItem(-1);
        }

        //---
        private void MenuItemsInsert_Click(object sender, EventArgs e)
        {
            if (listBoxItems.Items.Count == 0)
            {
                MenuItemsAdd_Click(sender, e);
                return;
            }
            if (listBoxItems.SelectedIndex < 0)
            {
                MessageBox.Show("No item selected. Please indicate the one\r\n" +
                    "before which you want to insert a new one.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            addInsertItem(listBoxItems.SelectedIndex);
        }

        //---
        private void MenuItemsDel_Click(object sender, EventArgs e)
        {
            if (listBoxItems.Items.Count == 0)
            {
                MessageBox.Show("No item to delete.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBoxItems.SelectedIndex < 0)
            {
                MessageBox.Show("Selected no item to delete.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ind = listBoxItems.SelectedIndex;
            icons[listBoxIcons.SelectedIndex].del(ind);

            if (icons[listBoxIcons.SelectedIndex].isEmpty()) return;

            if (ind >= listBoxItems.Items.Count) ind = listBoxItems.Items.Count - 1;

            listBoxItems.SelectedIndex = ind;
        }

        //---
        private void MenuItemsMoveUp_Click(object sender, EventArgs e)
        {
            if (listBoxItems.Items.Count == 0)
            {
                MessageBox.Show("No Item to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBoxItems.SelectedIndex < 0)
            {
                MessageBox.Show("Selected no Item to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ind = listBoxItems.SelectedIndex;
            if (ind == 0)
            {
                MessageBox.Show("You can't move up this Item.", "Error.",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            icons[listBoxIcons.SelectedIndex].down(ind);
            listBoxItems.SelectedIndex = ind - 1;
        }

        //---
        private void MenuItemsMoveDown_Click(object sender, EventArgs e)
        {
            if (listBoxItems.Items.Count == 0)
            {
                MessageBox.Show("No Item to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBoxItems.SelectedIndex < 0)
            {
                MessageBox.Show("Selected no Item to move.", "Error.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ind = listBoxItems.SelectedIndex;
            if (ind == listBoxItems.Items.Count - 1)
            {
                MessageBox.Show("You can't move down this Item.", "Error.",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            icons[listBoxIcons.SelectedIndex].up(ind);
            listBoxItems.SelectedIndex = ind + 1;
        }

        //---
        private void MenuItemsProperty_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(2);
        }

        #endregion menu Items

        //---

        #region menu settings

        private void MenuBackgroundColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = canvasPanel.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                setting.bgColor = colorDialog1.Color;
                canvasPanel.BackColor = setting.bgColor;
            }
        }

        //---
        private void MenuAutoLoad_Click(object sender, EventArgs e)
        {
            setting.autoLoad = !setting.autoLoad;
            MenuAutoLoad.Checked = setting.autoLoad;
        }

        //---
        private void MenuRemember_Click(object sender, EventArgs e)
        {
            setting.windowSave = !setting.windowSave;
            MenuRemember.Checked = setting.windowSave;
        }

        //---
        private void MenuDefColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            color.Default = (eColor)MenuDefColor.SelectedIndex;
            for (int i = 0; i < propertyTab.Controls.Count; i++)
            {
                if (propertyTab.Controls[i].Name == "cbdef")
                {
                    (propertyTab.Controls[i] as ComboBox).SelectedIndex = MenuDefColor.SelectedIndex;
                    return;
                }
            }
        }

        //---
        private void MenuSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            color.Skin = (eSkin)MenuSkin.SelectedIndex;
            for (int i = 0; i < propertyTab.Controls.Count; i++)
                if (propertyTab.Controls[i].Name == "cbskn")
                {
                    (propertyTab.Controls[i] as ComboBox).SelectedIndex = MenuSkin.SelectedIndex;
                    return;
                }
        }

        #endregion menu settings

        //---

        #region menu Info

        //---
        private void MenuStatistic_Click(object sender, EventArgs e)
        {
            FormStat f = new FormStat(icons);
            f.ShowDialog();
        }

        //---
        private void MenuAbout_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();
            f.ShowDialog();
        }

        #endregion menu Info

        //---
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveSettings();
            if (icons.change)
            {
                if (MessageBox.Show("Closing the application will result in the\r\n" +
                      " loss of unsaved data. Continue anyway?",
                      "Warning.",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Warning,
                      MessageBoxDefaultButton.Button2) == DialogResult.No)
                { e.Cancel = true; }
            }
        }

        //---
        private void loadFile(string f)
        {
            if (File.Exists(f) == false)
            {
                MessageBox.Show("No file found.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                return;
            }
            Text = progName + " - " + f;
            pathToFile = f;

            icons.parse(File.ReadAllText(f, Encoding.Default));
            setting.addFile(f);
            recentFileUpdate();
        }

        //---
        private void saveFile()
        {
            File.WriteAllText(pathToFile, icons.ToString(), Encoding.Default);
            setting.addFile(pathToFile);
            recentFileUpdate();
            Text = progName + " - " + pathToFile;
            icons.change = false;
        }

        //---
        private void recentFileUpdate()
        {
            string[] rf = setting.getRecentFiles();
            if (rf[0] == "")
            {
                MenuRecently.Enabled = false;
                MenuClearList.Enabled = false;
            }
            else
            {
                MenuRecently.Enabled = true;
                MenuClearList.Enabled = true;
            }
            for (int i = 0; i < 10; i++)
            {
                tabTS[i].Text = rf[i];
                if (rf[i] == "") tabTS[i].Visible = false;
                else
                {
                    tabTS[i].Text = "&" + i.ToString() + ": " + tabTS[i];
                    tabTS[i].Visible = true;
                }
            }
        }

        //---
        protected bool canNew()
        {
            if (icons.empty() || icons.change == false) return true;
            if (MessageBox.Show("Pressing the 'YES' button will result in the\r\n" +
                     " loss of unsaved data. Continue anyway?",
                     "Warning.",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Warning,
                     MessageBoxDefaultButton.Button2) == DialogResult.Yes) return true;
            return false;
        }

        //---
        private void bgColorChange()
        {
            canvasPanel.BackColor = setting.bgColor;
        }

        //---
        private void autoLoadUpDate()
        {
            MenuAutoLoad.Checked = setting.autoLoad;
        }

        //---
        private void windowSaveUpdate()
        {
            MenuRemember.Checked = setting.windowSave;
            if (MenuRemember.Checked)
            {
                int w = setting.windowSize.Width;
                int h = setting.windowSize.Height;
                int x = setting.windowLocation.X;
                int y = setting.windowLocation.Y;

                if (w < this.MinimumSize.Width) w = this.MinimumSize.Width;
                if (h < this.MinimumSize.Height) h = this.MinimumSize.Height;
                setting.windowSize = new Size(w, h);

                this.Size = setting.windowSize;
                this.Location = setting.windowLocation;
            }
        }

        //---
        private void dataChange(object sender, EventArgs e)
        {
            toolSave.Enabled = icons.change;
        }

        //---
        protected void actualizeIcon(object sender, EventArgs e)
        {
            listBoxIcons.Items.Clear();
            List<string> l = icons.getNames();
            listBoxIcons.Items.AddRange(l.ToArray());

            labelAmount.Text = icons.getICOcount().ToString();
            string send = "actualizeIcon";
            listBoxIcons_SelectedIndexChanged(send, e);
            listBoxItems_SelectedIndexChanged(send, e);
        }

        //---
        protected void actualizeItem(object sender, EventArgs e)
        {
            listBoxItems.Items.Clear();
            List<string> l = icons[listBoxIcons.SelectedIndex].getNames();
            listBoxItems.Items.AddRange(l.ToArray());

            labelAmountItem.Text = icons[listBoxIcons.SelectedIndex].getItemCount().ToString();
            string send = "actualizeItem";
            listBoxItems_SelectedIndexChanged(send, e);
        }

        //---
        protected void addInsert(int ind)
        {
            if (icons.getICOcount() == 127)
            {
                MessageBox.Show("127 is the maximum number of icons.",
                    "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int it = ind;
            if (ind < 0)
            {
                it = listBoxIcons.Items.Count;
            }
            string t = "ICO_" + it.ToString();
            IcoName f = new IcoName(t);

            if (f.ShowDialog() == DialogResult.OK)
            {
                t = f.icoName;

                int err = icons.isGoodName(t);
                if (err < 0)
                {
                    ICO ic = new Designer2.ICO(t);
                    ic.eventSelectedIndex_Change += new ICO.selectedIndex_Change(itemSelIndChange);
                    if (ind < 0)
                    {
                        icons.add(ic);
                        listBoxIcons.SelectedIndex = listBoxIcons.Items.Count - 1;
                    }
                    else
                    {
                        icons.insert(ind, ic);
                        listBoxIcons.SelectedIndex = ind;
                    }
                    return;
                }
                string[] errTab = { "The name must have at least one character.",
                    "The first character of the name must be '_' or a letter.",
                    "Permissible characters are letters, numbers or '_'.",
                    "The name must be unique. This name already exists." };
                MessageBox.Show(errTab[err],
                                    "Error.",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        //---
        protected void addInsertItem(int ind)
        {
            Item ob = new Item();
            int sw = comboBoxItems.SelectedIndex;

            switch (sw)
            {
                case 0:
                    ob = new point();
                    break;

                case 1:
                    ob = new mLine();
                    break;

                case 2:
                    ob = new circle();
                    break;

                case 3:
                    ob = new ellipse();
                    break;

                case 4:
                    ob = new triangle();
                    break;

                case 5:
                    ob = new rectangle();
                    break;

                case 6:
                    ob = new roundRect();
                    break;

                case 7:
                    ob = new polygon();
                    break;

                case 8:
                    ob = new arc();
                    break;

                case 9:
                    ob = new color();
                    break;

                default:

                    break;
            }

            if (ind < 0)
            {
                if (listBoxIcons.SelectedIndex < 0) return;
                icons[listBoxIcons.SelectedIndex].add(ob);
                listBoxItems.SelectedIndex = listBoxItems.Items.Count - 1;
            }
            else
            {
                icons[listBoxIcons.SelectedIndex].insert(ind, ob);
                listBoxItems.SelectedIndex = ind;
            }
        }

        //---
        private void listBoxIcons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxIcons.SelectedIndex < 0 || icons.empty())
            {
                SelectedIcoLabel.Text = "No Icon selected.";
            }
            else
            {
                SelectedIcoLabel.Text = icons[listBoxIcons.SelectedIndex].Name;
            }
            loadICOtoTab(listBoxIcons.SelectedIndex);
        }

        //---
        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxItems.SelectedIndex < 0)
            {
                selectedItemLabel.Text = "No Item selected.";
            }
            else
            {
                selectedItemLabel.Text = icons[listBoxIcons.SelectedIndex].getName(listBoxItems.SelectedIndex);

                icons[listBoxIcons.SelectedIndex].selectedIndex = listBoxItems.SelectedIndex;
            }
            //itemSelIndChange(sender, e);
        }

        //---
        private void selectedTypeChange(object sender, EventArgs e)
        {
            Bitmap tr = Properties.Resources.No_picture;
            int ind = -1;
            if (sender == MenuItemsType) ind = MenuItemsType.SelectedIndex;
            if (sender == comboBoxItems) ind = comboBoxItems.SelectedIndex;

            if (ind == MenuItemsType.SelectedIndex &&
                                ind == comboBoxItems.SelectedIndex) return;
            if (ind == -1)
            {
                tr.MakeTransparent(Color.White);
                pictureBox.Image = tr;

                buttonAddItem.Text = "Add (inactive)";
                buttonInsertItem.Text = "Insert (inactive)";
                comboBoxItems.Text = "";
                return;
            }

            if (sender == MenuItemsType) comboBoxItems.SelectedIndex = ind;
            if (sender == comboBoxItems) MenuItemsType.SelectedIndex = ind;

            MenuItemsAdd.Text = "&Add " + MenuItemsType.Items[ind];
            MenuItemsInsert.Text = "I&nsert " + MenuItemsType.Items[ind];

            buttonAddItem.Text = "&Add " + MenuItemsType.Items[ind];
            buttonInsertItem.Text = "I&nsert " + MenuItemsType.Items[ind];

            switch (ind)
            {
                case 0:

                    tr = Properties.Resources.Point;

                    break;

                case 1:
                    tr = Properties.Resources.mLine;

                    break;

                case 2:
                    tr = Properties.Resources.Circle;

                    break;

                case 3:
                    tr = Properties.Resources.Ellipse;
                    break;

                case 4:
                    tr = Properties.Resources.Triangle;

                    break;

                case 5:
                    tr = Properties.Resources.Rectangle;

                    break;

                case 6:
                    tr = Properties.Resources.roundRect;
                    break;

                case 7:
                    tr = Properties.Resources.Polygon;
                    break;

                case 8:
                    tr = Properties.Resources.Arc;

                    break;

                case 9:
                    tr = Properties.Resources.Color1;
                    break;

                default:

                    break;
            }
            tr.MakeTransparent(Color.White);
            pictureBox.Image = tr;
        }

        //---
        protected void loadICOtoTab(int ind)
        {
            if (ind < 0)
            {
                itemsTabState(false);
                textBoxName.Text = "No Icon selected.";
                listBoxItems.Items.Clear();
                menuItemsState(false);
            }
            else
            {
                itemsTabState(true);
                textBoxName.Text = icons[ind].Name;
                listBoxItems.Items.Clear();
                menuItemsState(true);

                List<string> l = icons[ind].getNames();
                listBoxItems.Items.AddRange(l.ToArray());

                if (l.Count > 0) listBoxItems.SelectedIndex = icons[listBoxIcons.SelectedIndex].selectedIndex;
            }

            string sender = "loadICOtoTab";
            listBoxItems_SelectedIndexChanged(sender, new EventArgs());
        }

        //---
        protected void menuItemsState(bool s)
        {
            menuIconChangeName.Enabled = s;
            MenuItemsAdd.Enabled = s;
            MenuItemsInsert.Enabled = s;
            MenuItemsDel.Enabled = s;
            MenuItemsMoveUp.Enabled = s;
            MenuItemsMoveDown.Enabled = s;
        }

        //---

        protected void itemsTabState(bool fl)
        {
            textBoxName.Enabled = fl;
            labelAmountItem.Enabled = fl;
            label2.Enabled = fl;
            comboBoxItems.Enabled = fl;
            listBoxItems.Enabled = fl;
            buttonAddItem.Enabled = fl;
            buttonInsertItem.Enabled = fl;
            buttonDelItem.Enabled = fl;
            buttonUpItem.Enabled = fl;
            buttonDownItem.Enabled = fl;
            groupBox1.Enabled = fl;
            pictureBox.Visible = fl;
            pictureBox9.Enabled = fl;
            pictureBox10.Enabled = fl;

            if (fl)
            {
                itemsTab.BackColor = SystemColors.Info;
            }
            else
            {
                itemsTab.BackColor = SystemColors.InactiveBorder;

                buttonAddItem.Text = "Add (inactive)";
                buttonInsertItem.Text = "Insert (inactive)";
                comboBoxItems.Text = "Inactive";
            }
        }

        //---
        private void comboBoxItems_TextChanged(object sender, EventArgs e)
        {
            selectedTypeChange(comboBoxItems, e);
        }

        //---
        protected void itemSelIndChange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(string)) return;
            propertyTab.Controls.Clear();
            if (listBoxIcons.SelectedIndex >= 0)
            {
                int ind = icons[listBoxIcons.SelectedIndex].selectedIndex;
                if (ind >= 0 && listBoxItems.Items.Count > 0)
                {
                    propertyTab.BackColor = SystemColors.Info;

                    List<Control> ob = icons[listBoxIcons.SelectedIndex][ind].loadItem();

                    propertyTab.Controls.AddRange(ob.ToArray());

                    if (listBoxItems.Items.Count > 1)
                    {
                        propertyTab.Controls.Add(buttonPrewItem);
                        propertyTab.Controls.Add(buttonNextItem);
                    }
                    return;
                }
            }

            propertyTab.BackColor = SystemColors.InactiveBorder;
            Label l = new Label();
            l.Text = " No Item selected.";
            l.Font = new Font(l.Font.Name, l.Font.Size + 4,
                l.Font.Style, l.Font.Unit);
            l.Top = 20;
            l.AutoSize = true;
            l.ForeColor = SystemColors.GrayText;
            propertyTab.Controls.Add(l);
        }

        //---
        private void listBoxIcons_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MenuItemsList_Click(sender, e);
        }

        //---
        private void listBoxItems_DoubleClick(object sender, EventArgs e)
        {
            MenuItemsProperty_Click(sender, e);
        }

        //---
        private void buttonPrewItem_Click(object sender, EventArgs e)
        {
            int ind = listBoxItems.SelectedIndex;
            if (ind < 0) return;
            ind--;
            if (ind < 0) ind = listBoxItems.Items.Count - 1;
            listBoxItems.SelectedIndex = ind;
        }

        //---
        private void buttonNextItem_Click(object sender, EventArgs e)
        {
            int ind = listBoxItems.SelectedIndex;
            if (ind < 0) return;
            ind++;
            if (ind > (listBoxItems.Items.Count - 1)) ind = 0;
            listBoxItems.SelectedIndex = ind;
        }

        //---
        private void MenuSettings_DropDownOpening(object sender, EventArgs e)
        {
            MenuSkin.SelectedIndex = (int)color.Skin;
            MenuDefColor.SelectedIndex = (int)color.Default;
        }

        //---
        private void propertyTab_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < propertyTab.Controls.Count; i++)
            {
                if (propertyTab.Controls[i].Name == "lbMl")
                {
                    (propertyTab.Controls[i] as ListBox).Height = propertyTab.Height -
                        (propertyTab.Controls[i] as ListBox).Top - 4;
                    return;
                }
            }
        }

        //---
        private void propertyTab_ControlAdded(object sender, ControlEventArgs e)
        {
            propertyTab_Resize(sender, e);
        }
    }
}
