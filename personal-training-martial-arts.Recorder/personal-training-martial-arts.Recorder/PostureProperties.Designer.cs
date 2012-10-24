namespace personal_training_martial_arts.Recorder
{
    partial class PostureProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textDifficulty = new System.Windows.Forms.TextBox();
            this.textDescription = new System.Windows.Forms.TextBox();
            this.textName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textDifficulty);
            this.groupBox1.Controls.Add(this.textDescription);
            this.groupBox1.Controls.Add(this.textName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 142);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Propiedades de la postura";
            // 
            // textDifficulty
            // 
            this.textDifficulty.Location = new System.Drawing.Point(6, 111);
            this.textDifficulty.Name = "textDifficulty";
            this.textDifficulty.Size = new System.Drawing.Size(331, 20);
            this.textDifficulty.TabIndex = 2;
            this.textDifficulty.Text = "Dificultad (en número) de la postura";
            // 
            // textDescription
            // 
            this.textDescription.Location = new System.Drawing.Point(6, 45);
            this.textDescription.Multiline = true;
            this.textDescription.Name = "textDescription";
            this.textDescription.Size = new System.Drawing.Size(331, 60);
            this.textDescription.TabIndex = 1;
            this.textDescription.Text = "Descripción de la postura";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(6, 19);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(331, 20);
            this.textName.TabIndex = 0;
            this.textName.Text = "Nombre de la postura";
            this.textName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // PostureProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 192);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "PostureProperties";
            this.Text = "Posture Recorder";
            this.Load += new System.EventHandler(this.PostureProperties_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.TextBox textDifficulty;
        private System.Windows.Forms.TextBox textDescription;
    }
}