using System;
using System.Windows.Forms;

namespace Designer2
{
    public partial class IcoName : Form
    {
        private string n = "";

        //---
        public string icoName
        {
            get
            {
                return n;
            }
        }

        //---
        public IcoName(string n)
        {
            InitializeComponent();
            textBox1.Text = n;
        }

        //---
        private void button1_Click(object sender, EventArgs e)
        {
            n = textBox1.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        //---
        private void IcoName_Load(object sender, EventArgs e)
        {
        }

        //---
    }
}
