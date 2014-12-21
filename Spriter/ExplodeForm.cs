using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Spriter
{
    public partial class ExplodeForm : Form
    {
        private bool okay_ = false;

        public int InputWidth 
        {
            get 
            {
                try 
                { 
                    return System.Convert.ToInt32(inputWidth.Text); 
                }
                catch (Exception) 
                {
                    return -1;
                }
            }
        }

        public int InputHeight
        {
            get
            {
                try
                {
                    return System.Convert.ToInt32(inputWidth.Text);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        
        public bool Okay {
            get { return okay_;  }
        }

        public ExplodeForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            okay_ = true;
            this.Close();
        }
    }
}
