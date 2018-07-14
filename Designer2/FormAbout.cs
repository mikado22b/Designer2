using System;
using System.Drawing;
using System.Windows.Forms;

namespace Designer2
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            Bitmap d;

            d = Properties.Resources.Designer2;
            d.MakeTransparent(Color.White);
            pictureBox2.Image = d;
            DateTime c = Utils.GetLinkerDateTime(System.Reflection.Assembly.GetExecutingAssembly());
            label2.Text = "Created at " + c.ToString();
        }
    }
}
