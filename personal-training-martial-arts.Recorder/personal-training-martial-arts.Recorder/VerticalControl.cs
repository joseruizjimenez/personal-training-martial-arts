using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace personal_training_martial_arts.Recorder
{
    public partial class VerticalControl : Form
    {
        public delegate void ChangeAlingmentFunction(int x);
        private Func<int, int> CallBackFunction { get; set; }

        public VerticalControl(Func<int, int> cb)
        {
            this.CallBackFunction = cb;
            InitializeComponent();
        }

        private void verticalAlignment_Scroll(object sender, EventArgs e)
        {
            this.CallBackFunction(this.verticalAlignment.Value);
            //this.verticalAlignment.Enabled = false;
            System.Threading.Thread.Sleep(1000);
            //this.verticalAlignment.Enabled = true;
        }
    }
}
