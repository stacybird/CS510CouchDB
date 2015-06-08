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
            try {
                MessageBox.Show(new QueryA().Run());
            }
            catch (QueryException)
            {
            } catch (Exception x) {
                MessageBox.Show("Error in Application: " + x.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                MessageBox.Show(new QueryB().Run());
            }
            catch (QueryException)
            {
            } catch (Exception x)
            {
                MessageBox.Show("Error in Application: " + x.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try {
                MessageBox.Show(new QueryC().Run());
            } catch (QueryException) {
            } catch (Exception x)
            {
                MessageBox.Show("Error in Application: " + x.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try {
                MessageBox.Show(new QueryD().Run());
            }
            catch (QueryException)
            {
            } catch (Exception x)
            {
                MessageBox.Show("Error in Application: " + x.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try {
                MessageBox.Show(new QueryE().Run());
            }
            catch (QueryException)
            {
            } catch (Exception x)
            {
                MessageBox.Show("Error in Application: " + x.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try {
                MessageBox.Show(new QueryF().Run());
            }
            catch (QueryException)
            {
            } catch (Exception x)
            {
                MessageBox.Show("Error in Application: " + x.ToString());
            }
        }
    }
}
