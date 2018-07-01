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
    public partial class FormZoom : Form
    {
        private int v;

        //---
        public int zoomValue
        {
            get
            {
                return v;
            }
        }

        //---
        public FormZoom()
        {
            InitializeComponent();
        }

        //---
        public FormZoom(int z)
        {
            InitializeComponent();
            trackBar.Value = z;
            comboBox.Text = z.ToString();
        }

        //---
        private void buttonL_Click(object sender, EventArgs e)
        {
            if (trackBar.Value > trackBar.Minimum) trackBar.Value--;
        }

        //---
        private void buttonH_Click(object sender, EventArgs e)
        {
            if (trackBar.Value < trackBar.Maximum) trackBar.Value++;
        }

        //---
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            comboBox.Text = trackBar.Value.ToString();
        }

        //---
        private void comboBox_TextUpdate(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(comboBox.Text, out value))
                trackBar.Value = value;
        }

        //---
        private void buttonOK_Click(object sender, EventArgs e)
        {
            v = trackBar.Value;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        //---
    }
}
