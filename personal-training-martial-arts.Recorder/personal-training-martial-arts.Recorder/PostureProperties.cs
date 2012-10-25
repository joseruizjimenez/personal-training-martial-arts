using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using personal_training_martial_arts.Posture;
using Microsoft.Kinect;

namespace personal_training_martial_arts.Recorder
{
    public partial class PostureProperties : Form
    {
        private Skeleton sk_rec;

        public PostureProperties(Skeleton sk)
        {
            this.sk_rec = sk;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostureLibrary.storePosture(
                                        new PostureInformation(this.name.Text,
                                                               this.description.Text,
                                                               Int16.Parse(this.difficulty.Text),
                                                               this.sk_rec
                                                               )
                                       );
            this.Hide();
        }

        private void PostureProperties_Load(object sender, EventArgs e)
        {

        }
    }
}
