namespace personal_training_martial_arts.Recorder
{
    partial class VerticalControl
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
            this.verticalAlignment = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.verticalAlignment)).BeginInit();
            this.SuspendLayout();
            // 
            // verticalAlignment
            // 
            this.verticalAlignment.Location = new System.Drawing.Point(12, 12);
            this.verticalAlignment.Maximum = 5;
            this.verticalAlignment.Minimum = -5;
            this.verticalAlignment.Name = "verticalAlignment";
            this.verticalAlignment.Size = new System.Drawing.Size(260, 45);
            this.verticalAlignment.TabIndex = 0;
            this.verticalAlignment.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.verticalAlignment.Scroll += new System.EventHandler(this.verticalAlignment_Scroll);
            // 
            // VerticalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 63);
            this.Controls.Add(this.verticalAlignment);
            this.Name = "VerticalControl";
            this.Text = "VerticalControl";
            ((System.ComponentModel.ISupportInitialize)(this.verticalAlignment)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar verticalAlignment;
    }
}