using System;
using System.Windows.Forms;

namespace EntityProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminForm AF = new AdminForm();
            AF.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnaliticForm AF2 = new AnaliticForm();
            AF2.Show();
        }
    }
}
