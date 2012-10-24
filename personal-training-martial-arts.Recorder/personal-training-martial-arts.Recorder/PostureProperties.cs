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
        public Boolean isReady { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string difficulty { get; set; }
        public Skeleton skeletonToRecord;

        public PostureProperties(Skeleton skeleton)
        {
            this.isReady = false;
            this.skeletonToRecord = skeleton;
            InitializeComponent();
        }

        private void PostureProperties_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.name = this.textName.Text;
            this.description = this.textDescription.Text;
            this.difficulty = this.textDifficulty.Text;
            this.isReady = true;

            PostureLibrary.storePosture(new PostureInformation(this.name, this.description, Int16.Parse(this.difficulty), skeletonToRecord));
            this.Hide();
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
