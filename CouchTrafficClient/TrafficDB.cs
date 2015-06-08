using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CouchTrafficClient
{
    public partial class TrafficDB : Form
    {
        public TrafficDB()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: provide new Form for query results
            new QueryA().RunAsyncWithForm(null, button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // TODO: provide new Form for query results
            new QueryB().RunAsyncWithForm(null, button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // TODO: provide new Form for query results
            new QueryC().RunAsyncWithForm(null, button3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // TODO: provide new Form for query results
            new QueryD().RunAsyncWithForm(null, button4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // TODO: provide new Form for query results
            new QueryE().RunAsyncWithForm(null, button5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // TODO: provide new Form for query results
            new QueryF().RunAsyncWithForm(null, button6);
        }
    }
}
