using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LumbrerasJanaLeiBSIT2A
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmRegister rForm = new frmRegister();
            rForm.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmStore sForm = new frmStore();
            sForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmPricebook pForm = new frmPricebook();
            pForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmVendors vForm = new frmVendors();
            vForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmUsers uForm = new frmUsers();
            uForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmTime tForm = new frmTime();
            tForm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmLogin logForm = new frmLogin();
            this.Hide();
            logForm.Show();
        }
    }
}
