using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace textBox
{
    public partial class formSHOW : Form
    {
        public formSHOW()
        {
            InitializeComponent();
        }

        void splitContainer1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue.ToString() == "27")
            {
                this.Close();
            }
        }

        private void splitContainer1_Resize(object sender, System.EventArgs e)
        {
            splitContainer1.SplitterDistance = splitContainer1.Width / 2;
            splitContainer2.SplitterDistance = (splitContainer1.Panel1.Height / 3) * 2;
            splitContainer3.SplitterDistance = splitContainer2.Panel2.Height;

            splitContainer4.SplitterDistance = splitContainer1.Panel1.Width / 2;

            splitContainer5.SplitterDistance = splitContainer4.Panel1.Width / 2;

            splitContainer6.SplitterDistance = splitContainer4.Panel1.Width / 2;

            splitContainer7.SplitterDistance = (splitContainer1.Panel1.Width / 3) *2;

            splitContainer8.SplitterDistance = splitContainer7.Panel1.Width / 2;

            splitContainer9.SplitterDistance = splitContainer1.Panel2.Height / 3;
        }

        private void formSHOW_Load(object sender, EventArgs e)
        {
            this.splitContainer1.Focus();
            pitboxSpeed1.Image = Properties.Resources._0;
            pitboxSpeed2.Image = Properties.Resources._0;
            pitboxSpeed3.Image = Properties.Resources._0;
            pitBoxBattery.Image = Properties.Resources.battery100;
            this.labelLoadState.Text = "";
        }

        void labelLoadState_TextChanged(object sender, System.EventArgs e)
        {
            labelLoadState.MaximumSize = new System.Drawing.Size(splitContainer9.Panel1.Width, 0);
        }
    }
}
