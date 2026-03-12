namespace NatureOfCodeTest
{
    partial class RadialVelocityForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlRVGraph = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.trkXScale = new System.Windows.Forms.TrackBar();
            this.trkYScale = new System.Windows.Forms.TrackBar();
            this.lblXScale = new System.Windows.Forms.Label();
            this.lblYScale = new System.Windows.Forms.Label();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkXScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkYScale)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRVGraph
            // 
            this.pnlRVGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRVGraph.Location = new System.Drawing.Point(0, 0);
            this.pnlRVGraph.Name = "pnlRVGraph";
            this.pnlRVGraph.Size = new System.Drawing.Size(800, 370);
            this.pnlRVGraph.TabIndex = 0;
            this.pnlRVGraph.BackColor = System.Drawing.Color.Black;
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlControls.Controls.Add(this.lblYScale);
            this.pnlControls.Controls.Add(this.lblXScale);
            this.pnlControls.Controls.Add(this.trkYScale);
            this.pnlControls.Controls.Add(this.trkXScale);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 370);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(800, 80);
            this.pnlControls.TabIndex = 1;
            // 
            // trkXScale
            // 
            this.trkXScale.Location = new System.Drawing.Point(12, 23);
            this.trkXScale.Maximum = 730; // 2 years in days
            this.trkXScale.Minimum = 30;  // 1 month
            this.trkXScale.Name = "trkXScale";
            this.trkXScale.Size = new System.Drawing.Size(350, 45);
            this.trkXScale.TabIndex = 0;
            this.trkXScale.Value = 365;
            this.trkXScale.Scroll += new System.EventHandler(this.trkScale_Scroll);
            // 
            // trkYScale
            // 
            this.trkYScale.Location = new System.Drawing.Point(400, 23);
            this.trkYScale.Maximum = 1000;
            this.trkYScale.Minimum = 1;
            this.trkYScale.Name = "trkYScale";
            this.trkYScale.Size = new System.Drawing.Size(350, 45);
            this.trkYScale.TabIndex = 1;
            this.trkYScale.Value = 100;
            this.trkYScale.Scroll += new System.EventHandler(this.trkScale_Scroll);
            // 
            // lblXScale
            // 
            this.lblXScale.AutoSize = true;
            this.lblXScale.ForeColor = System.Drawing.Color.White;
            this.lblXScale.Location = new System.Drawing.Point(12, 7);
            this.lblXScale.Name = "lblXScale";
            this.lblXScale.Size = new System.Drawing.Size(124, 13);
            this.lblXScale.TabIndex = 2;
            this.lblXScale.Text = "X-Axis Scale (Time Window)";
            // 
            // lblYScale
            // 
            this.lblYScale.AutoSize = true;
            this.lblYScale.ForeColor = System.Drawing.Color.White;
            this.lblYScale.Location = new System.Drawing.Point(400, 7);
            this.lblYScale.Name = "lblYScale";
            this.lblYScale.Size = new System.Drawing.Size(126, 13);
            this.lblYScale.TabIndex = 3;
            this.lblYScale.Text = "Y-Axis Scale (Velocity)";
            // 
            // RadialVelocityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlRVGraph);
            this.Controls.Add(this.pnlControls);
            this.Name = "RadialVelocityForm";
            this.Text = "Radial Velocity Graph";
            this.Load += new System.EventHandler(this.RadialVelocityForm_Load);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkXScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkYScale)).EndInit();
            this.ResumeLayout(false);

        }

        public System.Windows.Forms.Panel pnlRVGraph;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.TrackBar trkXScale;
        private System.Windows.Forms.TrackBar trkYScale;
        private System.Windows.Forms.Label lblXScale;
        private System.Windows.Forms.Label lblYScale;
    }
}
