using System;
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
            label1.Text = "Pixel size " + intTofloat(z);
            label3.Text = ((int)(1000 / ICOdraw.step + 1)).ToString();
        }

        //---
        protected string intTofloat(int i)
        {
            if (i < 0) i = 0;
            if (i > 1000) i = 1000;
            return ((float)i / ICOdraw.step + 1).ToString();
        }

        //---
        protected int floatToInt(float f)
        {
            f -= 1;
            if (f < 0) f = 0;
            if (f > 1000 / ICOdraw.step) f = 1000 / ICOdraw.step;
            return (int)(f * ICOdraw.step);
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
            label1.Text = "Pixel size " + intTofloat(trackBar.Value);
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
